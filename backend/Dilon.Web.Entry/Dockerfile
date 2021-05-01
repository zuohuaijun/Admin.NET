FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY . .
RUN apt-get update -y && apt-get install -y libgdiplus && apt-get clean && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
ENTRYPOINT ["dotnet", "Dilon.Web.Entry.dll"]