﻿FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Afisha.NotificationService/Afisha.NotificationService.csproj", "src/Afisha.NotificationService/"]
COPY ["src/RabbitMQModels/RabbitMQModels.csproj", "src/RabbitMQModels/"]
RUN dotnet restore "src/Afisha.NotificationService/Afisha.NotificationService.csproj"
COPY . .
WORKDIR "/src/src/Afisha.NotificationService"
RUN dotnet build "Afisha.NotificationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Afisha.NotificationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Afisha.NotificationService.dll"]
