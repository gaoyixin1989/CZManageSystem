using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using System.Collections.Generic;
using ZManageSystem.Core;
using System;

namespace CZManageSystem.Service.SysManger
{
    public interface ISysMenuService:IBaseService<SysMenu>
    {
        SysMenu GetMenuById(int MenuId);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        // bool delMenuByIds(int[] menuids);
        IList<SysMenu> GetMenuByPid(int Pid);

        IList<SysMenu> getMenuByUser(string username,Guid userid);


    }
}

