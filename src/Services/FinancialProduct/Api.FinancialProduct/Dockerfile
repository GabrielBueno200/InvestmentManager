FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .
RUN dotnet restore ./Services/FinancialProduct/Api.FinancialProduct/Api.FinancialProduct.csproj

WORKDIR /app/Services/FinancialProduct/Api.FinancialProduct
RUN dotnet publish -c release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /dist
COPY --from=build /dist ./
ENTRYPOINT ["dotnet", "Api.FinancialProduct.dll"]