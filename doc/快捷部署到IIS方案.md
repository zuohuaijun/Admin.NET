## 快捷部署到IIS方案

### 后端配置

#### 1. IIS配置

- 新建站点，配置端口和路径，路径为后台项目发布后的文件夹

![image](https://images.gitee.com/uploads/images/2021/0512/210840_3eccd2b2_1572418.png "IIS1.png")

- 将应用程序池CLR版设置为“无托管代码”

![image](https://images.gitee.com/uploads/images/2021/0512/210905_f633c45c_1572418.png "IIS2.png")

- 打开OK

![image](https://images.gitee.com/uploads/images/2021/0512/210955_8669e0b6_1572418.png "IIS3.png")


### 前端配置



#### 1. 准备工作

请先安装两个IIS模块，URL重写和代理需要用到

###### URL Rewrite

- 官网地址：https://www.iis.net/downloads/microsoft/url-rewrite

- 网盘地址：https://pan.baidu.com/s/1WikAe1Qxv_WIIthRrVZ6sg  提取码：7lbq

###### Application Request Routing

- 官网地址：https://www.iis.net/downloads/microsoft/application-request-routing

- 网盘地址：https://pan.baidu.com/s/1SSO0oJWguJ1xSSxXiL9jCg  提取码：ixqb 

#### 2. IIS配置

- 新建站点，配置端口和路径，路径为前台项目打包后的dist文件夹

![image](https://images.gitee.com/uploads/images/2021/0512/211127_a3b22049_1572418.png "IIS4.png")

- URL重写配置（手动配置，或者直接使用下方提供的web.confg文件内容）

![image](https://images.gitee.com/uploads/images/2021/0512/211146_2890f00d_1572418.png "IIS5.png")

![image](https://images.gitee.com/uploads/images/2021/0512/211158_bed29f77_1572418.png "IIS6.png")

![image](https://images.gitee.com/uploads/images/2021/0512/211211_960c34a0_1572418.png "IIS7.png")


```
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="api" enabled="true" patternSyntax="Wildcard" stopProcessing="true">
          <match url="*api/*" />
          <action type="Rewrite" url="http://localhost:5566/{R:2}" />
        </rule>
        <rule name="hubs" enabled="true" patternSyntax="Wildcard" stopProcessing="true">
          <match url="*hubs/*" />
          <action type="Rewrite" url="http://localhost:5566/hubs/{R:2}" />
        </rule>
        <rule name="index" stopProcessing="true">
          <match url="^((?!(api)).)*$" />
          <conditions>
                        <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
                        <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

- 配置代理

![image](https://images.gitee.com/uploads/images/2021/0512/211227_5491dbe2_1572418.png "IIS8.png")

![image](https://images.gitee.com/uploads/images/2021/0512/211238_65613c8e_1572418.png "IIS9.png")

![image](https://images.gitee.com/uploads/images/2021/0512/211250_d5d10f80_1572418.png "IIS10.png")

- 打开，登录完成OK

![image](https://images.gitee.com/uploads/images/2021/0512/211303_cc69fb93_1572418.png "IIS11.png")

![image](https://images.gitee.com/uploads/images/2021/0512/211313_9a7e6b84_1572418.png "IIS12.png")