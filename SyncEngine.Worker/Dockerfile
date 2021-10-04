FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore SyncEngine.Worker.csproj

COPY . ./
RUN dotnet publish SyncEngine.Worker.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "SyncEngine.Worker.dll"]`