class Home {
  static render() {
    return `
        <!DOCTYPE html>
        <html lang="pt-BR">
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
    `;
  }
}

module.exports = Home;
