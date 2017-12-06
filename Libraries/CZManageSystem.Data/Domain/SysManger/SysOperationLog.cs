using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class SysOperationLog
    {
        public ObjectId Id { get; set; }
        /// <summary>
        /// 操作：查看，添加、修改、上传、导入、导出、推送
        /// </summary>
        public OperationType Operation { get; set; }


        /// <summary>
        /// 操作时间
        /// </summary>
        public Nullable<DateTime> OperationTime { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string OperationDesc { get; set; }
        /// <summary>
        /// 操作页面
        /// </summary>
        public string OperationPage { get; set; }
        /// <summary>
        ///  用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 操作ip
        /// </summary>
        public string OperationIp { get; set; }

        public SysOperationLog()
        {
            OperationTime = DateTime.Now;
        }
    }


    public enum OperationType
    {
        [Description("浏览，用于标记列表访问")]
        Browse,
        [Description("查看")]
        View,
        [Description("添加")]
        Add,
        [Description("编辑")]
        Edit,
        [Description("保存")]
        Save,
        [Description("删除")]
        Delete,
        [Description("导入")]
        Import,
        [Description("导出")]
        Export,
        [Description("登录")]
        Loin,
        [Description("退出")]
        Logout,
        [Description("打印")]
        Print,
        [Description("设置")]
        Setting,
        [Description("推送")]
        Push

        //string view {
        //    set { value = "查看"; }
        //    get { return view; }
        //}
        //string add { get { return "添加"; } }
        //string edit { get { return "编辑"; } }
        //string delete { get { return "删除"; } }
        //string import { get { return "导入"; } }
        //string export { get { return "导入"; } }
        //string login { get { return "登录"; } }
        //string logout { get { return "退出"; } }
        //string print { get { return "打印"; } }
    }

}
