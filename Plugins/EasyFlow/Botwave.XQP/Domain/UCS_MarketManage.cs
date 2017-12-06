using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.XQP.Domain
{
    #region 营销管理
    /// <summary>
    /// 营销管理
    /// </summary>
    /// 
    public class UCS_MarketManage
    {
        public string MARK_PLAN_CD{ get; set; }
        public string CMCC_BRANCH_CD{ get; set; }
        public string MARK_PLAN_NAM{ get; set; }
        public string CMCC_CODE { get; set; }
        public string START_DT{ get; set; }
        public string STOP_DT { get; set; }
        public string Product { get; set; }
        public string Principal { get; set; }
        public string ProgramIntro { get; set; }
        public string Productivity { get; set; }
        public decimal Resource { get; set; }
        public decimal PersonAverage { get; set; }
        public string Dptype { get; set; }
        public string RealName { get; set; }
        public decimal thresvalue { set; get; }
        public string batchname { set; get; }
    }
        #endregion
}
