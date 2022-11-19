FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Titanic.Api.csproj", "./"]
RUN dotnet restore "Titanic.Api.csproj"
COPY . .
# RUN dotnet build "Titanic.Api.csproj" -c Release -o /app/build

# FROM build AS publish
RUN dotnet publish "Titanic.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Titanic.Api.dll"]
