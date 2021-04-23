using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [ApiDescriptionSettings(Name = "User", Order = 150)]
    public class SysUserService : ISysUserService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysUser> _sysUserRep;  // 用户表仓储 
        private readonly IUserManager _userManager;

        private readonly ISysCacheService _sysCacheService;
        private readonly ISysEmpService _sysEmpService;
        private readonly ISysUserDataScopeService _sysUserDataScopeService;
        private readonly ISysUserRoleService _sysUserRoleService;
        private readonly ISysOrgService _sysOrgService;

        public SysUserService(IRepository<SysUser> sysUserRep,
                              IUserManager userManager,
                              ISysCacheService sysCacheService,
                              ISysEmpService sysEmpService,
                              ISysUserDataScopeService sysUserDataScopeService,
                              ISysUserRoleService sysUserRoleService,
                              ISysOrgService sysOrgService)
        {
            _sysUserRep = sysUserRep;
            _userManager = userManager;
            _sysCacheService = sysCacheService;
            _sysEmpService = sysEmpService;
            _sysUserDataScopeService = sysUserDataScopeService;
            _sysUserRoleService = sysUserRoleService;
            _sysOrgService = sysOrgService;
        }

        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysUser/page")]
        public async Task<dynamic> QueryUserPageList([FromQuery] UserInput input)
        {
            var superAdmin = _userManager.SuperAdmin;
            var searchValue = input.SearchValue;
            var pid = input.SysEmpParam.OrgId;

            var sysEmpRep = Db.GetRepository<SysEmp>();
            var sysOrgRep = Db.GetRepository<SysOrg>();
            var dataScopes = await GetUserDataScopeIdList(_userManager.UserId);
            var users = await _sysUserRep.DetachedEntities
                                         .Join(sysEmpRep.DetachedEntities, u => u.Id, e => e.Id, (u, e) => new { u, e })
                                         .Join(sysOrgRep.DetachedEntities, n => n.e.OrgId, o => o.Id, (n, o) => new { n, o })
                                         .Where(!string.IsNullOrEmpty(searchValue), x => (x.n.u.Account.Contains(input.SearchValue) ||
                                                                                    x.n.u.Name.Contains(input.SearchValue) ||
                                                                                    x.n.u.Phone.Contains(input.SearchValue)))
                                         .Where(!string.IsNullOrEmpty(pid), x => (x.n.e.OrgId == long.Parse(pid) ||
                                                                            x.o.Pids.Contains($"[{pid.Trim()}]")))
                                         .Where(input.SearchStatus >= 0, x => x.n.u.Status == input.SearchStatus)
                                         .Where(!superAdmin, x => x.n.u.AdminType != AdminType.SuperAdmin)
                                         .Where(!superAdmin && dataScopes.Count > 0, x => dataScopes.Contains(x.n.e.OrgId))
                                         .Select(u => u.n.u.Adapt<UserOutput>()).ToPagedListAsync(input.PageNo, input.PageSize);

            //var emps = new List<Task<EmpOutput>>();
            //users.Items.ToList().ForEach(u =>
            //{
            //    emps.Add(_sysEmpService.GetEmpInfo(long.Parse(u.Id)));
            //});
            //await Task.WhenAll(emps);
            foreach (var user in users.Items)
            {
                user.SysEmpInfo = await _sysEmpService.GetEmpInfo(long.Parse(user.Id));
            }
            return XnPageResult<UserOutput>.PageResult(users);
        }

        /// <summary>
        /// 增加用户       
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/add")]
        [UnitOfWork]
        public async Task AddUser(AddUserInput input)
        {
            // 数据范围检查
            CheckDataScope(input);

            var isExist = await _sysUserRep.AnyAsync(u => u.Account == input.Account, false);
            if (isExist) throw Oops.Oh(ErrorCode.D1003);

            var user = input.Adapt<SysUser>();
            user.Password = MD5Encryption.Encrypt(input.Password);
            if (string.IsNullOrEmpty(user.Name))
                user.Name = user.Account;
            if (string.IsNullOrEmpty(user.NickName))
                user.NickName = user.Account;
            var newUser = await _sysUserRep.InsertNowAsync(user);
            input.SysEmpParam.Id = newUser.Entity.Id.ToString();
            // 增加员工信息
            await _sysEmpService.AddOrUpdate(input.SysEmpParam);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/delete")]
        [UnitOfWork]
        public async Task DeleteUser(DeleteUserInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (user.AdminType == AdminType.SuperAdmin)
                throw Oops.Oh(ErrorCode.D1014);

            // 数据范围检查
            CheckDataScope(input);

            // 直接删除用户
            await user.DeleteAsync();

            // 删除员工及附属机构职位信息
            await _sysEmpService.DeleteEmpInfoByUserId(user.Id);

            //删除该用户对应的用户-角色表关联信息
            await _sysUserRoleService.DeleteUserRoleListByUserId(user.Id);

            //删除该用户对应的用户-数据范围表关联信息
            await _sysUserDataScopeService.DeleteUserDataScopeListByUserId(user.Id);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/edit")]
        [UnitOfWork]
        public async Task UpdateUser(UpdateUserInput input)
        {
            // 数据范围检查
            CheckDataScope(input);

            // 排除自己并且判断与其他是否相同
            var isExist = await _sysUserRep.AnyAsync(u => u.Account == input.Account && u.Id != input.Id, false);
            if (isExist) throw Oops.Oh(ErrorCode.D1003);

            var user = input.Adapt<SysUser>();
            await user.UpdateExcludeAsync(new[] { nameof(SysUser.Password), nameof(SysUser.Status), nameof(SysUser.AdminType) }, true);
            input.SysEmpParam.Id = user.Id.ToString();
            // 更新员工及附属机构职位信息
            await _sysEmpService.AddOrUpdate(input.SysEmpParam);
        }

        /// <summary>
        /// 查看用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysUser/detail")]
        public async Task<dynamic> GetUser([FromQuery] QueryUserInput input)
        {
            var user = await _sysUserRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
            var userDto = user.Adapt<UserOutput>();
            if (userDto != null)
            {
                userDto.SysEmpInfo = await _sysEmpService.GetEmpInfo(user.Id);
            }
            return userDto;
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/changeStatus")]
        public async Task ChangeUserStatus(UpdateUserInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (user.AdminType == AdminType.SuperAdmin)
                throw Oops.Oh(ErrorCode.D1015);

            if (!Enum.IsDefined(typeof(CommonStatus), input.Status))
                throw Oops.Oh(ErrorCode.D3005);
            user.Status = input.Status;
        }

        /// <summary>
        /// 授权用户角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/grantRole")]
        public async Task GrantUserRole(UpdateUserInput input)
        {
            // 数据范围检查
            CheckDataScope(input);
            await _sysUserRoleService.GrantRole(input);
        }

        /// <summary>
        /// 授权用户数据范围
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/grantData")]
        public async Task GrantUserData(UpdateUserInput input)
        {
            // 清除缓存
            await _sysCacheService.DelAsync(CommonConst.CACHE_KEY_DATASCOPE + $"{input.Id}");

            // 数据范围检查
            CheckDataScope(input);
            await _sysUserDataScopeService.GrantData(input);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/updateInfo")]
        public async Task UpdateUserInfo(UpdateUserInput input)
        {
            var user = input.Adapt<SysUser>();
            await user.UpdateExcludeAsync(new[] { nameof(SysUser.AdminType) });
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/updatePwd")]
        public async Task UpdateUserPwd(ChangePasswordUserInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (MD5Encryption.Encrypt(input.Password) != user.Password)
                throw Oops.Oh(ErrorCode.D1004);
            user.Password = MD5Encryption.Encrypt(input.NewPassword);
        }

        /// <summary>
        /// 获取用户拥有角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysUser/ownRole")]
        public async Task<dynamic> GetUserOwnRole([FromQuery] QueryUserInput input)
        {
            return await _sysUserRoleService.GetUserRoleIdList(input.Id);
        }

        /// <summary>
        /// 获取用户拥有数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysUser/ownData")]
        public async Task<dynamic> GetUserOwnData([FromQuery] QueryUserInput input)
        {
            return await _sysUserDataScopeService.GetUserDataScopeIdList(input.Id);
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/resetPwd")]
        public async Task ResetUserPwd(QueryUserInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            user.Password = MD5Encryption.Encrypt(CommonConst.DEFAULT_PASSWORD);
        }

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/updateAvatar")]
        public async Task UpdateAvatar(UploadAvatarInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            user.Avatar = input.Avatar.ToString();
        }

        /// <summary>
        /// 获取用户选择器
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysUser/selector")]
        public async Task<dynamic> GetUserSelector([FromQuery] UserInput input)
        {
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            return await _sysUserRep.DetachedEntities
                                    .Where(name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%"))
                                    .Where(u => u.Status != CommonStatus.DELETED)
                                    .Where(u => u.AdminType != AdminType.SuperAdmin)
                                    .Select(u => new
                                    {
                                        u.Id,
                                        u.Name
                                    }).ToListAsync();
        }

        /// <summary>
        /// 用户导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysUser/export")]
        public async Task<IActionResult> ExportUser([FromQuery] UserInput input)
        {
            var users = _sysUserRep.DetachedEntities.AsQueryable();

            var memoryStream = new MemoryStream();
            memoryStream.SaveAs(users);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return await Task.FromResult(new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "user.xlsx"
            });
        }

        /// <summary>
        /// 用户导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/import")]
        public async Task ImportUser(IFormFile file)
        {
            var path = Path.Combine(Path.GetTempPath(), $"{YitIdHelper.NextId()}.xlsx");
            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            //var rows = MiniExcel.Query(path); // 解析
            //foreach (var row in rows)
            //{
            //    var a = row.A;
            //    var b = row.B;
            //    // 入库等操作

            //}
        }

        /// <summary>
        /// 根据用户Id获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>       
        [NonAction]
        public async Task<dynamic> GetUserById(long userId)
        {
            return await _sysUserRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// 将OAuth账号转换成账号
        /// </summary>
        /// <param name="authUser"></param>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        [NonAction]
        public async Task SaveAuthUserToUser(AuthUserInput authUser, UserInput sysUser)
        {
            var user = sysUser.Adapt<SysUser>();
            user.AdminType = AdminType.None; // 非管理员

            // oauth账号与系统账号判断
            var isExist = await _sysUserRep.DetachedEntities.AnyAsync(u => u.Account == authUser.Username);
            user.Account = isExist ? authUser.Username + DateTime.Now.Ticks : authUser.Username;
            user.Name = user.NickName = authUser.Nickname;
            user.Email = authUser.Email;
            user.Sex = authUser.Gender;
            await user.InsertAsync();
        }

        /// <summary>
        /// 获取用户数据范围（机构Id集合）并缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>       
        [NonAction]
        public async Task<List<long>> GetUserDataScopeIdList(long userId)
        {
            var dataScopes = await _sysCacheService.GetDataScope(userId); // 先从缓存里面读取
            if (dataScopes == null || dataScopes.Count < 1)
            {
                if (!_userManager.SuperAdmin)
                {
                    var orgId = await _sysEmpService.GetEmpOrgId(userId);
                    // 获取该用户对应的数据范围集合
                    var userDataScopeIdListForUser = await _sysUserDataScopeService.GetUserDataScopeIdList(userId);
                    // 获取该用户的角色对应的数据范围集合
                    var userDataScopeIdListForRole = await _sysUserRoleService.GetUserRoleDataScopeIdList(userId, orgId);
                    dataScopes = userDataScopeIdListForUser.Concat(userDataScopeIdListForRole).Distinct().ToList(); // 并集
                }
                else
                {
                    dataScopes = await _sysOrgService.GetAllDataScopeIdList();
                }
                await _sysCacheService.SetDataScope(userId, dataScopes); // 缓存结果
            }
            return dataScopes;
        }

        /// <summary>
        /// 获取用户数据范围（机构Id集合）
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetUserDataScopeIdList()
        {
            var userId = _userManager.UserId;
            var dataScopes = await GetUserDataScopeIdList(userId);
            return dataScopes;
        }

        /// <summary>
        /// 检查普通用户数据范围
        /// </summary>
        /// <param name="userParam"></param>
        /// <returns></returns>
        private async void CheckDataScope(UserInput userParam)
        {
            // 如果当前用户不是超级管理员，则进行数据范围校验
            if (!_userManager.SuperAdmin)
            {
                var dataScopes = await GetUserDataScopeIdList(_userManager.UserId);
                if (dataScopes == null || (userParam.SysEmpParam.OrgId != null && !dataScopes.Contains(long.Parse(userParam.SysEmpParam.OrgId))))
                    throw Oops.Oh(ErrorCode.D1013);
            }
        }
    }
}
