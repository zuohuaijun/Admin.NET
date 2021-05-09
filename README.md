
<div align="center"><h1 align="center">Admin.NET</a></h1></div>
<div align="center"><h3 align="center">前后端分离架构，开箱即用，紧随前沿技术</h3></div>

<div align="center">

[![star](https://gitee.com/zuohuaijun/Admin.NET/badge/star.svg?theme=dark)](https://gitee.com/zuohuaijun/Admin.NET/stargazers)
[![fork](https://gitee.com/zuohuaijun/Admin.NET/badge/fork.svg?theme=dark)](https://gitee.com/zuohuaijun/Admin.NET/members)
[![star](https://gitee.com/dotnetchina/Furion/badge/star.svg?theme=gvp)](https://gitee.com/dotnetchina/Furion/stargazers)
[![star](https://gitee.com/xiaonuobase/snowy/badge/star.svg?theme=gray)](https://gitee.com/xiaonuobase/snowy/stargazers)
[![GitHub license](https://img.shields.io/badge/license-Apache2-yellow)](https://gitee.com/dotnetchina/Furion/blob/master/LICENSE)

</div>

### 🍟 概述

* 基于.NET 5实现的通用权限管理平台（RBAC模式）。整合最新技术高效快速开发，前后端分离模式，开箱即用。
* 后台基于Furion框架，前端基于小诺Antd Vue框架。集EFCore、多租户、分库读写分离、缓存、数据校验、鉴权、动态API、gRPC等众多黑科技于一身。
* 模块化架构设计，层次清晰，业务层推荐写到单独模块，框架升级不影响业务!
* 核心模块包括：用户、角色、职位、组织机构、菜单、字典、日志、多应用管理、文件管理、定时任务等功能。
* 代码量少、上手简单、功能强大、易扩展，轻松开发从现在开始！

```
如果对您有帮助，您可以点右上角 💘Star💘收藏一下 ，获取第一时间更新，谢谢！！！
```

## 🍁 框架拓展包

|                                                                 包类型                                                                             | 名称                             |                                                                                 版本                                                                                                        | 描述                   |
| :------------------------------------------------------------------------------------------------------------------------------------------------: | -------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | ---------------------- |
|       [![nuget](https://shields.io/badge/-Nuget-yellow?cacheSeconds=604800)](https://www.nuget.org/packages/Furion.Extras.Admin.NET/)              | Furion.Extras.Admin.NET          |              [![nuget](https://img.shields.io/nuget/v/Furion.Admin.NET.Template.App.svg?cacheSeconds=10800)](https://www.nuget.org/packages/Furion.Extras.Admin.NET/)                          | Admin.NET 核心包       |

```
可自行按照 Furion 框架脚手架初始化工程，然后引用此包即可，其他层配置见源代码。
```

## 🍀 框架脚手架

|                                                                 模板类型                                                                           | 名称                             |                                                                                 版本                                                                                                      | 描述                   |
| :------------------------------------------------------------------------------------------------------------------------------------------------: | -------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | ---------------------- |
|       [![nuget](https://shields.io/badge/-Nuget-yellow?cacheSeconds=604800)](https://www.nuget.org/packages/Furion.Admin.NET.Template.App/)        | Furion.Admin.NET.Template.App    |              [![nuget](https://img.shields.io/nuget/v/Furion.Admin.NET.Template.App.svg?cacheSeconds=10800)](https://www.nuget.org/packages/Furion.Admin.NET.Template.App/)              | Admin.NET 框架模板     |

```
打开 CMD 或 Powershell 执行dotnet命令

1、安装脚手架
dotnet new --install Furion.Admin.NET.Template.App

2、更新脚手架
dotnet new --install Furion.Admin.NET.Template.App

3、使用脚手架（生成之后推荐将所有的 nuget 包更新到最新版本）
dotnet new Admin.NET -n 你的项目名称

其实安装之后可以直接在VS里面进行可视化及创建工程
```

### 😎 衍生版本

【Admin.NET】隆重推出SqlSugar版本

- 👉 SqlSugar版本：[https://gitee.com/zhengguojing/admin-net-sqlsugar](https://gitee.com/zhengguojing/admin-net-sqlsugar)

`如果集成其他ORM，请参照各自操作使用说明。系统默认EFCore不会处理其他ORM实体等，请自行处理。`

### 👑 多租户简介

框架目前采用基于共享数据库TenantId的方式实现，后期可无缝迁移转换到基于多库或者Schema模式。

* 平台超管对租户进行增删改查操作，对各租户进行权限（菜单）的分配，租户管理员密码默认123456
* 租户管理员根据平台分配的权限再对本租户内用户进一步权限划分
* 针对新开发的业务功能，平台超管可以针对性分配给各租户（比如某租户购买后才有此功能菜单等） 

### 🥞 更新日志

更新日志 [点击查看](https://gitee.com/zuohuaijun/Admin.NET/commits/master)

### 🍿 在线体验

- 体验地址：[http://39.107.123.76:82/](http://39.107.123.76:82/)
- 开发者租户：用户名：superAdmin，密码：123456
- 公司1租户： 公司1租户管理员（用户名：zuohuaijun@163.com 密码：123456），公司1租户普通用户（用户名：dilon@163.com 密码：123456）           

### 🍄 快速启动

需要安装：VS2019（最新版）、npm或yarn（最新版）

* 启动后台：打开backend/Admin.NET.sln解决方案，直接运行（F5）即可启动（数据库默认SQLite）
* 启动前端：VSCode或HBuilder，打开frontend文件夹，进行依赖下载，运行npm install或yarn命令，再运行npm run serve或 yarn run serve
* 浏览器访问：`http://localhost:81` （默认前端端口为：81，后台端口为：5566）
<table>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/f1.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/f0.png"/></td>
    </tr>
</table>

### 🏀 分层说明
```
├─Admin.NET.Application             ->业务应用层，在此写您具体业务代码🌻🌻🌻
├─Admin.NET.Core                    ->框架核心层，后期准备做成NuGet包直接引用即可
├─Admin.NET.Database.Migrations     ->架构维护层，主要存放迁移中间文件
├─Admin.NET.EntityFramework.Core    ->EF Core配置层，主要配置数据库及相关
├─Admin.NET.Web.Core                ->Web核心层，主要是服务注册及鉴权
├─Admin.NET.Web.Entry               ->Web入口层/启动层，可任意更换
注：建议自己的业务代码直接写在【Admin.NET.Application】层里面，包括实体与服务等，或者单独新建个业务应用工程，进行模块化开发。
😛其他层尽量不要管，可随框架升级而无缝升级。
```

### 📖 帮助文档

👉后台文档：
* Furion后台框架文档 [https://dotnetchina.gitee.io/furion/docs/source](https://dotnetchina.gitee.io/furion/docs/source)

👉前端文档：
* 小诺前端业务文档 [https://doc.xiaonuo.vip/snowy_vue/bizs/](https://doc.xiaonuo.vip/snowy_vue/bizs/)

1. Ant Design Pro of Vue 使用文档 [https://pro.antdv.com/docs/getting-started](https://pro.antdv.com/docs/getting-started)
2. Ant Design of Vue 组件文档 [https://www.antdv.com/docs/vue/getting-started-cn/](https://www.antdv.com/docs/vue/getting-started-cn/)
3. Vue 开发文档 [https://cn.vuejs.org/v2/guide/](https://cn.vuejs.org/v2/guide/)

👉快捷部署到 linux 文档：

- [Admin.NET 快捷部署到 linux 方案 | Wynnyo Blog](http://wynnyo.com/archives/publish-linux)
- [本地 md文件](./build/readme.md)

👉代码生成器使用教程：

- [本地 md文件](./doc/代码生成器使用.md)

👉fork项目后该这样做后续开发：

- [本地 md文件](./doc/fork项目后该这样做后续开发.md)

👉关于signalr使用：

-  [wynnyo/vue-signalr: Signalr client for vue js (github.com)](https://github.com/wynnyo/vue-signalr)

😎通读以上文档，您就可以玩转本项目了（其实您已经是高手了）。项目使用上的问题，文档中基本都可以找到答案。


### 🍎 效果图

<table>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/1.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/2.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/3.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/4.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/5.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/6.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/7.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/8.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/9.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/10.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/11.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/12.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/13.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/14.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/15.png"/></td>
    </tr>
</table>

### 🍖 详细功能

1. 主控面板、控制台页面，可进行工作台，分析页，统计等功能的展示。
2. 用户管理、对企业用户和系统管理员用户的维护，可绑定用户职务，机构，角色，数据权限等。
3. 应用管理、通过应用来控制不同维度的菜单展示。
4. 机构管理、公司组织架构维护，支持多层级结构的树形结构。
5. 职位管理、用户职务管理，职务可作为用户的一个标签，职务目前没有和权限等其他功能挂钩。
6. 菜单管理、菜单目录，菜单，和按钮的维护是权限控制的基本单位。
7. 角色管理、角色绑定菜单后，可限制相关角色的人员登录系统的功能范围。角色也可以绑定数据授权范围。
8. 字典管理、系统内各种枚举类型的维护。
9. 访问日志、用户的登录和退出日志的查看和管理。
10. 操作日志、用户的操作业务的日志的查看和管理。
11. 服务监控、服务器的运行状态，CPU、内存、网络等信息数据的查看。
12. 在线用户、当前系统在线用户的查看。
13. 公告管理、系统的公告的管理。
14. 文件管理、文件的上传下载查看等操作，文件可使用本地存储，阿里云oss，腾讯cos接入，支持拓展。
15. 定时任务、定时任务的维护，通过cron表达式控制任务的执行频率。
16. 系统配置、系统运行的参数的维护，参数的配置与系统运行机制息息相关。
17. 邮件发送、发送邮件功能。
18. 短信发送、短信发送功能，可使用阿里云sms，腾讯云sms，支持拓展。


### 💪 数据库操作

本框架ORM默认采用EF Core开发，加上拓展比如SqlSugar，理论上兼容并支持所有类型数据库。😜

【MySQL】

1. Admin.NET.EntityFramework.Core 项目安装 ``` Pomelo.EntityFrameworkCore.MySql，Nuget 需安装 5.0 版本 (支持 MySql 5.x +)  MySql.EntityFrameworkCore：支持 (MySql 8.x +) ```
2. DefaultDbContext.cs 指定 DbProvider , ```[AppDbContext("DefaultConnection", DbProvider.MySql)]```
3. dbsettings.json 配置 "DefaultConnection": ```"Data Source=localhost;Database=Admin.NET;User ID=root;Password=000000;pooling=true;port=3306;sslmode=none;CharSet=utf8;"```
4. 打开程序包管理器控制台，默认项目Admin.NET.Database.Migrations 执行命令:```Add-Migration Init和update-database```

【SQLServer】

1. Admin.NET.EntityFramework.Core 项目安装 ``` Microsoft.EntityFrameworkCore.SqlServer ```
2. DefaultDbContext.cs 指定 DbProvider , ```[AppDbContext("DefaultConnection", DbProvider.SqlServer)]```
3. dbsettings.json 配置 "DefaultConnection": ```"Server=localhost;Database=Admin.NET;User=sa;Password=000000;MultipleActiveResultSets=True;"```
4. 打开程序包管理器控制台，默认项目Admin.NET.Database.Migrations 执行命令:```Add-Migration Init 和 update-database```

```
提示：其他类型数据库依次类推，首先添加EF的Core版包，然后指定数据库类型，修改数据库连接字符串，执行EF迁移命令即可。
```

【EF批量操作】

使用 Zack.EFCore.Batch [https://hub.fastgit.org/yangzhongke/Zack.EFCore.Batch](https://hub.fastgit.org/yangzhongke/Zack.EFCore.Batch) 安装对应包即可
1. MSSQL：Zack.EFCore.Batch.MSSQL
2. MySql：Zack.EFCore.Batch.MySQL.Pomelo
3. Npgsql：Zack.EFCore.Batch.Npgsql
4. Oracle：Zack.EFCore.Batch.Oracle
5. Sqlite：Zack.EFCore.Batch.Sqlite

### ⚡ 近期计划

- [x] 集成多租户功能
- [x] 集成代码生成器
- [x] 集成导入导出
- [x] 在线用户及黑名单
- [ ] 邮件发送
- [ ] 短信发送
- [ ] 集成微信开发
- [ ] 实现电商应用插件
- [ ] 集成工作流

### 🥦 补充说明

* 基于.NET 5平台 Furion 开发框架与小诺Antd Vue版本相结合，实时跟随基架升级而升级！
* 持续集百家所长，完善与丰富本框架基础设施，为.NET生态增加一种选择！
* 后期会推出基于此框架的相关应用场景案例，提供给大家使用！
* 有问题讨论的小伙伴可加群一起学习讨论。 QQ群【87333204】
<a target="_blank" href="https://qm.qq.com/cgi-bin/qm/qr?k=pN8R-P3pJaW9ILoOXwpRGN2wdCHWtUTE&jump_from=webapi"><img border="0" src="//pub.idqqimg.com/wpa/images/group.png" alt="Admin.NET" title="Admin.NET"></a>

## 🍻 贡献代码

`Admin.NET` 遵循 `Apache-2.0` 开源协议，欢迎大家提交 `PR` 或 `Issue`。

感谢每一位贡献代码的朋友。**感谢 [TLog 作者](https://gitee.com/bryan31) 提供的贡献者实时头像。**

[![Giteye chart](https://chart.giteye.net/gitee/zuohuaijun/Admin.NET/JRFF5WLM.png)](https://giteye.net/chart/JRFF5WLM)


### 💐 特别鸣谢
- 👉 Furion：  [https://dotnetchina.gitee.io/furion](https://dotnetchina.gitee.io/furion)
- 👉 xiaonuo：[https://gitee.com/xiaonuobase/snowy](https://gitee.com/xiaonuobase/snowy)
- 👉 k-form-design：[https://gitee.com/kcz66/k-form-design](https://gitee.com/kcz66/k-form-design)
- 👉 MiniExcel：[https://gitee.com/dotnetchina/MiniExcel](https://gitee.com/dotnetchina/MiniExcel)
- 👉 SqlSugar：[https://gitee.com/dotnetchina/SqlSugar](https://gitee.com/dotnetchina/SqlSugar)


如果对您有帮助，您可以点右上角 💘Star💘支持一下，这样我们才有持续下去的动力，谢谢！！！