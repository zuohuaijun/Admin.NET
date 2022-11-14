using System;
using SqlSugar;
using System.ComponentModel;
using Admin.NET.Core;
namespace Admin.NET.Application.Entity
{
     /// <summary>
     /// 代码测试
     /// </summary>
      [SugarTable("generate_test")]
      [Description("代码测试")]
      public class generatetest  : EntityBase
      {
          /// <summary>
          /// 编码
          /// </summary>
          public string Code { get; set; }
          /// <summary>
          /// 名称
          /// </summary>
          public string Name { get; set; }
          /// <summary>
          /// 价格
          /// </summary>
          public decimal Price { get; set; }
          /// <summary>
          /// 过期日期
          /// </summary>
          public DateTime ExpireDate { get; set; }
          /// <summary>
          /// 状态
          /// </summary>
          public bool Status { get; set; }
}	
}