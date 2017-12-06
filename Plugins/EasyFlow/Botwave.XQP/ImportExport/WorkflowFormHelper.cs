using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Web;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;

namespace Botwave.XQP.ImportExport
{
    public class WorkflowFormHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowFormHelper));

        private static IFormDefinitionExporter _formDefinitionExporter = Spring.Context.Support.WebApplicationContext.Current["FormDefinitionExporter"] as IFormDefinitionExporter;
        private static readonly string _formTemplatePath = HttpContext.Current.Server.MapPath("~/App_Data/Templates/WorkflowFormExportTemplate.xls");

        public static IList<FormDefinition> GetFormDefinitions(string formName)
        {
            return IBatisMapper.Select<FormDefinition>("bwdf_FormDefinitions_Select_By_Name", formName);
        }

        public static bool SetCurrentVersionRelation(Guid workflowId, Guid formDefinitionId)
        {
            string sql = string.Format(@"delete bwdf_FormDefinitionInExternals where (EntityType='Form_Workflow' and EntityId = '{1}');
                insert into bwdf_FormDefinitionInExternals(FormDefinitionId, EntityType, EntityId) values ('{0}', 'Form_Workflow', '{1}');",
                    formDefinitionId, workflowId);

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
            return true;
        }

        public static bool SetCurrentVersion(Guid workflowId, Guid formDefinitionId, string actor)
        {
            string sql = string.Format(@"update bwdf_FormDefinitions  set IsCurrentVersion=0, LastModifier='{0}', LastModTime=getdate()
                where IsCurrentVersion=1 and [Name] in(select [Name] from bwdf_FormDefinitions where [Id]='{1}');
                update bwdf_FormDefinitions set IsCurrentVersion=1, LastModifier='{0}', LastModTime=getdate() where [id]='{1}';
                delete bwdf_FormDefinitionInExternals where (EntityType='Form_Workflow' and EntityId = '{2}');
                insert into bwdf_FormDefinitionInExternals(FormDefinitionId, EntityType, EntityId) values ('{1}', 'Form_Workflow', '{2}');",
                    actor, formDefinitionId, workflowId);

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
            return true;
        }

        public static bool Delete(Guid formDefinitionId, string actor)
        {
            if (formDefinitionId == Guid.Empty)
                return false;
            string sql = string.Format(@"update bwdf_FormDefinitions set Enabled=0, LastModifier='{1}', LastModTime=getdate()
                  where [Id] = '{0}'", formDefinitionId, DbUtils.FilterSQL(actor));

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
            return true;
        }

        public static void Export(HttpResponse response, Guid formDefinitionId)
        {
            string exportFilePath = string.Empty;

            try
            {
                exportFilePath = _formDefinitionExporter.Export(formDefinitionId, _formTemplatePath);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

            string downloadFileName = string.Format("{0}_表单模板.xls", System.IO.Path.GetFileNameWithoutExtension(exportFilePath));

            Download(exportFilePath, downloadFileName, response, true);
        }

        /// <summary>
        /// 下载文件.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="response"></param>
        /// <param name="shouldDelete"></param>
        public static void Download(string path, string fileName, HttpResponse response, bool shouldDelete)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                throw new FileNotFoundException("未能找到文件，" + path + " 不存在或已删除。");

            FileInfo file = new FileInfo(path);
            SetResponseHeader(response, string.IsNullOrEmpty(fileName) ? file.Name : fileName);
            response.AddHeader("Content-Length", file.Length.ToString());

            response.WriteFile(path);
            response.Flush();

            if (shouldDelete)
            {
                File.Delete(path);
            }
        }

        public static bool SetResponseHeader(HttpResponse response, string filename)
        {
            if (response == null)
                return false;
            filename = HttpUtility.UrlEncode(filename);

            response.Clear();
            response.AddHeader("Pragma", "public");
            response.AddHeader("Expires", "0");
            response.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
            response.AddHeader("Content-Type", "application/force-download");
            response.AddHeader("Content-Type", "application/octet-stream");
            response.AddHeader("Content-Type", "application/download");
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", filename));
            response.AddHeader("Content-Transfer-Encoding", "binary");

            return true;
        }
    }
}
