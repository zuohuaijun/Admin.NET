using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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

        public SysTenantService(IRepository<SysTenant, MultiTenantDbContextLocator> sysTenantRep, IRepository<SysUser> sysUserService, IRepository<SysOrg> sysOrgService, IRepository<SysRole> sysRoleService, ISysRoleMenuService sysRoleMenuService, IRepository<SysEmp> sysEmpService)
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
            // 初始化公司
            SysOrg sysOrg = new SysOrg()
            {
                TenantId = tenantId,
                Pid = 0,
                Pids = "[0],",
                Name = companyName,
                Code = "1",
                Contacts = newTenant.AdminName,
                Tel = newTenant.Phone
            };
            var newOrg = _sysOrgService.InsertNow(sysOrg);
            long orgId = newOrg.Entity.Id;
            //初始化角色
            SysRole sysRole = new SysRole()
            {
                TenantId = tenantId,
                Code = "1",
                Name = "系统管理员",
                SysOrgs = new List<SysOrg>() { newOrg.Entity },
            };
            var newRole = _sysRoleService.InsertNow(sysRole);

            //初始化用户
            SysUser sysUser = new SysUser()
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
                SysOrgs = new List<SysOrg>() { newOrg.Entity },


            };
            var newUser = _sysUserService.InsertNow(sysUser);

            //初始化职工
            SysEmp sysEmp = new SysEmp()
            {
                Id = newUser.Entity.Id,
                JobNum = "1",
                OrgId = newOrg.Entity.Id,
                OrgName = newOrg.Entity.Name,
                TenantId = tenantId

            };
            var newSysEmp = _sysEmpService.InsertNow(sysEmp);

            //初始化角色权限 -此处应该根据启用的App初始。
            GrantRoleMenuInput roleMenuInput = new GrantRoleMenuInput()
            {
                Id = newRole.Entity.Id,
                //应根据用户购买的应用进行查询
                GrantMenuIdList = new List<long>() {
                    142000000010563,
                    142000000010581,
                    142000000010582,
                    142000000010583,
                    142000000010584,
                    142000000010585,
                    142000000010586,
                    142000000010587,
                    142000000010588,
                    142000000010589,
                    142000000010590,
                    142000000010591,
                    142000000014629,
                    142000000014630,
                    142000000014631,
                    142000000014632,
                    142000000110564,
                    142000000110565,
                    142000000110566,
                    142000000110567,
                    142000000110568,
                    142000000110569,
                    142000000110570,
                    142000000110571,
                    142000000110572,
                    142000000110573,
                    142000000110574,
                    142000000110575,
                    142000000110576,
                    142000000110577,
                    142000000110578,
                    142000000110579,
                    142000000110580,
                    142307000914633,
                    142307000914651,
                    142307000914652,
                    142307000914653,
                    142307000914654,
                    142307000914655,
                    142307000914656,
                    142307000914657,
                    142307000914658,
                    142307000914659,
                    142307000914660,
                    142307000914661,
                    142307000915661,
                    142307070910660,
                    142307070910661,
                    142307070910662
                }

            };
            await _sysRoleMenuService.GrantMenu(roleMenuInput);
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
