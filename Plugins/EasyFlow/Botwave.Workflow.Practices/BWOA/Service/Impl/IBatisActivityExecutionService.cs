using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using Botwave.Workflow.Domain;
using Botwave.Workflow;
using Botwave.Workflow.Practices.BWOA.Domain;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Domain;
using Botwave.XQP.Domain;

namespace Botwave.Workflow.Practices.BWOA.Service.Impl
{
    public class IBatisActivityExecutionService 
    {
        #region 字段

        /// <summary>
        /// 邮件信息类型值.
        /// </summary>
        private const int EmailMessageType = 1;
        /// <summary>
        /// 短信信息类型值.
        /// </summary>
        private const int SMSMessageType = 2;

        #endregion
        
        #region IWorkflowReportService Members

        #region === 获取日常报销数据 ===
        /// <summary>
        /// 获取日常报销数据
        /// </summary>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldShow"></param>
        /// <param name="fieldGroup"></param>
        /// <param name="stWhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetApplyInfoPage(string startDT, string endDT, int pageIndex, int pageSize, string fieldShow, string fieldGroup,string fieldOrder, string stWhere, ref int recordCount)
        {
            string tableName = "vw_bwwf_ApplyInfo";
            string fieldKey = "ID";           
            string strWhere = "";
            StringBuilder where = new StringBuilder();

            if (!string.IsNullOrEmpty(stWhere))
                where.AppendFormat("{0} ", stWhere);           

            if (where.ToString().Length > 0)
            {
                strWhere = "1=1 " + where.ToString() + fieldGroup;
            }
            else
            {
                strWhere = "1=1 " + fieldGroup;
            }

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, strWhere, ref recordCount);
            return dt;
        }
        #endregion 

        #region === 获取手机通讯费数据 ===
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strwhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetApplicationInfoPage(Guid? workflowId, string startDT, string endDT, int pageIndex, int pageSize, string strwhere, ref int recordCount)
        {
            string tableName = "vw_bwwf_ApplicationInfo";
            string fieldKey = "WorkFlowID";
            string fieldOrder = "ApplyDate DESC";
            string fieldShow = "WorkFlowID,WorkflowName,ApplyName,ApplyDate,DateName,BudgetMan,ApplyMoney,ApplyType,Explain";

            StringBuilder where = new StringBuilder();

            if (workflowId.HasValue)
                where.AppendFormat("(WorkFlowID = '{0}')", workflowId.Value.ToString());

            if (!string.IsNullOrEmpty(startDT))
                where.AppendFormat(" AND (ApplyDate >= CAST('{0}' AS datetime))", startDT);
            if (!string.IsNullOrEmpty(endDT))
                where.AppendFormat(" AND (ApplyDate <= CAST('{0}' AS datetime))", endDT);
            if (!string.IsNullOrEmpty(strwhere))
                where.AppendFormat(" AND {0}", strwhere);

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }
        #endregion 

        #region === 获取假期数据 ===
        /// <summary>
        /// 获取假期数据
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strwhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetLeaveInfoPage(Guid? workflowId, string startDT, string endDT, int pageIndex, int pageSize, string strwhere, ref int recordCount)
        {
            string tableName = "vw_bwwf_LeaveInfo";
            string fieldKey = "WorkFlowID";
            string fieldOrder = "CreateDT DESC";
            string fieldShow = "WorkFlowID,WorkflowName,ApplyName,LeaveType,TotalDay,CreateDT,StartDT,EndDT";
            strwhere += " and LeaveType<>'overtime'";
            StringBuilder where = new StringBuilder();

            //if (workflowId.HasValue)
            //    where.AppendFormat("(WorkFlowID = '{0}')", workflowId.Value.ToString());

            if (!string.IsNullOrEmpty(startDT))
                where.AppendFormat(" (CreateDT >= CAST('{0}' AS datetime))", startDT);
            if (!string.IsNullOrEmpty(endDT))
                where.AppendFormat(" AND (CreateDT <= CAST('{0}' AS datetime))", endDT);
            if (!string.IsNullOrEmpty(strwhere))
                where.AppendFormat(" AND {0}", strwhere);

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }
        #endregion 

        #region === 获取加班数据 ===
        /// <summary>
        /// 获取加班数据
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strwhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetOverTimeInfoPage(Guid? workflowId, string startDT, string endDT, int pageIndex, int pageSize, string strwhere, ref int recordCount)
        {
            string tableName = "vw_bwwf_LeaveInfo";
            string fieldKey = "WorkFlowID";
            string fieldOrder = "CreateDT DESC";
            string fieldShow = "WorkFlowID,WorkflowName,ApplyName,LeaveType,TotalDay,CreateDT,StartDT,EndDT";
            strwhere += " and LeaveType='overtime'";
            StringBuilder where = new StringBuilder();

            //if (workflowId.HasValue)
            //    where.AppendFormat("(WorkFlowID = '{0}')", workflowId.Value.ToString());

            if (!string.IsNullOrEmpty(startDT))
                where.AppendFormat(" (CreateDT >= CAST('{0}' AS datetime))", startDT);
            if (!string.IsNullOrEmpty(endDT))
                where.AppendFormat(" AND (CreateDT <= CAST('{0}' AS datetime))", endDT);
            if (!string.IsNullOrEmpty(strwhere))
                where.AppendFormat(" AND {0}", strwhere);

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }
        #endregion 

