# Runtime leve
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY . .
RUN dotnet restore "GoodHamburgerProject/GoodHamburgerProject.csproj"
RUN dotnet publish "GoodHamburgerProject/GoodHamburgerProject.csproj" -c Release -o /app/publish

# Final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "GoodHamburgerProject.dll"]