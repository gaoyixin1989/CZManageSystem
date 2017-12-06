using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Botwave.Workflow.Practices.CZMCC
{
    /// <summary>
    /// 销售精英竞赛酬金申告流程接口
    /// </summary>
    public interface IGratuityService
    {
        /// <summary>
        /// 提交申告单
        /// </summary>
        /// <param name="title">申请单标题</param>
        /// <param name="fromEmployeeId">发起人工号</param>
        /// <param name="fromEmployeeName">发起人姓名</param>
        /// <param name="toEmployeeId">处理人工号</param>
        /// <param name="fileUrl">附件地址</param>
        /// <param name="detailUrl">详细信息地址</param>
        /// <param name="applyStyle">卡类型</param>
        /// <param name="description">原因描述</param>
        /// <returns></returns>
        bool SendGratuityFlow(string title, string fromEmployeeId, string fromEmployeeName, string toEmployeeId, string fileUrl, string detailUrl, int applyStyle, string description);

        /// <summary>
        /// 获取申告单列表
        /// </summary>
        /// <param name="employeeId">省工号</param>
        /// <returns></returns>
        DataSet ApplyListDs(string employeeId);

        /// <summary>
        /// 获取申告单信息
        /// </summary>
        /// <param name="applyId">单号/受理号</param>
        /// <returns></returns>
        DataSet ApplyRowDs(string applyId);

    }
}
