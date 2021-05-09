using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统机构表种子数据
    /// </summary>
    public class SysOrgSeedData : IEntitySeedData<SysOrg>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysOrg> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysOrg{TenantId=142307070918780, Id=142307070910539, Pid=0, Pids="[0],", Name="华夏集团", Code="hxjt", Sort=100, Remark="华夏集团", Status=0 },
                new SysOrg{TenantId=142307070918780, Id=142307070910540, Pid=142307070910539, Pids="[0],[142307070910539],", Name="华夏集团北京分公司", Code="hxjt_bj", Sort=100, Remark="华夏集团北京分公司", Status=0 },
                new SysOrg{TenantId=142307070918780, Id=142307070910541, Pid=142307070910539, Pids="[0],[142307070910539],", Name="华夏集团成都分公司", Code="hxjt_cd", Sort=100, Remark="华夏集团成都分公司", Status=0 },
                new SysOrg{TenantId=142307070918780, Id=142307070910542, Pid=142307070910540, Pids="[0],[142307070910539],[142307070910540],", Name="研发部", Code="hxjt_bj_yfb", Sort=100, Remark="华夏集团北京分公司研发部", Status=0 },
                new SysOrg{TenantId=142307070918780, Id=142307070910543, Pid=142307070910540, Pids="[0],[142307070910539],[142307070910540],", Name="企划部", Code="hxjt_bj_qhb", Sort=100, Remark="华夏集团北京分公司企划部", Status=0 },
                new SysOrg{TenantId=142307070918780, Id=142307070910544, Pid=142307070910541, Pids="[0],[142307070910539],[142307070910541],", Name="市场部", Code="hxjt_cd_scb", Sort=100, Remark="华夏集团成都分公司市场部", Status=0 },
                new SysOrg{TenantId=142307070918780, Id=142307070910545, Pid=142307070910541, Pids="[0],[142307070910539],[142307070910541],", Name="财务部", Code="hxjt_cd_cwb", Sort=100, Remark="华夏集团成都分公司财务部", Status=0 },
                new SysOrg{TenantId=142307070918780, Id=142307070910546, Pid=142307070910544, Pids="[0],[142307070910539],[142307070910541],[142307070910544],", Name="市场部二部", Code="hxjt_cd_scb_2b", Sort=100, Remark="华夏集团成都分公司市场部二部", Status=0 },
                new SysOrg{TenantId=142307070918781, Id=142307070910547, Pid=0, Pids="[0],", Name="租户1公司", Code="gsmc", Sort=100, Remark="公司名称", Status=0 },
                new SysOrg{TenantId=142307070918782, Id=142307070910548, Pid=0, Pids="[0],", Name="租户2公司", Code="gsmc", Sort=100, Remark="公司名称", Status=0 },
            };
        }
    }
}