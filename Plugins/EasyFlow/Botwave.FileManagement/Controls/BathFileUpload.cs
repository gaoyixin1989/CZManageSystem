using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.gmcc.itc;
using System.ComponentModel;
using Brettle.Web.NeatUpload;
using Botwave.Commons;
using Botwave.Web;

namespace Botwave.FileManagement.Controls
{
    /// <summary>
    /// 批量文件上传
    /// </summary>
    [ToolboxData("<{0}:BathFileUpload runat=server></{0}:BathFileUpload>")]
    public class BathFileUpload : CompositeControl
    {
        private List<Attachment> attachmentlist = new List<Attachment>();

        private AuxiliaryClass.UploadType uploadType;
        private string saveFileDir;

        private DynamicPanel uploadPanel;
        private InputFile defaultInputFile;
        /// <summary>
        /// 设置操作者

        /// </summary>
        [Description("获取 和 设置操作者"),
        Browsable(false)]
        public string Creator
        {
            get
            {
                if (ViewState["Creator"] == null)
                { return ""; }
                else
                    return ViewState["Creator"].ToString();

            }
            set { ViewState["Creator"] = value; }
        }
        /// <summary>
        /// 设置文件备注
        /// </summary>
        [Description("设置文件备注"),
        Browsable(true)]
        public string Remark
        {
            get
            {
                if (ViewState["Remark"] == null)
                { return ""; }
                else
                    return ViewState["Remark"].ToString();
            }
            set { ViewState["Remark"] = value; }
        }

        /// <summary>
        /// 设置文件保存的方式

        /// </summary>
        [Description("设置上传文件保存的方式。"),
        Browsable(true)]
        [DefaultValue(AuxiliaryClass.UploadType.WebDAV)]
        public AuxiliaryClass.UploadType UploadType
        {
            set { uploadType = value; }
        }

        /// <summary>
        /// 设置文件上传的类型，多个用,连接，如（.xls,.txt）]。

        /// </summary>
        [Description("设置上传的类型[扩展名称，多个用<,>连接，如（.xls,.txt）]。"),
        Browsable(true)]
        public string FileType
        {
            get
            {
                if (ViewState["FileType"] == null)
                { return ""; }
                else
                    return ViewState["FileType"].ToString();

            }
            set { ViewState["FileType"] = value; }
        }

        /// <summary>
        /// 得到上传文件的附件信息

        /// </summary>
        [Browsable(false)]
        public List<Attachment> AttachmentList
        {
            get { return this.attachmentlist; }
        }


        /// <summary>
        /// 文件保存的地址，在（web.config中appSettings设置）

