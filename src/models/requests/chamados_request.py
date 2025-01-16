from .base_request import BaseRequest
from models.enums.routes_enum import RoutesEnum
from urllib.parse import urlencode

class ChamadosParamsRequest():
    def __init__(
        self,
        id: int = None,
        situacao: int = None,
        solicitante: int = None,
        tipo: int = None,
        responsavel: int = None,
        unidade: int = None,
        abertoStart: str = None,
        abertoEnd: str = None,
        concluidoStart: str = None,
        concluidoEnd: str = None,
        resolvidoStart: str = None,
        resolvidoEnd: str = None,
        ultimaAlteracaoStart: str = None,
        ultimaAlteracaoEnd: str = None
    ):
        self.start = 0
        self.limit = 100
        self.id = id
        self.situacao = situacao
        self.solicitante = solicitante
        self.tipo = tipo
        self.responsavel = responsavel
        self.unidade = unidade
        self.abertoStart = abertoStart
        self.abertoEnd = abertoEnd
        self.concluidoStart = concluidoStart
        self.concluidoEnd = concluidoEnd
        self.resolvidoStart = resolvidoStart
        self.resolvidoEnd = resolvidoEnd
        self.ultimaAlteracaoStart = ultimaAlteracaoStart
        self.ultimaAlteracaoEnd = ultimaAlteracaoEnd

    def __str__(self):
        return (
            f"Start={self.start}\n"
            f"Limit={self.limit}\n"
            f"Id={self.id or 'Não informado'}\n"
            f"Situacao={self.situacao or 'Não informado'}\n"
            f"Solitante={self.solicitante or 'Não informado'}\n"
            f"Tipo={self.tipo or 'Não informado'}\n"
            f"Responsavel={self.responsavel or 'Não informado'}\n"
            f"Unidade={self.unidade or 'Não informado'}\n"
            f"AbertoStart={self.abertoStart or 'Não informado'}\n"
            f"AbertoEnd={self.abertoEnd or 'Não informado'}\n"
            f"ConcluidoStart={self.concluidoStart or 'Não informado'}\n"
            f"ConcluidoEnd={self.concluidoEnd or 'Não informado'}\n"
            f"ResolvidoStart={self.resolvidoStart or 'Não informado'}\n"
            f"ResolvidoEnd={self.resolvidoEnd or 'Não informado'}\n"
            f"UltimaAlteracaoStart={self.ultimaAlteracaoStart or 'Não informado'}\n"
            f"UltimaAlteracaoEnd={self.ultimaAlteracaoEnd or 'Não informado'}\n"
        )
    
    def getUrlParams(self):
        params = {
            "start": self.start,
            "limit": self.limit,
            "id": self.id,
            "situacao": self.situacao,
            "solicitante": self.solicitante,
            "tipo": self.tipo,
            "responsavel": self.responsavel,
            "unidade": self.unidade,
            "abertoStart": self.abertoStart,
            "abertoEnd": self.abertoEnd,
            "concluidoStart": self.concluidoStart,
            "concluidoEnd": self.concluidoEnd,
            "resolvidoStart": self.resolvidoStart,
            "resolvidoEnd": self.resolvidoEnd,
            "ultimaAlteracaoStart": self.ultimaAlteracaoStart,
            "ultimaAlteracaoEnd": self.ultimaAlteracaoEnd,
        }

        params = {k: v for k, v in params.items() if v is not None}

        return params
    
class ChamadosRequest(BaseRequest):
    def __init__(self, authorization: str, params: ChamadosParamsRequest):
        super().__init__(authorization)
        self.params = params

    def __str__(self):
        return (
            f"Authorization: {self.authorization}\n"
            f"Content-Type: {self.contentType}\n"
            f"{self.params.__str__()}"
        )
    
    def buildUrl(self):
        queryString = urlencode(self.params.getUrlParams())
        return f"{RoutesEnum.Chamados}?{queryString}"
    
    def getHeaders(self):
        return {
            "Authorization": self.authorization,
            "Content-Type": self.contentType
        }