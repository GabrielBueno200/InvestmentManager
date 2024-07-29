FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .
RUN dotnet restore ./Services/Negotiation/Api.Negotiation/Api.Negotiation.csproj

WORKDIR /app/Services/Negotiation/Api.Negotiation
RUN dotnet publish -c release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /dist
COPY --from=build /dist ./
ENTRYPOINT ["dotnet", "Api.Negotiation.dll"]