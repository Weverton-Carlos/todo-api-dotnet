# ========================
# 🔹 Etapa 1 - Build
# ========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas o csproj (cache de restore)
COPY src/TodoDesafio.Api/TodoDesafio.Api.csproj src/TodoDesafio.Api/
COPY src/TodoDesafio.Application/TodoDesafio.Application.csproj src/TodoDesafio.Application/
COPY src/TodoDesafio.Domain/TodoDesafio.Domain.csproj src/TodoDesafio.Domain/
COPY src/TodoDesafio.Infrastructure/TodoDesafio.Infrastructure.csproj src/TodoDesafio.Infrastructure/

RUN dotnet restore src/TodoDesafio.Api/TodoDesafio.Api.csproj

# Copia o restante do código
COPY . .

# Publica a aplicação
RUN dotnet publish src/TodoDesafio.Api/TodoDesafio.Api.csproj -c Release -o /app/publish

# ========================
# 🔹 Etapa 2 - Runtime
# ========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Configuração da porta
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "TodoDesafio.Api.dll"]