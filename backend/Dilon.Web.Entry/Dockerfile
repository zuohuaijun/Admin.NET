FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY . .
ENTRYPOINT ["dotnet", "Dilon.Web.Entry.dll"]