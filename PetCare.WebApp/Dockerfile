FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PetCare.WebApp/PetCare.WebApp.csproj", "PetCare.WebApp/"]
COPY ["PetCare.Shared/PetCare.Shared.csproj", "PetCare.Shared/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]

RUN dotnet restore "PetCare.WebApp/PetCare.WebApp.csproj"

COPY . .
WORKDIR "/src/PetCare.WebApp"
RUN dotnet build "PetCare.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PetCare.WebApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetCare.WebApp.dll"]