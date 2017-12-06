using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.SysManager
{

    public class SysDeptmentController : Controller
    {
        // GET: SysDeptment
        [SysOperation(OperationType.Browse, "访问部门管理页面")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            ViewData["DpId"] = id;
            return View();
        }

        public ActionResult GET(string id)
        {

            ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
            var dept = _sysDeptmentService.FindById(id);
            return Json(dept);
        }
        public ActionResult GetAllSysDeptment()
        {
            ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
            int count = 0;

            var modelList = _sysDeptmentService.List().Where(u => u.Type == 1).OrderBy(u => u.DeptOrderNo).ThenBy(u => u.DpId);// (out count,null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            return Json(new { items = modelList, count = modelList.Count() }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetDeptDataStartFromID(string[] ids)
        {
            ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
            List<Depts> listData = new List<Depts>();
            if (ids != null && ids.Length > 0)
            {
                List<string> allIds = GetDpIdFromIds(ids.ToList());
                listData = _sysDeptmentService.List().Where(u => u.Type == 1).OrderBy(u => u.DeptOrderNo).ThenBy(u => u.DpId).Where(u => allIds.Contains(u.DpId)).ToList();
            }
            else
            {
                listData = _sysDeptmentService.List().Where(u => u.Type == 1).OrderBy(u => u.DeptOrderNo).ThenBy(u => u.DpId).ToList();
            }
            return Json(new { items = listData, count = listData.Count() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据部门组织Ids获取包括自身的所有子节点的id
        /// </summary>
        /// <param name="ids">组织Ids</param>
        /// <returns></returns>
        private List<string> GetDpIdFromIds(List<string> ids)
        {
            List<string> listResult = new List<string>();
            if (ids == null || ids.Count == 0)
                return listResult;
            ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
            List<string> temp = ids;
            while (temp != null && temp.Count > 0)
            {
                listResult.AddRange(temp);
                temp = _sysDeptmentService.List().Where(u => u.Type == 1).Where(u => temp.Contains(u.ParentDpId)).Select(u => u.DpId).ToList();
            }
            listResult = listResult.Distinct().ToList();
            return listResult;
        }

        /// <summary>
        /// 更加Ids获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult GetDataByIds(string[] ids)
        {
            ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
            List<Depts> listData = new List<Depts>();
            if (ids != null && ids.Length > 0)
                listData = _sysDeptmentService.List().Where(u => u.Type == 1).OrderBy(u => u.DeptOrderNo).ThenBy(u => u.DpId).Where(u => ids.Contains(u.DpId)).ToList();
            return Json(listData, JsonRequestBehavior.AllowGet);
        }
        [SysOperation(OperationType.Delete, "删除部门信息")]
        public ActionResult Delete(int[] ids)
        {
            ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
            return Json(new { status = 0, message = "成功" });
        }
        [SysOperation(OperationType.Save, "保存部门信息")]
        public ActionResult Save(Depts deptment)
        {
            if (string.IsNullOrEmpty(deptment.DpName))
            {
                return Json(new { status = -1, message = "失败" });
            }
            ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
            if (deptment.DpId != null)
            {
                deptment.LastModTime = DateTime.Now;
                deptment.DpFullName = deptment.DpFullName + ">" + deptment.DpName;
                _sysDeptmentService.Update(deptment);
                return Json(new { status = 0, message = "成功" });
            }
            else
            {

                deptment.DpId = Guid.NewGuid().ToString();

                deptment.CreatedTime = DateTime.Now;
                deptment.DpFullName = deptment.DpFullName + ">" + deptment.DpName;
                _sysDeptmentService.Insert(deptment);
                return Json(new { status = 0, message = "成功" });
            }
        }
        [SysOperation(OperationType.Delete, "删除部门信息")]
        public ActionResult DeleteById(string[] idList)
        {
            var _sysDeptmentService = new SysDeptmentService();
            var obj = new Depts();
            try
            {
                foreach (var id in idList)
                {
                    obj = _sysDeptmentService.FindById(id);
                    if (obj != null && obj.DpId != null)
                    {
                        _sysDeptmentService.Delete(obj);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { status = 1, message = "服务器异常" });
            }
            return Json(new { status = 0, message = "成功" });
        }

    }
}