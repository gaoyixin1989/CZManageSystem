namespace Botwave.Easyflow.API
{
    using Botwave.Easyflow.API.Entity;
    using Botwave.Easyflow.API.Util;
    using log4net;
    using System;
    using System.Web.Services;
    using System.Xml.Linq;

    public class WorkflowAPIWebService : WebService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WorkflowAPIWebService));

        public virtual ManageEntity GetManageEntity(string ObjectXML)
        {
            ManageEntity entity = new ManageEntity();
            try
            {
                entity = new WorkflowAPIXmlHelp().GetEntity<ManageEntity>(ObjectXML);
            }
            catch (WorkflowAPIException exception)
            {
                throw exception;
            }
            return entity;
        }

        public virtual SearchEntity GetSearchEntity(string ObjectXML)
        {
            SearchEntity entity = new SearchEntity();
            try
            {
                entity = new WorkflowAPIXmlHelp().GetEntity<SearchEntity>(ObjectXML);
            }
            catch (WorkflowAPIException exception)
            {
                throw exception;
            }
            return entity;
        }

        public virtual string SetXmlReturn(string AppAuth, string ErrorMsg)
        {
            return this.SetXmlReturn(AppAuth, "", ErrorMsg, "");
        }

        public virtual string SetXmlReturn(string AppAuth, string Success, string ErrorMsg, string xContent)
        {
            WorkflowAPIXmlHelp help = new WorkflowAPIXmlHelp();
            try
            {
                if (!string.IsNullOrEmpty(AppAuth))
                {
                    help.AppAuth = AppAuth;
                }
                if (!string.IsNullOrEmpty(Success))
                {
                    help.Success = Success;
                }
                if (!string.IsNullOrEmpty(ErrorMsg))
                {
                    help.ErrorMsg = ErrorMsg;
                }
                if (!string.IsNullOrEmpty(xContent))
                {
                    help.XContent = XElement.Parse(xContent);
                }
            }
            catch (Exception exception)
            {
                log.Error(exception);
                throw new WorkflowAPIException(3);
            }
            return help.ToString();
        }

        public virtual bool Validate(string systemId, string sysAccount, string sysPassword, APIResult Api)
        {
            Api.AppAuth = "0";
            Api.Message = "";
            return true;
        }
    }
}

