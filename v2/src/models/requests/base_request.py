class BaseRequest:
    def __init__(self, authorization: str, contentType: str = "application/json;charset=UTF-8"):
        self.authorization = authorization
        self.contentType = contentType

    def __str__(self):
        return f"Authorization: {self.authorization}\nContent-Type: {self.contentType}"