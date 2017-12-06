using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// API系统注册类
    /// </summary>
    public class CZRegistSystem
    {
        public Guid SystemId { get; set; }
        public string SystemName { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public int Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public DateTime LastModTime { get; set; }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="item"></param>
        public static void Insert(CZRegistSystem item)
        {
            IBatisMapper.Insert("CZ_RegistSystem_Insert", item);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="item"></param>
        public static int Update(CZRegistSystem item)
        {
           return IBatisMapper.Update("CZ_RegistSystem_Update", item);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="item"></param>
        public static CZRegistSystem SelectById(Guid Id)
        {
            return IBatisMapper.Load<CZRegistSystem>("CZ_RegistSystem_Select_Id", Id);
        }

        /// <summary>
        /// 根据系统名获取数据
        /// </summary>
        /// <param name="item"></param>
        public static CZRegistSystem SelectByName(string name)
        {
            return IBatisMapper.Load<CZRegistSystem>("CZ_RegistSystem_Select_Name", name);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="item"></param>
        public static IList<CZRegistSystem> Select()
        {
            return IBatisMapper.Select<CZRegistSystem>("CZ_RegistSystem_Select", null);
        }
    }
}
