# Est�gio base: imagem para execu��o da aplica��o
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Est�gio de build: imagem para compilar a aplica��o
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG CACHE_BUSTER=1

WORKDIR .

# Copiar os arquivos de projeto e restaurar depend�ncias
COPY ["ApiSults.Presentation.Api/ApiSults.Presentation.Api.csproj", "ApiSults.Presentation.Api/"]
COPY ["ApiSults.Application/ApiSults.Application.csproj", "ApiSults.Application/"]
COPY ["ApiSults.Infrastructure.Data/ApiSults.Infrastructure.Data.csproj", "ApiSults.Infrastructure.Data/"]

# Restaurar depend�ncias sem cache
RUN dotnet restore "ApiSults.Presentation.Api/ApiSults.Presentation.Api.csproj" --no-cache

# Copiar o restante dos arquivos do projeto
COPY . .

# Construir a aplica��o
WORKDIR "ApiSults.Presentation.Api"
RUN dotnet build "ApiSults.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Est�gio de publica��o: otimizar o tamanho da imagem final
FROM build AS publish
RUN dotnet publish "ApiSults.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Est�gio final: imagem de execu��o com a aplica��o pronta
FROM base AS final
WORKDIR /app

# Copiar os artefatos publicados para a imagem final
COPY --from=publish /app/publish .

# Definir o ponto de entrada da aplica��o
ENTRYPOINT ["dotnet", "ApiSults.Presentation.Api.dll"]