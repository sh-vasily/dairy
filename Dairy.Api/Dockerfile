FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dairy.Api/Dairy.Api.csproj", "Dairy.Api/"]
COPY ["Dairy.Contracts/Dairy.Contracts.csproj", "Dairy.Contracts/"]
RUN dotnet restore "Dairy.Api/Dairy.Api.csproj"
COPY . .
WORKDIR "/src/Dairy.Api"
RUN dotnet build "Dairy.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dairy.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dairy.Api.dll"]
