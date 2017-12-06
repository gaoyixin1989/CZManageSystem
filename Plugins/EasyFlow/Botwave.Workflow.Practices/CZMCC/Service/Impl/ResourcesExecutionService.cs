using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Botwave.Extension.IBatisNet;
using Botwave.XQP.Domain;

namespace Botwave.Workflow.Practices.CZMCC.Service.Impl
{
    public class ResourcesExecutionService
    {
        /// <summary>
        /// 根据借用资源获取资源信息列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetResourcesByType(int type)
        {
            DataTable dt;
            string sql = "SELECT [ID],[ResourcesType],[ResourcesModel] ,[SerialNumber],[State] ,[CreateDT] FROM cz_BorrowResources where [ResourcesType] = " + type;

            dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据借用资源获取资源信息列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetResourcesByTypeNew(int type)
        {
            DataTable dt;
            string sql = "SELECT [ID],[ResourcesType],[ResourcesModel] ,[SerialNumber],[State] ,[CreateDT] FROM cz_BorrowResources where State = 0 AND [ResourcesType] = " + type;

            dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            return dt;
        }

        /// <summary>
        /// 新增借用资源信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="resourceMode"></param>
        /// <param name="serialNum"></param>
        public bool SaveResourcesInfo(int type,string resourceMode,string serialNum)
        {
            int count = 0;

            string sql = "INSERT INTO cz_BorrowResources([ID],[ResourcesType],[ResourcesModel],[SerialNumber],[State]) VALUES(@ID,@ResourcesType,@ResourcesModel,@SerialNumber,@State)";

            string strSql = string.Format("SELECT Count(*) FROM cz_BorrowResources where [ResourcesType] = {0} AND ResourcesModel = '{1}' AND SerialNumber = '{2}'", type, resourceMode, serialNum);

            count = Convert.ToInt32(IBatisDbHelper.ExecuteScalar(CommandType.Text, strSql));

            if (count > 0)
            {
                return false;
            }
            else
            {
                Guid Id = Guid.NewGuid();

                SqlParameter[] parm = new SqlParameter[]
                {
                    new SqlParameter("@ID",SqlDbType.UniqueIdentifier),
                    new SqlParameter("@ResourcesType",SqlDbType.Int),
                    new SqlParameter("@ResourcesModel",SqlDbType.VarChar,50),
                    new SqlParameter("@SerialNumber",SqlDbType.VarChar,50),
                    new SqlParameter("@State",SqlDbType.Int)
                };
                parm[0].Value = Id;
                parm[1].Value = type;
                parm[2].Value = resourceMode;
                parm[3].Value = serialNum;
                parm[4].Value = 0;


                IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
     
             }
            return true;
        }

        /// <summary>
        /// 删除借用资源信息
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteResourcesInfo(Guid Id)
        {
            string sql = "DELETE FROM cz_BorrowResources WHERE ID = '" + Id + "'";

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text,sql);
        }

        /// <summary>
        /// 更新借用资源信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="type"></param>
        /// <param name="resourceMode"></param>
        /// <param name="serialNum"></param>
        /// <param name="state"></param>
        public void UpdateResourcesInfo(Guid Id, int type, string resourceMode, string serialNum, int state)
        {
            string sql = "UPDATE cz_BorrowResources SET [ResourcesType] = @ResourcesType,[ResourcesModel] = @ResourcesModel,[SerialNumber] = @SerialNumber,[State] = @State WHERE [ID] = @ID";

            SqlParameter[] parm = new SqlParameter[]
            {
                new SqlParameter("@ResourcesType",SqlDbType.Int),
                new SqlParameter("@ResourcesModel",SqlDbType.VarChar,50),
                new SqlParameter("@SerialNumber",SqlDbType.VarChar,50),
                new SqlParameter("@State",SqlDbType.Int),
                new SqlParameter("@ID",SqlDbType.UniqueIdentifier)
            };
            parm[0].Value = type;
            parm[1].Value = resourceMode;
            parm[2].Value = serialNum;
            parm[3].Value = state;
            parm[4].Value = Id;

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text,sql,parm);
        }

        /// <summary>
        /// 更新借用资源状态.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="state"></param>
        public void UpdateResourcesInfo(Guid Id,int state)
        {
            string sql = "UPDATE cz_BorrowResources SET [State] = @State WHERE [ID] = @ID";

            SqlParameter[] parm = new SqlParameter[]
            {
                new SqlParameter("@State",SqlDbType.Int),
                new SqlParameter("@ID",SqlDbType.UniqueIdentifier)
            };
            parm[0].Value = state;
            parm[1].Value = Id;

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }

        public void UpdateBorrowInfo(Guid modelId, Guid workflowInstanceId, int returnState)
        {
            string sql = "UPDATE cz_ResourcesWorkInfo SET [ReturnState] = @ReturnState WHERE [ModelID]=@ModelID AND [WorkflowInstanceId] = @WorkflowInstanceId";

            SqlParameter[] parm = new SqlParameter[]
            {
                new SqlParameter("@ReturnState",SqlDbType.Int),
                new SqlParameter("@ModelID",SqlDbType.UniqueIdentifier),
                new SqlParameter("@WorkflowInstanceId",SqlDbType.UniqueIdentifier)
            };
            parm[0].Value = returnState;
            parm[1].Value = modelId;
            parm[2].Value = workflowInstanceId;

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }

