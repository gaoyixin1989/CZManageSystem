using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.AnnualLeave;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.AnnualLeave;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.AnnualLeave
{
    public class AnnualLeaveManageController : BaseController
    {
        // GET: AnnualLeaveManage
        IHRAnnualLeaveService _hrannualleaveservice = new HRAnnualLeaveService();
        ISysUserService _userService = new SysUserService();
        [SysOperation(OperationType.Browse, "访问年休假管理页面")]
        public ActionResult Index()
        {
            ViewData["Year"] = DateTime.Now.ToString("yyyy");
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, HRAnnualLeaveQueryBuilder queryBuilder = null)
        {
            int count = 0;

            //当有部门的条件时，应该先查询出该条件下所包含的所有部门(自身和子级部门)
            if (queryBuilder.DpId != null)
                queryBuilder.DpId = Get_Subdept_ByDept(queryBuilder.DpId);
            if (queryBuilder.DpId == null || queryBuilder.DpId.Count == 0||(queryBuilder.DpId.Count == 1&& string.IsNullOrEmpty(queryBuilder.DpId[0].ToString())))
                queryBuilder.DpId = null;
            var modelList = _hrannualleaveservice.GetForPagingByCondition(this.WorkContext.CurrentUser, out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _userobj = _userService.FindByFeldName(u => u.UserId == item.UserId);
                List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                listResult.Add(new
                {
                    Id = item.Id,
                    item.UserName,
                    item.UserId,
                    item.VYears,
                    item.VDays,
                    item.FdYearVDays,
                    item.FdLastYearVDays,
                    item.BcYearVDays,
                    EmployeeId = _userobj.EmployeeId,
                    DpName = _tmpdplist[0],//CommonFunction.getDeptNamesByIDs(_userobj.DpId),
                    DpmName = _tmpdplist[1],
                    RealName = _userobj.RealName
                });
            }
            return Json(new { items = listResult, count = count });
        }

        public ActionResult Edit(int? id)
        {
            ViewData["id"] = id;
            return View();
        }
        public ActionResult GetDataByID(int id)
        {
            var item = _hrannualleaveservice.FindById(id);
            return Json(new
            {
                item.Id,
                item.VYears,
                item.VDays,
                item.UserName,
                item.UserId,
                item.FdLastYearVDays,
                item.FdYearVDays,
                item.BcYearVDays
            });

        }
        [SysOperation(OperationType.Delete, "删除年休假数据")]
        public ActionResult Delete(int[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _hrannualleaveservice.List().Where(u => Ids.Contains(u.Id)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _hrannualleaveservice.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                IsSuccess = successCount > 0 ? true : false,
                SuccessCount = successCount
            });
        }


        [SysOperation(OperationType.Save, "保存年休假数据")]
        public ActionResult Save(HRAnnualLeave dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过 

            var list = _hrannualleaveservice.List().Where(u => dataObj.VYears.Contains(u.VYears) && u.UserName == dataObj.UserName && u.Id != dataObj.Id).ToList();

            if(list.Count>0)
            {
                tip = "用户该年年休假数据已建立！";
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
            dataObj.FdLastYearVDays = dataObj.FdLastYearVDays?? 0;
            dataObj.BcYearVDays = dataObj.BcYearVDays ?? 0;
            //if (string.IsNullOrEmpty(dataObj.BcYearVDays.Value.ToString()))
            //    dataObj.BcYearVDays = 0;

            if (dataObj.Id == 0)//新增
            {
                result.IsSuccess = _hrannualleaveservice.Insert(dataObj);
            }
            else
            {//编辑                
                result.IsSuccess = _hrannualleaveservice.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        #region 其他的方法
        /// <summary>
        /// 根据部门组织Ids获取包括自身的所有子节点的id
        /// </summary>
        /// <param name="ids">组织Ids</param>
        /// <returns></returns>
        private List<string> Get_Subdept_ByDept(List<string> ids)
        {
            List<string> listResult = new List<string>();
            if (ids == null || ids.Count == 0)
                return listResult;
            string[] temp = ids[0].Split(',');
            for (int i = 0; i < temp.Length; i++)
            {
                var mm = new EfRepository<string>().Execute<string>(string.Format("select * from  dbo.Get_Subdept_ByDept ('{0}')", temp[i].ToString())).ToList();
                listResult.AddRange(mm);
            }
            return listResult;
        }        
        #endregion

    }


}