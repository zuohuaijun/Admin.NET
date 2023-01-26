# 启动前准备

*   安装docker,docker-compose 环境

*   使用vs编译后台Admin.NET复制发布文件到docker/app/&#x20;

:::warning
注意Database.json文件不需要修改，不要覆盖掉了
:::

*   如果服务器有node环境使用build.sh编译前端文件到 [docker/nginx/dist]()目录。或者将编译结果放进 [docker/nginx/dist]()目录，

:::warning
注意.env.production 文件配置 接口地址配置为 VITE\_API\_URL = '/prod-api'
:::

***

# 启动命令

`docker-compose up -d`

# 访问入口

***<http://ip:9100>***
