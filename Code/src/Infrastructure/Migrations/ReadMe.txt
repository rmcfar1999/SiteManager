Run from webui project (where context is created in program.cs/config file exists)

1) dotnet ef migrations add Initial --project ..\Infrastructure\Infrastructure.csproj
2) dotnet ef database update --project ..\Infrastructure\Infrastructure.csproj

