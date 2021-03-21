<div align="center">
    <p align="left">
        <img src="./_web/public/logo.png" height="50" alt="logo"/>
    </p>
</div>

<div align="center"><h1 align="center">Admin.NET</h1></div>
<div align="center"><h3 align="center">前后端分离架构，开箱即用，紧随前沿技术</h3></div>

### 🍟 概述

* 基于.NET 5实现的通用权限管理平台（RBAC权限模式）。整合最新技术高效快速开发，前后端分离模式，开箱即用。
* 后台基于Furion框架，默认EF Core并支持自定义ORM，数据源随意切换并带多租户功能，分库读写分离、缓存、数据校验、鉴权、动态API、Grpc等众多黑科技集一身。
前端使用AntDesign-Vue-Pro + Vue2.X开发，紧跟前沿技术，前端自带字典翻译。
* 核心模块包括：用户、角色、职位、组织机构、菜单、字典、日志、多应用管理、文件管理、定时任务等功能。代码量少、学习简单、功能强大、轻量级、易扩展，轻松开发从现在开始！

### 🥦 框架说明

* 基于.NET 5平台Furion开发框架与小诺VUE版本相结合，实时跟随基架升级而升级！
* 模块化架构设计，层次清晰，业务层推荐写到单独模块，框架升级不影响业务!
* 持续集百家所长，完善与丰富本框架基础设施，为.NET生态增加一种选择！
* 后期会推出基于此框架的相关应用场景案例，提供给大家使用！
* 有问题讨论的小伙伴可加QQ：[515096995](https://wpa.qq.com/msgrd?v=3&uin=515096995&_blank)，一起学习讨论。

### 🍿 在线体验

体验地址：[http://www.dilon.vip/](http://www.dilon.vip/)（用户名：superAdmin，密码：123456）

### 🍄 快速启动

需要安装：VS2019（最新版）、npm或yarn（最新版）

* 启动后台：打开Dilon.sln解决方案，直接运行（FF5）即可启动（数据库默认SQLite）
* 启动前端：打开_web文件夹，进行依赖下载，运行npm install或yarn命令，再运行npm run serve或 yarn run serve
* 浏览器访问：http://localhost:81 （默认前端端口为：81，后台端口为：5566）

### 🍎 效果图

<table>
    <tr>
        <td><img src="https://oscimg.oschina.net/oscnet/up-62d4b535dadbfa8ff343cb290d58be43ef0.png"/></td>
        <td><img src="https://oscimg.oschina.net/oscnet/up-98b3e79f8008b6319ce6394d80172ff02a3.png"/></td>
    </tr>
    <tr>
        <td><img src="https://images.gitee.com/uploads/images/2020/1208/133142_37420daa_1980003.jpeg"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2020/1208/133250_3749a395_1980003.jpeg"/></td>
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
12. 在线用户、当前系统在线用户的查看。【正在实现中...】
13. 公告管理、系统的公告的管理。
14. 文件管理、文件的上传下载查看等操作，文件可使用本地存储，阿里云oss，腾讯cos接入，支持拓展。【目前只支持本地存储...】
15. 定时任务、定时任务的维护，通过cron表达式控制任务的执行频率。
16. 系统配置、系统运行的参数的维护，参数的配置与系统运行机制息息相关。【完善中...】
17. 邮件发送、发送邮件功能。【正在实现中...】
18. 短信发送、短信发送功能，可使用阿里云sms，腾讯云sms，支持拓展。【正在实现中...】

### 🍖 更新日志

更新日志 [点击查看](https://gitee.com/yuebon/YuebonNetCore/commits/master)

### 🍻 版权说明

- 本技术框架采用 Apache License2.0 协议
- 如果要为项目做出贡献，请查看 [贡献指南](https://dotnetchina.gitee.io/furion/docs/contribute)
- 代码可用于个人项目等接私活或企业项目脚手架使用，开源版完全免费

### 💐 特别鸣谢
- 👉Furion：  [https://dotnetchina.gitee.io/furion](https://dotnetchina.gitee.io/furion)
- 👉 xiaonuo：[https://gitee.com/xiaonuobase/xiaonuo-vue](https://gitee.com/xiaonuobase/xiaonuo-vue)

如果对您有帮助，您可以点 "Star" 支持一下，这样才有持续下去的动力，谢谢！



