FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY *.sln .
COPY Architect.Common/*.csproj ./Architect.Common/
COPY Architect.Migrator/*.csproj ./Architect.Migrator/
COPY Architect.Web/*.csproj ./Architect.Web/
COPY Architect.Tests/*.csproj ./Architect.Tests/
COPY NuGet.config .
RUN dotnet restore

COPY Architect.Common/* ./Architect.Common/
RUN ls -al ./Architect.Common
COPY Architect.Migrator/* ./Architect.Migrator/
COPY Architect.Web/* ./Architect.Web/
WORKDIR /source/Architect.Web
RUN dotnet publish -c release -o /build --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /build ./
COPY Architect.Migrator/Data/people.csv ./Data/people.csv
ENTRYPOINT ["dotnet", "Architect.Web.dll"]
