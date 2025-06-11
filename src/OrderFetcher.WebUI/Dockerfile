# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY OrderFetcher.sln ./
COPY OrderFetcher.WebUI/*.csproj OrderFetcher.WebUI/
COPY OrderFetcher.Application/*.csproj OrderFetcher.Application/
COPY OrderFetcher.Domain/*.csproj OrderFetcher.Domain/
COPY OrderFetcher.Infrastructure/*.csproj OrderFetcher.Infrastructure/

COPY OrderFetcher.Infrastructure/Orders /src/OrderFetcher.Infrastructure/Orders

RUN dotnet restore OrderFetcher.WebUI/OrderFetcher.WebUI.csproj

COPY . .

RUN dotnet publish OrderFetcher.WebUI/OrderFetcher.WebUI.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

RUN mkdir -p /app/Orders /app/errors /app/processed

COPY OrderFetcher.Infrastructure/Orders/*.eml /app/Orders/

EXPOSE 80

ENTRYPOINT ["dotnet", "OrderFetcher.WebUI.dll"]