        /// </summary>
        [Description("文件保存的地址，在（web.config中appSettings设置）"),
        Browsable(true)]
        public string SaveFileDir
        {
            set { this.saveFileDir = value; }
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


        private string cacheUploadPath
        {
            get
            {
                if (uploadType == AuxiliaryClass.UploadType.Localhost)
                    return System.IO.Path.Combine(WebUtils.GetAppStrPath(), System.Configuration.ConfigurationManager.AppSettings[saveFileDir].ToString());
                return System.Configuration.ConfigurationManager.AppSettings[saveFileDir].ToString();
            }
            //get { return System.Configuration.ConfigurationManager.AppSettings[saveFileDir].ToString(); }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        public void LoadFileToServer()
        {
            UoloadInputFile();
            UploadChildInputFile();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //this.CreateChildControls1();
            base.Render(writer);
        }

        protected override void CreateChildControls()
        {
            //清除现有的子控件及其 ViewState 
            this.Controls.Clear();
            this.ClearChildViewState();
            base.CreateChildControls();

            // 生成控件树 
            // 生成环境表格（一行，两个单元格） 
            Table myTable = new Table();
            //build the table row生成表格中的行 
            TableRow row1 = new TableRow();
            TableRow row2 = new TableRow();
            myTable.Rows.Add(row1);
            myTable.Rows.Add(row2);
            // 生成单元格 
            TableCell myCell1 = new TableCell();
            TableCell myCell2 = new TableCell();

            uploadPanel = new DynamicPanel();
            uploadPanel.ID = "MyUploadPanel";

            defaultInputFile = new InputFile();
            defaultInputFile.ID = "MyInputFile";

            Anthem.Button addInputFile = new Anthem.Button();
            addInputFile.CausesValidation = false;
            addInputFile.Text = "增加";
            addInputFile.UpdateAfterCallBack = true;
            addInputFile.Click += new EventHandler(addInputFile_Click);

            Panel tempPanel = new Panel();

            tempPanel.Controls.Add(defaultInputFile);

            tempPanel.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

            tempPanel.Controls.Add(addInputFile);

            myCell1.Controls.Add(uploadPanel);
            myCell2.Controls.Add(tempPanel);
            row1.Cells.Add(myCell1);
            row2.Cells.Add(myCell2);

            Controls.Add(myTable);
        }

        /// <summary>
        /// 添加上传控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addInputFile_Click(object sender, EventArgs e)
        {
            uploadPanel.AutoUpdateAfterCallBack = true;
            int idIndex = uploadPanel.Controls.Count;
            Panel pcontrol = new Panel();
            pcontrol.ID = string.Format("p_{0}", idIndex);

            InputFile inputFile = new InputFile();
            inputFile.ID = string.Format("ipf_{0}", idIndex);
            pcontrol.Controls.Add(inputFile);

            uploadPanel.Controls.Add(pcontrol);
        }

        private void UoloadInputFile()
        {
            if (defaultInputFile.ContentLength > 0)
            {
                string fileName = defaultInputFile.FileName;//获取上传文件的全路径
                string extenName = System.IO.Path.GetExtension(fileName);//获取扩展名

                string tempStr = Guid.NewGuid().ToString();
                string zipFileName = tempStr + extenName;
                if (!CheckFileType(extenName))
                    return;
                if (FileService.UploadFile(defaultInputFile, this.uploadType, zipFileName, this.cacheUploadPath, this.CacheFileManager))
                //if (CacheFileManager.UploadFile(defaultInputFile.FileContent, zipFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.Creator = Creator;
                    attachment.Title = FileUtils.GetFileName(fileName);
                    attachment.FileSize = defaultInputFile.ContentLength;
                    attachment.MimeType = extenName;
                    attachment.FileName = cacheUploadPath + tempStr + attachment.MimeType;
                    attachment.Remark = Remark;
                    attachment.Id = attachment.Create();
                    attachmentlist.Add(attachment);
                }
            }
        }

        private void UploadChildInputFile()
        {
            // 上传手动添加的上传控件中的附件

            foreach (Control childControl in this.uploadPanel.Controls)
            {
                // 上传控件都放在 Panel 服务器控件中
                if (childControl.GetType() != typeof(Panel)) continue;

                foreach (Control inputControl in childControl.Controls)
                {
                    // 找到上传控件
                    if (inputControl.GetType() != typeof(InputFile)) continue;
                    InputFile tempInputFile = inputControl as InputFile;
                    if (tempInputFile != null && tempInputFile.HasFile)
                    {
                        string fileName = tempInputFile.FileName;//获取上传文件的全路径
                        string extenName = System.IO.Path.GetExtension(fileName);//获取扩展名

                        string tempStr = Guid.NewGuid().ToString();
                        string zipFileName = tempStr + extenName;
                        if (!CheckFileType(extenName))
                            continue;
                        if (FileService.UploadFile(tempInputFile, this.uploadType, zipFileName, this.cacheUploadPath, this.CacheFileManager))
                        //if (CacheFileManager.UploadFile(tempInputFile.FileContent, zipFileName))
                        {
                            Attachment attachment = new Attachment();
                            attachment.Creator = Creator;
                            attachment.Title = FileUtils.GetFileName(fileName);
                            attachment.FileSize = tempInputFile.ContentLength;
                            attachment.MimeType = extenName;
                            attachment.FileName = cacheUploadPath + tempStr + attachment.MimeType;
                            attachment.Remark = Remark;
                            attachment.Id = attachment.Create();
                            attachmentlist.Add(attachment);
                        }
                    }

                }
            }
        }

        private bool CheckFileType(string fileExtensionName)
        {
            if (string.IsNullOrEmpty(this.FileType))
                return true;
            if (!this.FileType.Contains(",") && FileType == fileExtensionName)
                return true;
            string[] arm = this.FileType.Split(',');
            for (int i = 0; i < arm.Length; i++)
            {
                if (arm[i] == fileExtensionName)
                    return true;
            }
            return false;
        }

    }
}
