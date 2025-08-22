FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/STC.Auth.WebAPI/STC.Auth.WebAPI.csproj", "src/Presentation/STC.Auth.WebAPI/"]
COPY ["src/Infrastructure/STC.Auth.Infrastructure/STC.Auth.Infrastructure.csproj", "src/Infrastructure/STC.Auth.Infrastructure/"]
COPY ["src/Core/STC.Auth.Application/STC.Auth.Application.csproj", "src/Core/STC.Auth.Application/"]
COPY ["STC.Shared/src/Utilities/STC.Shared.Utilities.Response/STC.Shared.Utilities.Response.csproj", "STC.Shared/src/Utilities/STC.Shared.Utilities.Response/"]
COPY ["STC.Shared/src/Utilities/STC.Shared.Utilities.Pagination/STC.Shared.Utilities.Pagination.csproj", "STC.Shared/src/Utilities/STC.Shared.Utilities.Pagination/"]
COPY ["src/Core/STC.Auth.Domain/STC.Auth.Domain.csproj", "src/Core/STC.Auth.Domain/"]
COPY ["STC.Shared/src/Utilities/STC.Shared.Logger/STC.Shared.Logger.csproj", "STC.Shared/src/Utilities/STC.Shared.Logger/"]
COPY ["STC.Shared/src/Utilities/STC.Shared.JwtAuthentication/STC.Shared.JwtAuthentication.csproj", "STC.Shared/src/Utilities/STC.Shared.JwtAuthentication/"]
COPY ["src/Infrastructure/STC.Auth.Persistence.EfCore/STC.Auth.Persistence.EfCore.csproj", "src/Infrastructure/STC.Auth.Persistence.EfCore/"]
RUN dotnet restore "src/Presentation/STC.Auth.WebAPI/STC.Auth.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Presentation/STC.Auth.WebAPI"
RUN dotnet build "STC.Auth.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "STC.Auth.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "STC.Auth.WebAPI.dll"]
