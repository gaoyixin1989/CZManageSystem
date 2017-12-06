using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Data;

namespace Botwave.Workflow.Practices.CZMCC
{
    /// <summary>
    /// Summary description for GratuityFlowService
    /// </summary>
    [WebService(Namespace = "http://www.botwave.com/", Name = "GratuityFlowService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GratuityFlowService : System.Web.Services.WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(GratuityFlowService));

        private IGratuityService gratuityService;

        public IGratuityService GratuityService
        {
            set { gratuityService = value; }
        }

        public GratuityFlowService()
        {
            Spring.Context.IApplicationContext applicationContext = Spring.Context.Support.ContextRegistry.GetContext();
            gratuityService = applicationContext.GetObject("gratuityService") as IGratuityService;
            if (null == gratuityService)
            {
                log.Error("the gratuityService is null");
            }
        }

        #region IGratuityFlowService Members
        
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
        [WebMethod]
        public bool SendGratuityFlow(string title, string fromEmployeeId, string fromEmployeeName, string toEmployeeId, string fileUrl, string detailUrl, int applyStyle, string description)
        {
            return gratuityService.SendGratuityFlow(title, fromEmployeeId, fromEmployeeName, toEmployeeId, fileUrl, detailUrl, applyStyle, description);
        }

        /// <summary>
        /// 获取申告单列表
        /// </summary>
        /// <param name="employeeId">省工号</param>
        /// <returns></returns>
        [WebMethod]
        public System.Data.DataSet ApplyListDs(string employeeId)
        {
            log.Info("invoke GratuityFlowService[ApplyListDs]:" + employeeId);
            return gratuityService.ApplyListDs(employeeId);
        }

        /// <summary>
        /// 获取申告单信息
        /// </summary>
        /// <param name="applyId">单号/受理号</param>
        /// <returns></returns>
        [WebMethod]
        public System.Data.DataSet ApplyRowDs(string applyId)
        {
            log.Info("invoke GratuityFlowService[ApplyRowDs]:" + applyId);
            return gratuityService.ApplyRowDs(applyId);
        }

        #endregion        
    }
}