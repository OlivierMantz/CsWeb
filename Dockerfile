#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BackEnd/BackEnd.csproj", "BackEnd/"]
COPY ["BackEndTests/BackEndTests.csproj", "BackEndTests/"]
RUN dotnet restore "BackEnd/BackEnd.csproj"
RUN dotnet restore "BackEndTests/BackEndTests.csproj"
COPY . .
WORKDIR "/src/BackEnd"
RUN dotnet build "BackEnd.csproj" -c Release -o /app/build

# Use the build image to run tests
# Note: You can comment out this stage if you don't want to include test run in the image build process
# and prefer to run it in the CI/CD pipeline.
FROM build AS testrunner
WORKDIR /src/BackEndTests
CMD ["dotnet", "test", "--no-restore", "-c", "Release", "--logger", "trx"]

# Use the ASP.NET core runtime image for the final image which is production ready
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "BackEnd.dll"]

