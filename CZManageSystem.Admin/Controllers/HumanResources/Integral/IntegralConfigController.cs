using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.HumanResources.Integral;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.Integral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.IntegralConfig
{
    public class IntegralConfigController : BaseController
    {
        // GET: IntegralConfig
        IHRCintegralConfigService _cintegralconfigservice = new HRCintegralConfigService();
        ITIntegralConfigService _tintegralconfigservice = new TIntegralConfigService();
        IHRRankConfigService _hrrankconfigservice = new HRRankConfigService();
        [SysOperation(OperationType.Browse, "访问培训积分配置页面")]
        public ActionResult CourseIndex()
        {
            return View();
        }

        public ActionResult CourseEdit(int? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
        public ActionResult GetCourseIntegralListData(int pageIndex = 1, int pageSize = 5)
        {
            int count = 0;
            var modelList = _cintegralconfigservice.GetForPaging(out count, null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (HRCintegralConfig)u).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var method = "";
                var range = "";
                range = item.Mindays + "<=培训天数<" + item.Maxdays;
                if (item.BuseFormula == "公式")
                    method = "计算公式：天数*" + item.Times;
                else if (item.BuseFormula == "常量")
                    method = "积分常量：" + item.Integral;
                listResult.Add(new
                {
                    item.Id,
                    Range = range,
                    Method = method
                });
            }
            return Json(new { items = listResult, count = count });
        }
        public ActionResult GetCourseIntegralDataByID(int id)
        {
            var item = _cintegralconfigservice.FindById(id);
            return Json(new
            {
                item.Mindays,
                item.Maxdays,
                item.Id,
                item.BuseFormula,
                item.Integral,
                item.Times
            });


        }


        [SysOperation(OperationType.Delete, "删除培训积分配置数据")]
        public ActionResult CourseIntegralDelete(int[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _cintegralconfigservice.List().Where(u => Ids.Contains(u.Id)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _cintegralconfigservice.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                IsSuccess = successCount > 0 ? true : false,
                SuccessCount = successCount
            });
        }
        [SysOperation(OperationType.Save, "保存培训积分配置数据")]
        public ActionResult CourseIntegralSave(HRCintegralConfig dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            HRCintegralConfig obj = new HRCintegralConfig();
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            if (dataObj.Mindays == null|| dataObj.Maxdays == null)
            {
                tip = "培训天数范围不能为空";
            }
            if (dataObj.BuseFormula == null)
            {
                tip = "计算方式不能为空";
            }else
            {
                if(dataObj.BuseFormula == "公式")
                {
                    if (dataObj.Times == null)
                        tip = "天数*函数不能为空";
                }else
                {
                    if (dataObj.Integral == null)
                        tip = "积分常量不能为空";
                }                    
            }           
            if (tip == "")
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion
            if (dataObj.Id == 0)//新增
            {
                result.IsSuccess = _cintegralconfigservice.Insert(dataObj);
            }
            else
            {//编辑
                if (dataObj.BuseFormula == "公式")
                {
                    dataObj.Integral = null;
                }
                else
                {
                    dataObj.Times = null;
                }
                result.IsSuccess = _cintegralconfigservice.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }
        [SysOperation(OperationType.Browse, "访问职级积分配置页面")]
        public ActionResult RankIndex()
        {
            return View();
        }
        public ActionResult RankEdit(int? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
        public ActionResult GetRankDataByID(int id)
        {
            var item = _hrrankconfigservice.FindById(id);
            return Json(new
            {
                item.Id,
                item.SGrade,
                item.EGrade,
                item.Integral
            });
        }

        public ActionResult GetRankListData(int pageIndex = 1, int pageSize = 5)
        {
            int count = 0;
            var modelList = _hrrankconfigservice.GetForPaging(out count, null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (HRRankConfig)u).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var range = "";
                range = item.SGrade + "<=职级<=" + item.EGrade;
                listResult.Add(new
                {
                    item.Id,
                    Range = range,
                    Integral = item.Integral
                });
            }
            return Json(new { items = listResult, count = count });
        }
        [SysOperation(OperationType.Delete, "删除职级积分配置数据")]
        public ActionResult RankDelete(int[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _hrrankconfigservice.List().Where(u => Ids.Contains(u.Id)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _hrrankconfigservice.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                IsSuccess = successCount > 0 ? true : false,
                SuccessCount = successCount
            });
        }
        [SysOperation(OperationType.Save, "保存职级积分配置数据")]
        public ActionResult RankSave(HRRankConfig dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            HRRankConfig obj = new HRRankConfig();
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            if (dataObj.SGrade == null || dataObj.EGrade == null)
            {
                tip = "培训天数范围不能为空";
            }
            if (dataObj.Integral == null)
            {
                tip = "计算方式不能为空";
            }            
            if (tip == "")
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion
            if (dataObj.Id == 0)//新增
            {
                result.IsSuccess = _hrrankconfigservice.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _hrrankconfigservice.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }
        public ActionResult TeachingEdit(int? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
        [SysOperation(OperationType.Browse, "访问授课积分配置页面")]
        public ActionResult TeachingIndex()
        {
            return View();
        }

        public ActionResult GetTeachingIntegralListData(int pageIndex = 1, int pageSize = 5)
        {
            int count = 0;
            var modelList = _tintegralconfigservice.GetForPaging(out count, null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (HRTIntegralConfig)u).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var method = "";
                var range = "";
                range = item.Mindays + "<=培训天数<" + item.Maxdays;
                if (item.BuseFormula == "公式")
                    method = "计算公式：天数*" + item.Times;
                else if (item.BuseFormula == "常量")
                    method = "积分常量：" + item.Integral;
                listResult.Add(new
                {
                    item.Id,
                    Range = range,
                    Method = method
                });
            }
            return Json(new { items = listResult, count = count });
        }




        public ActionResult GetTIntegralConfigDataByID(int id)
        {
            var item = _tintegralconfigservice.FindById(id);
            return Json(new
            {
                item.Mindays,
                item.Maxdays,
                item.Id,
                item.BuseFormula,
                item.Integral,
                item.Times
            });

        }


        [SysOperation(OperationType.Save, "保存授课积分配置数据")]
        public ActionResult TIntegralConfigSave(HRTIntegralConfig dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            HRTIntegralConfig obj = new HRTIntegralConfig();
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            if (dataObj.Mindays == null || dataObj.Maxdays == null)
            {
                tip = "培训天数范围不能为空";
            }
            if (dataObj.BuseFormula == null)
            {
                tip = "计算方式不能为空";
            }
            else
            {
                if (dataObj.BuseFormula == "公式")
                {
                    if (dataObj.Times == null)
                        tip = "天数*函数不能为空";
                }
                else
                {
                    if (dataObj.Integral == null)
                        tip = "积分常量不能为空";
                }
            }
            if (tip == "")
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion
            if (dataObj.Id == 0)//新增
            {
                result.IsSuccess = _tintegralconfigservice.Insert(dataObj);
            }
            else
            {//编辑
                if (dataObj.BuseFormula == "公式")
                {
                    dataObj.Integral = null;
                }
                else
                {
                    dataObj.Times = null;
                }
                result.IsSuccess = _tintegralconfigservice.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }




            //#region 验证数据是否合法
            //string tip = "";
            //bool isValid = false;//是否验证通过            
            //if (dataObj.Times == null)
            //{
            //    tip = "计算公式不能为空";
            //}
            //if (tip == "")
            //{
            //    isValid = true;
            //}
            //else
            //{
            //    isValid = false;
            //}
            //if (!isValid)
            //{
            //    result.IsSuccess = false;
            //    result.Message = tip;
            //    return Json(result);
            //}
            //#endregion
            //if (dataObj.Id == 0)//新增
            //{
            //    result.IsSuccess = _tintegralconfigservice.Insert(dataObj);
            //}
            //else
            //{//编辑
            //    result.IsSuccess = _tintegralconfigservice.Update(dataObj);
            //}
            //if (!result.IsSuccess)
            //{
            //    result.Message = "保存失败";
            //}
            return Json(result);
        }
        [SysOperation(OperationType.Delete, "删除授课积分配置数据")]
        public ActionResult TeachingIntegralDelete(int[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _tintegralconfigservice.List().Where(u => Ids.Contains(u.Id)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _tintegralconfigservice.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                IsSuccess = successCount > 0 ? true : false,
                SuccessCount = successCount
            });
        }

    }
}