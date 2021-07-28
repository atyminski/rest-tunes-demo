FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV PORT=80
EXPOSE $PORT

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./", "/src/"]
RUN dotnet restore "Gevlee.RestTunes.csproj"
RUN dotnet build "Gevlee.RestTunes.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Gevlee.RestTunes.csproj" -c Release -o /app
RUN curl https://github.com/lerocha/chinook-database/raw/434f13993d7dc33e37271d082fe9eff379ea7abb/ChinookDatabase/DataSources/Chinook_Sqlite_AutoIncrementPKs.sqlite -o /app/db.sqlite -sL

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN chmod 777 db.sqlite
CMD ASPNETCORE_URLS="http://*:$PORT" dotnet Gevlee.RestTunes.dll