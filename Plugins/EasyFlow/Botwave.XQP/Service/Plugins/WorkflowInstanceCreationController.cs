using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Security;
using Botwave.Workflow;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Util;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 流程实例创建控制器类.
    /// </summary>
    public class WorkflowInstanceCreationController : IWorkflowInstanceCreationController
    {
        //private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(XqpWorkflowInstanceCreationController));

        #region IWorkflowInstanceCreationController Members

        /// <summary>
        /// 判断是否可以发单,主要依据:
        /// (1) 用户是否具有发单权限;
        /// (2) 指定流程的发单数是否超过月最大限制或周最大限制.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="actor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool CanCreate(Guid workflowId, string actor, object args)
        {
            // 用户是否具有发单权限.
            string resourceId = GetResourceIdOfWorkflowInstanceCreation(workflowId);
            if (null == resourceId)//找不到相关权限资源

            {
                throw new WorkflowException("找不到该流程的相关权限资源, 请联系管理员.");
                //return false;
            }
            LoginUser user = args as LoginUser;
            if (!user.Resources.ContainsKey(resourceId))
            {
                throw new WorkflowException("您没有该流程的提单权限.");
                //return false;
            }

            // 指定流程的发单数是否超过月最大限制或周最大限制.
            WorkflowProfile profile = WorkflowProfile.LoadByWorkflowId(workflowId);
            if (profile == null)
                return true;

            int maxUndoneCount = profile.MaxCreationUndone;
            if (maxUndoneCount > 0)
            {
                int count = GetWorkflowInstanceUndoneCount(workflowId, actor);
                if (count >= maxUndoneCount)
                {
                    throw new WorkflowException("您该流程的未完成的工单数已超过最大数.");
                    //return false;
                }
            }

            // 发单数限制.
            int weekMaxCount = profile.MaxCreationInWeek;
            int monthMaxCount = profile.MaxCreationInMonth;

            if (weekMaxCount > 0 || monthMaxCount > 0)
            {
                #region creation control

                // 发单数的时间限制.
                DateTime startedTime = DateTime.Now;
                // 周发单控制的起始时间.
                DateTime weekStartedTime = startedTime.AddDays(-((int)startedTime.DayOfWeek));
                // 月发单控制的起始时间
                DateTime monthStartedTime = new DateTime(startedTime.Year, startedTime.Month, 1);

                string controlType = profile.CreationControlType;
                if (controlType == null)
                    controlType = string.Empty;
                int count = 0;

                // 发单数部门(科室)限制
                string dpId = (null == user.DpId) ? string.Empty : user.DpId;
                dpId = GetCreationControlDpId(controlType, dpId);

                if (string.IsNullOrEmpty(dpId))
                {
                    // 默认控制类型
                    count = GetWorkflowInstanceCount(workflowId, weekStartedTime); // 周发单数
                    if (count >= weekMaxCount)
                    {
                        throw new WorkflowException("您每周该流程的提单数已超过最大数.");
                        //return false;
                    }
                    count = GetWorkflowInstanceCount(workflowId, monthStartedTime); // 月发单数.
                    if (count >= monthMaxCount)
                    {
                        throw new WorkflowException("您每月该流程的提单数已超过最大数.");
                        //return false;
                    }
                }
                else
                {
                    // 部门控制或者科室控制.
                    count = GetWorkflowInstanceCount(workflowId, weekStartedTime, dpId); // 周发单数
                    if (count >= weekMaxCount)
                    {
                        throw new WorkflowException("您每周该流程的提单数已超过最大数");
                        //return false;
                    }
                    count = GetWorkflowInstanceCount(workflowId, monthStartedTime, dpId); // 月发单数.
                    if (count >= monthMaxCount)
                    {
                        throw new WorkflowException("您每月该流程的提单数已超过最大数.");
                        //return false;
                    }
                }
                #endregion
            }
            return true;
        }

        /// <summary>
        /// urgency 值为 0 时，表示发单页面加载时，对发单控制.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="actor"></param>
        /// <param name="urgency"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool CanCreate(Guid workflowId, string actor, int urgency, object args)
        {
            // 当页面提交时，对紧急单进行检查.
            // 非紧急单返回 true.
            if (urgency == 0)
                return true;

            WorkflowCreationControl creationControl = WorkflowCreationControl.GetWorkflowCreationControl(workflowId, urgency);
            if (creationControl == null)
                return true;

            // 发单数限制.
            int weekMaxCount = creationControl.MaxCreationInWeek;
            int monthMaxCount = creationControl.MaxCreationInMonth;

            if (weekMaxCount > 0 || monthMaxCount > 0)
            {
                #region creation control

                // 发单数时间限制.
                DateTime startedTime = DateTime.Now;
                // 周发单起始时间.
                DateTime weekStartedTime = startedTime.AddDays(-((int)startedTime.DayOfWeek));
                // 月发单起始时间

                DateTime monthStartedTime = new DateTime(startedTime.Year, startedTime.Month, 1);

                string creationControlType = creationControl.CreationControlType;
                if (creationControlType == null)
                    creationControlType = string.Empty;
                int count = 0;
                creationControlType = creationControlType.ToLower();

                // 部门编号
                string dpId = string.Empty;
                // 非默认控制 & 非用户控制

                if (!string.IsNullOrEmpty(creationControlType) && !creationControlType.Equals("user"))
                {
                    LoginUser user = args as LoginUser;
                    // 发单数部门(科室)限制
                    dpId = (null == user.DpId) ? string.Empty : user.DpId;
                    dpId = GetCreationControlDpId(creationControlType, dpId);
                }

                if (string.IsNullOrEmpty(creationControlType))
                {
                    // 默认控制类型.
                    count = GetWorkflowInstanceCount(workflowId, weekStartedTime, urgency);
                    if (count >= weekMaxCount)
                    {
                        throw new WorkflowException("您每周该流程的紧急单的提单数已超过最大数.");
                        //return false;
                    }
                    count = GetWorkflowInstanceCount(workflowId, monthStartedTime, urgency);
                    if (count >= monthMaxCount)
                    {
                        throw new WorkflowException("您每月该流程的紧急单的提单数已超过最大数.");
                        //return false;
                    }
                }
                else if (creationControlType.Equals("user"))
                {
                    // 用户控制类型.
                    count = GetWorkflowInstanceCountByActor(workflowId, weekStartedTime, urgency, actor);
                    if (count >= weekMaxCount)
                    {
                        throw new WorkflowException("您每周该流程的紧急单的提单数已超过最大数.");
                        //return false;
                    }
                    count = GetWorkflowInstanceCountByActor(workflowId, monthStartedTime, urgency, actor);
                    if (count >= monthMaxCount)
                    {
                        throw new WorkflowException("您每月该流程的紧急单的提单数已超过最大数.");
                        //return false;
                    }
                }
                else if (!string.IsNullOrEmpty(dpId))
                {
                    // 部门控制或者科室控制.
                    count = GetWorkflowInstanceCount(workflowId, weekStartedTime, urgency, dpId);
                    if (count >= weekMaxCount)
                    {
                        throw new WorkflowException("您每周该流程的紧急单的提单数已超过最大数.");
                        //return false;
                    }
                    count = GetWorkflowInstanceCount(workflowId, monthStartedTime, urgency, dpId);
                    if (count >= monthMaxCount)
                    {
                        throw new WorkflowException("您每月该流程的紧急单的提单数已已超过最大数.");
                        //return false;
                    }
                }
                #endregion
            }
            return true;
        }
        #endregion

        #region private methods

        /// <summary>
        /// 获取资源编号.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        private static string GetResourceIdOfWorkflowInstanceCreation(Guid workflowId)
        {
            string sql = String.Format(@"select r.ResourceId 
from bw_Resources as r left join bwwf_Workflows as w on r.[Name] = w.WorkflowName
where r.Type = 'workflow' and w.WorkflowId = '{0}' 
    and w.Enabled = 1 and w.IsCurrent = 1 and w.IsDeleted = 0", workflowId);
            object obj = IBatisDbHelper.ExecuteScalar(System.Data.CommandType.Text, sql);
            if (null == obj)
            {
                return null;
            }

            string workflowResourceId = obj.ToString();
            string resourceId = workflowResourceId + "0001";    //提单的权限资源


            return resourceId;
        }

        /// <summary>
        /// 获取发单控制的部门(科室)ID.
        /// </summary>
        /// <param name="creationControlType"></param>
        /// <param name="dpId"></param>
        /// <returns></returns>
        private static string GetCreationControlDpId(string creationControlType, string dpId)
        {
            if (string.IsNullOrEmpty(dpId))
                return string.Empty;
            creationControlType = creationControlType.ToLower();
            if (creationControlType.Equals("dept")) // 部门控制
                dpId = GmccDeptHelper.GetDeptIdByDpId(dpId);
            else if (creationControlType.Equals("room")) // 科室控制
                dpId = GmccDeptHelper.GetRoomIdByDpId(dpId);
            else
                dpId = string.Empty;
            return dpId;
        }

        /// <summary>
        /// 获取指定用户的未完成工单数.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static int GetWorkflowInstanceUndoneCount(Guid workflowId, string userName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("UserName", userName);

            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_WfInstanceCreationController_Count_Undone_ByCreator", parameters);
        }

        /// <summary>
        /// 普通单的默认控制.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startedTime"></param>
        /// <returns></returns>
        private static int GetWorkflowInstanceCount(Guid workflowId, DateTime startedTime)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("StartedTime", startedTime);

            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_WfInstanceCreationController_Count_Adv_ByWorkflowId", parameters);
        }

        /// <summary>
        /// 普通单的部门控制或者科室控制.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startedTime"></param>
        /// <param name="dpId"></param>
        /// <returns></returns>
        private static int GetWorkflowInstanceCount(Guid workflowId, DateTime startedTime, string dpId)
        {
            // 部门控制或者科室控制.
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("StartedTime", startedTime);
            parameters.Add("DpId", dpId);

            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_WfInstanceCreationController_Count_ByWorkflowIdAndDpId", parameters);
        }

        /// <summary>
        /// 紧急单的默认控制类型.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startedTime"></param>
        /// <param name="urgency"></param>
        /// <returns></returns>
        private static int GetWorkflowInstanceCount(Guid workflowId, DateTime startedTime, int urgency)
        {
            // 默认控制类型.
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("StartedTime", startedTime);
            parameters.Add("Urgency", urgency);
            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_WfInstanceCreationController_Count_Adv_ByWorkflowId", parameters);
        }

        /// <summary>
        /// 紧急单的部门控制或者科室控制.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startedTime"></param>
        /// <param name="urgency"></param>
        /// <param name="dpId"></param>
        /// <returns></returns>
        private static int GetWorkflowInstanceCount(Guid workflowId, DateTime startedTime, int urgency, string dpId)
        {
            // 部门控制或者科室控制.
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("StartedTime", startedTime);
            parameters.Add("Urgency", urgency);
            parameters.Add("DpId", dpId);

            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_WfInstanceCreationController_Count_Adv_ByWorkflowIdAndDpId", parameters);
        }

        /// <summary>
        /// 紧急单的用户控制类型.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startedTime"></param>
        /// <param name="urgency"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        private static int GetWorkflowInstanceCountByActor(Guid workflowId, DateTime startedTime, int urgency, string actor)
        {
            // 用户控制类型.
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("StartedTime", startedTime);
            parameters.Add("Urgency", urgency);
            parameters.Add("UserName", actor);

            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_WfInstanceCreationController_Count_Adv_ByCreator", parameters);
        }
        #endregion
    }
}
