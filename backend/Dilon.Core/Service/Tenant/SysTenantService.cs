using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 租户服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Tenant", Order = 100)]
    public class SysTenantService : ISysTenantService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysTenant, MultiTenantDbContextLocator> _sysTenantRep;    // 租户表仓储
        private readonly IRepository<SysUser> _sysUserService; // 系统用户服务
        private readonly IRepository<SysOrg> _sysOrgService;
        private readonly IRepository<SysRole> _sysRoleService;
        private readonly ISysRoleMenuService _sysRoleMenuService;
        private readonly IRepository<SysEmp> _sysEmpService;

        public SysTenantService(IRepository<SysTenant, MultiTenantDbContextLocator> sysTenantRep,
                                IRepository<SysUser> sysUserService,
                                IRepository<SysOrg> sysOrgService,
                                IRepository<SysRole> sysRoleService,
                                ISysRoleMenuService sysRoleMenuService,
                                IRepository<SysEmp> sysEmpService)
        {
            _sysTenantRep = sysTenantRep;
            _sysUserService = sysUserService;
            _sysOrgService = sysOrgService;
            _sysRoleService = sysRoleService;
            _sysRoleMenuService = sysRoleMenuService;
            _sysEmpService = sysEmpService;
        }

        /// <summary>
        /// 分页查询租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysTenant/page")]
        public async Task<dynamic> QueryTenantPageList([FromQuery] TenantInput input)
        {
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var host = !string.IsNullOrEmpty(input.Host?.Trim());
            var tenants = await _sysTenantRep.DetachedEntities
                                             .Where((name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%")))
                                             .Select(u => u.Adapt<TenantOutput>())
                                             .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<TenantOutput>.PageResult(tenants);
        }

        /// <summary>
        /// 增加租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTenant/add")]
        public async Task AddTenant(AddTenantInput input)
        {
            var isExist = await _sysTenantRep.DetachedEntities.AnyAsync(u => u.Name == input.Name);
            if (isExist)
                throw Oops.Oh(ErrorCode.D1300);

            var tenant = input.Adapt<SysTenant>();
            var newTenant = _sysTenantRep.InsertNow(tenant);
            await InitNewTenant(newTenant.Entity);
        }

        /// <summary>
        /// 新增租户时，初始化数据
        /// </summary>
        /// <param name="newTenant"></param>
        public async Task InitNewTenant(SysTenant newTenant)
        {
            long tenantId = newTenant.Id;
            string email = newTenant.Email;
            string companyName = newTenant.Name;
            // 初始化公司（组织结构）
            SysOrg sysOrg = new()
            {
                TenantId = tenantId,
                Pid = 0,
                Pids = "[0],",
                Name = companyName,
                Code = "1",
                Contacts = newTenant.AdminName,
                Tel = newTenant.Phone
            };
            var newOrg = await _sysOrgService.InsertNowAsync(sysOrg);

            // 初始化角色
            SysRole sysRole = new()
            {
                TenantId = tenantId,
                Code = "1",
                Name = "系统管理员",
                SysOrgs = new List<SysOrg>() { newOrg.Entity }
            };
            var newRole = await _sysRoleService.InsertNowAsync(sysRole);

            // 初始化用户
            SysUser sysUser = new()
            {
                TenantId = tenantId,
                Account = email,
                Password = "e10adc3949ba59abbe56e057f20f883e",
                Name = newTenant.AdminName,
                NickName = newTenant.AdminName,
                Email = newTenant.Email,
                Phone = newTenant.Phone,
                AdminType = AdminType.None,
                SysRoles = new List<SysRole>() { newRole.Entity },
                SysOrgs = new List<SysOrg>() { newOrg.Entity }
            };
            var newUser = await _sysUserService.InsertNowAsync(sysUser);

            // 初始化职工
            SysEmp sysEmp = new()
            {
                Id = newUser.Entity.Id,
                JobNum = "1",
                OrgId = newOrg.Entity.Id,
                OrgName = newOrg.Entity.Name
            };
            await _sysEmpService.InsertNowAsync(sysEmp);
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTenant/delete")]
        public async Task DeleteTenant(DeleteTenantInput input)
        {
            var tenant = await _sysTenantRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await _sysTenantRep.DeleteAsync(tenant);
        }

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTenant/edit")]
        public async Task UpdateTenant(UpdateTenantInput input)
        {
            var isExist = await _sysTenantRep.DetachedEntities.AnyAsync(u => (u.Name == input.Name || u.Host == input.Host) && u.Id != input.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.D1300);

            var tenant = input.Adapt<SysTenant>();
            await _sysTenantRep.UpdateAsync(tenant, true);
        }

        /// <summary>
        /// 获取租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysTenant/detail")]
        public async Task<SysTenant> GetTenant([FromQuery] QueryTenantInput input)
        {
            return await _sysTenantRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
        }
    }
}
