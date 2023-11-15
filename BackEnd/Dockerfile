# Use the ASP.NET Core runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the build stage to compile the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BackEnd/BackEnd.csproj", "./BackEnd/"]
RUN dotnet restore "BackEnd/BackEnd.csproj"
COPY . .
WORKDIR "/src/BackEnd"
RUN dotnet build "BackEnd.csproj" -c Release -o /app/build
RUN dotnet publish "BackEnd.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BackEnd.dll"]
