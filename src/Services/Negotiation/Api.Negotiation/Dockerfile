FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN mkdir /app
WORKDIR /app
RUN echo "Diretório atual: $(pwd)"
COPY . .
RUN dotnet restore Api.Negotiation/*.csproj

WORKDIR /app/Api.Negotiation
RUN dotnet publish -c release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /dist
COPY --from=build /dist ./
ENTRYPOINT ["dotnet", "Api.Negotiation.dll"]