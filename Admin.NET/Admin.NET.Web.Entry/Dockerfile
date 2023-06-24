FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5005

COPY . .
ENTRYPOINT ["dotnet", "Admin.NET.Web.Entry.dll"]