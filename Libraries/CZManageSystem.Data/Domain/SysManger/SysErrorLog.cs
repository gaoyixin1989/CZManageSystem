using MongoDB.Bson;
using System;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class SysErrorLog
    {
        public ObjectId Id { get; set; }
        /// <summary>
        /// 错误标题
        /// </summary>
        public string ErrorTitle { get; set; }
        /// <summary>
        /// 错误时间
        /// </summary>
        public Nullable<DateTime> ErrorTime {get;set;}
        /// <summary>
        /// 错误描述
        /// </summary>
        public string ErrorDesc { get; set; }
        /// <summary>
        /// 错误页面
        /// </summary>
        public string ErrorPage { get; set; }
        /// <summary>
        ///  用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        public SysErrorLog()
        {
            ErrorTime = DateTime.Now;
        }
    }
}
