using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.Commons;
using Brettle.Web.NeatUpload;
using Botwave.XQP.Commons;
using Botwave.XQP.Domain;
using com.gmcc.itc;

namespace Botwave.XQP.Web.Controls
{
    /// <summary>
    /// 单个文件上传
    /// </summary>
    [ToolboxData("<{0}:FileUpload runat=server></{0}:FileUpload>")]
    public class FileUpload : InputFile
    {
        #region getter/setter

        /// <summary>
        /// 上传后，得到上传文件对应表附件attachmentId
        /// </summary>
        [Browsable(false)]
        public virtual Guid AttachmentId
        {
            get
            {
                if (ViewState["AttachmentId"] == null)
                { return Guid.Empty; }
                else
                    return (Guid)ViewState["AttachmentId"];
            }
            set { ViewState["AttachmentId"] = value; }
        }

        /// <summary>
        /// 设置文件上传的类型.
        /// </summary>
        [
        Category("Data"),
        Description("文件上传的类型[扩展名称，多个用 , 连接，如（.excel,.txt）"),
        Browsable(true)
        ]
        public virtual string FileType
        {
            get
            {
                string s = (string)ViewState["FileType"];
                return (s == null) ? String.Empty : s;
            }
            set { ViewState["FileType"] = value; }
        }

        #endregion

        /// <summary>
        /// 上传文件至文件服务器.
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="saveFileName"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public ActionResult Upload(string creator, string saveFileName, string remark)
        {
            ActionResult result = new ActionResult();
            if (this.ContentLength > 0)
            {
                string savePath = System.IO.Path.Combine(UploadHelper.UploadDirectory, saveFileName);

                if (!CheckFileType())
                {
                    result.Success = false;
                    if (!String.IsNullOrEmpty(this.FileType))
                        result.Message = "请上传" + this.FileType + "类型的文件!";
                    else result.Message = "上传的文件类型不能为可执行文件!";
                    return result;
                }
                if (UploadFile(saveFileName))
                {
                    SaveAttachmentInfo(creator, savePath, remark);
                    result.Success = true;
                    result.Message = "文件上传成功!";
                }
                else
                {
                    result.Success = false;
                    result.Message = "文件上传失败，请重新上传!";
                }
            }
            else
            {
                result.Success = false;
                result.Message = "请选择要上传的文件!";
            }
            return result;
        }

        #region Methods

        private bool UploadFile(string fileName)
        {
            return UploadHelper.Upload(this.FileContent, fileName);
        }

        private void SaveAttachmentInfo(string creator, string savePath, string remark)
        {
            Attachment attachment = new Attachment();
            attachment.Creator = creator;
            attachment.Title = FileUtils.GetFileName(this.FileName);
            attachment.FileSize = this.ContentLength;
            attachment.MimeType = System.IO.Path.GetExtension(this.FileName);
            attachment.FileName = savePath;
            attachment.Remark = remark;
            AttachmentId = attachment.Create();
        }

        private bool CheckFileType()
        {
            string type = System.IO.Path.GetExtension(this.FileName);
            //可执行文件验证

            type = type.ToLower();
            if (type == ".exe" || type == ".com" || type == ".bat" || type == ".msi" || type == ".lnk")
                return false;

            if (string.IsNullOrEmpty(this.FileType))
                return true;

            if (!this.FileType.Contains(",") && FileType.ToLower() == type)
                return true;
            string[] arm = this.FileType.Split(',');
            for (int i = 0; i < arm.Length; i++)
            {
                if (arm[i].ToLower() == type.ToLower())
                    return true;
            }
            return false;
        }
        #endregion
    }
}
