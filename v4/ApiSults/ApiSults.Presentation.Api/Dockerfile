# Estágio base: imagem para execução da aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Estágio de build: imagem para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG CACHE_BUSTER=1

WORKDIR .

# Copiar os arquivos de projeto e restaurar dependências
COPY ["ApiSults.Presentation.Api/ApiSults.Presentation.Api.csproj", "ApiSults.Presentation.Api/"]
COPY ["ApiSults.Application/ApiSults.Application.csproj", "ApiSults.Application/"]
COPY ["ApiSults.Infrastructure.Data/ApiSults.Infrastructure.Data.csproj", "ApiSults.Infrastructure.Data/"]

# Restaurar dependências sem cache
RUN dotnet restore "ApiSults.Presentation.Api/ApiSults.Presentation.Api.csproj" --no-cache

# Copiar o restante dos arquivos do projeto
COPY . .

# Construir a aplicação
WORKDIR "ApiSults.Presentation.Api"
RUN dotnet build "ApiSults.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio de publicação: otimizar o tamanho da imagem final
FROM build AS publish
RUN dotnet publish "ApiSults.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio final: imagem de execução com a aplicação pronta
FROM base AS final
WORKDIR /app

# Copiar os artefatos publicados para a imagem final
COPY --from=publish /app/publish .

# Definir o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "ApiSults.Presentation.Api.dll"]