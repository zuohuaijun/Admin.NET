# 启动前准备

*   安装docker,docker-compose 环境

*   使用vs编译后台Admin.NET复制发布文件到*docker/app/*

*   如果服务器有node环境使用build.sh编译前端文件到 *docker/nginx/dist*。或者将编译结果放进 *docker/nginx/dist*

# 注意事项

1.  *docker/app/Configuration/Database.json* 文件不需要修改，不要覆盖掉了
2.  *app/Configuration/App.json* 主要配置了api端口5050，如果你的端口也是这个可以覆盖
2.  *Web/.env.production* 文件配置接口地址配置为 VITE\_API\_URL = '/prod-api'
3.  nginx，mysql配置文件无需修改

***

# 启动命令

`docker-compose up -d`

# 访问入口

***<http://ip:9100>***
***<https://ip:9103>***
