using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.gmcc.itc;
using System.ComponentModel;
using Brettle.Web.NeatUpload;
using Botwave.Web;
using Botwave.Commons;

namespace Botwave.FileManagement.Controls
{
    /// <summary>
    /// 单个文件上传
    /// </summary>
    [ToolboxData("<{0}:FileUpload runat=server></{0}:FileUpload>")]
    public class FileUpload : InputFile
    {
        #region getter/setter
        /// <summary>
        /// 文件上传人

        /// </summary>
        [
        Description("文件上传人"),
        Browsable(false)
        ]
        public virtual string Creator
        {
            get
            {
                string s = (string)ViewState["Creator"];
                return (s == null) ? String.Empty : s;
            }
            set { ViewState["Creator"] = value; }
        }

        /// </summary>
        [
        Description("文件归属人"),
        Browsable(false)
        ]
        public virtual string OwnerShip
        {
            get
            {
                string s = (string)ViewState["OwnerShip"];
                return (s == null) ? String.Empty : s;
            }
            set { ViewState["OwnerShip"] = value; }
        }

        /// <summary>
        /// 设置文件备注
        /// </summary>
        [
        Description("设置文件备注"),
        Browsable(true),
        Category("Data")
        ]
        public virtual string Remark
        {
            get
            {
                string s = (string)ViewState["Remark"];
                return (s == null) ? String.Empty : s;
            }
            set { ViewState["Remark"] = value; }
        }
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
        /// 指定上传文件保存的方式

        /// </summary>
        [
        Description("指定上传文件保存的方式。"),
        Browsable(true),
        Category("Data"),
        DefaultValue(AuxiliaryClass.UploadType.WebDAV)
        ]
        public virtual AuxiliaryClass.UploadType UploadType
        {
            get
            {
                if (ViewState["UploadType"] == null)
                    return AuxiliaryClass.UploadType.WebDAV;
                else
                    return (AuxiliaryClass.UploadType)ViewState["UploadType"];
            }
            set { ViewState["UploadType"] = value; }
        }

        /// <summary>
        /// 设置文件上传的类型。

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

        /// <summary>
        /// 文件保存的地址，在（web.config中appSettings设置）

        /// </summary>
        [
        Description("文件保存的地址，在（web.config中appSettings设置）"),
        Browsable(true),
        DefaultValue("UploadVirtualPath"),
        Category("Data")
        ]
        public virtual string SaveFileDir
        {
            get
            {
                string s = (string)ViewState["SaveFileDir"];
                return (s == null) ? "UploadVirtualPath" : s;
            }
            set { ViewState["SaveFileDir"] = value; }
        }

        [Browsable(false)]
        public virtual string SaveFileName
        {
            get
            {
                string s = (string)ViewState["SaveFileName"];
                return (s == null) ? String.Empty : s;
            }
            set { ViewState["SaveFileName"] = value; }
        }

        private com.gmcc.itc.FileManager cacheFileManager;
        private com.gmcc.itc.FileManager CacheFileManager
        {
            get
            {
                if (cacheFileManager == null)
                    cacheFileManager = new com.gmcc.itc.FileManager(Page.Server.MapPath("~/FileSever.config"));
                return cacheFileManager;
            }
        }

        private string CacheUploadPath
        {
            get
            {
                if (UploadType == AuxiliaryClass.UploadType.Localhost)
                    return System.IO.Path.Combine(WebUtils.GetAppStrPath(), System.Configuration.ConfigurationManager.AppSettings[SaveFileDir].ToString());
                return System.Configuration.ConfigurationManager.AppSettings[SaveFileDir].ToString();
            }
        }
        #endregion

        /// <summary>
        /// 上传文件至文件服务器
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFileToServer()
        {
            ActionResult result = new ActionResult();
            if (this.ContentLength > 0)
            {
                if (!CheckFileType())
                {
                    result.Success = false;
                    if (!String.IsNullOrEmpty(this.FileType))
                        result.Message = "请上传" + this.FileType + "类型的文件!";
                    else result.Message = "上传的文件类型不能为可执行文件!";
                    return result;
                }
                if (UploadFile())
                {
                    SaveAttachmentInfo();
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
        private bool UploadFile()
        {
            SaveFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(FileName);
            return FileService.UploadFile(this, UploadType, SaveFileName, CacheUploadPath, CacheFileManager);
        }

        private void SaveAttachmentInfo()
        {
            Attachment attachment = new Attachment();
            attachment.Creator = Creator;
            attachment.Title = FileUtils.GetFileName(this.FileName);
            attachment.FileSize = this.ContentLength;
            attachment.MimeType = System.IO.Path.GetExtension(this.FileName);
            attachment.FileName = CacheUploadPath + SaveFileName;
            attachment.Remark = Remark;
            attachment.OwnerShip = OwnerShip;
            AttachmentId = attachment.Create();
        }

        private bool CheckFileType()
        {
            string type = System.IO.Path.GetExtension(this.FileName);
            //可执行文件验证

            if (type.ToLower() == ".exe" || type.ToLower() == ".com" || type.ToLower() == ".bat" || type.ToLower() == ".msi")
                return false;

            if (string.IsNullOrEmpty(this.FileType))
                return true;

            if (!this.FileType.Contains(",") && FileType.ToLower() == type.ToLower())
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
