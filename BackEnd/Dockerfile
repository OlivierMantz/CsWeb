#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the SDK image to build and test the project.
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BackEnd/BackEnd.csproj", "./BackEnd/"]
COPY ["BackEndTests/BackEndTests.csproj", "./BackEndTests/"]
RUN dotnet restore "./BackEnd/BackEnd.csproj"
RUN dotnet restore "./BackEndTests/BackEndTests.csproj"
COPY . .
WORKDIR "/src/BackEnd"
RUN dotnet build "BackEnd.csproj" -c Release -o /app/build

# Use the build image to run tests
FROM build AS testrunner
WORKDIR /src/BackEndTests
CMD ["dotnet", "test", "--no-restore", "-c", "Release", "--logger", "trx"]

# Use the ASP.NET core runtime image for the final image which is production ready
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "BackEnd.dll"]
