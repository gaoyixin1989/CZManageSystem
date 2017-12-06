using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class DataDictionaryController : BaseController
    {
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        // GET: DataDictionary

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        [SysOperation(OperationType.Browse, "访问数据字典页面")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增编辑页面
        /// </summary>
        /// <returns></returns>
        
        public ActionResult Edit(string id = "")
        {
            ViewData["id"] = id;
            if (string.IsNullOrEmpty(id))
                ViewBag.Title = "字典新增";
            else
                ViewBag.Title = "字典编辑";

            return View();
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">数据量</param>
        /// <param name="DDName">字典名称</param>
        /// <param name="searchDDName">用于搜索的字典名称</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, DataDicQueryBuilder queryBuilder = null)
        {
            int count = 0;
                
            var modelList = _dataDictionaryService.QueryDataByPage(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, queryBuilder?.DDName, queryBuilder?.searchDDName);
            return Json(new { items = modelList, count = count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(string id = null)
        {
            Guid guid = new Guid();
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out guid))
                return View("Index");
            var obj = _dataDictionaryService.FindById(guid);
            return Json(new
            {
                obj.DDId,
                obj.DDName,
                obj.DDValue,
                obj.DDText,
                obj.ValueType,
                obj.EnableFlag,
                obj.DefaultFlag,
                obj.OrderNo,
                obj.Creator,
                obj.LastModifier,
                Createdtime = obj.Createdtime == null ? "" : obj.Createdtime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                LastModTime = obj.LastModTime == null ? "" : obj.LastModTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Save,"保存数据字典")]
        public ActionResult Save(DataDictionary dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (dataObj.DDName == null || string.IsNullOrEmpty(dataObj.DDName.Trim()))
                tip = "字典名称不能为空";
            else if (dataObj.ValueType == null || string.IsNullOrEmpty(dataObj.ValueType.Trim()))
                tip = "字典类型不能为空";
            else if (dataObj.DDValue == null || string.IsNullOrEmpty(dataObj.DDValue.Trim()))
                tip = "字典值不能为空";
            else if (dataObj.DDText == null || string.IsNullOrEmpty(dataObj.DDText.Trim()))
                tip = "字典文本不能为空";
            else
            {
                isValid = true;
                dataObj.DDName = dataObj.DDName.Trim();
                dataObj.ValueType = dataObj.ValueType.Trim();
                dataObj.DDValue = dataObj.DDValue.Trim();
                dataObj.DDText = dataObj.DDText.Trim();
            }

            //相同“字典名称”，不能存在相同的值
            var objs = _dataDictionaryService.List().Where(u => u.DDName == dataObj.DDName && u.DDValue == dataObj.DDValue && u.DDId != dataObj.DDId);
            if (objs.Count() > 0)
            {
                isValid = false;
                tip = "该字典值已经存在";
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion
            if (dataObj.DDId == Guid.Parse("00000000-0000-0000-0000-000000000000"))//新增
            {
                dataObj.DDId = Guid.NewGuid();
                dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                dataObj.Createdtime = DateTime.Now;
                result.IsSuccess = _dataDictionaryService.Insert(dataObj);
            }
            else
            {//编辑
                dataObj.LastModifier = this.WorkContext.CurrentUser.UserName;
                dataObj.LastModTime = DateTime.Now;
                result.IsSuccess = _dataDictionaryService.Update(dataObj);
            }
            if (result.IsSuccess && (dataObj.DefaultFlag ?? false))//字典中，相同的“字典名称”只能有一项为“默认选项”
            {

            }

            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }

            return Json(result);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Delete, "删除数据字典")]
        public ActionResult Delete(Guid[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _dataDictionaryService.List().Where(u => ids.Contains(u.DDId)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _dataDictionaryService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        /// <summary>
        /// 获取字典分组信息（根据字典名称分组）
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListDictNameGroup()
        {
            return Json(_dataDictionaryService.GetDictNameGroup(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据字典名称获取字典数据
        /// </summary>
        /// <param name="DDName">名称</param>
        /// <returns></returns>
        public ActionResult GetDictListByName(string DDName)
        {
            int count = 0;
            var modelList = new List<DataDictionary>();
            if (!string.IsNullOrEmpty(DDName))
            {
                modelList = _dataDictionaryService.QueryDataByPage(out count, 0, int.MaxValue, DDName, null).ToList();
                modelList = modelList.Where(u => (u.EnableFlag ?? false)).ToList();
            }

            List<object> itemResult = new List<object>();
            foreach (var model in modelList)
            {
                itemResult.Add(new
                {
                    name = model.DDValue,
                    value = model.DDValue,
                    text = model.DDText
                });
            }

            var defaultObj = modelList.Where(u => (u.DefaultFlag ?? false)).FirstOrDefault();//默认选项
            string defaultValue = defaultObj == null ? null : defaultObj.DDValue;//默认值
            return Json(new { items = itemResult, defaultvalue = defaultValue, count = count }, JsonRequestBehavior.AllowGet);
        }

    }
}