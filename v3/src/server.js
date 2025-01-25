const express = require("express");
const fs = require("fs");
const {
  ChamadosParamsRequest,
  ChamadosRequest,
} = require("./models/requests/chamados.request");
const nodeCron = require("node-cron");

const app = express();
const port = 80;

app.use(express.static("public"));
app.use(express.json());

const configsFile = "configs.json";
const dataFile = "data.json";
let hasData = false;
let updatingData = false;

function getConfigs() {
  try {
    const configs = fs.readFileSync(configsFile, { encoding: "utf-8" });
    return JSON.parse(configs);
  } catch {
    setConfigs({
      autoRefresh: true,
      minutes: 1,
      lastRefresh: null,
      token: null,
    });
    return getConfigs();
  }
}

function setConfigs(configs) {
  fs.writeFileSync(configsFile, JSON.stringify(configs), { encoding: "utf-8" });
}

function getData() {
  try {
    const data = fs.readFileSync(dataFile, { encoding: "utf-8" });
    return JSON.parse(data);
  } catch {
    return [];
  }
}

function setData(data) {
  const tempFile = `${dataFile}.tmp`;
  try {
    fs.writeFileSync(tempFile, JSON.stringify(data), { encoding: "utf-8" });
    fs.renameSync(tempFile, dataFile);
  } catch {
    if (fs.existsSync(tempFile)) {
      fs.unlinkSync(tempFile);
    }
  }
}

async function updateDataAsync(newData) {
  return await new Promise((resolve) => {
    console.log("Updating data...");
    updatingData = true;
    const data = getData();
    for (const newTicket of newData) {
      const ticket = data.find((ticket) => ticket.id === newTicket.id);
      if (ticket) {
        Object.assign(ticket, newTicket);
      } else {
        data.push(newTicket);
      }
    }
    setData(data);
    updatingData = false;
    return resolve();
  });
}

async function refreshDataAsync() {
  return await new Promise(async (resolve) => {
    console.log("Refreshing data...");
    let configs = getConfigs();
    const token = configs.token;
    const lastRefresh = configs.lastRefresh || "2000-01-01T00:00:00Z";

    const params = new ChamadosParamsRequest();

    if (lastRefresh) params.ultimaAlteracaoStart = lastRefresh;

    let request = new ChamadosRequest(token, params);
    let url = request.buildUrl();

    const allData = [];

    console.log("getting data...");

    while (true) {
      const response = await fetch(url, {
        headers: request.getHeaders(),
      });

      if (response.status !== 200) {
        console.log(await response.json());
        return resolve({ error: `Erro na requisição: ${response.status}` });
      }

      const responseJson = await response.json();
      const data = responseJson?.data;

      if (!data)
        return resolve({ error: "Formato inesperado da resposta da API" });

      allData.push(...data);

      if (responseJson.start >= responseJson.totalPage) break;

      params.start = responseJson.start + 1;
      request = new ChamadosRequest(token, params);
      url = request.buildUrl();
      console.log(
        `getting data... page ${params.start} of ${responseJson.totalPage}`
      );
    }

    await updateDataAsync(allData);
    configs.lastRefresh = new Date().toISOString().split(".")[0] + "Z";
    setConfigs(configs);
    console.log("Data refreshed!");
    hasData = true;

    return resolve();
  });
}

async function autoRefreshDataAsync() {
  return await new Promise(async (resolve) => {
    const configs = getConfigs();
    const autoRefresh = configs.autoRefresh;
    const minutes = configs.minutes;
    const lastRefresh = configs.lastRefresh;
    const token = configs.token;

    if (!autoRefresh || !token) return resolve();

    if (!lastRefresh) {
      await refreshDataAsync();
      return resolve();
    }

    const lastRefreshDate = new Date(lastRefresh);
    const currentDate = new Date();
    const timeSinceLastRefresh = (currentDate - lastRefreshDate) / 60000;

    if (timeSinceLastRefresh >= minutes) {
      await refreshDataAsync();
      return resolve();
    }

    return resolve();
  });
}

app.get("/", (req, res) => {
  fs.readFile("./pages/home/home.html", "utf-8", (err, data) => {
    if (err) {
      console.error(err);
      return res.status(500).send("Erro ao ler o arquivo HTML");
    } else {
      return res.send(data);
    }
  });
});

app.get("/configs", (_, res) => {
  return res.status(200).json(getConfigs());
});

app.post("/configs", (req, res) => {
  const autoRefresh = req.body.autoRefresh;
  const minutes = req.body.minutes;
  const token = req.body.token;

  if (!autoRefresh || !minutes || !token) {
    return res.status(400).json({ error: "Nenhuma configuração foi enviada" });
  }

  if (typeof autoRefresh !== "boolean") {
    return res
      .status(400)
      .json({ error: "O autoRefresh precisa ser um booleano" });
  }

  if (typeof minutes !== "number") {
    return res
      .status(400)
      .json({ error: "A quantidade de minutos precisa ser um inteiro" });
  }

  let configs = getConfigs();
  configs.autoRefresh = autoRefresh;
  configs.minutes = minutes;
  configs.token = token;
  setConfigs(configs);

  return res.status(201).json(getConfigs());
});

app.get("/chamados", async (req, res) => {
  while (updatingData) {
    setTimeout(() => {}, 1000);
  }

  if (!hasData) {
    return res
      .status(422)
      .json({ error: "Os dados ainda nao foram carregados" });
  }

  const departamento = req.query.departamento;
  const departamentos = req.query.departamentos;

  let data = getData();

  if (departamento)
    try {
      data = data.filter(
        (ticket) => ticket.departamento.id === parseInt(departamento)
      );
      return res.status(200).json(data);
    } catch {
      return res
        .status(400)
        .json({ error: "O departamento precisa ser um inteiro" });
    }

  if (departamentos)
    try {
      data = data.filter((ticket) =>
        departamentos
          .split(",")
          .map((x) => parseInt(x))
          .includes(ticket.departamento.id)
      );
      return res.status(200).json(data);
    } catch {
      return res
        .status(400)
        .json({ error: "Os departamentos precisam ser uma lista de inteiros" });
    }

  return res.status(200).json(data);
});

app.patch("/chamados", async (req, res) => {
  if (!hasData)
    return res
      .status(422)
      .json({ error: "Os dados ainda nao foram carregados" });

  const configs = getConfigs();
  const token = configs.token;
  const autoRefresh = configs.autoRefresh;

  if (!token) return res.status(422).json({ error: "Token nao configurado" });

  if (autoRefresh)
    return res.status(422).json({ error: "Atualizacao automatica ativada" });

  await refreshDataAsync();
  return res.status(204);
});

app.listen(port, () => {
  let isRunning = false;
  nodeCron.schedule("*/5 * * * * *", () => {
    if (isRunning) return;
    isRunning = true;
    autoRefreshDataAsync().then(() => {
      isRunning = false;
    });
  });
  console.log(`Servidor iniciado na porta ${port}`);
});
