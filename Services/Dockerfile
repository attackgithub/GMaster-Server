FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Services/Services.csproj", "Services/"]
RUN dotnet restore "Services/Services.csproj"
COPY . .
WORKDIR "/src/Services"
RUN dotnet build "Services.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Services.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Services.dll"]