# ── Stage 1: build ──────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
 
COPY *.csproj ./
RUN dotnet restore
 
COPY . .
RUN dotnet publish -c Release -o /app/publish
 
# ── Stage 2: runtime ─────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
 
# Copy only the built files from the build stage
COPY --from=build /app/publish .
 
EXPOSE 5000
 
ENTRYPOINT ["dotnet", "PhoneNumberRegister.dll"]