        #region === 获取采购数据 ===
        /// <summary>
        /// 获取采购数据
        /// </summary>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldShow"></param>
        /// <param name="fieldGroup"></param>
        /// <param name="stWhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetPurchaseInfoPage(string startDT, string endDT, int pageIndex, int pageSize, string fieldShow, string fieldGroup, string fieldOrder, string stWhere, ref int recordCount)
        {
            string tableName = "bw_PurChaseInfo";
            string fieldKey = "ID";
            string strWhere = "";
            StringBuilder where = new StringBuilder();

            if (!string.IsNullOrEmpty(stWhere))
                where.AppendFormat("{0} ", stWhere);

            if (where.ToString().Length > 0)
            {
                strWhere = "1=1 " + where.ToString() + fieldGroup;
            }
            else
            {
                strWhere = "1=1 " + fieldGroup;
            }

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, strWhere, ref recordCount);
            return dt;
        }
        #endregion 

        #region === 获取系统部署数据 ===
        /// <summary>
        /// 获取系统部署数据
        /// </summary>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldShow"></param>
        /// <param name="fieldGroup"></param>
        /// <param name="stWhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetSysDeployInfoPage(string startDT, string endDT, int pageIndex, int pageSize, string fieldShow, string fieldGroup, string fieldOrder, string stWhere, ref int recordCount)
        {
            string tableName = "bw_SysDeployInfo";
            string fieldKey = "ID";
            string strWhere = "";
            StringBuilder where = new StringBuilder();

            if (!string.IsNullOrEmpty(stWhere))
                where.AppendFormat("{0} ", stWhere);

            if (where.ToString().Length > 0)
            {
                strWhere = "1=1 " + where.ToString() + fieldGroup;
            }
            else
            {
                strWhere = "1=1 " + fieldGroup;
            }

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, strWhere, ref recordCount);
            return dt;
        }
        #endregion 

        #region === 获取文档验收数据 ===
        /// <summary>
        /// 获取文档验收数据
        /// </summary>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldShow"></param>
        /// <param name="fieldGroup"></param>
        /// <param name="stWhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetDocInspectInfoPage(string startDT, string endDT, int pageIndex, int pageSize, string fieldShow, string fieldGroup, string fieldOrder, string stWhere, ref int recordCount)
        {
            string tableName = "bw_DocInspectInfo";
            string fieldKey = "ID";
            string strWhere = "";
            StringBuilder where = new StringBuilder();

            if (!string.IsNullOrEmpty(stWhere))
                where.AppendFormat("{0} ", stWhere);

            if (where.ToString().Length > 0)
            {
                strWhere = "1=1 " + where.ToString() + fieldGroup;
            }
            else
            {
                strWhere = "1=1 " + fieldGroup;
            }

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, strWhere, ref recordCount);
            return dt;
        }
        #endregion 

        public IList<WorkflowDefinition> GetWorkflowDefinitionListNew()
        {
            return IBatisMapper.Select<WorkflowDefinition>("bwwf_Workflows_SelectNew", null);
        }
        

        /// <summary>
        /// 获取待阅的通知人

        /// </summary>
        /// <param name="workFlowID"></param>
        /// <returns></returns>
        public IList<UserInfo> GetNotifyActors(Guid workFlowID)
        {
            return IBatisMapper.Select<UserInfo>("oa_WorkflowNotify_Select_NotifyUser", workFlowID);
        }
        #endregion      

        #region === 保存流水帐 ===

        #region === 保存报销流水帐 ===
        public void SaveFlowWaterAccount(WorkflowInstance workflowInstance, ActivityExecutionContext context)
        {
            ApplyStat apply = new ApplyStat();
            ApplicationInfo aInfo = new ApplicationInfo();
            try
            {
                apply.ApplyName = context.Variables["txtExpenseUser"].ToString();
                apply.ApplyDate = DateTime.Parse(context.Variables["txtExpenseDate"].ToString());
                int k = 0;

                string happDate = "HappenDateN";
                string dept = "DeptsN";
                string invoice = "InvoiceN";
                string receipt = "ReceiptN";
                string cheque = "ChequeN";
                string summary = "SummaryN";
                string bankNum = "BankNumN";
                string cash = "CashN";
                string BudgetMan = "BudgetManN";
                string AppType = "AppTypeN";
                string Remark = "RemarkN";
                apply.WorkFlowInstanceID = workflowInstance.WorkflowInstanceId;

                for (int i = 4; i < context.Variables.Count - 7; i += 9)
                {
                    apply.Depts = context.Variables[dept + k.ToString()].ToString();
                    apply.InvoiceNum = context.Variables[invoice + k.ToString()].ToString();                
                    //apply.RreceiptNum = context.Variables[receipt + k.ToString()].ToString();
                    //apply.ChequeNum = context.Variables[cheque + k.ToString()].ToString();
                    apply.RreceiptNum = "";
                    apply.ChequeNum = "";
                    apply.Summary = context.Variables[summary + k.ToString()].ToString();
                    //apply.BankNum = (context.Variables[bankNum + k.ToString()].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables[bankNum + k.ToString()].ToString());
                    apply.BankNum = 0;
                    apply.CashNum = (context.Variables[cash + k.ToString()].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables[cash + k.ToString()].ToString());
                    apply.BudgetMan = context.Variables[BudgetMan + k.ToString()].ToString();
                    apply.ApplyType = context.Variables[AppType + k.ToString()].ToString();
                    apply.Remark = context.Variables[Remark + k.ToString()].ToString();
                    apply.HappenDate = Convert.ToDateTime(context.Variables[happDate + k.ToString()].ToString());

                    SaveApplyInfo(apply);

                    k += 1;
                }
            }
            catch
            { }
            try
            {
                aInfo.ID = workflowInstance.WorkflowId;
                aInfo.WorkFlowInstanceID = workflowInstance.WorkflowInstanceId;
                aInfo.ApplyName = context.Variables["F1"].ToString();
                aInfo.ApplyDate = Convert.ToDateTime(context.Variables["F2"].ToString());
                aInfo.DeptName = context.Variables["F8"].ToString();
                aInfo.BudgetMan = context.Variables["F4"].ToString();
                aInfo.ApplyMoney = (context.Variables["F5"].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables["F5"].ToString());
                aInfo.ApplyType = context.Variables["F6"].ToString();
                aInfo.Explain = context.Variables["F7"].ToString();

                if (context.Variables.ContainsKey("F10"))
                {
                    aInfo.ApplyMoney = (context.Variables["F10"].ToString() == "") ? Convert.ToDecimal(context.Variables["F5"].ToString()) : Convert.ToDecimal(context.Variables["F10"].ToString());
                    aInfo.BeginDate = Convert.ToDateTime(context.Variables["F11"].ToString());
                    aInfo.EndDate = Convert.ToDateTime(context.Variables["F12"].ToString());

                    SaveApplicationInfoNew(aInfo);
                }
                else
                {
                    SaveApplicationInfo(aInfo);
                }
            }
            catch { }
        }
