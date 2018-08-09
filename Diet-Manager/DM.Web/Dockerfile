FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Diet-Manager/Diet-Manager.csproj Diet-Manager/
RUN dotnet restore Diet-Manager/Diet-Manager.csproj
COPY . .
WORKDIR /src/Diet-Manager
RUN dotnet build Diet-Manager.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Diet-Manager.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Diet-Manager.dll"]
