using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 系统菜单服务
    /// </summary>
    [ApiDescriptionSettings(Name = "系统菜单", Order = 195)]
    public class SysMenuService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysMenu> _sysMenuRep;
        private readonly IUserManager _userManager;
        private readonly SysRoleMenuService _sysRoleMenuService;
        private readonly SysUserRoleService _sysUserRoleService;

        public SysMenuService(SqlSugarRepository<SysMenu> sysMenuRep,
            IUserManager userManager,
            SysRoleMenuService sysRoleMenuService,
            SysUserRoleService sysUserRoleService)
        {
            _sysMenuRep = sysMenuRep;
            _userManager = userManager;
            _sysRoleMenuService = sysRoleMenuService;
            _sysUserRoleService = sysUserRoleService;
        }

        /// <summary>
        /// 获取登录菜单树
        /// </summary>
        /// <returns></returns>
        [HttpGet("/getMenuList")]
        public async Task<List<MenuOutput>> GetLoginMenuTree()
        {
            if (_userManager.SuperAdmin)
            {
                var menuList = await _sysMenuRep.AsQueryable()
                    .Where(u => u.Type != MenuTypeEnum.Btn)
                    .OrderBy(u => u.OrderNo).ToTreeAsync(u => u.Children, u => u.Pid, 0);
                return menuList.Adapt<List<MenuOutput>>();
            }
            else
            {
                var menuIdList = await GetMenuIdList();
                var menuList = await _sysMenuRep.AsQueryable()
                    .Where(u => u.Type != MenuTypeEnum.Btn)
                    .Where(u => menuIdList.Contains(u.Id))
                    .OrderBy(u => u.OrderNo).ToTreeAsync(u => u.Children, u => u.Pid, 0);
                return menuList.Adapt<List<MenuOutput>>();
            }
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysMenu/list")]
        public async Task<List<SysMenu>> GetMenuList([FromQuery] MenuInput input)
        {
            var menuIdList = new List<long>();
            if (!_userManager.SuperAdmin)
                menuIdList = await GetMenuIdList();

            if (!string.IsNullOrWhiteSpace(input.Title) || input.Type > 0)
            {
                return await _sysMenuRep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title))
                    .WhereIF(input.Type > 0, u => u.Type == (MenuTypeEnum)input.Type)
                    .WhereIF(menuIdList.Count > 1, u => menuIdList.Contains(u.Id))
                    .OrderBy(u => u.OrderNo).ToListAsync();
            }

            return await _sysMenuRep.AsQueryable()
                .WhereIF(menuIdList.Count > 1, u => menuIdList.Contains(u.Id))
                .OrderBy(u => u.OrderNo).ToTreeAsync(u => u.Children, u => u.Pid, 0);
        }

        /// <summary>
        /// 增加菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysMenu/add")]
        public async Task AddMenu(AddMenuInput input)
        {
            var isExist = await _sysMenuRep.IsAnyAsync(u => u.Name == input.Name);
            if (isExist)
                throw Oops.Oh(ErrorCodeEnum.D4000);

            // 校验菜单参数
            CheckMenuParam(input.Adapt<MenuInput>());

            var menu = input.Adapt<SysMenu>();
            await _sysMenuRep.InsertAsync(menu);
        }

        /// <summary>
        /// 更新菜单
        /// </summary>upda
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysMenu/update"),]
        public async Task UpdateMenu(UpdateMenuInput input)
        {
            var isExist = await _sysMenuRep.IsAnyAsync(u => u.Name == input.Name && u.Id != input.Id);
            if (isExist)
                throw Oops.Oh(ErrorCodeEnum.D4000);

            // 校验菜单参数
            CheckMenuParam(input.Adapt<MenuInput>());

            var menu = input.Adapt<SysMenu>();
            await _sysMenuRep.AsUpdateable(menu).IgnoreColumns(true).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysMenu/delete")]
        [SqlSugarUnitOfWork]
        public async Task DeleteMenu(DeleteMenuInput input)
        {
            var menuTreeList = await _sysMenuRep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id);
            var menuIdList = menuTreeList.Select(u => u.Id).ToList();

            await _sysMenuRep.DeleteAsync(u => menuIdList.Contains(u.Id));

            // 级联删除角色菜单数据
            await _sysRoleMenuService.DeleteRoleMenuByMenuIdList(menuIdList);
        }

        /// <summary>
        /// 增加和编辑时检查菜单数据
        /// </summary>
        /// <param name="input"></param>
        private static void CheckMenuParam(MenuInput input)
        {
            var type = input.Type;
            //var component = input.Component;
            var permission = input.Permission;
            //if (type == (int)MenuTypeEnum.Dir || type == (int)MenuTypeEnum.Menu)
            //{
            //    if (string.IsNullOrEmpty(component))
            //        throw Oops.Oh(ErrorCodeEnum.D4001);
            //}
            //else
            if (type == (int)MenuTypeEnum.Btn)
            {
                if (string.IsNullOrEmpty(permission))
                    throw Oops.Oh(ErrorCodeEnum.D4003);
                if (!permission.Contains(':'))
                    throw Oops.Oh(ErrorCodeEnum.D4004);
            }
        }

        /// <summary>
        /// 获取按钮权限列表(登录)
        /// </summary>
        /// <returns></returns>
        [HttpGet("getPermCode")]
        public async Task<dynamic> GetPermCodeList()
        {
            if (_userManager.SuperAdmin)
            {
                return await _sysMenuRep.AsQueryable()
                    .Where(u => u.Type == MenuTypeEnum.Btn)
                    .Select(u => u.Permission).ToListAsync();
            }
            else
            {
                var menuIdList = await GetMenuIdList();
                return await _sysMenuRep.AsQueryable()
                    .Where(u => u.Type == MenuTypeEnum.Btn)
                    .Where(u => menuIdList.Contains(u.Id))
                    .Select(u => u.Permission).ToListAsync();
            }
        }

        /// <summary>
        /// 获取当前用户菜单Id集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<long>> GetMenuIdList()
        {
            var roleIdList = await _sysUserRoleService.GetUserRoleIdList(_userManager.UserId);
            return await _sysRoleMenuService.GetRoleMenuIdList(roleIdList);
        }
    }
}