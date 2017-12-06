using System;

namespace Botwave.XQP.API.Entity
{
    [Serializable]
    public class ApiResult
    {
        /// <summary>  
        /// 应用系统验证结果. 
        /// </summary> 
        public int AppAuth { get; set; }

        /// <summary>  
        /// 返回结果的消息内容 
        /// </summary> 
        public string Message { get; set; }

        /// <summary>
        /// 成功 (1) OR 失败(-1)
        /// </summary>
        public string Success { get; set; }
    }
}
