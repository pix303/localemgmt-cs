FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Localemgmt.Api/Localemgmt.Api.csproj", "Localemgmt.Api/"]
COPY ["Localemgmt.Application/Localemgmt.Application.csproj", "Localemgmt.Application/"]
COPY ["Localemgmt.Contracts/Localemgmt.Contracts.csproj", "Localemgmt.Contracts/"]
COPY ["Localemgmt.Domain/Localemgmt.Domain.csproj", "Localemgmt.Domain/"]
COPY ["Localemgmt.Infrastructure/Localemgmt.Infrastructure.csproj", "Localemgmt.Infrastructure/"]
COPY ["Localemgmt-cs.sln", "./"]

RUN dotnet restore

COPY . .

WORKDIR "Localemgmt.Api"
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish



FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Localemgmt.Api.dll"]
