using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
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
                new SysOrg{Id=1, Pid=0, Pids="[0],", Name="华夏集团", Code="hxjt", Sort=100, Remark="华夏集团", Status=0 },
                new SysOrg{Id=2, Pid=1, Pids="[0],[1],", Name="华夏集团北京分公司", Code="hxjt_bj", Sort=100, Remark="华夏集团北京分公司", Status=0 },
                new SysOrg{Id=3, Pid=1, Pids="[0],[1],", Name="华夏集团成都分公司", Code="hxjt_cd", Sort=100, Remark="华夏集团成都分公司", Status=0 },
                new SysOrg{Id=4, Pid=2, Pids="[0],[1],[2],", Name="研发部", Code="hxjt_bj_yfb", Sort=100, Remark="华夏集团北京分公司研发部", Status=0 },
                new SysOrg{Id=5, Pid=2, Pids="[0],[1],[2],", Name="企划部", Code="hxjt_bj_qhb", Sort=100, Remark="华夏集团北京分公司企划部", Status=0 },
                new SysOrg{Id=6, Pid=3, Pids="[0],[1],[3],", Name="市场部", Code="hxjt_cd_scb", Sort=100, Remark="华夏集团成都分公司市场部", Status=0 },
                new SysOrg{Id=7, Pid=3, Pids="[0],[1],[3],", Name="财务部", Code="hxjt_cd_cwb", Sort=100, Remark="华夏集团成都分公司财务部", Status=0 },
                new SysOrg{Id=8, Pid=6, Pids="[0],[1],[3],[6],", Name="市场部二部", Code="hxjt_cd_scb_2b", Sort=100, Remark="华夏集团成都分公司市场部二部", Status=0 }
            };
        }
    }
}
