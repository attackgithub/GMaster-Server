FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["App/App.csproj", "GMaster/"]
RUN dotnet restore "GMaster/App.csproj"
COPY . .
WORKDIR "/src/GMaster"
RUN dotnet build "App.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "App.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GMaster.dll"]