FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN mkdir /app
WORKDIR /app

COPY . .
RUN dotnet restore Api.Auth/*.csproj

WORKDIR /app/Api.Auth
RUN dotnet publish -c release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /dist
COPY --from=build /dist ./
ENTRYPOINT ["dotnet", "Api.Auth.dll"]