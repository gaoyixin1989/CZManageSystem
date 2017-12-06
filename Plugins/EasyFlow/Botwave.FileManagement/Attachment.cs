using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Botwave.Extension.IBatisNet;
using Botwave.Commons;

namespace Botwave.FileManagement
{
    /// <summary>
    /// 附件
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
        private string ownership;

        /// <summary>
        /// 标识
        /// </summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 最后更新者
        /// </summary>
        public string LastModifier
        {
            get { return lastModifier; }
            set { lastModifier = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastModTime
        {
            get { return lastModTime; }
            set { lastModTime = value; }
        }

        /// <summary>
        /// 附件标题/名称
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 文件名(包括路径)
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string MimeType
        {
            get { return mimeType; }
            set { mimeType = value; }
        }

        /// <summary>
        /// 文件大小 b
        /// </summary>
        public decimal FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int Downloads
        {
            get { return downloads; }
            set { downloads = value; }
        }

        /// <summary>
        /// 附件归属对象（多个用“,”隔开）  为空为任何人
        /// </summary>
        public string OwnerShip
        {
            get { return ownership; }
            set { ownership = value; }
        }
        #endregion

        public Guid Create()
        {
            return this.Create(Guid.NewGuid());
        }

        public Guid Create(Guid attachementId)
        {
            this.id = attachementId;
            IBatisMapper.Insert("Attachment_Insert", this);
            return attachementId;
        }

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
            //删除服务器上的附件.
            Attachment att = LoadById(id);
            if (null != att)
            {
                string fileName = att.FileName.Substring(att.FileName.LastIndexOf("/") + 1);
                FileService.DeleteFileFromServer(fileName);
                IBatisMapper.Delete("Attachment_Delete", id);
            }            
        }

        /// <summary>
        /// 依据附件id查询附件详细纪录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Attachment LoadById(Guid id)
        {
            return IBatisMapper.Load<Attachment>("Attachment_Select_By_Id", id);
        }

        /// <summary>
        /// 查询所有的附件
        /// </summary>
        /// <returns></returns>
        public static IList<Attachment> SelectAll()
        {
            return IBatisMapper.Select<Attachment>("Attachment_Select_By_Id");
        }

        
    }
}
