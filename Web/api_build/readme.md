# 前端 API 请求代理生成

前端 API 请求代理，可以通过 swagger.json 生成 typescript-axios 客户端的方式生成，然后拷贝到 src/api-services 文件夹

## 手动生成

- 打开 https://editor.swagger.io/
- 拷贝 swagger.json 内容到网站左侧
- 选择顶部【Generate Client】-【typescript-axios】生成客户端并下载
- 将下载的 zip 内容拷贝到 src/api-services 文件夹中替换
  > 详情参照 https://furion.baiqian.ltd/docs/clientapi#563-%E7%94%9F%E6%88%90%E5%AE%A2%E6%88%B7%E7%AB%AF%E8%AF%B7%E6%B1%82%E4%BB%A3%E7%A0%81

## 脚本一键生成

Swagger Codegen 读取 swagger.json 生成 typescript-axios 客户端后，直接拷贝到 src/api-services 文件夹中

> Swagger Codegen 可以通过为任何 API 生成服务端代码和客户端代码的方式来简化 OpenAPI 的构建过程，因此，项目开发团队可以更好地关注 API 的实现和应用
> Github：https://github.com/swagger-api/swagger-codegen

### 环境准备

- 安装 Java 运行时，最低要求 Java 8

  - 可使用 Microsoft Build of OpenJDK
  - 下载地址：https://learn.microsoft.com/zh-cn/java/openjdk/download

- 设置 JAVA_HOME 环境变量
- 下载 Swagger Codegen

  - 3.0.41 下载地址：https://repo1.maven.org/maven2/io/swagger/codegen/v3/swagger-codegen-cli/3.0.41/swagger-codegen-cli-3.0.41.jar
  - 可自行下载其他更高版本
  - 下载后，将文件重命名为 `swagger-codegen-cli.jar` 并放到当前目录下

### 如何使用

- 启动 API 端服务，确保 http://localhost:5005/ 可以访问
- Windows：运行 `build.bat`
- Linux / Mac：运行`./build.sh`

> http://localhost:5005/ 为默认的 API 地址，如果要连接其他地址，请自行调整对应脚本
