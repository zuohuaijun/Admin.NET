using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 系统菜单服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Menu", Order = 146)]
    public class SysMenuService : ISysMenuService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysMenu> _sysMenuRep;  // 菜单表仓储  

        private readonly IUserManager _userManager;
        private readonly ISysCacheService _sysCacheService;
        private readonly ISysUserRoleService _sysUserRoleService;
        private readonly ISysRoleMenuService _sysRoleMenuService;

        public SysMenuService(IRepository<SysMenu> sysMenuRep,
                              IUserManager userManager,
                              ISysCacheService sysCacheService,
                              ISysUserRoleService sysUserRoleService,
                              ISysRoleMenuService sysRoleMenuService)
        {
            _sysMenuRep = sysMenuRep;
            _userManager = userManager;
            _sysCacheService = sysCacheService;
            _sysUserRoleService = sysUserRoleService;
            _sysRoleMenuService = sysRoleMenuService;
        }

        /// <summary>
        /// 获取用户权限(按钮权限标识集合)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<string>> GetLoginPermissionList(long userId)
        {
            var permissions = await _sysCacheService.GetPermission(userId); // 先从缓存里面读取
            if (permissions == null || permissions.Count < 1)
            {
                if (!_userManager.SuperAdmin)
                {
                    var roleIdList = await _sysUserRoleService.GetUserRoleIdList(userId);
                    var menuIdList = await _sysRoleMenuService.GetRoleMenuIdList(roleIdList);
                    permissions = await _sysMenuRep.DetachedEntities.Where(u => menuIdList.Contains(u.Id))
                                                                    .Where(u => u.Type == MenuType.BTN)
                                                                    .Where(u => u.Status == CommonStatus.ENABLE)
                                                                    .Select(u => u.Permission).ToListAsync();
                }
                else
                {
                    permissions = await _sysMenuRep.DetachedEntities
                                                   .Where(u => u.Type == MenuType.BTN)
                                                   .Where(u => u.Status == CommonStatus.ENABLE)
                                                   .Select(u => u.Permission).ToListAsync();
                }
                await _sysCacheService.SetPermission(userId, permissions); // 缓存结果
            }
            return permissions;
        }

        /// <summary>
        /// 获取用户AntDesign菜单集合
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<AntDesignTreeNode>> GetLoginMenusAntDesign(long userId, string appCode)
        {
            var antDesignTreeNodes = await _sysCacheService.GetMenu(userId, appCode); // 先从缓存里面读取
            if (antDesignTreeNodes == null || antDesignTreeNodes.Count < 1)
            {
                var sysMenuList = new List<SysMenu>();
                // 管理员则展示所有系统菜单
                if (_userManager.SuperAdmin)
                {
                    sysMenuList = await _sysMenuRep.DetachedEntities
                                                   .Where(u => u.Status == CommonStatus.ENABLE)
                                                   .Where(u => u.Application == appCode)
                                                   .Where(u => u.Type != MenuType.BTN)
                                                   //.Where(u => u.Weight != (int)MenuWeight.DEFAULT_WEIGHT)
                                                   .OrderBy(u => u.Sort).ToListAsync();
                }
                else
                {
                    // 非管理员则获取自己角色所拥有的菜单集合
                    var roleIdList = await _sysUserRoleService.GetUserRoleIdList(userId);
                    var menuIdList = await _sysRoleMenuService.GetRoleMenuIdList(roleIdList);
                    sysMenuList = await _sysMenuRep.DetachedEntities
                                                   .Where(u => menuIdList.Contains(u.Id))
                                                   .Where(u => u.Status == CommonStatus.ENABLE)
                                                   .Where(u => u.Application == appCode)
                                                   .Where(u => u.Type != MenuType.BTN)
                                                   .OrderBy(u => u.Sort).ToListAsync();
                }
                // 转换成登录菜单
                antDesignTreeNodes = sysMenuList.Select(u => new AntDesignTreeNode
                {
                    Id = u.Id,
                    Pid = u.Pid,
                    Name = u.Code,
                    Component = u.Component,
                    Redirect = u.OpenType == MenuOpenType.OUTER ? u.Link : u.Redirect,
                    Path = u.OpenType == MenuOpenType.OUTER ? u.Link : u.Router,
                    Meta = new Meta
                    {
                        Title = u.Name,
                        Icon = u.Icon,
                        Show = u.Visible == YesOrNot.Y.ToString(),
                        Link = u.Link,
                        Target = u.OpenType == MenuOpenType.OUTER ? "_blank" : ""
                    }
                }).ToList();
                await _sysCacheService.SetMenu(userId, appCode, antDesignTreeNodes); // 缓存结果
            }
            return antDesignTreeNodes;
        }

        /// <summary>
        /// 获取用户菜单所属的应用编码集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<string>> GetUserMenuAppCodeList(long userId)
        {
            var roleIdList = await _sysUserRoleService.GetUserRoleIdList(userId);
            var menuIdList = await _sysRoleMenuService.GetRoleMenuIdList(roleIdList);
            return await _sysMenuRep.DetachedEntities.Where(u => menuIdList.Contains(u.Id))
                                                     .Where(u => u.Status == CommonStatus.ENABLE)
                                                     .Select(u => u.Application).ToListAsync();
        }

        /// <summary>
        /// 系统菜单列表（树表）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysMenu/list")]
        public async Task<dynamic> GetMenuList([FromQuery] MenuInput input)
        {
            var application = !string.IsNullOrEmpty(input.Application?.Trim());
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var menus = await _sysMenuRep.DetachedEntities.Where((application, u => u.Application == input.Application.Trim()),
                                                                 (name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%")))
                                                          .Where(u => u.Status == CommonStatus.ENABLE).OrderBy(u => u.Sort)
                                                          .Select(u => u.Adapt<MenuOutput>())
                                                          .ToListAsync();
            return new TreeBuildUtil<MenuOutput>().DoTreeBuild(menus);
        }

        /// <summary>
        /// 创建Pids格式 
        /// 如果pid是0顶级节点，pids就是 [0];
        /// 如果pid不是顶级节点，pids就是 pid菜单的 pids + [pid] + ,
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private async Task<string> CreateNewPids(long pid)
        {
            if (pid == 0L)
            {
                return "[0],";
            }
            else
            {
                var pmenu = await _sysMenuRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == pid);
                return pmenu.Pids + "[" + pid + "],";
            }
        }

        /// <summary>
        /// 增加和编辑时检查参数
        /// </summary>
        /// <param name="input"></param>
        private static void CheckMenuParam(MenuInput input)
        {
            var type = input.Type;
            var router = input.Router;
            var permission = input.Permission;
            var openType = input.OpenType;

            if (type.Equals((int)MenuType.DIR))
            {
                if (string.IsNullOrEmpty(router))
                    throw Oops.Oh(ErrorCode.D4001);
            }
            else if (type.Equals((int)MenuType.MENU))
            {
                if (string.IsNullOrEmpty(router))
                    throw Oops.Oh(ErrorCode.D4001);
                if (string.IsNullOrEmpty(openType.ToString()))
                    throw Oops.Oh(ErrorCode.D4002);
            }
            else if (type.Equals((int)MenuType.BTN))
            {
                if (string.IsNullOrEmpty(permission))
                    throw Oops.Oh(ErrorCode.D4003);
                if (!permission.Contains(":"))
                    throw Oops.Oh(ErrorCode.D4004);
                // 判断该资源是否存在
                //permission = ":" + permission;
                //var urlSet = resourceCache.getAllResources();
                //if (!urlSet.Contains(permission.Replace(":","/")))
                //    throw Oops.Oh(ErrorCode.meu1005);                
            }
        }

        /// <summary>
        /// 增加系统菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysMenu/add")]
        public async Task AddMenu(AddMenuInput input)
        {
            var isExist = await _sysMenuRep.DetachedEntities.AnyAsync(u => u.Code == input.Code); // u.Name == input.Name
            if (isExist)
                throw Oops.Oh(ErrorCode.D4000);

            // 校验参数
            CheckMenuParam(input);

            var menu = input.Adapt<SysMenu>();
            menu.Pids = await CreateNewPids(input.Pid);
            menu.Status = CommonStatus.ENABLE;
            await menu.InsertAsync();

            // 清除缓存
            await _sysCacheService.DelByPatternAsync(CommonConst.CACHE_KEY_MENU);
        }

        /// <summary>
        /// 删除系统菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysMenu/delete")]
        [UnitOfWork]
        public async Task DeleteMenu(DeleteMenuInput input)
        {
            var childIdList = await _sysMenuRep.DetachedEntities.Where(u => u.Pids.Contains(input.Id.ToString()))
                                                                .Select(u => u.Id).ToListAsync();
            childIdList.Add(input.Id);

            var menus = await _sysMenuRep.Where(u => childIdList.Contains(u.Id)).ToListAsync();
            menus.ForEach(u =>
            {
                u.Delete();
            });
            // 级联删除该菜单及子菜单对应的角色-菜单表信息
            await _sysRoleMenuService.DeleteRoleMenuListByMenuIdList(childIdList);

            // 清除缓存
            await _sysCacheService.DelByPatternAsync(CommonConst.CACHE_KEY_MENU);
        }

        /// <summary>
        /// 更新系统菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysMenu/edit"),]
        public async Task UpdateMenu(UpdateMenuInput input)
        {
            // Pid和Id不能一致，一致会导致无限递归
            if (input.Id == input.Pid)
                throw Oops.Oh(ErrorCode.D4006);

            var isExist = await _sysMenuRep.DetachedEntities.AnyAsync(u => u.Code == input.Code && u.Id != input.Id); // u.Name == input.Name
            if (isExist)
                throw Oops.Oh(ErrorCode.D4000);

            // 校验参数
            CheckMenuParam(input);
            // 如果是编辑，父id不能为自己的子节点
            var childIdList = await _sysMenuRep.DetachedEntities.Where(u => u.Pids.Contains(input.Id.ToString()))
                                                                .Select(u => u.Id).ToListAsync();
            if (childIdList.Contains(input.Pid))
                throw Oops.Oh(ErrorCode.D4006);

            var oldMenu = await _sysMenuRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);

            // 生成新的pids
            var newPids = await CreateNewPids(input.Pid);

            // 是否更新子应用的标识
            var updateSubAppsFlag = false;
            // 是否更新子节点的pids的标识
            var updateSubPidsFlag = false;

            // 如果应用有变化
            if (input.Application != oldMenu.Application)
            {
                // 父节点不是根节点不能移动应用
                if (oldMenu.Pid != 0L)
                    throw Oops.Oh(ErrorCode.D4007);
                updateSubAppsFlag = true;
            }
            // 父节点有变化
            if (input.Pid != oldMenu.Pid)
                updateSubPidsFlag = true;

            // 开始更新所有子节点的配置
            if (updateSubAppsFlag || updateSubPidsFlag)
            {
                // 查找所有叶子节点，包含子节点的子节点
                var menuList = await _sysMenuRep.Where(u => EF.Functions.Like(u.Pids, $"%{oldMenu.Id}%")).ToListAsync();
                // 更新所有子节点的应用为当前菜单的应用
                if (menuList.Count > 0)
                {
                    // 更新所有子节点的application
                    if (updateSubAppsFlag)
                    {
                        menuList.ForEach(u =>
                        {
                            u.Application = input.Application;
                        });
                    }

                    // 更新所有子节点的pids
                    if (updateSubPidsFlag)
                    {
                        menuList.ForEach(u =>
                        {
                            // 子节点pids组成 = 当前菜单新pids + 当前菜单id + 子节点自己的pids后缀
                            var oldParentCodesPrefix = oldMenu.Pids + "[" + oldMenu.Id + "],";
                            var oldParentCodesSuffix = u.Pids.Substring(oldParentCodesPrefix.Length);
                            var menuParentCodes = newPids + "[" + oldMenu.Id + "]," + oldParentCodesSuffix;
                            u.Pids = menuParentCodes;
                        });
                    }
                }
            }

            // 更新当前菜单
            oldMenu = input.Adapt<SysMenu>();
            oldMenu.Pids = newPids;
            await oldMenu.UpdateAsync(ignoreNullValues: true);

            // 清除缓存
            await _sysCacheService.DelByPatternAsync(CommonConst.CACHE_KEY_MENU);
        }

        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysMenu/detail")]
        public async Task<dynamic> GetMenu(QueryMenuInput input)
        {
            return await _sysMenuRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
        }

        /// <summary>
        /// 获取系统菜单树，用于新增、编辑时选择上级节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysMenu/tree")]
        public async Task<dynamic> GetMenuTree([FromQuery] MenuInput input)
        {
            var application = !string.IsNullOrEmpty(input.Application?.Trim());
            var menus = await _sysMenuRep.DetachedEntities
                                         .Where(application, u => u.Application == input.Application.Trim())
                                         .Where(u => u.Status == CommonStatus.ENABLE)
                                         .Where(u => u.Type == MenuType.DIR || u.Type == MenuType.MENU)
                                         .OrderBy(u => u.Sort)
                                         .Select(u => new MenuTreeOutput
                                         {
                                             Id = u.Id,
                                             ParentId = u.Pid,
                                             Value = u.Id.ToString(),
                                             Title = u.Name,
                                             Weight = u.Weight
                                         }).ToListAsync();
            return new TreeBuildUtil<MenuTreeOutput>().DoTreeBuild(menus);
        }

        /// <summary>
        /// 获取系统菜单树，用于给角色授权时选择
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysMenu/treeForGrant")]
        public async Task<dynamic> TreeForGrant([FromQuery] MenuInput input)
        {
            var menuIdList = new List<long>();
            if (!_userManager.SuperAdmin)
            {
                var roleIdList = await _sysUserRoleService.GetUserRoleIdList(_userManager.UserId);
                menuIdList = await _sysRoleMenuService.GetRoleMenuIdList(roleIdList);
            }

            var application = !string.IsNullOrEmpty(input.Application?.Trim());
            var menus = await _sysMenuRep.DetachedEntities
                                         .Where(application, u => u.Application == input.Application.Trim())
                                         .Where(u => u.Status == CommonStatus.ENABLE)
                                         .Where(menuIdList.Count > 0, u => menuIdList.Contains(u.Id))
                                         .OrderBy(u => u.Sort).Select(u => new MenuTreeOutput
                                         {
                                             Id = u.Id,
                                             ParentId = u.Pid,
                                             Value = u.Id.ToString(),
                                             Title = u.Name,
                                             Weight = u.Weight
                                         }).ToListAsync();
            return new TreeBuildUtil<MenuTreeOutput>().DoTreeBuild(menus);
        }

        /// <summary>
        /// 根据应用编码判断该机构下是否有状态为正常的菜单
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<bool> HasMenu(string appCode)
        {
            return await _sysMenuRep.DetachedEntities.AnyAsync(u => u.Application == appCode && u.Status != CommonStatus.DELETED);
        }

        /// <summary>
        /// 根据系统应用切换菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("/sysMenu/change")]
        public async Task<List<AntDesignTreeNode>> ChangeAppMenu(ChangeAppMenuInput input)
        {
            return await GetLoginMenusAntDesign(_userManager.UserId, input.Application);
        }
    }
}
