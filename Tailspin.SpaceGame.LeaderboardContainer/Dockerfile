FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Tailspin.SpaceGame.LeaderboardContainer/Tailspin.SpaceGame.LeaderboardContainer.csproj", "Tailspin.SpaceGame.LeaderboardContainer/"]

RUN dotnet restore "Tailspin.SpaceGame.LeaderboardContainer/Tailspin.SpaceGame.LeaderboardContainer.csproj"
COPY . .
WORKDIR "/src/Tailspin.SpaceGame.LeaderboardContainer"
RUN dotnet build "Tailspin.SpaceGame.LeaderboardContainer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tailspin.SpaceGame.LeaderboardContainer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tailspin.SpaceGame.LeaderboardContainer.dll"]