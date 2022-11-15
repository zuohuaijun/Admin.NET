using System;
using SqlSugar;
using System.ComponentModel;
using Admin.NET.Core;
namespace Admin.NET.Application.Entity
{
     /// <summary>
     /// 
     /// </summary>
      [SugarTable("d_tenant_business")]
      [Description("")]
      public class 测试  : EntityBase
      {
          /// <summary>
          /// 
          /// </summary>
          public string Name { get; set; }
          /// <summary>
          /// 
          /// </summary>
          public long Age { get; set; }
          /// <summary>
          /// 
          /// </summary>
          public DateTime BirthDate { get; set; }
          /// <summary>
          /// 
          /// </summary>
          public long TenantId { get; set; }
}	
}