FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OCP/OCP.API.csproj", "OCP/"]
RUN dotnet restore "OCP/OCP.API.csproj"

COPY . .
WORKDIR "/src/OCP.API"

RUN dotnet publish "OCP.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "OCP.API.dll"]