#endregion 

        #region 
        private void SaveApplyInfo(ApplyStat apply)
        {
            Hashtable ht = new Hashtable();
            Guid applyID = Guid.NewGuid();

            ht.Add("ID", applyID);
            ht.Add("ApplyDate", apply.ApplyDate);
            ht.Add("ApplyType", apply.ApplyType);
            ht.Add("ApplyName", apply.ApplyName);
            ht.Add("Depts", apply.Depts);
            ht.Add("InvoiceNum", apply.InvoiceNum);
            ht.Add("RreceiptNum", apply.RreceiptNum);
            ht.Add("ChequeNum", apply.ChequeNum);
            ht.Add("Summary", apply.Summary);
            ht.Add("BankNum", apply.BankNum);
            ht.Add("CashNum", apply.CashNum);
            ht.Add("BudgetMan", apply.BudgetMan);
            ht.Add("Remark", apply.Remark);
            ht.Add("HappenDate", apply.HappenDate);
            ht.Add("WorkFlowInstanceID", apply.WorkFlowInstanceID);

            IBatisMapper.Insert("bwwf_ApplyInfo_Insert", ht);
        }

        private void SaveApplicationInfoNew(ApplicationInfo aInfo)
        {
            Hashtable ht = new Hashtable();
            //Guid applyID = Guid.NewGuid();

            ht.Add("ID", aInfo.WorkFlowInstanceID);
            ht.Add("WorkFlowID", aInfo.ID);
            ht.Add("ApplyName", aInfo.ApplyName);
            ht.Add("ApplyDate", aInfo.ApplyDate);
            ht.Add("DateName", aInfo.DeptName);
            ht.Add("BudgetMan", aInfo.BudgetMan);
            ht.Add("ApplyMoney", aInfo.ApplyMoney);
            ht.Add("ApplyType", aInfo.ApplyType);
            ht.Add("Explain", aInfo.Explain);
            ht.Add("BeginDate", aInfo.BeginDate);
            ht.Add("EndDate", aInfo.EndDate);

            IBatisMapper.Insert("bwwf_ApplicationInfo_InsertNew", ht);
        }

        private void SaveApplicationInfo(ApplicationInfo aInfo)
        {
            Hashtable ht = new Hashtable();
            //Guid applyID = Guid.NewGuid();

            ht.Add("ID", aInfo.WorkFlowInstanceID);
            ht.Add("WorkFlowID", aInfo.ID);
            ht.Add("ApplyName", aInfo.ApplyName);
            ht.Add("ApplyDate", aInfo.ApplyDate);
            ht.Add("DateName", aInfo.DeptName);
            ht.Add("BudgetMan", aInfo.BudgetMan);
            ht.Add("ApplyMoney", aInfo.ApplyMoney);
            ht.Add("ApplyType", aInfo.ApplyType);
            ht.Add("Explain", aInfo.Explain);

            IBatisMapper.Insert("bwwf_ApplicationInfo_Insert", ht);
        }
        #endregion 

        #region === 保存采购流水账 ===
        //保存采购流水账

        public void SavePurchaseWaterAccount(Guid workflowId, Guid workflowInstanceId, ActivityExecutionContext context)
        {
            PurchaseInfo pInfo = new PurchaseInfo();
            pInfo.WorkFlowID = workflowId;
            pInfo.PurchaseMan = context.Variables["txtExpenseUser"].ToString();
            pInfo.PurchaseDate = DateTime.Parse(context.Variables["txtExpenseDate"].ToString());
            pInfo.Tel = context.Variables["txtTel"].ToString();
            pInfo.WorkFlowInstanceID = workflowInstanceId;

            int k = 0, m = 0 ;
            //
            if (!context.Variables.ContainsKey("A4"))
            {
                try
                {
                    string waterID = "NumN";
                    string equipmentName = "EquipmentN";
                    string quantity = "QuantityN";
                    string unitPrice = "CashN";
                    string total = "TotalN";
                    string remark = "RemarkN";                    

                    //依次循环行获取表单数据

                    for (int i = 3; i < context.Variables.Count - 7; i += 6)
                    {
                        Guid pID = Guid.NewGuid();
                        pInfo.ID = pID;
                        pInfo.WaterID = context.Variables[waterID + k.ToString()].ToString();
                        pInfo.EquipmentName = context.Variables[equipmentName + k.ToString()].ToString();
                        pInfo.Quantity = (context.Variables[quantity + k.ToString()].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables[quantity + k.ToString()].ToString());
                        pInfo.UnitPrice = (context.Variables[unitPrice + k.ToString()].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables[unitPrice + k.ToString()].ToString());
                        pInfo.Total = (context.Variables[total + k.ToString()].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables[total + k.ToString()].ToString());
                        pInfo.Remark = context.Variables[remark + k.ToString()].ToString();

                        SavePurChaseInfo(pInfo);

                        k += 1;
                    }
                }
                catch
                {

                }
            }
            else
            {
                string oldWaterID = "NumN";
                string newWaterID = "Num2N";
                string equipmentName = "Equipment2N";
                string quantity = "Quantity2N";
                string unitPrice = "Cash2N";
                string total = "Total2N";
                string remark = "Remark2N";

                //先判断提单时有多少行
                for (int i = 3; i < context.Variables.Count - 8; i += 6)
                {
                    if (context.Variables.ContainsKey(oldWaterID + k.ToString()))
                    {
                        k += 1;
                    }
                }

                //从5+k*6开始循环获取行数据
                for (int j = 5+k*6; j < context.Variables.Count - 8; j += 6)
                {
                    Guid pID = Guid.NewGuid();
                    pInfo.ID = pID;
                    pInfo.WaterID = context.Variables[newWaterID + m.ToString()].ToString();
                    pInfo.EquipmentName = context.Variables[equipmentName + m.ToString()].ToString();
                    pInfo.Quantity = (context.Variables[quantity + m.ToString()].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables[quantity + m.ToString()].ToString());
                    pInfo.UnitPrice = (context.Variables[unitPrice + m.ToString()].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables[unitPrice + m.ToString()].ToString());
                    pInfo.Total = (context.Variables[total + m.ToString()].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables[total + m.ToString()].ToString());
                    pInfo.Remark = context.Variables[remark + m.ToString()].ToString();

                    SavePurChaseInfo(pInfo);

                    m += 1;
                }
            }
        }
        

        //插入采购信息
        private void SavePurChaseInfo(PurchaseInfo pInfo)
        {
            Hashtable ht = new Hashtable();
            //Guid applyID = Guid.NewGuid();

            ht.Add("ID", pInfo.ID);
            ht.Add("workFlowID", pInfo.WorkFlowID);
            ht.Add("workFlowinstanceID", pInfo.WorkFlowInstanceID);
            ht.Add("waterID", pInfo.WaterID );
            ht.Add("equipmentName", pInfo.EquipmentName );
            ht.Add("quantity", pInfo.Quantity);
            ht.Add("unitPrice", pInfo.UnitPrice );
            ht.Add("total", pInfo.Total );
            ht.Add("purchaseDate", pInfo.PurchaseDate );
            ht.Add("remark", pInfo.Remark );
            ht.Add("purchaseMan", pInfo.PurchaseMan );
            ht.Add("tel", pInfo.Tel);

            IBatisMapper.Insert("bwwf_PurChaseInfo_Insert", ht);
        }
        #endregion

        #region === 保存系统部署流水账 ===

        //保存系统部署流水账

        public void SaveSysDeployWaterAccount(Guid workflowId, Guid workflowInstanceId, string title, ActivityExecutionContext context)
        {
            SysDeployInfo sdInfo = new SysDeployInfo();
            try
            {
                sdInfo.WorkFlowID = workflowId;
                sdInfo.WorkFlowInstanceID = workflowInstanceId;
                sdInfo.Title = title;
                sdInfo.Deployer = context.Variables["F1"].ToString();
                sdInfo.EnglishName  = context.Variables["F6"].ToString();
                sdInfo.DeployDT  = DateTime.Parse(context.Variables["F2"].ToString());
                sdInfo.DeptName  = context.Variables["F8"].ToString();
                sdInfo.PM  = context.Variables["F4"].ToString();
                sdInfo.Remark  = context.Variables["F5"].ToString();
                sdInfo.URL = context.Variables["F9"].ToString();
                sdInfo.Reason = context.Variables["F10"].ToString();
                sdInfo.DoWay = context.Variables["F11"].ToString();
                sdInfo.FeedBack = context.Variables["F7"].ToString();
                sdInfo.Result  =context.Variables["F12"].ToString();
                sdInfo.CreateDT = DateTime.Now;

                SaveSysDeployInfo(sdInfo);
            }
            catch
            {

            }
        }

        //插入系统部署信息
        private void SaveSysDeployInfo(SysDeployInfo sdInfo)
        {
            Hashtable ht = new Hashtable();
            Guid ID = Guid.NewGuid();
            ht.Add("ID", ID);
            ht.Add("workFlowID", sdInfo.WorkFlowID);
            ht.Add("title", sdInfo.Title);
            ht.Add("workFlowinstanceID", sdInfo.WorkFlowInstanceID);
            ht.Add("deployer", sdInfo.Deployer);
            ht.Add("englishName", sdInfo.EnglishName);
            ht.Add("deployDT", sdInfo.DeployDT );
            ht.Add("deptName", sdInfo.DeptName);
            ht.Add("PM", sdInfo.PM );
            ht.Add("remark", sdInfo.Remark);
            ht.Add("url", sdInfo.URL );
            ht.Add("reason", sdInfo.Reason );
            ht.Add("doWay", sdInfo.DoWay);
            ht.Add("feedBack", sdInfo.FeedBack);
            ht.Add("result", sdInfo.Result);
            ht.Add("createDT", sdInfo.CreateDT);

            IBatisMapper.Insert("bwwf_SysDeployInfo_Insert", ht);
        }
        #endregion 

        #region === 保存文档验收流水账 ===

        //保存文档验收流水账

        public void SaveDocInspectWaterAccount(Guid workflowId, Guid workflowInstanceId, string title, ActivityExecutionContext context)
        {
            DocInspectInfo diInfo = new DocInspectInfo();
            try
            {
                diInfo.WorkFlowID = workflowId;
                diInfo.WorkFlowInstanceID = workflowInstanceId;
                diInfo.Title = title;
                diInfo.Sender = context.Variables["F1"].ToString();
                diInfo.EnglishName = context.Variables["F6"].ToString();
                diInfo.SendDT = DateTime.Parse(context.Variables["F2"].ToString());
                diInfo.DeptName = context.Variables["F8"].ToString();
                diInfo.PM = context.Variables["F4"].ToString();
                diInfo.Remark = context.Variables["F5"].ToString();
                diInfo.DocType = context.Variables["F9"].ToString();
                diInfo.CreateDT = DateTime.Now;

                SaveDocInspectInfo(diInfo);
            }
            catch
            {

            }
        }

        //插入文档验收信息
        private void SaveDocInspectInfo(DocInspectInfo diInfo)
        {
            Hashtable ht = new Hashtable();
            Guid ID = Guid.NewGuid();
            ht.Add("ID", ID);
            ht.Add("workFlowID", diInfo.WorkFlowID);
            ht.Add("title", diInfo.Title);
            ht.Add("workFlowinstanceID", diInfo.WorkFlowInstanceID);
            ht.Add("sender", diInfo.Sender );
            ht.Add("englishName", diInfo.EnglishName);
            ht.Add("sendDT", diInfo.SendDT );
            ht.Add("deptName", diInfo.DeptName);
            ht.Add("PM", diInfo.PM);
            ht.Add("remark", diInfo.Remark);
            ht.Add("docType", diInfo.DocType);
            ht.Add("createDT", diInfo.CreateDT);

            IBatisMapper.Insert("bwwf_DocInspectInfo_Insert", ht);
        }
        #endregion 

        public DataTable GetDeptProjectInfo()
        {
            DataTable dt;
            string sql = " SELECT ID,ProjectManagerID,ProjectID,ProjectName,State,UserName,EnglishName,ProjectCode,ProjectCatalog FROM bw_DeptProjectInfo WHERE (State !=4 And IsExpense=1) OR (ProjectCatalog='非项目类') order by ProjectCode asc ";
            dt = IBatisDbHelper.ExecuteDataset(System.Data.CommandType.Text, sql).Tables[0];

            return dt;
        }

        //根据用户名获取采购工单

        public DataTable GetPurchaseInfoByUserId(string userId)
        {
            DataTable dt;
            string sql = " select bwwf_Tracking_Workflows.Title ,creator,bw_PurChaseInfo.workflowinstanceid,sum(total) from bw_PurChaseInfo ";
            sql += " left join dbo.bwwf_Tracking_Workflows ";
            sql += " on bw_PurChaseInfo.workflowinstanceid = dbo.bwwf_Tracking_Workflows.workflowinstanceid";
            sql += " where creator= '" + userId + "'";
            sql += " group by title,creator,bw_PurChaseInfo.workflowinstanceid,FinishedTime order by FinishedTime desc";
            dt = IBatisDbHelper.ExecuteDataset(System.Data.CommandType.Text, sql).Tables[0];

            return dt;
        }

        //获取年假天数
        public DataSet GetTotalYearInfo(string userName)
        {
            Botwave.WebServiceClients.HRAttendanceService hrattendance = Botwave.WebServiceClients.ServiceFactory.GetHRAttendanceService();

            return hrattendance.GetYearByMonthAndUserName("botwave", "password", userName, DateTime.Now.Year, DateTime.Now.Month);
            return null;
        }

        #endregion

        #region === 保存还款流水帐 ===

        public void SaveRepaymentWaterAccount(Guid ActivityID,ActivityExecutionContext context)
        {
            RepaymentInfo ri = new RepaymentInfo();
            try
            {
                string payDate = "PayDateN";
                string payMan = "PayManN";
                string payNum = "PayNumN";
                string oweNum = "OweNumN";
                string explain = "ExplainN";

                int k = 0;

                for (int  i= 9; i < context.Variables.Count;i += 5 )
                {
                    Guid exID = Guid.NewGuid();

                    ri.ID = exID;
                    ri.ActivityID = ActivityID;
                    ri.PayDate = Convert.ToDateTime(context.Variables[payDate + k.ToString()]);
                    ri.PayMan = context.Variables[payMan + k.ToString()].ToString();
                    ri.payNum = context.Variables[payNum + k.ToString()].ToString();
                    ri.ArrearsNum = context.Variables[oweNum + k.ToString()].ToString();
                    ri.Explain = context.Variables[explain + k.ToString()].ToString();
                
                    SaveRepaymentInfo(ri);

                    k ++;
                }
            }
            catch
            {
            }

        }

        private void SaveRepaymentInfo(RepaymentInfo rInfo)
        {
            Hashtable ht = new Hashtable();

            ht.Add("ID", rInfo.ID);
            ht.Add("ActivityID", rInfo.ActivityID);
            ht.Add("PayDate", rInfo.PayDate);
            ht.Add("PayMan", rInfo.PayMan);
            ht.Add("payNum", rInfo.payNum);
            ht.Add("ArrearsNum", rInfo.ArrearsNum);
            ht.Add("Explain", rInfo.Explain);

            IBatisMapper.Insert("bwwf_SaveRepaymentInfo_Insert", ht);
        }

        #endregion

        #region === 保存假期流水帐 ===
        public void SaveFlowLeaveWaterAccount(string creator,Guid workflowId,Guid workflowInstanceId, ActivityExecutionContext context)
        {

            AppLeaveInfo alInfo = new AppLeaveInfo();
            try
            {
                alInfo.ID = workflowInstanceId;
                alInfo.WorkFlowID = workflowId;
                alInfo.ApplyName = creator;
                alInfo.ApplyDate = DateTime.Now;
                alInfo.BeginDate = Convert.ToDateTime(context.Variables["A1"].ToString());
                alInfo.EndDate = Convert.ToDateTime(context.Variables["A2"].ToString());
                alInfo.ApplyTotal = float.Parse(context.Variables["TotalDayApply"].ToString());
                alInfo.LeaveType = context.Variables["LeaveType"].ToString();

                SaveLeaveInfo(alInfo);
            }
            catch
            {

            }
        }

        private void SaveLeaveInfo(AppLeaveInfo alInfo)
        {
            Hashtable ht = new Hashtable();

            ht.Add("ID", alInfo.ID);
            ht.Add("WorkFlowID", alInfo.WorkFlowID);
            ht.Add("ApplyName", alInfo.ApplyName);
            ht.Add("LeaveType", alInfo.LeaveType);
            ht.Add("TotalDay", alInfo.ApplyTotal);
            ht.Add("CreateDT", alInfo.ApplyDate);
            ht.Add("StartDT", alInfo.BeginDate);
            ht.Add("EndDT", alInfo.EndDate);

            IBatisMapper.Insert("bwwf_LeaveInfo_Insert", ht);
        }
        #endregion 

        #region === 短信/邮件提醒 ===

        /// <summary>
        /// 发送邮件提醒与短信提醒信息.
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromMobile"></param>
        /// <param name="nextActivityIntances"></param>
        /// <param name="_workflowSetting"></param>
        /// <param name="operateType"></param>
        public void SendMessage(string fromEmail, string fromMobile, IList<ActivityInstance> nextActivityIntances, WorkflowProfile _workflowSetting, int operateType,string nextUserName)
        {
            if (nextActivityIntances == null || nextActivityIntances.Count == 0)
                return;

            foreach (ActivityInstance activity in nextActivityIntances)
            {
                InsertMessage(fromEmail, fromMobile, _workflowSetting.EmailNotifyFormat, _workflowSetting.SmsNotifyFormat,_workflowSetting.WorkflowName , activity, operateType,nextUserName );
            }
        }

        /// <summary>
        /// 执行插入电子邮件和短信提醒.
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromMobile"></param>
        /// <param name="emailNotifyFormat"></param>
        /// <param name="smsNotifyFormat"></param>
        /// <param name="activity"></param>
        /// <param name="operateType"></param>
        private static void InsertMessage(string fromEmail, string fromMobile, string emailNotifyFormat, string smsNotifyFormat, string flowName,ActivityInstance activity, int operateType,string nextUserName)
        {
            Guid activityInstanceId = activity.ActivityInstanceId;
            string workflowTitle = activity.WorkItemTitle;
            string activityName = activity.ActivityName;

            string emailContent = FormatNotifyMessage(emailNotifyFormat,flowName, workflowTitle, activityName, operateType);
            string smsContent = FormatNotifyMessage(smsNotifyFormat, flowName,workflowTitle, activityName, operateType);
            int i = 0;
            i = GetNotifyType(nextUserName, activityInstanceId);
            if (i == 1 || i == 2)
            {
                InsertEmail(fromEmail, emailContent, activityInstanceId);
            }
            if (i == 1 || i == 3)
            {
                InsertSMS(fromMobile, smsContent, activityInstanceId);
            }
        }

        /// <summary>
        /// 获取指定用户和步骤实例的提醒类型(0 都禁止提醒.1 都允许提醒.2 启用邮件提醒.3 启用短信提醒.).
        /// </summary>
        /// <param name="userName">用户名.</param>
        /// <param name="activityInstanceId">步骤实例编号.</param>
        /// <returns>(0 都禁止提醒.1 都允许提醒.2 启用邮件提醒.3 启用短信提醒.)默认值为 1.</returns>
        private static int GetNotifyType(string userName, Guid activityInstanceId)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("UserName", userName);
            parameters.Add("ActivityInstanceId", activityInstanceId);

            object result = IBatisMapper.Mapper.QueryForObject("oa_WorkflowNotify_Select_NotifyType", parameters);
            if (result == null)
                return 1; // 默认情况下都允许
            return (int)result;
        }

        /// <summary>
        /// 执行插入电子邮件提醒信息.
        /// </summary>
        /// <param name="messageForm"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        private static void InsertEmail(string messageForm, string messageBody, Guid activityInstanceId)
        {
            IBatisMapper.Insert("xqp_WorkflowReminders_InsertNew", GetMessageParameters(messageForm, EmailMessageType, messageBody, activityInstanceId));
        }

        /// <summary>
        /// 执行插入短信提醒信息.
        /// </summary>
        /// <param name="messageForm"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        private static void InsertSMS(string messageForm, string messageBody, Guid activityInstanceId)
        {
            IBatisMapper.Insert("xqp_WorkflowReminders_InsertNew", GetMessageParameters(messageForm, SMSMessageType, messageBody, activityInstanceId));
        }

        /// <summary>
        /// 获取提醒信息插入数据参数集合.
        /// </summary>
        /// <param name="messageFrom"></param>
        /// <param name="MessageType"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        private static Hashtable GetMessageParameters(string msgTo, int msgType, string msgBody, Guid activityInstanceId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("MsgType", msgType);
            parameters.Add("MsgTo", msgTo);
            parameters.Add("MsgBody", msgBody);
            parameters.Add("ActivityInstanceId", activityInstanceId);
            return parameters;
        }

        /// <summary>
        /// 格式化指定提醒信息内容.
        /// </summary>
        /// <param name="messageFormat"></param>
        /// <param name="workflowTitle"></param>
        /// <param name="activityName"></param>
        /// <param name="operateType"></param>
        /// <returns></returns>
        public static string FormatNotifyMessage(string messageFormat,string flowName, string workflowTitle, string activityName, int operateType)
        {
            messageFormat = messageFormat.ToLower();
            messageFormat = messageFormat.Replace("#flowname#", flowName);
            messageFormat = messageFormat.Replace("#title#", workflowTitle);
            messageFormat = messageFormat.Replace("#activityname#", activityName);
            messageFormat = messageFormat.Replace("#operatetype#", operateType == TodoInfo.OpBack ? "已退回到待办事宜" : "进入");
            return messageFormat;
        }
        #endregion

        public DataTable GetTaskListByUserName(string userName, string workflowName, string condition, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_Tracking_Todo";
            string fieldKey = "ActivityInstanceId";
            string fieldShow = @"ActivityInstanceId, UserName, State, ProxyName, OperateType, 
                          IsCompleted, CreatedTime,FinishedTime, Actor, ActivityName, Title, WorkflowAlias,WorkflowName, 
                          WorkflowInstanceId, SheetId,StartedTime, Urgency, Importance, 
                          Creator, CreatorName,  TodoActors";
            string fieldOrder = "State ASC, Urgency DESC, CreatedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(IsCompleted = 0) AND (UserName = '{0}')", userName);
            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat(" AND (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(condition))
            {
                condition = condition + " AND " + where.ToString();
            }
            else
            {
                condition = where.ToString();
            }

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, condition.ToString(), ref recordCount);
        }

        public DataTable GetWorkflowTrackingPager(string workflowName, string keywords, string startDT, string endDT, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_Tracking_Search";
            string fieldKey = "ActivityInstanceId";
            string fieldShow = "ActivityInstanceId, ActivityName, ActorName, WorkflowInstanceId, CreatedTime, Title, WorkflowAlias, CreatorName, AliasImageUrl, IsCompleted ,WorkflowName";
            string fieldOrder = "StartedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat(" (State = 2)");

            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat(" AND (WorkflowName = '{0}')", workflowName);
            
            if (!string.IsNullOrEmpty(keywords))
                where.AppendFormat(" AND (Title like '%{0}%')", keywords);
            if (!string.IsNullOrEmpty(startDT))
                where.AppendFormat(" AND (CreatedTime >= '{0}')", startDT);
            if (!string.IsNullOrEmpty(endDT))
                where.AppendFormat(" AND (CreatedTime <= '{0}')", endDT);


            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }

        #region === 删除 ===
        public void DeleteApplyInfo(Guid workflowinstanceid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("WorkFlowInstanceID", workflowinstanceid);

            IBatisMapper.Delete("bwwf_ApplyInfo_Delete", ht);
        }

        public void DeleteApplicationInfo(Guid workflowinstanceid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("WorkFlowInstanceID", workflowinstanceid);

            IBatisMapper.Delete("bwwf_ApplicationInfo_Delete", ht);
        }

        public void DeleteLeaveInfo(Guid workflowinstanceid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("WorkFlowInstanceID", workflowinstanceid);

            IBatisMapper.Delete("bwwf_LeaveInfo_Delete", ht);
        }

        //删除采购流水帐

        public void DeletePurchaseInfo(Guid workflowinstanceid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("WorkFlowInstanceID", workflowinstanceid);

            IBatisMapper.Delete("bwwf_PurchaseInfo_Delete", ht);
        }

        public void DeleteTrackingWorkflows(Guid workflowinstanceid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("WorkFlowInstanceID", workflowinstanceid);

            IBatisMapper.Delete("bwwf_Tracking_Workflows_Delete", ht);
        }

        public void DeleteTrackingActivity(Guid workflowinstanceid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("WorkFlowInstanceID", workflowinstanceid);

            IBatisMapper.Delete("bwwf_Tracking_Activities_Completed_Delete", ht);
        }
        #endregion 

        public void ReSaveFlowWaterAccount(Guid workflowID, Guid workflowInstanceID , ActivityExecutionContext context,string creator,string workflowName)
        {
            if (workflowName == "日常报销")
            {
                ReSaveApplyInfoWaterAccount(workflowInstanceID,context);
            }
            else if (workflowName == "请假申请流程" || workflowName == "加班申请流程")
            {
                ReSaveFlowLeaveWaterAccount(creator, workflowID,workflowInstanceID, context);
            }
            else if (workflowName == "采购申请" )
            {
                //先删除

                DeletePurchaseInfo(workflowInstanceID);
                //后保存

                ReSaveFlowPurchaseWaterAccount(creator, workflowID, workflowInstanceID, context);
            }
            
        }

        public void ReSaveApplyInfoWaterAccount(Guid workflowInstanceID, ActivityExecutionContext context)
        {
            ApplyStat apply = new ApplyStat();
            try
            {
                apply.ApplyName = context.Variables["txtExpenseUser"].ToString();
                apply.ApplyDate = DateTime.Parse(context.Variables["txtExpenseDate"].ToString());

                int k = 0;

                string happDate = "HappenDateN";
                string dept = "DeptsN";
                string invoice = "InvoiceN";
                string receipt = "ReceiptN";
                string cheque = "ChequeN";
                string summary = "SummaryN";
                string bankNum = "BankNumN";
                string cash = "CashN";
                string BudgetMan = "BudgetManN";
                string AppType = "AppTypeN";
                string Remark = "RemarkN";
                apply.WorkFlowInstanceID = workflowInstanceID;
                //先删除再插入
                DeleteApplyInfo(workflowInstanceID);
                for (int i = 3; i < context.Variables.Count - 7; i += 9)
                {
                    try
                    {
                            
                            string[] cashes = context.Variables[cash + k.ToString()].ToString().Split(new Char[] { ',' });
                            if (cashes.Length > 1)
                            {
                                string[] depts = context.Variables[dept + k.ToString()].ToString().Split(new Char[] { ',' });
                                string[] invoices = context.Variables[invoice + k.ToString()].ToString().Split(new Char[] { ',' });
                                string[] summarys = context.Variables[summary + k.ToString()].ToString().Split(new Char[] { ',' });
                                string[] budgets = context.Variables[BudgetMan + k.ToString()].ToString().Split(new Char[] { ',' });
                                string[] applyTypes = context.Variables[AppType + k.ToString()].ToString().Split(new Char[] { ',' });
                                string[] happDates = context.Variables[happDate + k.ToString()].ToString().Split(new Char[] { ',' });
                                for (int j = 0; j < cashes.Length; j++)
                                {

                                    apply.Depts = depts[j];
                                    apply.InvoiceNum = invoices[j];
                                    apply.RreceiptNum = "";
                                    apply.ChequeNum = "";
                                    apply.Summary = summarys[j];
                                    apply.BankNum = 0;
                                    apply.CashNum = (cashes[j] == "") ? 0 : Convert.ToDecimal(cashes[j]);
                                    apply.BudgetMan = budgets[j];
                                    apply.ApplyType = applyTypes[j];
                                    try
                                    {
                                        apply.Remark = context.Variables[Remark + k.ToString()].ToString();
                                    }
                                    catch
                                    {
                                        apply.Remark = "";
                                    }
                                    apply.HappenDate = Convert.ToDateTime(happDates[j]);
                                    if (ExistApplyInfo(apply.ApplyName, apply.ApplyType, apply.Depts, apply.InvoiceNum, apply.HappenDate.ToString(), apply.CashNum.ToString()) == 0)
                                    {
                                        SaveApplyInfo(apply);
                                    }
                                }
                                i -= 9;

                            }
                            else
                            {
                                apply.Depts = context.Variables[dept + k.ToString()].ToString();
                                apply.InvoiceNum = context.Variables[invoice + k.ToString()].ToString();
                                apply.RreceiptNum = "";
                                apply.ChequeNum = "";
                                apply.Summary = context.Variables[summary + k.ToString()].ToString();
                                apply.BankNum = 0;
                                apply.CashNum = (context.Variables[cash + k.ToString()].ToString() == "") ? 0 : Convert.ToDecimal(context.Variables[cash + k.ToString()].ToString());
                                apply.BudgetMan = context.Variables[BudgetMan + k.ToString()].ToString();
                                apply.ApplyType = context.Variables[AppType + k.ToString()].ToString();
                                try
                                {
                                    apply.Remark = context.Variables[Remark + k.ToString()].ToString();
                                }
                                catch
                                {
                                    apply.Remark = "";
                                }
                                apply.HappenDate = Convert.ToDateTime(context.Variables[happDate + k.ToString()].ToString());
                                //if (ExistApplyInfo(apply.ApplyName, apply.ApplyType, apply.Depts, apply.InvoiceNum, apply.HappenDate.ToString(), apply.CashNum.ToString()) == 0)
                                //{
                                    SaveApplyInfo(apply);
                                //}
                               
                            }
                            k += 1;
                    }
                    catch
                    {
                        k += 1;
                    }
                }
            }
            catch (Exception ee)
            {

            }
        }

        /// <summary>
        /// 生成采购流水账

        /// </summary>
        /// <param name="creator"></param>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="context"></param>
        public void ReSaveFlowPurchaseWaterAccount(string creator, Guid workflowId, Guid workflowInstanceId, ActivityExecutionContext context)
        {
            SavePurchaseWaterAccount(workflowId, workflowInstanceId, context);
        }

        /// <summary>
        /// 生成考勤流水账

        /// </summary>
        /// <param name="creator"></param>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="context"></param>
        public void ReSaveFlowLeaveWaterAccount(string creator, Guid workflowId, Guid workflowInstanceId, ActivityExecutionContext context)
        {

            if (ExistLeaveInfo(workflowId, creator, context.Variables["LeaveType"].ToString(), context.Variables["TotalDayApply"].ToString(), context.Variables["A1"].ToString(), context.Variables["A2"].ToString()) == 0)
            {

                SaveFlowLeaveWaterAccount(creator, workflowId, workflowInstanceId, context);
            }

        }
        /// <summary>
        /// 判断是否存在日常报销流水账

        /// </summary>
        /// <param name="ApplyName"></param>
        /// <param name="ApplyType"></param>
        /// <param name="Depts"></param>
        /// <param name="InvoiceNum"></param>
        /// <param name="HappenDate"></param>
        /// <param name="CashNum"></param>
        /// <returns></returns>
        public int ExistApplyInfo(string ApplyName, string ApplyType, string Depts, string InvoiceNum, string HappenDate, string CashNum)
        {
            DataTable dt;
            int i = 0;
            string sql = " SELECT ID FROM bw_ApplyInfo WHERE ApplyName='"+ApplyName+"' AND ApplyType='"+ApplyType+"' AND Depts='"+Depts+"' AND InvoiceNum='"+InvoiceNum+"' AND HappenDate='"+HappenDate+"' AND CashNum='"+CashNum+"' ";
            dt = IBatisDbHelper.ExecuteDataset(System.Data.CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                i = 1;
            }
            else
            {
                i = 0;
            }
            return i;
        }
        /// <summary>
        /// 判断是否存考勤流水账

        /// </summary>
        /// <param name="WorkflowID"></param>
        /// <param name="ApplyName"></param>
        /// <param name="LeaveType"></param>
        /// <param name="TotalDay"></param>
        /// <param name="StartDT"></param>
        /// <param name="EndDT"></param>
        /// <returns></returns>
        public int ExistLeaveInfo(Guid WorkflowID, string ApplyName, string LeaveType, string TotalDay,string StartDT, string EndDT)
        {
            DataTable dt;
            int i = 0;
            string sql = " SELECT ID FROM bw_LeaveInfo WHERE WorkflowID='" + WorkflowID + "' AND ApplyName='" + ApplyName + "' AND LeaveType='" + LeaveType + "' AND TotalDay='" + TotalDay + "' AND StartDT='" + StartDT + "' AND EndDT='" + EndDT + "' ";
            dt = IBatisDbHelper.ExecuteDataset(System.Data.CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                i = 1;
            }
            else
            {
                i = 0;
            }
            return i;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable  GetUserList()
        {
            DataTable dt;
            int i = 0;
            string sql = " SELECT * FROM bw_Users";
            dt = IBatisDbHelper.ExecuteDataset(System.Data.CommandType.Text, sql).Tables[0];
            
            return dt;
        }
    }
}
