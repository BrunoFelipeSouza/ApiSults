import datetime
import json
import time
import threading
from flask import Flask, jsonify, render_template, request

app = Flask(__name__)

configsFile = "configs.json"
dataFile = "data.json"

def getConfigs():
    try:
        with open(configsFile, "r") as file:
            return json.load(file)
    except:
        setConfigs({
            "autoRefresh": True,
            "minutes": 10,
            "lastRefresh": datetime.date(2000, 1, 1).strftime("%Y-%m-%dT%H:%M:%SZ")
        })
        return getConfigs()
    
def setConfigs(config):
    with open(configsFile, "w", encoding="utf-8") as file:
        json.dump(config, file, ensure_ascii=False, indent = 4)

def getData():
    try:
        with open(dataFile, "r") as file:
            return json.load(file)
    except:
        return []
    
def verifyData():
    configs = getConfigs()
    autoRefresh = configs["autoRefresh"]
    minutes = configs["minutes"]
    lastRefresh = configs["lastRefresh"]

    lastRefreshDate = datetime.datetime.strptime(lastRefresh, "%Y-%m-%dT%H:%M:%SZ")
    currentDate = datetime.datetime.now(datetime.timezone.utc).replace(microsecond=0, tzinfo=None)

    print(lastRefreshDate, currentDate)
    
def setData(data):
    with open(dataFile, "w", encoding="utf-8") as file:
        json.dump(data, file, ensure_ascii=False, indent = 4)

@app.route("/")
def homePage():
    return render_template("home.html")

@app.route("/configs", methods=["GET"])
def getConfigurations():
    return jsonify(getConfigs()), 200

@app.route("/configs", methods=["POST"])
def setConfigurations():
    autoRefresh = request.get_json()["autoRefresh"]
    print(autoRefresh)
    minutes = request.get_json()["minutes"]
    
    if autoRefresh is None and minutes is None:
        return jsonify({"error": "Nenhuma configuração foi enviada"}), 400
    
    if not isinstance(autoRefresh, bool):
        return jsonify({"error": "O autoRefresh precisa ser um booleano"}), 400

    if not isinstance(minutes, int):
        return jsonify({"error": "A quantidade de minutos precisa ser um inteiro"}), 400
    
    configs = getConfigs()
    configs["autoRefresh"] = autoRefresh
    configs["minutes"] = minutes
    setConfigs(configs)

    return jsonify(getConfigs()), 201

def backgroundJob():
    while True:
        verifyData()
        time.sleep(5)

if __name__ == '__main__':
    thread = threading.Thread(target=backgroundJob, daemon=True)
    thread.start()
    app.run(debug=True, use_reloader=False, port=80)
