FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5005

COPY . .

# 设置语言/区域设置环境变量
ENV LANG zh-Hans

# 使用阿里云的镜像源进行更新
RUN sed -i 's/deb.debian.org/mirrors.aliyun.com/' /etc/apt/sources.list

# 更新包管理器并安装free命令
RUN apt-get update && apt-get install -y procps

ENTRYPOINT ["dotnet", "Admin.NET.Web.Entry.dll"]