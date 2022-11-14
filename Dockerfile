FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 82

ENV ASPNETCORE_URLS=http://+:82

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["C_Solution.csproj", "./"]
RUN dotnet restore "C_Solution.csproj"
COPY . .
RUN dotnet publish "C_Solution.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "C_Solution.dll"]
