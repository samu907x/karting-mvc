# Versión de .NET: debe coincidir con <TargetFramework> de KartingMvc.csproj (net10.0)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
# El servicio de hosting indica el puerto en la variable PORT
CMD ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet KartingMvc.dll
