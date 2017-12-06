<%@ WebService Language="C#" Class="WorkflowAPIService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Botwave.Easyflow.API;
using Botwave.Easyflow.API.Entity;
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class WorkflowAPIService : WorkflowAPIWebService
{
    private Botwave.XQP.API.Interface.IWorkflowAPIService _WorkflowAPIService;
    public WorkflowAPIService()
    {
        _WorkflowAPIService = Spring.Context.Support.WebApplicationContext.Current["WorkflowAPIServices"] as Botwave.XQP.API.Interface.IWorkflowAPIService;//注入名称目前有重复,稍后改正
    }

    /// <summary>
    /// 登录方法
    /// </summary>
    /// <param name="systemId">综合应用平台分配的系统标识号</param>
    /// <param name="sysAccount">接入综合应用平台的账号</param>
    /// <param name="sysPassword">接入综合应用平台的密码</param>
    /// <param name="userName">用户portal 帐号</param>
    /// <param name="passWord">用户portal 密码</param>
    /// <returns></returns>
    /*[WebMethod]
    public string LoginWorkflow(string systemId, string sysAccount, string sysPassword, string userName,string passWord)
    {
        string ret_val = string.Empty;
        // 综合应用平台验证
        APIResult Api = new APIResult();
        if (!this.Validate(systemId, sysAccount, sysPassword, Api))
        {
            ret_val = this.SetXmlReturn(Api.AppAuth, Api.Message);
            return ret_val;
        }

        string xContent = string.Empty, Success = string.Empty, ErrorMsg = Api.Message;
        try
        {
            Success = _WorkflowAPIService.LoginWorkflow(userName, passWord);
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
        }
        finally
        {
            ret_val = this.SetXmlReturn(Api.AppAuth, Success, ErrorMsg, xContent);
        }

        return ret_val;
    }*/
    
    /// <summary>
    /// 查询类方法集合
    /// </summary>
    /// <param name="systemId">流程易分配的系统标识号</param>
    /// <param name="sysAccount">接入流程易平台的账号</param>
    /// <param name="sysPassword">接入流程易平台的密码</param>
    /// <param name="ObjectXML">XML集合</param>
    /// <returns></returns>
    [WebMethod]
    public string SearchWorkflow(string systemId,string sysAccount, string sysPassword, string ObjectXML)
    {
        string ret_val = string.Empty;
        // 综合应用平台验证
        APIResult Api = new APIResult();
        if (!this.IsValidate(systemId, sysAccount, sysPassword, Api))
        {
            ret_val = this.SetXmlReturn(Api.AppAuth, Api.Message);
            return ret_val;
        }

        string xContent = string.Empty, Success = string.Empty, ErrorMsg = Api.Message;
        try
        {
            SearchEntity search = this.GetSearchEntity(ObjectXML.ToLower());//xml全部转小写，解析XML字符串，转换为SearchEntity实体类
            xContent = _WorkflowAPIService.SearchWorkflow(search);
            Success = "1";
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
        }
        finally {
            ret_val = this.SetXmlReturn(Api.AppAuth, Success, ErrorMsg, xContent);
        }

        return ret_val;
    }

    /// <summary>
    /// 处理类方法集合
    /// </summary>
    /// <param name="systemId">流程易分配的系统标识号</param>
    /// <param name="sysAccount">接入流程易平台的账号</param>
    /// <param name="sysPassword">接入流程易平台的密码</param>
    /// <param name="ObjectXML">XML集合</param>
    /// <returns></returns>
    [WebMethod]
    public string ManageWorkflow(string systemId, string sysAccount, string sysPassword, string ObjectXML)
    {
        string ret_val = string.Empty;
        // 综合应用平台验证
        APIResult Api = new APIResult();

        if (!this.IsValidate(systemId, sysAccount, sysPassword, Api))
        {
            ret_val = this.SetXmlReturn(Api.AppAuth, Api.Message);
            return ret_val;
        }
        string xContent = string.Empty, Success = string.Empty, ErrorMsg = Api.Message;
        try
        {
            ManageEntity manage = this.GetManageEntity(ObjectXML);//xml全部转小写
            xContent = _WorkflowAPIService.ManageWorkflow(manage);
            Success = "1";
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
        }
        finally
        {
            ret_val = this.SetXmlReturn(Api.AppAuth, Success, ErrorMsg, xContent);
        }

        return ret_val;
    }

    /// <summary>
    /// 嵌入流程修改嵌入页面的保存状态
    /// </summary>
    /// <param name="workflowinstancid">工单ID</param>
    /// <param name="page">页面名称</param>
    /// <returns></returns>
    /*[WebMethod]
    public string SaveAs(string workflowinstancid, string page, int state)
    {
        string ret_val = string.Empty;
        ret_val = _WorkflowAPIService.SaveAs(workflowinstancid, page, state);
        return ret_val;
    }*/

    /// <summary>
    /// 流程易本地注册系统验证
    /// </summary>
    /// <param name="systemId"></param>
    /// <param name="sysAccount"></param>
    /// <param name="sysPassword"></param>
    /// <param name="Api"></param>
    /// <returns></returns>
    public bool IsValidate(string systemId, string sysAccount, string sysPassword, APIResult Api)
    {
        if (string.IsNullOrEmpty(systemId))
        {
            Api.AppAuth = "0";
            Api.Message = "系统ID不能为空！";
            return false;
        }
        else if (string.IsNullOrEmpty(sysAccount))
        {
            Api.AppAuth = "0";
            Api.Message = "系统账号不能为空！";
            return false;
        }
        else if (string.IsNullOrEmpty(sysPassword))
        {
            Api.AppAuth = "0";
            Api.Message = "系统密码不能为空！";
            return false;
        }
        try
        {
            Guid id = new Guid(systemId);
        }
        catch
        {
            Api.AppAuth = "0";
            Api.Message = "系统ID不合法，必须为包含带 4 个短划线的 32 位数的GUID！";
            return false;
        }
        Botwave.XQP.Domain.CZRegistSystem item = Botwave.XQP.Domain.CZRegistSystem.SelectById(new Guid(systemId));
        Botwave.XQP.Domain.CZRegistSystem acountItem = Botwave.XQP.Domain.CZRegistSystem.SelectByName(sysAccount);
        if (item == null)
        {
            Api.AppAuth = "0";
            Api.Message = "系统ID不存在！";
            return false;
        }
        else if(acountItem == null)
        {
            Api.AppAuth = "0";
            Api.Message = "系统账号不存在！";
            return false;
        } 
        else if(item.SystemName!=acountItem.SystemName)
        {
            Api.AppAuth = "0";
            Api.Message = "系统ID与系统账号不匹配！";
            return false;
        } 
        else if(!sysPassword.Equals(Botwave.Commons.TripleDESHelper.Decrypt(item.Password)))
        {
            Api.AppAuth = "0";
            Api.Message = "密码错误！";
            return false;
        }
        return true;
    }
}

