import datetime
import json
import os
import requests
import time
import threading
from models.requests.chamados_request import ChamadosParamsRequest, ChamadosRequest
from flask import Flask, jsonify, render_template_string, request

app = Flask(__name__)

configsFile = "configs.json"
dataFile = "data.json"
hasData = False
updatingData = False

def getConfigs():
    try:
        with open(configsFile, "r", encoding="utf-8") as file:
            return json.load(file)
    except:
        setConfigs({
            "autoRefresh": True,
            "minutes": 1,
            "lastRefresh": None,
            "token": None
        })
        return getConfigs()
    
def setConfigs(config):
    with open(configsFile, "w", encoding="utf-8") as file:
        json.dump(config, file, ensure_ascii=False, indent = 4)

def getData():
    try:
        with open(dataFile, "r", encoding="utf-8") as file:
            return json.load(file)
    except:
        return []
    
def setData(data):
    tempFile = f"{dataFile}.tmp"
    try:
        with open(tempFile, "w", encoding="utf-8") as file:
            json.dump(data, file, ensure_ascii=False, indent=4)
        os.replace(tempFile, dataFile)
    except Exception as e:
        if os.path.exists(tempFile):
            os.remove(tempFile)
        print(f"Erro ao gravar dados: {e}")
    
def updateData(newData):
    print("Updating data...")
    global updatingData
    updatingData = True
    data = getData()
    for newTicket in newData:
        ticket = next((ticket for ticket in data if ticket['id'] == newTicket['id']), None)
        if ticket:
            ticket.update(newTicket)
        else:
            data.append(newTicket)
    setData(data)
    updatingData = False
    
def refreshData():
    print("Refreshing data...")
    configs = getConfigs()
    token = configs["token"]
    lastRefresh = configs["lastRefresh"]
    lastRefresh = datetime.date(2000, 1, 1).strftime("%Y-%m-%dT%H:%M:%SZ") if lastRefresh is None else lastRefresh
    
    params = ChamadosParamsRequest()

    if lastRefresh is not None:
        params.ultimaAlteracaoStart = lastRefresh

    request = ChamadosRequest(token, params)
    url = request.buildUrl()

    allData = []

    print(f"getting data...")
    while True:
        response = requests.get(url, headers=request.getHeaders())

        if response.status_code != 200:
            return jsonify({"error": f"Erro na requisição: {response.status_code}"}), 500
        
        data = response.json()

        if 'data' not in data:
            return jsonify({"error": "Formato inesperado da resposta da API"}), 500
        
        allData.extend(data['data'])

        if data['start'] >= data['totalPage']:
            break
        
        params.start = data['start'] + 1
        request = ChamadosRequest(token, params)
        url = request.buildUrl()
        print(f"getting data... page {params.start} of {data['totalPage']}")

    updateData(allData)
    configs["lastRefresh"] = datetime.datetime.now(datetime.timezone.utc).replace(microsecond=0, tzinfo=None).strftime("%Y-%m-%dT%H:%M:%SZ")
    setConfigs(configs)
    print("Data refreshed!")
    global hasData
    hasData = True

    return
    
def autoRefreshData():
    configs = getConfigs()
    autoRefresh = configs["autoRefresh"]
    minutes = configs["minutes"]
    lastRefresh = configs["lastRefresh"]
    token = configs["token"]

    if not autoRefresh or not token:
        return
    
    if lastRefresh is None:
        refreshData()
        return

    lastRefreshDate = datetime.datetime.strptime(lastRefresh, "%Y-%m-%dT%H:%M:%SZ")
    currentDate = datetime.datetime.now(datetime.timezone.utc).replace(microsecond=0, tzinfo=None)
    timeSinceLastRefresh = (currentDate - lastRefreshDate).total_seconds() / 60

    if timeSinceLastRefresh >= minutes:
        refreshData()
        return