        /// <summary>
        /// 保存资源借用历史记录
        /// </summary>
        /// <param name="modelID">资源ID</param>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        /// <param name="activityInstanceId">流程步骤实例编号.</param>
        public void SaveBorrowInfo(Guid modelID,  Guid workflowInstanceId, Guid activityInstanceId)
        {
            string sql = "INSERT INTO cz_ResourcesWorkInfo([ID] ,[ModelID] ,[CorrWorkID], WorkflowInstanceId, ReturnState) VALUES(@ID ,@ModelID ,@CorrWorkID, @WorkflowInstanceId, @ReturnState)";

            Guid Id = Guid.NewGuid();

            SqlParameter[] parm = new SqlParameter[]
            {
                new SqlParameter("@ID",SqlDbType.UniqueIdentifier),
                new SqlParameter("@ModelID",SqlDbType.UniqueIdentifier),
                new SqlParameter("@CorrWorkID",SqlDbType.UniqueIdentifier),
                new SqlParameter("@WorkflowInstanceId",SqlDbType.UniqueIdentifier),
                new SqlParameter("@ReturnState",SqlDbType.Int)
            };
            parm[0].Value = Id;
            parm[1].Value = modelID;
            parm[2].Value = activityInstanceId;
            parm[3].Value = workflowInstanceId;
            parm[4].Value = 0;

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text,sql,parm);
        }

        /// <summary>
        /// 获取资源借用历史记录
        /// </summary>
        /// <param name="resoursId">资源ID</param>
        /// <returns></returns>
        public DataTable GetBorrowInfo(Guid resoursId)
        {
            DataTable dt;

            string sql = "SELECT [ID],[ModelID],[CorrWorkID],[CreateDT] FROM cz_ResourcesWorkInfo Where ModelID ='" + resoursId + "'";

            dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            return dt;
        }

        public DataTable GetCurrentBorrowInfo(Guid resoursId)
        {
            DataTable dt;

            string sql = @"SELECT res.ID, res.ModelID, res.CorrWorkID, res.CreateDT, res.WorkflowInstanceId, res.ReturnState, tw.WorkflowId, tw.SheetId, tw.Creator,  tw.Title, u.RealName CreatorName, d.DpFullName
                    FROM cz_ResourcesWorkInfo AS res
                        LEFT JOIN cz_BorrowResources rw ON rw.ID = res.ModelID 
                        LEFT JOIN bwwf_Tracking_Workflows AS tw ON res.WorkflowInstanceId = tw.WorkflowInstanceId
                        LEFT JOIN bw_Users u ON u.UserName = tw.Creator
                        LEFT JOIN bw_Depts d ON d.DpId = u.DpId
                    WHERE rw.State = 1 AND res.ReturnState = 0 AND res.ModelID = @ModelID
                    ORDER BY res.CreateDT desc";

            SqlParameter[] parm = new SqlParameter[]
            {
                new SqlParameter("@ModelID",SqlDbType.UniqueIdentifier)
            };
            parm[0].Value = resoursId;

            dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];

            return dt;
        }

        /// <summary>
        /// 获取资源信息.
        /// </summary>
        /// <param name="resoursId">资源ID</param>
        /// <returns></returns>
        public DataRow GetResourceInfo(Guid resoursId)
        {
            DataTable dt;

            string sql = "SELECT [ID], ResourcesType, ResourcesModel, SerialNumber, State, CreateDT FROM cz_BorrowResources WHERE [ID]='" + resoursId + "'";

            dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reId"></param>
        /// <returns></returns>
        public DataTable GetBorrowWorkFlow(Guid reId,string keywords, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_cz_BorrowWorkFlow";
            string fieldKey = "ID";
            string fieldShow = "ResourcesModel,SerialNumber,State ,ID,CorrWorkID,CreateDT,Title,ResourcesType";
            string fieldOrder = "CreateDT DESC";

            StringBuilder where = new StringBuilder();
            where.Append("(1=1)");

            if (reId != null)
                where.AppendFormat(" AND (ID = '{0}')", reId);
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat("{0}", keywords);
            }

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="userName"></param>
        /// <param name="notifyType"></param>
        /// <returns></returns>
        public IList<WorkflowNotifyActor> GetNotifyActors(Guid activityInstanceId, string userName, int notifyType, int operateType)
        {
            DataTable dt,dt1;
            IList<WorkflowNotifyActor> notifyActors = new List<WorkflowNotifyActor>();

            string sql = "SELECT [UserName],[Email],[Mobile],[RealName] FROM bw_Users where username = '" + userName + "'";
            string strsql = "SELECT ActivityInstanceId FROM bwwf_Tracking_Activities WHERE (WorkflowInstanceId = '" + activityInstanceId + "')";

            dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            dt1 = IBatisDbHelper.ExecuteDataset(CommandType.Text, strsql).Tables[0];

            WorkflowNotifyActor re = new WorkflowNotifyActor();
            re.UserName = userName;
            re.Email = dt.Rows[0]["Email"].ToString();
            re.Mobile = dt.Rows[0]["Mobile"].ToString();
            re.RealName = dt.Rows[0]["RealName"].ToString();
            re.ActivityInstanceId = new Guid(dt1.Rows[0]["ActivityInstanceId"].ToString());
            re.NotifyType = notifyType;
            re.OperateType = operateType;
            notifyActors.Add(re);

            return notifyActors;
        }

        //获取用户信息
        public DataTable GetUserInfo(string userName)
        {
            DataTable dt;

            string sql = "SELECT [UserName],[Email],[Mobile],[RealName] FROM bw_Users where username = '" + userName + "'";

            dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            return dt;
        }
    }
}
