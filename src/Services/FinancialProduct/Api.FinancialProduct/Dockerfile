FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN mkdir /app
WORKDIR /app

COPY . .
RUN dotnet restore Api.FinancialProduct/*.csproj

WORKDIR /app/Api.FinancialProduct
RUN dotnet publish -c release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /dist
COPY --from=build /dist ./
ENTRYPOINT ["dotnet", "Api.FinancialProduct.dll"]