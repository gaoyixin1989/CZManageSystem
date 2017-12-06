using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Botwave.Easyflow.API.Entity;
using Botwave.XQP.API.Enums;
using Botwave.Easyflow.API;
using Botwave.XQP.API.Service;
using System.Data;
using Botwave.XQP.API.Util;
using System.Xml.Linq;

namespace Botwave.XQP.API.Interface
{
    /// <summary>
    /// webservice服务实现
    /// </summary>
    public class WorkflowAPIService:IWorkflowAPIService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowAPIService));

        #region 属性

        /// <summary>
        /// 登录
        /// </summary>
        public ILoginAPIService LoginAPIService { set; get; }

        /// <summary>
        /// 查询实现类
        /// </summary>
        public ISearchAPIService SearchAPIService { set; get; }

        /// <summary>
        ///  处理实现类
        /// </summary>
        public IManageAPIService ManageAPIService { set; get; }

        /// <summary>
        /// 嵌入页面判断保存状态
        /// </summary>
        public ISaveAsAPIService SaveAsAPIService { set; get; }

        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public string LoginWorkflow(string userName, string passWord)
        {
            string strReturn = string.Empty;
            strReturn = ((int)LoginAPIService.Login(userName, passWord)).ToString();
            return strReturn;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public string SearchWorkflow(SearchEntity search)
        {
            string strReturn = string.Empty;

            if (!Enum.IsDefined(typeof(SearchCommandEnum), search.Action))//判断命令是否存在
            {
                throw new WorkflowAPIException(4);
            }

            ValidateUser(search.UserName, search.KeyPassword);//验证用户名以及唯一key

            int recordCount = 0;
            DataTable dt = SearchExecute(search, ref recordCount);
            strReturn = XmlAnalysisHelp.GetXmlContent(dt, search, recordCount);

            return strReturn;
        }

        public string SearchWorkflow(string workflowProperties, IDictionary<string, object> formVariables)
        {
            string strReturn = string.Empty;
            string ret_val = string.Empty;
            string xContent = string.Empty, Success = string.Empty, ErrorMsg = string.Empty;
            WorkflowAPIWebService workflowAPIWebService = new WorkflowAPIWebService();
            try
            {
                SearchEntity search = workflowAPIWebService.GetSearchEntity(workflowProperties.ToLower());//xml全部转小写
                Success = "1";
                if (!Enum.IsDefined(typeof(SearchCommandEnum), search.Action))//判断命令是否存在
                {
                    throw new WorkflowAPIException(4);
                }

                //ValidateUser(search.UserName, search.KeyPassword);//验证用户名以及唯一key

                int recordCount = 0;
                DataTable dt = SearchExecute(search, ref recordCount);
                strReturn = XmlAnalysisHelp.GetXmlContent(dt, search, formVariables, recordCount);
            }
            catch (Exception ex)
            {
                if (ex.StackTrace.Length > 10)
                {
                    Success = "-99";
                }
                else
                {
                    Success = ex.StackTrace;
                }

                ErrorMsg = ex.Message;
                log.Error("Error in SearchWorkflow:" + ex.ToString());
            }
            finally
            {
                ret_val = workflowAPIWebService.SetXmlReturn("", Success, ErrorMsg, strReturn);
            }


            return ret_val;
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="manage"></param>
        /// <returns></returns>
        public string ManageWorkflow(ManageEntity manage)
        {
            string strReturn = string.Empty;

            if (!Enum.IsDefined(typeof(ManageCommandEnum), manage.Action.ToLower()))
            {
                throw new WorkflowAPIException(4);
            }

            ValidateUser(manage.UserName, manage.KeyPassword);

            string strResult = ManageExecute(manage);
            if (!string.IsNullOrEmpty(strResult))
                strReturn = XmlAnalysisHelp.GetXmlContent(strResult, manage);

            return strReturn;
        }

        /// <summary>
        /// 执行查询类别
        /// </summary>
        /// <param name="search"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        private DataTable SearchExecute(SearchEntity search,ref int recordCount)
        {
            DataTable dtReturn = null;
            object obj = Enum.Parse(typeof(SearchCommandEnum), search.Action);
            SearchCommandEnum command= (SearchCommandEnum)obj;
            switch (command)
            {
                case SearchCommandEnum.todolist://待办列表
                    dtReturn = SearchAPIService.GetTodoList(search.UserName, search.Workflows, search.KeyWords, search.PageIndex, search.PageCount, ref recordCount);
                    break;
                case SearchCommandEnum.donelist://已办列表
                    dtReturn = SearchAPIService.GetDoneList(search.UserName, search.Workflows, search.KeyWords,search.BeginTime,search.Endtime, search.PageIndex, search.PageCount, ref recordCount);
                    break;
                case SearchCommandEnum.workflowlist://流程列表
                    dtReturn = SearchAPIService.GetWorkflow(search.UserName, search.GroupName);
                    break;
                case SearchCommandEnum.state://获取状态
                    dtReturn = SearchAPIService.GetWorkflowState(search.WorkflowInstanceId, search.SheetId);
                    break;
                case SearchCommandEnum.recordlist://工单处理记录
                    dtReturn = SearchAPIService.GetWorkflowRecordActivityList(search.WorkflowInstanceId, search.SheetId);
                    break;
                case SearchCommandEnum.mytasklist://我的工单
                    dtReturn = SearchAPIService.GetMyTasksList(search.UserName, search.Workflows, search.KeyWords, search.State, search.BeginTime, search.Endtime, search.PageIndex, search.PageCount, ref recordCount);
                    break;
                case SearchCommandEnum.menugrouplist://获取分组
                    dtReturn = SearchAPIService.GetMenuGroup();
                    break;
                case SearchCommandEnum.infolist://获取流程信息
                    #region
                    if (!string.IsNullOrEmpty(search.ActivityInstanceId))
                    {
                        dtReturn = SearchAPIService.GetNextActivityInfoList(search.UserName, search.ActivityInstanceId);
                    }
                    else
                    {
                        dtReturn = SearchAPIService.GetFieldInfoList(search.WorkflowAlias, search.Workflows);
                    }
                    break;
                    #endregion
                case SearchCommandEnum.detail://获取需求单详细信息
                    dtReturn = SearchAPIService.GetWorkflowDetail(search.WorkflowInstanceId, search.SheetId);
                    break;
                case SearchCommandEnum.info:
                    dtReturn = SearchAPIService.GetWorkflowInfo(search.WorkflowInstanceId);
                    break;
                //case SearchCommandEnum.definition://获取流程定义（暂时不开发）
                //    break;

                case SearchCommandEnum.searchquery:
                    #region
                    if (string.IsNullOrEmpty(search.Workflows))
                    {
                        dtReturn = SearchAPIService.GetSearchQueryList(search.UserName);
                    }
                    else
                    {
                        dtReturn = SearchAPIService.GetSearchQueryList(search.UserName,search.Workflows);
                    }
                    #endregion
                    break;
                case SearchCommandEnum.searchlist:
                    dtReturn = SearchAPIService.GetSearchList(search.UserName, search.BeginTime, search.Endtime, search.Workflows, search.Activities, search.Creator, search.Actor, search.TitleKeywords, search.ContentKeywords, search.SheetId, search.PageIndex, search.PageCount, ref recordCount);
                    break;
                case SearchCommandEnum.commentlist:
                    dtReturn = SearchAPIService.GetCommentList(search.WorkflowInstanceId);
                    break;
                case SearchCommandEnum.activity:
                    dtReturn = SearchAPIService.GetActivityActor(search.WorkflowInstanceId, search.Workflows, search.Actor, search.Activities, search.WorkflowProperties);
                    break;
                default:
                    break;
            }
            return dtReturn;
        }

        /// <summary>
        /// 执行处理类别
        /// </summary>
        /// <param name="manage"></param>
        /// <returns></returns>
        private string ManageExecute(ManageEntity manage)
        {
            string strReturn = string.Empty;
            object obj = Enum.Parse(typeof(ManageCommandEnum), manage.Action);
            ManageCommandEnum command = (ManageCommandEnum)obj;
            switch (command)
            {
                case ManageCommandEnum.assign:
                    ManageAPIService.AssignWorkflow(manage.UserName, manage.AssignedUser, manage.ActivityInstanceId, manage.Content);
                    break;
                case ManageCommandEnum.comment:
                    ManageAPIService.CommentWorkflow(manage.WorkflowInstanceId, manage.ActivityInstanceId, manage.UserName, manage.Content, manage.CommentProperties);
                    break;
                //case ManageCommandEnum.deploy:
                //    break;
                case ManageCommandEnum.execute:
                    strReturn=ManageAPIService.ExecuteWorkflow(manage.UserName, manage.ActivityInstanceId, manage.Command, manage.WorkflowProperties, manage.Content);
                    break;
                //case ManageCommandEnum.save:
                //    break;
                case ManageCommandEnum.start:
                    strReturn = ManageAPIService.StartWorkflow(manage.UserName, manage.WorkflowId, manage.WorkflowTitle, manage.WorkflowProperties);
                    break;                
                default:
                    break;
            }
            return strReturn;
        }

        /// <summary>
        /// 验证用户名以及唯一key
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="keyPassword">唯一key</param>
        private void ValidateUser(string userName, string keyPassword)
        {
            #region 进行用户名以及唯一key的验证

            DataTable dt = CommonHelper.GetUserInfo(userName);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new WorkflowAPIException(17);
            }
            //string key= dt.Rows[0]["UserKey"]==null?"":dt.Rows[0]["UserKey"].ToString ();
            //if (string.IsNullOrEmpty(key))
            //{
            //    throw new WorkflowAPIException(18);
            //}
            
            //if (!Object.Equals(key, keyPassword.ToLower()))
            //{
            //    throw new WorkflowAPIException(19);
            //}

            #endregion
        }

        public string SaveAs(string workflowinstanceid, string page, int state)
        {
            string ret_val = string.Empty;
            try
            {
                int count = SaveAsAPIService.SelectSystemPage(workflowinstanceid, page);
                if (count == 0)
                {
                    SaveAsAPIService.InsertSystemPage(workflowinstanceid, page, state);
                }
                else if (count >= 1)
                {
                    SaveAsAPIService.UpdateSystemPage(workflowinstanceid, page, state);
                }
                ret_val = "Success";
            }
            catch (Exception ee)
            {
                ret_val = "Error:" + ee.ToString();
            }
            return ret_val;
        }
    }
}
