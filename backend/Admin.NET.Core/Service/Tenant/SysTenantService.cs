using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 租户服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Tenant", Order = 100)]
    public class SysTenantService : ISysTenantService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysTenant, MultiTenantDbContextLocator> _sysTenantRep;    // 租户表仓储
        private readonly IRepository<SysUser> _sysUserRep;

        private readonly ISysRoleMenuService _sysRoleMenuService;
        private readonly ISysUserRoleService _sysUserRoleService;

        public SysTenantService(IRepository<SysTenant, MultiTenantDbContextLocator> sysTenantRep,
                                IRepository<SysUser> sysUserService,
                                ISysRoleMenuService sysRoleMenuService,
                                ISysUserRoleService sysUserRoleService)
        {
            _sysTenantRep = sysTenantRep;
            _sysUserRep = sysUserService;
            _sysRoleMenuService = sysRoleMenuService;
            _sysUserRoleService = sysUserRoleService;
        }

        /// <summary>
        /// 分页查询租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysTenant/page")]
        public async Task<dynamic> QueryTenantPageList([FromQuery] TenantPageInput input)
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
            var isExist = await _sysTenantRep.DetachedEntities.AnyAsync(u => u.Name == input.Name || u.Email == input.Email);
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
            var newOrg = await new SysOrg
            {
                TenantId = tenantId,
                Pid = 0,
                Pids = "[0],",
                Name = companyName,
                Code = companyName,
                Contacts = newTenant.AdminName,
                Tel = newTenant.Phone
            }.InsertNowAsync();

            // 初始化角色
            var newRole = await new SysRole
            {
                TenantId = tenantId,
                Code = "sys_manager_role",
                Name = "系统管理员",
                DataScopeType = DataScopeType.ALL,
                SysOrgs = new List<SysOrg>() { newOrg.Entity }
            }.InsertNowAsync();

            // 初始化租户管理员
            var newUser = await new SysUser
            {
                TenantId = tenantId,
                Account = email,
                Password = "e10adc3949ba59abbe56e057f20f883e",
                Name = newTenant.AdminName,
                NickName = newTenant.AdminName,
                Email = newTenant.Email,
                Phone = newTenant.Phone,
                AdminType = AdminType.Admin,
                SysRoles = new List<SysRole>() { newRole.Entity },
                SysOrgs = new List<SysOrg>() { newOrg.Entity }
            }.InsertNowAsync();

            // 初始化职工
            new SysEmp
            {
                Id = newUser.Entity.Id,
                JobNum = "D001",
                OrgId = newOrg.Entity.Id,
                OrgName = newOrg.Entity.Name
            }.InsertNow();
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

            // 删除与租户相关的表数据
            var users = await Db.GetRepository<SysUser>().Where(u => u.TenantId == input.Id, false, true).ToListAsync();
            users.ForEach(u => { u.Delete(); });

            var userIds = users.Select(u => u.Id).ToList();
            var userRoles = await Db.GetRepository<SysUserRole>().Where(u => userIds.Contains(u.SysUserId), false).ToListAsync();
            userRoles.ForEach(u => { u.Delete(); });

            var userDataScopes = await Db.GetRepository<SysUserDataScope>().Where(u => userIds.Contains(u.SysUserId), false).ToListAsync();
            userDataScopes.ForEach(u => { u.Delete(); });

            var emps = await Db.GetRepository<SysEmp>().Where(u => userIds.Contains(u.Id), false).ToListAsync();
            emps.ForEach(u => { u.Delete(); });

            var emppos = await Db.GetRepository<SysEmpPos>().Where(u => userIds.Contains(u.SysEmpId), false).ToListAsync();
            emppos.ForEach(u => { u.Delete(); });

            var empexts = await Db.GetRepository<SysEmpExtOrgPos>().Where(u => userIds.Contains(u.SysEmpId), false).ToListAsync();
            empexts.ForEach(u => { u.Delete(); });

            var roles = await Db.GetRepository<SysRole>().Where(u => u.TenantId == input.Id, false, true).ToListAsync();
            roles.ForEach(u => { u.Delete(); });

            var roleIds = roles.Select(u => u.Id).ToList();
            var roleMenus = await Db.GetRepository<SysRoleMenu>().Where(u => roleIds.Contains(u.SysRoleId), false).ToListAsync();
            roleMenus.ForEach(u => { u.Delete(); });

            var roleDataScopes = await Db.GetRepository<SysRoleDataScope>().Where(u => roleIds.Contains(u.SysRoleId), false).ToListAsync();
            roleDataScopes.ForEach(u => { u.Delete(); });

            var orgs = await Db.GetRepository<SysOrg>().Where(u => u.TenantId == input.Id, false, true).ToListAsync();
            orgs.ForEach(u => { u.Delete(); });

            var pos = await Db.GetRepository<SysPos>().Where(u => u.TenantId == input.Id, false, true).ToListAsync();
            pos.ForEach(u => { u.Delete(); });

            //var entities = Db.GetDbContext().Model.GetEntityTypes().Where(u => u.ClrType.BaseType.Name == typeof(DBEntityTenant).Name).ToList();
            //entities.ForEach(u =>
            //{
            //    // dynamic entity = Activator.CreateInstance(u.ClrType);
            //    dynamic rep = App.GetService(typeof(IRepository<,>).MakeGenericType(u.ClrType, typeof(MasterDbContextLocator)));// as IPrivateRepository<User>;
            //    //var dtList = rep.DetachedEntities.Where(e => e.TenantId == input.Id).ToList();
            //    //dtList.ForEach(u =>
            //    //{
            //    //    u.Delete();
            //    //});
            //});
        }

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTenant/edit")]
        public async Task UpdateTenant(UpdateTenantInput input)
        {
            var isExist = await _sysTenantRep.DetachedEntities.AnyAsync(u => (u.Name == input.Name || u.Email == input.Email) && u.Id != input.Id);
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

        /// <summary>
        /// 授权租户管理员角色菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTenant/grantMenu")]
        public async Task GrantMenu(GrantRoleMenuInput input)
        {
            var tenantAdminUser = await GetTenantAdminUser(input.Id);
            if (tenantAdminUser == null) return;
            var roleIds = await _sysUserRoleService.GetUserRoleIdList(tenantAdminUser.Id);
            input.Id = roleIds[0]; // 重置租户管理员角色Id
            await _sysRoleMenuService.GrantMenu(input);
        }

        /// <summary>
        /// 获取租户管理员角色拥有菜单Id集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysTenant/ownMenu")]
        public async Task<List<long>> OwnMenu([FromQuery] QueryTenantInput input)
        {
            var tenantAdminUser = await GetTenantAdminUser(input.Id);
            if (tenantAdminUser == null) return new List<long>();
            var roleIds = await _sysUserRoleService.GetUserRoleIdList(tenantAdminUser.Id);
            var tenantAdminRoleId = roleIds[0]; // 租户管理员角色Id
            return await _sysRoleMenuService.GetRoleMenuIdList(new List<long> { tenantAdminRoleId });
        }

        /// <summary>
        /// 重置租户管理员密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTenant/resetPwd")]
        public async Task ResetUserPwd(QueryTenantInput input)
        {
            var tenantAdminUser = await GetTenantAdminUser(input.Id);
            tenantAdminUser.Password = MD5Encryption.Encrypt(CommonConst.DEFAULT_PASSWORD);
            await _sysUserRep.UpdateIncludeAsync(tenantAdminUser, new[] { nameof(SysUser.Password) });
        }

        /// <summary>
        /// 获取租户管理员用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        private async Task<SysUser> GetTenantAdminUser(long tenantId)
        {
            return await _sysUserRep.Where(u => u.TenantId == tenantId, false, true)
                                    .Where(u => u.AdminType == AdminType.Admin).FirstOrDefaultAsync();
        }
    }
}