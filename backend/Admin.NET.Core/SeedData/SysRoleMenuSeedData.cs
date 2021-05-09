using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core.SeedData
{
    public class SysRoleMenuSeedData : IEntitySeedData<SysRoleMenu>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysRoleMenu> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                // 租户管理员角色默认有系统管理和业务应用菜单
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910563 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910581 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910582 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910583 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910584 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910585 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910586 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910587 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910588 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910589 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910590 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910591 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914629 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914630 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914631 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914632 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307000914633 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910564 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910565 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910566 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910567 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910568 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910569 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910570 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910571 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910572 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910573 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910574 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910575 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910576 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910577 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910578 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910579 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910580 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070918777 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914651 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914652 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914653 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914654 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914655 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914656 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914657 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914658 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914659 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914660 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914661 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910560 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910561 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070910562 },
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070914648 }, // 菜单授权树
                new SysRoleMenu{ SysRoleId=142307070910556, SysMenuId=142307070922874 }  // 头像上传
            };
        }
    }
}