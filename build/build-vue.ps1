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