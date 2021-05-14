## 快捷部署到 linux 方案

### 说明

- 这里只使用没有多余的服务器来做CI/CD, 在服务器上做 CI/CD需要很多内存, 一搬是单独服务器
- 如果想尝试可以在本地装 Jenkins 玩一下

### 准备工作

#### 安装 nginx 环境

- 自己百度

#### 安装 dotnet sdk

- [在 Linux 发行版上安装 .NET | Microsoft Docs](https://docs.microsoft.com/zh-cn/dotnet/core/install/linux)

#### 安装 supervisor 守护进程

- 自己百度

#### 上传凭证到服务器

- 打开cmd, 执行 ssh-keygen.exe, 一路回车
- scp ~/.ssh/id_rsa.pub root@81.70.44.26:~/.ssh
- 输入密码: ****
- ssh root:81.70.44.26
- 输入密码: ****
- cat ~/.ssh/id_rsa.pub >> ~/.ssh/authorized_keys
- /bin/systemctl restart sshd.service

### 环境配置

#### 目录设置

- 在根目录下新建 wwwroot

#### supervisor配置

- `cd /etc/supervisor/conf.d`

- `touch smart_prison_core.conf`

- ```shell
  #配置程序名称
  [program:DotnetSmartPrison]
  #运行程序的命令
  command=dotnet Dilon.Web.Entry.dll --urls="http://*:5000" 
  #命令执行的目录
  directory=/wwwroot/smart_prison_core
  #错误日志文件
  stderr_logfile=/var/log/smart_prison.err.log
  #输出日志文件
  stdout_logfile=/var/log/smart_prison.out.log 
  #进程环境变量
  environment=ASPNETCORE_ENVIRONMENT=Production
  #进程执行的用户身份
  user=root
  #程序是否自启动
  autostart=true
  #程序意外退出是否自动重启
  autorestart=true
  #启动时间间隔（秒）
  startsecs=5
  stopsignal=INT
  ```

- `supervisorctl shutdown  ` 

- `cd ..`

- `supervisor reload -c ./supervisord.conf`

#### nginx 配置

- `cd /etc/nginx/conf.d`

- `touch smart_prison_core.conf `

- ```
  server {
          listen        8001;
          location / {
              proxy_pass         http://localhost:5000;
              proxy_http_version 1.1;
              proxy_set_header   Upgrade $http_upgrade;
              proxy_set_header   Connection keep-alive;
              proxy_set_header   Host $host;
              proxy_cache_bypass $http_upgrade;
          }
  }
  ```

- `touch smart_prison_vue.conf`

- ```
  server{
      listen        9001;
  
      location / {
          root   /wwwroot/smart_prison_vue;
          index  index.html index.htm;
          try_files $uri $uri/ /index.html;
      }
  
      location /api/ {
          rewrite ^/api/(.*)$ /$1 break;
          proxy_pass http://127.0.0.1:5000;
      }
  	
      location /hubs/ {
          proxy_pass http://127.0.0.1:5000;
          proxy_http_version 1.1;
          proxy_set_header   Upgrade $http_upgrade;
          proxy_set_header   Connection keep-alive;
          proxy_set_header   Host $host;
          proxy_cache_bypass $http_upgrade;
      }
  	
      error_page  404              /404.html;
  
      error_page   500 502 503 504  /50x.html;
      location = /50x.html {
          root   /usr/share/nginx/html;
      }
  }
  ```
  
- `nginx -s reload`

### 执行脚本打包到服务器

#### 前端

- 编辑 build-vue.ps1, 需要修改你服务器的地址

- ```powershell
  # 定义服务器地址
  $remoteIp = "81.70.44.26"
  ```

- 其他根据自己的东西修改对应的变量

- ```powershell
  # supervisor 服务名称, 这个对应上边 smart_prison_core.conf 里的名称
  $supervisorServername = "DotnetSmartPrison"
  
  # 定义路径
  $buildFolder = (Get-Item -Path "./" -Verbose).FullName
  $coreFolder = Join-Path $buildFolder "../backend"
  $vueFolder = Join-Path $buildFolder "../frontend"
  $outputFolder = Join-Path $buildFolder "../outputs"
  ```

- 右击 build-vue.ps1 文件,  点击使用 powershell 运行

- 完整文件

  ```powershell
  # 定义服务器地址
  $remoteIp = "81.70.44.26"
  
  # 定义路径
  $buildFolder = (Get-Item -Path "./" -Verbose).FullName
  $vueFolder = Join-Path $buildFolder "../frontend"
  $outputFolder = Join-Path $buildFolder "../outputs"
  
  ## 清空本地历史
  Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
  New-Item -Path $outputFolder -ItemType Directory
  
  ## 发布前端
  
  ### 还原&打包
  Set-Location $vueFolder
  yarn
  yarn build
  
  ### 推送到服务器
  Set-Location $outputFolder
  ssh root@$remoteIp "rm -rf /wwwroot/smart_prison_vue; exit"
  scp -r (Join-Path $outputFolder "smart_prison_vue") root@$remoteIp:/wwwroot
  ```

  

#### 后端

- build-core.ps1

- 修改 supervisor 服务名称, 这个对应上边 smart_prison_core.conf 里的名称

- ```powershell
  # supervisor 服务名称, 这个对应上边 smart_prison_core.conf 里的名称
  $supervisorServername = "DotnetSmartPrison"
  ```

- 完整文件

  ```powershell
  # 定义服务器地址
  $remoteIp = "81.70.44.26"
  
  # supervisor 服务名称
  $supervisorServername = "DotnetSmartPrison"
  
  # 定义路径
  $buildFolder = (Get-Item -Path "./" -Verbose).FullName
  $coreFolder = Join-Path $buildFolder "../backend"
  $outputFolder = Join-Path $buildFolder "../outputs"
  
  ## 清空本地历史
  Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
  New-Item -Path $outputFolder -ItemType Directory
  
  ## 发布后端
  
  ### 还原&打包
  Set-Location $coreFolder
  dotnet restore
  dotnet publish --no-restore --output (Join-Path $outputFolder "smart_prison_core") --configuration Release 
  
  ### 推送到服务器
  Set-Location $outputFolder
  ssh root@$remoteIp "rm -rf /wwwroot/smart_prison_core; exit"
  scp -r (Join-Path $outputFolder "smart_prison_core") root@$remoteIp:/wwwroot
  
  ### dotnet 命令运行
  # ssh root@$remoteIp "cd /wwwroot/smart_prison_core; dotnet Dilon.Web.Entry.dll --urls http://*:5000; exit"
  
  ### 如果是用 supervisor 守护进程的需要使用
  ssh root@$remoteIp "sudo supervisorctl restart $supervisorServername; exit"
  ```

  

#### 合并打包

- build-all.ps1

- 完整文件

  ```powershell
  # 定义服务器地址
  $remoteIp = "81.70.44.26"
  
  # supervisor 服务名称
  $supervisorServername = "DotnetSmartPrison"
  
  # 定义路径
  $buildFolder = (Get-Item -Path "./" -Verbose).FullName
  $coreFolder = Join-Path $buildFolder "../backend"
  $vueFolder = Join-Path $buildFolder "../frontend"
  $outputFolder = Join-Path $buildFolder "../outputs"
  
  ## 清空本地历史
  Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
  New-Item -Path $outputFolder -ItemType Directory
  
  ## 发布前端
  
  ### 还原&打包
  Set-Location $vueFolder
  yarn
  yarn build
  
  ### 推送到服务器
  Set-Location $outputFolder
  ssh root@$remoteIp "rm -rf /wwwroot/smart_prison_vue; exit"
  scp -r (Join-Path $outputFolder "smart_prison_vue") root@$remoteIp:/wwwroot
  
  ## 发布后端
  
  ### 还原&打包
  Set-Location $coreFolder
  dotnet restore
  dotnet publish --no-restore --output (Join-Path $outputFolder "smart_prison_core") --configuration Release 
  
  ### 推送到服务器
  Set-Location $outputFolder
  ssh root@$remoteIp "rm -rf /wwwroot/smart_prison_core; exit"
  scp -r (Join-Path $outputFolder "smart_prison_core") root@$remoteIp:/wwwroot
  
  ### dotnet 命令运行
  # ssh root@$remoteIp "cd /wwwroot/smart_prison_core; dotnet Dilon.Web.Entry.dll --urls http://*:5000; exit"
  
  ### 如果是用 supervisor 守护进程的需要使用
  ssh root@$remoteIp "sudo supervisorctl restart $supervisorServername; exit"
  ```

  

