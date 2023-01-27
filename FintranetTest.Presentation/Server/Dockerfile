#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FintranetTest.Presentation/Server/FintranetTest.Presentation.Server.csproj", "FintranetTest.Presentation/Server/"]
COPY ["FintranetTest.Persistence/FintranetTest.Persistence.csproj", "FintranetTest.Persistence/"]
COPY ["FintranetTest.Common/FintranetTest.Common.csproj", "FintranetTest.Common/"]
COPY ["FintranetTest.Domain/FintranetTest.Domain.csproj", "FintranetTest.Domain/"]
COPY ["Framework/Framework.csproj", "Framework/"]
COPY ["FintranetTest.Presentation/Client/FintranetTest.Presentation.Client.csproj", "FintranetTest.Presentation/Client/"]
RUN dotnet restore "FintranetTest.Presentation/Server/FintranetTest.Presentation.Server.csproj"
COPY . .
WORKDIR "/src/FintranetTest.Presentation/Server"
RUN dotnet build "FintranetTest.Presentation.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FintranetTest.Presentation.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
ENTRYPOINT ["dotnet", "FintranetTest.Presentation.Server.dll"]