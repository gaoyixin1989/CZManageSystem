using CZManageSystem.Admin.Models;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Composite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.FilesManager
{
    public class UploadController : BaseController
    {

        IAdmin_AttachmentService _attchmentService = new Admin_AttachmentService();
        // GET: Upload


        public ActionResult Index(string Upguid = null, string FilePath = null)
        {
            ViewData["Upguid"] = Upguid;
            ViewData["FilePath"] = FilePath;
            return View();
        }
        public ActionResult GetOGSMDataByID(string id)
        {
            Guid AttachmentId = new Guid();
            Guid.TryParse(id, out AttachmentId);
            var modelList = _attchmentService.GetAllAttachmentList(AttachmentId);
            return Json(new { data = modelList });
        }
        // GET: Upload
        public ActionResult FileUpload(string Upguid, string FilePath)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            HttpPostedFileBase file = Request.Files[0];
            Admin_Attachment attachment = new Admin_Attachment();
            UploadHelper ulph = new UploadHelper(FilePath);
            HttpContext context = System.Web.HttpContext.Current;
            string aid = Upguid;// context.Request.Params["Upguid"];
            string fileName = null;
            try
            {
                fileName = FileHelper.GetUniqueFileName(file.FileName);
                fileName = ulph.Upload(file, fileName);//UploadHelper.Upload(file, fileName, FilePath);

                if (!string.IsNullOrEmpty(fileName))
                {
                    attachment.FileName = System.IO.Path.GetFileName(file.FileName);
                    attachment.MimeType = System.IO.Path.GetExtension(file.FileName);
                    attachment.FileSize = FileHelper.FileSize(file.ContentLength.ToString());
                    attachment.Upguid = new Guid(aid);
                    attachment.Fileupload = fileName;
                    attachment.Creator = this.WorkContext.CurrentUser.UserName;
                    attachment.CreatedTime = DateTime.Now;
                    attachment.Id = Guid.NewGuid();
                    result.IsSuccess = _attchmentService.Insert(attachment);
                    result.Message = "上传文件成功!";
                    var modelList = _attchmentService.GetAllAttachmentList(new Guid(Upguid));
                    result.data = modelList;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "上传文件错误!";
                }
            }
            catch (Exception ex)
            {
                string exstr = ex.ToString();
                result.IsSuccess = false;
                result.Message = "上传文件错误!";
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "上传文件失败",
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                throw;
            }
            return Json(result);
        }

        public ActionResult FileUploadDelete(Guid id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var objs = _attchmentService.FindById(id);
            var obj = _attchmentService.List().Where(u => u.Id == id).ToList();
            var Upguid = objs.Upguid.ToString();
            var modelList = _attchmentService.GetAllAttachmentList(new Guid(Upguid));
            if (objs != null)
            {
                if (_attchmentService.DeleteByList(obj))
                {
                    modelList = _attchmentService.GetAllAttachmentList(new Guid(Upguid));
                    result.IsSuccess = true;
                    result.Message = "删除成功";
                    result.data = modelList;
                }
            }
            if (!result.IsSuccess)
            {
                result.Message = "删除失败";
                result.data = modelList;
            }
            return Json(result);
        }


        public ActionResult FileDownload(Guid id)
        {
            string Fileupload = "";
            string Filename = "";
            try
            {
                var objs = _attchmentService.FindById(id);
                Fileupload = objs.Fileupload;
                Filename = objs.FileName;
                UploadHelper ulph = new UploadHelper("");
                //ulph.Download(Fileupload, Filename);
                var req = WebRequest.Create(Fileupload) as HttpWebRequest;
                req.Timeout = 100000;
                var ms = new MemoryStream();
                using (WebResponse resp = req.GetResponse())
                {
                    var stream = resp.GetResponseStream();
                    int k = 1024;
                    var buff = new byte[k];
                    while (k > 0)
                    {
                        k = stream.Read(buff, 0, 1024);
                        ms.Write(buff, 0, k);
                    }
                    ms.Flush();
                    ms.Seek(0L, SeekOrigin.Begin);//把指针移动到流的开头
                };
                Response.ContentType = "application/octet-stream";//通知浏览器下载文件而不是打开
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(Filename, System.Text.Encoding.UTF8));
                Response.BinaryWrite(ms.GetBuffer());
                Response.Flush();
                Response.End();
            }
            catch(Exception ex)
            {
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "下载文件失败",
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                throw;
            }            
            return new EmptyResult();
        }
    }
}