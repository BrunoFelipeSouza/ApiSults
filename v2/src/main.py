import datetime
import json
import requests
import time
import threading
from models.requests.chamados_request import ChamadosParamsRequest, ChamadosRequest
from flask import Flask, jsonify, render_template, request

app = Flask(__name__)

configsFile = "configs.json"
dataFile = "data.json"
hasData = False

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
    with open(dataFile, "w", encoding="utf-8") as file:
        json.dump(data, file, ensure_ascii=False, indent = 4)
    
def updateData(newData):
    data = getData()
    for newTicket in newData:
        ticket = next((ticket for ticket in data if ticket['id'] == newTicket['id']), None)
        if ticket:
            ticket.update(newTicket)
        else:
            data.append(newTicket)
    setData(data)
    
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
    return render_template("home.html")

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
    if not hasData:
        return jsonify({"error": "Os dados ainda nao foram carregados"}), 422

    departamento = request.args.get('departamento')
    departamentos = request.args.get('departamentos')

    data = getData()

    if departamento is not None:
        try:
            departamento = int(departamento)
        except:
            return jsonify({"error": "Departamento deve ser um inteiro"}), 400
        data = [ticket for ticket in data if ticket['departamento']['id'] == departamento]

    if departamentos is not None:
        try:
            departamentos = [int(x) for x in departamentos.split(',')]
        except:
            return jsonify({"error": "Departamentos devem ser uma lista de inteiros"}), 400
        data = [ticket for ticket in data if ticket['departamento']['id'] in departamentos]

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
