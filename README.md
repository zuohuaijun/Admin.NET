<div align="center">
    <p align="center">
        <img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/frontend/public/logo.png" height="50" alt="logo"/>
    </p>
</div>

<div align="center"><h1 align="center">Admin.NET</h1></div>
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
* 前端基于小诺Vue（antd）框架，整体RBAC基础数据结构+API接口风格采用小诺vue版本模式。
* 后台基于Furion框架，EFCore、多租户、分库读写分离、缓存、数据校验、鉴权、动态API、gRPC等众多黑科技集一身。
* 模块化架构设计，层次清晰，业务层推荐写到单独模块，框架升级不影响业务!
* 核心模块包括：用户、角色、职位、组织机构、菜单、字典、日志、多应用管理、文件管理、定时任务等功能。
* 代码量少、通俗易懂、功能强大、易扩展，轻松开发从现在开始！

```
如果对您有帮助，您可以点右上角 “Star” 收藏一下 ，获取第一时间更新，谢谢！
```

### 😎 衍生版本

【Admin.NET】隆重推出SqlSugar版本

- 👉 SqlSugar版本：[https://gitee.com/zhengguojing/admin-net-sqlsugar](https://gitee.com/zhengguojing/admin-net-sqlsugar)

`如果集成其他ORM，请参照各自操作使用说明。系统默认EFCore不会处理其他ORM实体等，请自行处理。`

### 🥞 更新日志

更新日志 [点击查看](https://gitee.com/zuohuaijun/Admin.NET/commits/master)

### ⚡ 近期计划

- [x] 集成多租户功能
- [x] 集成代码生成器
- [x] 集成导入导出
- [ ] 在线用户及黑名单
- [ ] 邮件发送
- [ ] 短信发送
- [ ] 集成微信开发
- [ ] 实现电商应用插件
- [ ] 集成工作流

### 🍿 在线体验

- 体验地址：[http://39.107.123.76:82/](http://39.107.123.76:82/)
- 开发者租户：用户名：superAdmin，密码：123456
- 公司1租户： 公司1租户管理员（用户名：admin1 密码：123456），公司1租户普通用户（用户名：zuohuaijun1 密码：123456）
           

### 🍄 快速启动

需要安装：VS2019（最新版）、npm或yarn（最新版）

* 启动后台：打开backend/Dilon.sln解决方案，直接运行（F5）即可启动（数据库默认SQLite）
* 启动前端：打开frontend文件夹，进行依赖下载，运行npm install或yarn命令，再运行npm run serve或 yarn run serve
* 浏览器访问：`http://localhost:81` （默认前端端口为：81，后台端口为：5566）
<table>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/f1.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/f0.png"/></td>
    </tr>
</table>

### 📖 帮助文档

👉后台文档：
* Furion后台框架文档 [https://dotnetchina.gitee.io/furion/docs/source](https://dotnetchina.gitee.io/furion/docs/source)

👉前端文档：
* 小诺前端业务文档 [https://doc.xiaonuo.vip/snowy_vue/bizs/](https://doc.xiaonuo.vip/snowy_vue/bizs/)
* Ant Design Pro of Vue 使用文档 [https://pro.antdv.com/docs/getting-started](https://pro.antdv.com/docs/getting-started)
* Ant Design of Vue 组件文档 [https://www.antdv.com/docs/vue/getting-started-cn/](https://www.antdv.com/docs/vue/getting-started-cn/)
* Vue 开发文档 [https://cn.vuejs.org/v2/guide/](https://cn.vuejs.org/v2/guide/)

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


### 👀 数据库切换

【MySQL】

1. Dilon.EntityFramework.Core 项目安装 ``` Pomelo.EntityFrameworkCore.MySql，Nuget 需勾选预览版且安装 .Alpha.2 版本 (支持 MySql 5.x +)  MySql.EntityFrameworkCore：支持 (MySql 8.x +) ```
2. DefaultDbContext.cs 指定 DbProvider , ```[AppDbContext("DefaultConnection", DbProvider.MySql)]```
3. dbsettings.json 配置 "DefaultConnection": ```"Data Source=localhost;Database=Dilon;User ID=root;Password=000000;pooling=true;port=3306;sslmode=none;CharSet=utf8;"```
4. 打开程序包管理器控制台，默认项目Dilon.Database.Migrations 执行命令:```Add-Migration Init和update-database```

【SQLServer】

1. Dilon.EntityFramework.Core 项目安装 ``` Microsoft.EntityFrameworkCore.SqlServer ```
2. DefaultDbContext.cs 指定 DbProvider , ```[AppDbContext("DefaultConnection", DbProvider.SqlServer)]```
3. dbsettings.json 配置 "DefaultConnection": ```"Server=localhost;Database=Dilon;User=sa;Password=000000;MultipleActiveResultSets=True;"```
4. 打开程序包管理器控制台，默认项目Dilon.Database.Migrations 执行命令:```Add-Migration Init 和 update-database```

```
提示：其他类型数据库依次类推，首先添加EF的Core版包，然后指定数据库类型，修改数据库连接字符串，执行EF命令即可。
```

### 🥦 补充说明

* 基于.NET 5平台 Furion 开发框架与小诺 Vue 版本相结合，实时跟随基架升级而升级！
* 持续集百家所长，完善与丰富本框架基础设施，为.NET生态增加一种选择！
* 后期会推出基于此框架的相关应用场景案例，提供给大家使用！
* 有问题讨论的小伙伴可加群一起学习讨论。 QQ群【87333204】
<a target="_blank" href="https://qm.qq.com/cgi-bin/qm/qr?k=pN8R-P3pJaW9ILoOXwpRGN2wdCHWtUTE&jump_from=webapi"><img border="0" src="//pub.idqqimg.com/wpa/images/group.png" alt="Admin.NET" title="Admin.NET"></a>

### 💐 特别鸣谢
- 👉 Furion：  [https://dotnetchina.gitee.io/furion](https://dotnetchina.gitee.io/furion)
- 👉 xiaonuo：[https://gitee.com/xiaonuobase/snowy](https://gitee.com/xiaonuobase/snowy)
- 👉 k-form-design：[https://gitee.com/kcz66/k-form-design](https://gitee.com/kcz66/k-form-design)
- 👉 MiniExcel：[https://gitee.com/dotnetchina/MiniExcel](https://gitee.com/dotnetchina/MiniExcel)
- 👉 SqlSugar：[https://gitee.com/dotnetchina/SqlSugar](https://gitee.com/dotnetchina/SqlSugar)


如果对您有帮助，您可以点 "Star" 支持一下，这样才有持续下去的动力，谢谢！！！
