<%@ WebHandler Language="C#" Class="uploadPic" %>

using System;
using System.Web;
using System.IO;

public class uploadPic : Botwave.Security.Web.PageBase,IHttpHandler
{
    protected string AllowExt = "7z|bmp|doc|docx|fla|flv|gif|gzip|jpeg|jpg|mid|mpeg|mpg|pdf|png|ppt|pptx|pxd|ram|rar|rtf|swf|tgz|tif|tiff|txt|vsd|xls|xlsx|xml|zip";//支持的文件格式 
    int FileMaxSize = 20*1024;//文件大小，单位为K
    protected string creator = "user";
    protected string entityId = "0";
    protected string entityType = "W_A";

    #region service properties

    private Botwave.Workflow.Extension.Service.IWorkflowFileService workflowFileService;
    private Botwave.Workflow.Extension.Service.IWorkflowAttachmentService workflowAttachmentService;

    public Botwave.Workflow.Extension.Service.IWorkflowFileService WorkflowFileService
    {
        get { return workflowFileService; }
        set { workflowFileService = value; }
    }

    public Botwave.Workflow.Extension.Service.IWorkflowAttachmentService WorkflowAttachmentService
    {
        get { return workflowAttachmentService; }
        set { workflowAttachmentService = value; }
    }
    #endregion
    
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        
        creator = context.Request.Params["creator"];
        entityId = context.Request.Params["entityID"];
        entityType = context.Request.Params["entityType"];
        string wiid = context.Request.Params["wiid"];
        
        HttpPostedFile fileUpload = context.Request.Files[0];
        workflowFileService = Spring.Context.Support.WebApplicationContext.Current["xqpWorkflowFileService"] as Botwave.Workflow.Extension.Service.IWorkflowFileService;
        workflowAttachmentService = Spring.Context.Support.WebApplicationContext.Current["xqpWorkflowAttachmentService"] as Botwave.Workflow.Extension.Service.IWorkflowAttachmentService;
        if (fileUpload != null)
        {
            try
            {
                creator = CurrentUserName;
                //上传图片的扩展名
                string fileExtension = fileUpload.FileName.Substring(fileUpload.FileName.LastIndexOf('.'));
                //判断文件格式
                //if (!CheckValidExt(fileExtension))
                //{
                //    context.Response.Write("错误提示：文件格式不正确！" + fileExtension);
                //    return;
                //}
                //判断文件大小
                //if (fileUpload.ContentLength > FileMaxSize * 1024)
                //{
                //    context.Response.Write("错误提示：上传的文件(" + fileUpload.FileName + ")超过最大限制：" + FileMaxSize + "KB");
                //    return;
                //}
                ////使用时间+随机数重命名文件
                //string strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff");//取得时间字符串
                //Random ran = new Random();
                //string strRan = Convert.ToString(ran.Next(100, 999));//生成三位随机数
                //string saveName = strDateTime + strRan + fileExtension;
                string fileName = null;
                try
                {
                    FileInfo file = new FileInfo(fileUpload.FileName);
                    if (file.Extension.ToLower() == ".exe" || file.Extension.ToLower() == ".com" || file.Extension.ToLower() == ".bat" || file.Extension.ToLower() == ".dll")
                    {
                        context.Response.Write("错误提示：上传文件的类型不能为可执行文件!");
                        return;
                    }

                    fileName = workflowFileService.UploadFile(fileUpload);
                    //fileName = Guid.NewGuid().ToString()+".rar";
                }
                catch (Exception ex)
                {
                    Botwave.XQP.Commons.LogWriter.Write(creator, ex);
                    context.Response.Write("错误提示：上传文件错误." + ex.Message);
                    return;
                }
                if (!string.IsNullOrEmpty(fileName))
                {
                    Guid workflowInstanceId = new Guid(wiid);
                    // 创建附件实体类.
                    string attachmentId = workflowAttachmentService.CreateAttachment(fileUpload, creator, fileName);
                    if (!string.IsNullOrEmpty(attachmentId))
                    {
                        // 创建实体关系类.
                        workflowAttachmentService.CreateAttachmentEntity(attachmentId, workflowInstanceId.ToString(), Botwave.Workflow.Extension.Util.WorkflowUtility.EntityType_WorkflowAttachment);
                    }
                    //LoadAttachment(workflowInstanceId);
                    context.Response.Write(attachmentId);
                }
                else
                {
                    context.Response.Write("错误提示：上传文件错误.");
                }
            }
            catch(Exception ex)
            {
                context.Response.Write("错误提示：上传失败:"+ex.Message);
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    /// <summary>
    /// 检测扩展名的有效性
    /// </summary>
    /// <param name="sExt">文件名扩展名</param>
    /// <returns>如果扩展名有效,返回true,否则返回false.</returns>
    public bool CheckValidExt(string strExt)
    {
        bool flag = false;
        string[] arrExt = AllowExt.Split('|');
        foreach (string filetype in arrExt)
        {
            if (filetype.ToLower() == strExt.ToLower().Replace(".", ""))
            {
                flag = true;
                break;
            }
        }
        return flag;
    }
}