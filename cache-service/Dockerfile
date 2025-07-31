# Etapa base com o runtime do ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia tudo e restaura
COPY . .
RUN dotnet restore "./product-service.csproj"

# Publica o projeto
RUN dotnet publish "./product-service.csproj" -c Release -o /app/publish

# Etapa final (runtime + arquivos publicados)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Variável de ambiente para conectar ao Redis
ENV REDIS__CONNECTIONSTRING=redis:6379

ENTRYPOINT ["dotnet", "product-service.dll"]
