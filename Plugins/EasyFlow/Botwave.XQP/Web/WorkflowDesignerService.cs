using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using Botwave.Workflow.Service;

namespace Botwave.XQP.Web
{
    /// <summary>
    /// 流程可视化设计数据保存服务类.
    /// </summary>
    [WebService(Namespace = "http://www.botwave.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WorkflowDesignerService : System.Web.Services.WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowDesignerService));
        private static IDeployService deployService;

        static WorkflowDesignerService()
        {
            deployService = Spring.Context.Support.WebApplicationContext.Current["deployService"] as IDeployService;
        }

        public WorkflowDesignerService()
        {
            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
        }

        /// <summary>
        /// 保存流程可视化设计的 XML 数据.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        [WebMethod(Description="保存流程可视化设计的 XML 数据.")]
        public string SaveWorkflowXml(string key, string xmlData)
        {
            string retrunValue = "false";
            if (string.IsNullOrEmpty(xmlData))
            {
                log.ErrorFormat("XML Data is Null Or Empty.[key:{0}]", key);
                return retrunValue;
            }
            try
            {
                using (StringReader reader = new StringReader(xmlData))
                {
                    using (XmlReader xmlReader = XmlReader.Create(reader))
                    {
                        string creator = Botwave.Security.LoginHelper.UserName;
                        DeployActionResult result = deployService.DeployWorkflow(xmlReader, creator);
                        if (xmlReader.ReadState != ReadState.Closed)
                            xmlReader.Close();

                        key = result.WorkflowId.ToString();
                        if (result.Success)
                            retrunValue = key;
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return retrunValue;
            }
            log.InfoFormat("[workflowId]{0}\r\n[data]{1}", key, xmlData);
            return retrunValue;
        }
    }
}
