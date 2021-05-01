# 定义服务器地址
$remoteIp = "81.70.44.26"

# 定义路径
$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$vueFolder = Join-Path $buildFolder "../frontend"
$coreFolder = Join-Path $buildFolder "../backend"
$entryFolder = Join-Path $coreFolder "Dilon.Web.Entry"
$outputFolder = Join-Path $buildFolder "../outputs"

## 清空本地历史
Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
New-Item -Path $outputFolder -ItemType Directory

## 发布后端

### 还原
Set-Location $coreFolder
dotnet restore

### 打包
Set-Location $entryFolder
dotnet publish --output (Join-Path $outputFolder "core") --configuration Release
Copy-Item ("Dockerfile") (Join-Path $outputFolder "core")
Copy-Item ("Dilon.db") (Join-Path $outputFolder "core")

### 拷贝docker-compose
Set-Location $buildFolder
Copy-Item ("./core/*.yml") (Join-Path $outputFolder "core")

## 发布前端

### 还原&打包
Set-Location $vueFolder
yarn
yarn build
Copy-Item (Join-Path $vueFolder "dist") (Join-Path $outputFolder "vue/") -Recurse
Copy-Item (Join-Path $vueFolder "Dockerfile") (Join-Path $outputFolder "vue")

### 拷贝docker-compose
Set-Location $buildFolder
Copy-Item ("./vue/*.*") (Join-Path $outputFolder "vue")

### 推送到服务器
Set-Location $outputFolder
ssh root@81.70.44.26 "rm -rf /wwwroot/dilon; exit"
scp -r $outputFolder root@81.70.44.26:/wwwroot
ssh root@81.70.44.26 "cd /wwwroot; mv outputs dilon; exit"

ssh root@81.70.44.26 "cd /wwwroot/dilon/core; docker-compose down -v; docker rmi dilon.core; docker-compose build; docker-compose up -d; exit"
ssh root@81.70.44.26 "cd /wwwroot/dilon/vue; docker-compose down -v; docker rmi dilon.vue; docker-compose build; docker-compose up -d; exit"

Write-Host 'Press Any Key!' -NoNewline
$null = [Console]::ReadKey('?')