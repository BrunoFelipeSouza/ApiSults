class Home {
  static render() {
    return `
          <!DOCTYPE html>
          <html lang="pt-BR">
          <head>
              <meta charset="UTF-8" />
              <meta name="viewport" content="width=device-width, initial-scale=1.0" />
              <title>Ferramenta Api Sults - Login</title>
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
                  background-color: #7a338a;
                  border-color: #7a338a;
              }
              footer {
                  margin-top: 30px;
                  text-align: center;
                  color: #fff;
              }
              </style>
          </head>
          <body>
              <div class="container" id="loginContainer">
                  <h1 class="text-center text-light">Api Sults - Login</h1>
                  <div class="card p-4 mt-4 text-center">
                      <h2>Digite a senha para acessar</h2>
                      <input type="password" id="passwordInput" class="form-control mt-3" placeholder="Senha" />
                      <button class="btn btn-primary mt-3" id="loginButton">Entrar</button>
                  </div>
              </div>
              
              <div id="appContainer" style="display: none;">
                  ${Home.renderMainContent()}
              </div>
              
              <script>
                  document.getElementById("loginButton").addEventListener("click", () => {
                      const password = document.getElementById("passwordInput").value;
                      if (password === "@P8e29e44@") {
                          document.getElementById("loginContainer").style.display = "none";
                          document.getElementById("appContainer").style.display = "block";
                      } else {
                          alert("Senha incorreta!");
                      }
                  });
              </script>
          </body>
          </html>
      `;
  }

  static renderMainContent() {
    return `
          <div class="container">
              <h1 class="text-center text-light">Api Sults</h1>
              <div class="card p-4 mt-4">
                  <h2>Configurações</h2>
                  <form id="configForm">
                      <div class="mb-3">
                          <label for="token" class="form-label">Token</label>
                          <input type="text" class="form-control" id="token" placeholder="Insira o token" required />
                      </div>
                      <div class="mb-3">
                          <label for="autoRefresh" class="form-label">Atualização Automática</label>
                          <select id="autoRefresh" class="form-select">
                              <option value="true">Ativado</option>
                              <option value="false">Desativado</option>
                          </select>
                      </div>
                      <div class="mb-3">
                          <label for="minutes" class="form-label">Intervalo (minutos)</label>
                          <input type="number" class="form-control" id="minutes" value="1" min="1" required />
                      </div>
                      <button type="button" class="btn btn-primary" id="saveConfig">Salvar Configurações</button>
                  </form>
              </div>
              <div class="card p-4 mt-4">
                  <h2>URLs Disponíveis</h2>
                  <ul>
                      <li><code>GET /chamados?departamento=ID</code>: Listar chamados por departamento</li>
                      <li><code>PATCH /chamados</code>: Atualizar chamados manualmente</li>
                  </ul>
              </div>
              <footer>
                  <p>Ferramenta desenvolvida por Natalli Moreira e Bruno Felipe.</p>
              </footer>
              <script>
                  document.addEventListener("DOMContentLoaded", () => {
                      fetch("/configs", {
                          method: "GET",
                          headers: { "Content-Type": "application/json" }
                      })
                      .then(response => response.ok ? response.json() : Promise.reject("Erro ao carregar configurações"))
                      .then(config => {
                          document.getElementById("token").value = config.token || "";
                          document.getElementById("autoRefresh").value = config.autoRefresh ? "true" : "false";
                          document.getElementById("minutes").value = config.minutes || 10;
                      })
                      .catch(error => console.error("Erro ao carregar configurações:", error));
                  });
                  document.getElementById("saveConfig").addEventListener("click", () => {
                      const configData = {
                          token: document.getElementById("token").value,
                          autoRefresh: document.getElementById("autoRefresh").value === "true",
                          minutes: parseInt(document.getElementById("minutes").value, 10)
                      };
                      fetch("/configs", {
                          method: "POST",
                          headers: { "Content-Type": "application/json" },
                          body: JSON.stringify(configData)
                      })
                      .then(response => response.ok ? alert("Configurações salvas com sucesso!") : alert("Erro ao salvar configurações."))
                      .catch(error => console.error("Erro:", error));
                  });
              </script>
          </div>
      `;
  }
}

module.exports = Home;