@app.route("/")
def homePage():
    return render_template_string("""
    <!DOCTYPE html>
    <html lang="en">
    <head>
        <meta charset="UTF-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>Ferramenta Api Sults</title>
        <link
        rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.1/css/bootstrap.min.css"
        />
        <style>
        body {
            background-color: #863997;
            font-family: Arial, sans-serif;
        }
      .container {
        margin-top: 30px;
      }
      .card {
        background-color: #ffffff;
        border: none;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
      }
      .btn-primary {
        background-color: #863997;
        border-color: #863997;
      }
      .btn-primary:hover {
        background-color: #863997;
        border-color: #863997;
      }
      footer {
        margin-top: 30px;
        text-align: center;
        color: #fff;
      }
    </style>
  </head>
  <body>
    <div class="container">
      <h1 class="text-center text-light">Api Sults</h1>

      <div class="card p-4 mt-4">
        <h2>Configurações</h2>
        <form id="configForm">
          <div class="mb-3">
            <label for="token" class="form-label">Token</label>
            <input
              type="text"
              class="form-control"
              id="token"
              placeholder="Insira o token"
              required
            />
          </div>
          <div class="mb-3">
            <label for="autoRefresh" class="form-label"
              >Atualização Automática</label
            >
            <select id="autoRefresh" class="form-select">
              <option value="true">Ativado</option>
              <option value="false">Desativado</option>
            </select>
          </div>
          <div class="mb-3">
            <label for="minutes" class="form-label">Intervalo (minutos)</label>
            <input
              type="number"
              class="form-control"
              id="minutes"
              value="1"
              min="1"
              required
            />
          </div>
          <button type="button" class="btn btn-primary" id="saveConfig">
            Salvar Configurações
          </button>
        </form>
      </div>

      <div class="card p-4 mt-4">
        <h2>URLs Disponíveis</h2>
        <ul>
          <li>
            <code>GET /chamados?departamento=ID</code>: Listar chamados por
            departamento
          </li>
          <li><code>PATCH /chamados</code>: Atualizar chamados manualmente</li>
        </ul>
      </div>
    </div>

    <footer>
      <p>Ferramenta desenvolvida por Natalli Moreira e Bruno Felipe.</p>
    </footer>

    <script>
      const loadConfig = () => {
        fetch("/configs", {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        })
          .then((response) => {
            if (response.ok) {
              return response.json();
            }
            throw new Error("Erro ao carregar configurações");
          })
          .then((config) => {
            document.getElementById("token").value = config.token || "";
            document.getElementById("autoRefresh").value = config.autoRefresh
              ? "true"
              : "false";
            document.getElementById("minutes").value = config.minutes || 10;
          })
          .catch((error) =>
            console.error("Erro ao carregar configurações:", error)
          );
      };

      const saveButton = document.getElementById("saveConfig");
      saveButton.addEventListener("click", () => {
        const token = document.getElementById("token").value;
        const autoRefresh =
          document.getElementById("autoRefresh").value === "true";
        const minutes = parseInt(document.getElementById("minutes").value, 10);

        const configData = { token, autoRefresh, minutes };

        fetch("/configs", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(configData),
        })
          .then((response) => {
            if (response.ok) {
              alert("Configurações salvas com sucesso!");
            } else {
              alert("Erro ao salvar configurações.");
            }
          })
          .catch((error) => console.error("Erro:", error));
      });

      document.addEventListener("DOMContentLoaded", loadConfig);
    </script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.1/js/bootstrap.bundle.min.js"></script>
  </body>
</html>
    """);

@app.route("/configs", methods=["GET"])
def getConfigurations():
    return jsonify(getConfigs()), 200

@app.route("/configs", methods=["POST"])
def setConfigurations():
    autoRefresh = request.get_json()["autoRefresh"]
    minutes = request.get_json()["minutes"]
    token = request.get_json()["token"]
    
    if autoRefresh is None and minutes is None:
        return jsonify({"error": "Nenhuma configuração foi enviada"}), 400
    
    if not isinstance(autoRefresh, bool):
        return jsonify({"error": "O autoRefresh precisa ser um booleano"}), 400

    if not isinstance(minutes, int):
        return jsonify({"error": "A quantidade de minutos precisa ser um inteiro"}), 400
    
    configs = getConfigs()
    configs["autoRefresh"] = autoRefresh
    configs["minutes"] = minutes
    configs["token"] = token
    setConfigs(configs)

    return jsonify(getConfigs()), 201

@app.route("/chamados", methods=["GET"])
def getChamados():
    while updatingData:
        time.sleep(1)

    if not hasData:
        return jsonify({"error": "Os dados ainda nao foram carregados"}), 422

    departamento = request.args.get('departamento')
    departamentos = request.args.get('departamentos')

    data = getData()

    if departamento is not None:
        try:
            departamento = int(departamento)
            data = [ticket for ticket in data if ticket['departamento']['id'] == departamento]
        except:
            return jsonify({"error": "Departamento deve ser um inteiro"}), 400

    if departamentos is not None:
        try:
            departamentos = [int(x) for x in departamentos.split(',')]
            data = [ticket for ticket in data if ticket['departamento']['id'] in departamentos]
        except:
            return jsonify({"error": "Departamentos devem ser uma lista de inteiros"}), 400

    return jsonify(data), 200

@app.route("/chamados", methods=["PATCH"])
def updateChamados():
    if not hasData:
        return jsonify({"error": "Os dados ainda nao foram carregados"}), 422
    
    configs = getConfigs()
    token = configs["token"]
    autoRefresh = configs["autoRefresh"]

    if not token:
        return jsonify({"error": "Token nao configurado"}), 400
    
    if autoRefresh:
        return jsonify({"error": "Atualizacao automatica ativada"}), 400

    refreshData()

    return jsonify(), 204

def backgroundJob():
    while True:
        autoRefreshData()
        time.sleep(5)

if __name__ == '__main__':
    hasData = getData().__len__() > 0
    thread = threading.Thread(target=backgroundJob, daemon=True)
    thread.start()
    app.run(debug=True, use_reloader=False, port=80)
