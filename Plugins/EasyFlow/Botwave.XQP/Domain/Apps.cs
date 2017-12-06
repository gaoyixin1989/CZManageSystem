using System;
using System.Collections.Generic;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// [xqp_Apps] 的实体类.
    /// </summary>
    public class Apps
    {
        #region Getter / Setter

        private int appId;
        private string appName = String.Empty;
        private string password = String.Empty;
        private bool enabled;
        private string remark = String.Empty;
        private bool accessType;
        private string settings = String.Empty;
        private string creator = String.Empty;
        private string lastModifier = String.Empty;
        private DateTime createdTime;
        private DateTime lastModTime;
        private string accessUrl;

        /// <summary>
        /// ID.
        /// </summary>
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        /// <summary>
        /// 应用系统名称.
        /// </summary>
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        /// <summary>
        /// 密码.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// 是否有效.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// 备注.
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 访问类型.
        /// 0 WEB；1 WEBSERVICE
        /// </summary>
        public bool AccessType
        {
            get { return accessType; }
            set { accessType = value; }
        }

        /// <summary>
        /// 设置信息.
        /// </summary>
        public string Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        /// <summary>
        /// 接入地址.
        /// </summary>
        public string AccessUrl
        {
            get { return accessUrl; }
            set { accessUrl = value; }
        }

        /// <summary>
        /// 创建人.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 最后修改人.
        /// </summary>
        public string LastModifier
        {
            get { return lastModifier; }
            set { lastModifier = value; }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// 最后修改时间.
        /// </summary>
        public DateTime LastModTime
        {
            get { return lastModTime; }
            set { lastModTime = value; }
        }

        #endregion

        #region 数据操作

        /// <summary>
        /// 创建.
        /// </summary>
        /// <returns></returns>
        public int Create()
        {
            IBatisMapper.Insert("xqp_Apps_Insert", this);
            return this.AppId;
        }

        /// <summary>
        /// 更新.
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return IBatisMapper.Update("xqp_Apps_Update", this);
        }

        /// <summary>
        /// 删除指定 Apps.
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            return IBatisMapper.Delete("xqp_Apps_Delete", this.AppId);
        }

        /// <summary>
        /// 获取指定的 Apps 信息.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static Apps LoadById(int appId)
        {
            return IBatisMapper.Load<Apps>("xqp_Apps_Select", appId);
        }

        /// <summary>
        /// 获取指定的 Apps 信息.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static Apps LoadByName(string appName)
        {
            return IBatisMapper.Load<Apps>("xqp_Apps_Select_By_Name", appName);
        }

        /// <summary>
        /// 获取全部 Apps 信息.
        /// </summary>
        /// <returns></returns>
        public static IList<Apps> Select()
        {
            return IBatisMapper.Select<Apps>("xqp_Apps_Select");
        }

        /// <summary>
        /// 判断指定应用呈现名称是否存在.
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static bool IsExists(string appName)
        {
            return (IBatisMapper.Mapper.QueryForObject<int>("xqp_Apps_Select_IsExists", appName) > 0);
        }

        #endregion
    }
}

