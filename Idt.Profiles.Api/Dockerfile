FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Idt.Profiles.Api/Idt.Profiles.Api.csproj", "Idt.Profiles.Api/"]
COPY ["Idt.Profiles.Shared/Idt.Profiles.Shared.csproj", "Idt.Profiles.Shared/"]
COPY ["Idt.Profiles.Services/Idt.Profiles.Services.csproj", "Idt.Profiles.Services/"]
COPY ["Idt.Profiles.Persistence/Idt.Profiles.Persistence.csproj", "Idt.Profiles.Persistence/"]
COPY ["Idt.Profiles.Dto/Idt.Profiles.Dto.csproj", "Idt.Profiles.Dto/"]
RUN dotnet restore "Idt.Profiles.Api/Idt.Profiles.Api.csproj"
COPY . .
WORKDIR "/src/Idt.Profiles.Api"
RUN dotnet build "Idt.Profiles.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Idt.Profiles.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Idt.Profiles.Api.dll"]
