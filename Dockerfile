FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env
WORKDIR /app
COPY src/ .
WORKDIR /app/TrainRecord.Api

ENV HUSKY=0
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Api.dll"]