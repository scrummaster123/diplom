FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Afisha.Web/Afisha.Web.csproj", "src/Afisha.Web/"]
COPY ["src/Afisha.Application/Afisha.Application.csproj", "src/Afisha.Application/"]
COPY ["src/Afisha.Domain/Afisha.Domain.csproj", "src/Afisha.Domain/"]
COPY ["src/Afisha.Infrastructure/Afisha.Infrastructure.csproj", "src/Afisha.Infrastructure/"]
COPY ["src/RabbitMQModels/RabbitMQModels.csproj", "src/RabbitMQModels/"]
RUN dotnet restore "src/Afisha.Web/Afisha.Web.csproj"
COPY . .
WORKDIR "/src/src/Afisha.Web"
RUN dotnet build "Afisha.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Production
RUN dotnet publish "Afisha.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Afisha.Web.dll"]
