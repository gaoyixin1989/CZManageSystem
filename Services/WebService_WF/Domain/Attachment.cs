using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_WF.Domain
{
    /// <summary>
    /// 工单附件属性
    /// </summary>
    [Serializable]
    public class Attachment
    {
        /// <summary>  
        /// 附件显示名称 
        /// </summary> 
        public string Name { get; set; }

        /// <summary>  
        /// 附件显示中文名称 
        /// </summary> 
        public string UserName { get; set; }

        /// <summary>  
        /// 附件实际下载地址URL 
        /// </summary> 
        public string Url { get; set; }

        /// <summary>  
        /// 上传人 
        /// </summary> 
        public string Creator { get; set; }

        /// <summary>  
        /// 上传时间 
        /// </summary> 
        public string CreatedTime { get; set; }
    }
}