FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Core/PetCare.Core.csproj", "PetCare.Core/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Shared/PetCare.Shared.csproj", "PetCare.Shared/"]

RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

COPY . .
WORKDIR "/src/PetCare.Api"

RUN dotnet build "PetCare.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PetCare.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetCare.Api.dll"]