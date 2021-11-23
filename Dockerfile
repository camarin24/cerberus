#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Intecgra.Cerberus.Api/Intecgra.Cerberus.Api.csproj", "Intecgra.Cerberus.Api/"]
RUN dotnet restore "Intecgra.Cerberus.Api/Intecgra.Cerberus.Api.csproj"
COPY . .
WORKDIR "/src/Intecgra.Cerberus.Api"
RUN dotnet build "Intecgra.Cerberus.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Intecgra.Cerberus.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Intecgra.Cerberus.Api.dll"]
