const RoutesEnum = require("../enums/routes.enum");

class ChamadosParamsRequest {
  constructor(
    id = null,
    situacao = null,
    solicitante = null,
    tipo = null,
    responsavel = null,
    unidade = null,
    abertoStart = null,
    abertoEnd = null,
    concluidoStart = null,
    concluidoEnd = null,
    resolvidoStart = null,
    resolvidoEnd = null,
    ultimaAlteracaoStart = null,
    ultimaAlteracaoEnd = null
  ) {
    this.start = 0;
    this.limit = 100;
    this.id = id;
    this.situacao = situacao;
    this.solicitante = solicitante;
    this.tipo = tipo;
    this.responsavel = responsavel;
    this.unidade = unidade;
    this.abertoStart = abertoStart;
    this.abertoEnd = abertoEnd;
    this.concluidoStart = concluidoStart;
    this.concluidoEnd = concluidoEnd;
    this.resolvidoStart = resolvidoStart;
    this.resolvidoEnd = resolvidoEnd;
    this.ultimaAlteracaoStart = ultimaAlteracaoStart;
    this.ultimaAlteracaoEnd = ultimaAlteracaoEnd;
  }

  getUrlParams() {
    const params = {
      start: this.start,
      limit: this.limit,
      id: this.id,
      situacao: this.situacao,
      solicitante: this.solicitante,
      tipo: this.tipo,
      responsavel: this.responsavel,
      unidade: this.unidade,
      abertoStart: this.abertoStart,
      abertoEnd: this.abertoEnd,
      concluidoStart: this.concluidoStart,
      concluidoEnd: this.concluidoEnd,
      resolvidoStart: this.resolvidoStart,
      resolvidoEnd: this.resolvidoEnd,
      ultimaAlteracaoStart: this.ultimaAlteracaoStart,
      ultimaAlteracaoEnd: this.ultimaAlteracaoEnd,
    };

    return Object.fromEntries(
      Object.entries(params).filter(([key, value]) => value !== null)
    );
  }
}

class ChamadosRequest {
  constructor(authorization, params) {
    this.authorization = authorization;
    this.params = params;
    this.contentType = "application/json";
  }

  buildUrl() {
    const queryString = new URLSearchParams(
      this.params.getUrlParams()
    ).toString();
    return `${RoutesEnum.Chamados}?${queryString}`;
  }

  getHeaders() {
    return {
      Authorization: this.authorization,
      "Content-Type": this.contentType,
    };
  }
}

module.exports = { ChamadosRequest, ChamadosParamsRequest };
