using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Service
{
    public interface ISaveAsAPIService
    {
        /// <summary>
        /// 查询接入页面是否有保存
        /// </summary>
        /// <param name="workflowinstanceid">工单ID</param>
        /// <param name="page">页面</param>
        /// <returns></returns>
         int SelectSystemPage(string workflowinstanceid, string page);

        /// <summary>
        /// 添加保存状态
        /// </summary>
        /// <param name="workflowinstanceid"></param>
        /// <param name="page"></param>
         void InsertSystemPage(string workflowinstanceid, string page,int state);

       /// <summary>
       /// 修改保存状态
       /// </summary>
       /// <param name="workflowinstanceid"></param>
       /// <param name="page"></param>
         void UpdateSystemPage(string workflowinstanceid, string page,int state);
    }
}
