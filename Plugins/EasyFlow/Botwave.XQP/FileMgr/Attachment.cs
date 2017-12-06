using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.FileMgr
{
    /// <summary>
    /// 附件信息类.
    /// </summary>
    public class Attachment
    {
        #region State 值

        /// <summary>
        /// 正常状态.
        /// </summary>
        public const int State_Normal = 0;
        /// <summary>
        /// 已经被删除.
        /// </summary>
        public const int State_Deleted = 1;

        #endregion

        #region gets / sets

        private int _attachmentId;
        private Guid _attachmentKey;
        private string _sysId;
        private int _state;
        private string _fileName;
        private string _filePath;
        private decimal _fileLength;
        private string _fileType;
        private string _remark;
        private string _creator;
        private DateTime _createdTime;
        private string _lastModifier;
        private DateTime _lastModTime;

        /// <summary>
        /// 附件编号.
        /// </summary>
        public int AttachmentId
        {
            get { return _attachmentId; }
            set { _attachmentId = value; }
        }

        /// <summary>
        /// 附件唯一键值.
        /// </summary>
        public Guid AttachmentKey
        {
            get { return _attachmentKey; }
            set { _attachmentKey = value; }
        }

        /// <summary>
        /// 附加所属应用系统编号.
        /// </summary>
        public string SysId
        {
            get { return _sysId; }
            set { _sysId = value; }
        }

        /// <summary>
        /// 附件状态. 0：表示正常状态；1 表示删除状态.
        /// </summary>
        public int State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 附件的文件名.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// 附件存储的路径.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        /// <summary>
        /// 附件的文件大小.
        /// </summary>
        public decimal FileLength
        {
            get { return _fileLength; }
            set { _fileLength = value; }
        }

        /// <summary>
        /// 附件的文件类型.
        /// </summary>
        public string FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        /// <summary>
        /// 附件备注信息.
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        /// <summary>
        /// 附件的创建人.
        /// </summary>
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        /// <summary>
        /// 附件创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        /// <summary>
        /// 附件的最近一次修改人.
        /// </summary>
        public string LastModifier
        {
            get { return _lastModifier; }
            set { _lastModifier = value; }
        }

        /// <summary>
        /// 附件的最近一次修改时间.
        /// </summary>
        public DateTime LastModTime
        {
            get { return _lastModTime; }
            set { _lastModTime = value; }
        }
        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public Attachment()
        {
            this._attachmentKey = Guid.NewGuid();
            this._state = State_Normal;
            this._createdTime = DateTime.Now;
            this._lastModTime = this._createdTime;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="postedFile">HttpPostedFile 文件流对象.</param>
        public Attachment(System.Web.HttpPostedFile postedFile)
            : this(postedFile, string.Empty)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="postedFile">HttpPostedFile 文件流对象.</param>
        /// <param name="creator">文件创建(上传)人.</param>
        public Attachment(System.Web.HttpPostedFile postedFile, string creator)
            : this()
        {
            this._fileName = Botwave.FileManager.FileManagerHelper.GetFileName(postedFile.FileName);
            this._fileLength = postedFile.ContentLength;
            this._fileType = postedFile.ContentType;
            this._creator = creator;
            this._lastModifier = creator;
        }
    }
}
