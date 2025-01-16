from flask import Flask, request, jsonify
from models.requests.chamados_request import ChamadosParamsRequest, ChamadosRequest
import requests

app = Flask(__name__)

@app.route('/get_all_chamados', methods=['GET'])
def get_all_chamados():
    token = request.args.get('token')
    inicio = request.args.get('inicio')
    fim = request.args.get('fim')
    responsaveis = request.args.get('responsaveis')

    if responsaveis is not None:
        responsaveis = [int(x) for x in responsaveis.split(',')]

    if not token:
        return jsonify({"error": "Token é obrigatório"}), 400

    all_data = []

    if not responsaveis:
        responsaveis = [None]

    for responsavel in responsaveis:
        params = ChamadosParamsRequest(abertoStart = inicio, abertoEnd = fim, responsavel = responsavel)
        request_obj = ChamadosRequest(token, params)
        url = request_obj.buildUrl()

        while True:
            response = requests.get(url, headers=request_obj.getHeaders())

            if response.status_code != 200:
                return jsonify({"error": f"Erro na requisição: {response.status_code}"}), 500
            
            data = response.json()
            all_data.extend(data['data'])

            if data['start'] + data['limit'] >= data['totalPage'] * data['limit']:
                break

            params.start = data['start'] + data['limit']
            request_obj = ChamadosRequest(token, params)
            url = request_obj.buildUrl()

    return jsonify({"data": all_data})

if __name__ == '__main__':
    app.run(debug=True)
