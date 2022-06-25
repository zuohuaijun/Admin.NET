# Admin.NET

#### 🎁 介绍
基于.NET6/Furion/SqlSugar实现的通用管理平台，前端Vue3/Vben。整合最新技术，模块插件式开发，前后端分离，开箱即用。


#### 📖 使用说明

1.  支持各种数据库，后台配置文件自行修改（自动生成数据库及种子数据）
2.  前端运行步骤：1、yarn安装依赖 2、pnpm serve运行 3、pnpm build打包


#### 🍎 大本营
1.  QQ群：[87333204](https://jq.qq.com/?_wv=1027&k=1t8iqf0G)
2.  微信号：zuohuaijun

#### 📖 常见问题
1. 修改实体类并同步到数据库
    - 修改配置文件Admin.NET.Application 》AppConfig.json 》EnableInitTable：true
    - 清理解决方案
    - ctrl+f5 / f5 启动项目
    - 在页面上发送任意请求（随便点个按钮），触发迁移
    - VS的输出窗口不停的滚动sql，代表正在迁移中
    - 迁移完成后，重新执行第1（将true改成false），2，3步，禁用迁移，提升项目启动速度
2. 跨库查询
    - 打开Admin.NET.Core 》 Extension 》 RepositoryExtension.cs
    - 找到 private static string GetTableName<T>(IAdo ado) 函数
    - 如果是固定数据库可以写死，多种不同库（mysql,sqlserve）同时使用，修改相关代码 库名.架构名.表名
    - 如果数据库表自动创建成功后，新增修改如果出现问题，一般就是此处返回的完整表名是有问题的，手动改此处!!!
```
        //根据实际的数据库类型 修改此处  如果固定使用一个数据库，可用直接写死
        var wholeTableName = $"{configId}.dbo.{tableName}";
        if (ado is MySqlProvider)
        {
            wholeTableName = $"{configId}.{tableName}";
        }
        else if (ado is SqlServerProvider)
        {
            wholeTableName = $"{configId}.dbo.{tableName}";
        }
        return wholeTableName;
```



#### 开发教程 💐 特别鸣谢
- 👉 Furion：[https://dotnetchina.gitee.io/furion](https://dotnetchina.gitee.io/furion)
- 👉 Vben：[https://vvbin.cn/doc-next/](https://vvbin.cn/doc-next/)
- 👉 SqlSugar：[https://gitee.com/dotnetchina/SqlSugar](https://gitee.com/dotnetchina/SqlSugar)
- 👉 CSRedis：[https://github.com/ctstone/csredis](https://github.com/ctstone/csredis)
- 👉 Magicodes.IE：[https://gitee.com/magicodes/Magicodes.IE](https://gitee.com/magicodes/Magicodes.IE)
- 👉 SKIT：[https://gitee.com/fudiwei/DotNetCore.SKIT.FlurlHttpClient.Wechat](https://gitee.com/fudiwei/DotNetCore.SKIT.FlurlHttpClient.Wechat)
- 👉 IdGenerator：[https://github.com/yitter/idgenerator](https://github.com/yitter/idgenerator)
- 👉 UAParser：[https://github.com/ua-parser/uap-csharp/](https://github.com/ua-parser/uap-csharp/)
- 👉 OnceMi.AspNetCore.OSS：[https://github.com/oncemi/OnceMi.AspNetCore.OSS](https://github.com/oncemi/OnceMi.AspNetCore.OSS)
