using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>
    /// 流程步骤定义属性
    /// </summary>
    [Serializable]
    public class Activity
    { 
        /// <summary>  
        /// 步骤定义名称 
        /// </summary> 
        public string Name { get; set; }

        /// <summary>  
        /// 步骤定义处理人数组 
        /// </summary> 
        public string[] Actors { get; set; }

    }
}

