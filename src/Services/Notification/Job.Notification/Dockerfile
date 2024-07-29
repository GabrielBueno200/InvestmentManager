FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .
RUN dotnet restore ./Services/Notification/Job.Notification/Job.Notification.csproj

WORKDIR /app/Services/Notification/Job.Notification
RUN dotnet publish -c release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /dist
COPY --from=build /dist ./
ENTRYPOINT ["dotnet", "Job.Notification.dll"]