using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.XQP.Commons;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 附件.
    /// </summary>
    public class Attachment
    {
        #region getter/setter

        private Guid id;
        private string creator;
        private string lastModifier;
        private DateTime? createdTime;
        private DateTime? lastModTime;

        private string title;
        private string fileName;
        private string mimeType;
        private decimal fileSize;
        private string remark;
        private int downloads;

        /// <summary>
        /// 标识.
        /// </summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 创建者.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 最后更新者.
        /// </summary>
        public string LastModifier
        {
            get { return lastModifier; }
            set { lastModifier = value; }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime? CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// 最后更新时间.
        /// </summary>
        public DateTime? LastModTime
        {
            get { return lastModTime; }
            set { lastModTime = value; }
        }

        /// <summary>
        /// 附件标题/名称.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 文件名(包括路径).
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 文件类型.
        /// </summary>
        public string MimeType
        {
            get { return mimeType; }
            set { mimeType = value; }
        }

        /// <summary>
        /// 文件大小.
        /// </summary>
        public decimal FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        /// <summary>
        /// 备注.
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 下载次数.
        /// </summary>
        public int Downloads
        {
            get { return downloads; }
            set { downloads = value; }
        }

        #endregion

        #region constructors

        /// <summary>
        /// 构造方法.
        /// </summary>
        public Attachment()
        {
            this.downloads = 0;
            this.id = Guid.NewGuid();
            this.createdTime = DateTime.Now;
            this.lastModTime = this.createdTime;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="postedFile"></param>
        public Attachment(System.Web.HttpPostedFile postedFile)
            : this()
        {
            if (postedFile == null)
                throw new ArgumentException("参数 postedFile 值为 null.");

            string postedPath = postedFile.FileName;
            int index = postedPath.LastIndexOf("\\");
            string postedName = postedPath.Substring(index + 1);

            this.title = FileUtils.GetFileName(postedName);
            this.mimeType = System.IO.Path.GetExtension(postedName);
            this.fileSize = postedFile.ContentLength;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="creator"></param>
        /// <param name="saveFileName"></param>
        public Attachment(System.Web.HttpPostedFile postedFile, string creator, string saveFileName)
            : this(postedFile)
        {
            this.fileName = UploadHelper.UploadRootDirectory + saveFileName;
            this.creator = creator;
            this.lastModifier = creator;
        }
        #endregion

        /// <summary>
        /// 插入附件数据.
        /// </summary>
        /// <returns></returns>
        public Guid Create()
        {
            return this.Create(this.id);
        }

        /// <summary>
        /// 插入附件数据.
        /// </summary>
        /// <param name="attachementId"></param>
        /// <returns></returns>
        public Guid Create(Guid attachementId)
        {
            this.id = attachementId;
            IBatisMapper.Insert("Attachment_Insert", this);
            return attachementId;
        }

        /// <summary>
        /// 更新附件信息.
        /// </summary>
        public void Update()
        {
            IBatisMapper.Update("Attachment_Update", this);
        }

        /// <summary>
        /// 删除一条附件纪录并删除服务器上的附件.
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteById(Guid id)
        {
            //删除服务器上的附件
            Attachment att = LoadById(id);
            string fileName = att.FileName.Substring(att.FileName.LastIndexOf("/") + 1);
            //AttachmentHelper.DeleteFileFromServer(fileName);
            UploadHelper.Delete(fileName);

            IBatisMapper.Delete("Attachment_Delete", id);
        }

        /// <summary>
        /// 依据附件id查询附件详细纪录.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Attachment LoadById(Guid id)
        {
            return IBatisMapper.Load<Attachment>("Attachment_Select_By_Id", id);
        }

        /// <summary>
        /// 查询所有的附件.
        /// </summary>
        /// <returns></returns>
        public static IList<Attachment> SelectAll()
        {
            return IBatisMapper.Select<Attachment>("Attachment_Select_By_Id");
        }
        
    }
}
