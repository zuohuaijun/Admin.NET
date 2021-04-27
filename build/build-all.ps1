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