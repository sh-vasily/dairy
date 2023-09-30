FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dairy.UserInterface/Dairy.UserInterface.csproj", "Dairy.UserInterface/"]
COPY ["Dairy.Contracts/Dairy.Contracts.csproj", "Dairy.Contracts/"]
RUN dotnet restore "Dairy.UserInterface/Dairy.UserInterface.csproj"
COPY . .
WORKDIR "/src/Dairy.UserInterface"
RUN dotnet build "Dairy.UserInterface.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dairy.UserInterface.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY Dairy.UserInterface/nginx.conf /etc/nginx/nginx.conf
