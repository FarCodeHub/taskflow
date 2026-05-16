# This stage is used to build the application.
# SDK image includes tools required for restore, build, and publish.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy solution and project files first.
# This improves Docker layer caching because package restore only reruns
# when project files change.
COPY TaskFlow.sln ./

COPY src/BuildingBlocks/TaskFlow.SharedKernel/TaskFlow.SharedKernel.csproj src/BuildingBlocks/TaskFlow.SharedKernel/
COPY src/Services/Tasks/TaskFlow.Tasks.Domain/TaskFlow.Tasks.Domain.csproj src/Services/Tasks/TaskFlow.Tasks.Domain/
COPY src/Services/Tasks/TaskFlow.Tasks.Application/TaskFlow.Tasks.Application.csproj src/Services/Tasks/TaskFlow.Tasks.Application/
COPY src/Services/Tasks/TaskFlow.Tasks.Infrastructure/TaskFlow.Tasks.Infrastructure.csproj src/Services/Tasks/TaskFlow.Tasks.Infrastructure/
COPY src/Services/Tasks/TaskFlow.Tasks.Contracts/TaskFlow.Tasks.Contracts.csproj src/Services/Tasks/TaskFlow.Tasks.Contracts/
COPY src/Services/Tasks/TaskFlow.Tasks.Api/TaskFlow.Tasks.Api.csproj src/Services/Tasks/TaskFlow.Tasks.Api/

RUN dotnet restore src/Services/Tasks/TaskFlow.Tasks.Api/TaskFlow.Tasks.Api.csproj

# Copy the full source code after restore.
COPY . .

RUN dotnet publish src/Services/Tasks/TaskFlow.Tasks.Api/TaskFlow.Tasks.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# This stage is used to run the application.
# Runtime image is smaller than SDK image and better for production.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "TaskFlow.Tasks.Api.dll"]