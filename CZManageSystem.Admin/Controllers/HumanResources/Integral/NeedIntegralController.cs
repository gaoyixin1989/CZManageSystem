using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Integral;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.Integral;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.NeedIntegral
{
    public class NeedIntegralController : BaseController
    {
        // GET: NeedIntegral
        ISysUserService _userService = new SysUserService();
        IHRNeedIntegralService needintegralservice = new HRNeedIntegralService();
        IUum_UserinfoService _uum_userinfoservice = new Uum_UserinfoService();
        [SysOperation(OperationType.Browse, "访问积分要求管理页面")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            ViewData["id"] = id;
            return View();
        }
        public ActionResult GetDataByID(Guid id)
        {
            var item = needintegralservice.FindById(id);
            var _userobj = _userService.FindByFeldName(u => u.UserId == item.UserId);
            var _uumobj = _uum_userinfoservice.FindByFeldName(u => u.employee == _userobj.EmployeeId);
            List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
            return Json(new
            {
                item.Id,
                DpName = _tmpdplist[0],
                //DpName = CommonFunction.getDeptNamesByIDs(_userobj.DpId),
                _userobj.RealName,
                item.UserName,
                item.UserId,
                item.NeedIntegral,
                PosiLevel = _uumobj != null ? _uumobj.userPosiLevel : "",
                item.YearDate
            });
        }


        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, HRNeedIntegralQueryBuilder queryBuilder = null)
        {
            int count = 0;

            //当有部门的条件时，应该先查询出该条件下所包含的所有部门(自身和子级部门)
            if (queryBuilder.DpId != null)
                queryBuilder.DpId = Get_Subdept_ByDept(queryBuilder.DpId);
            if (queryBuilder.DpId == null || queryBuilder.DpId.Count == 0 || (queryBuilder.DpId.Count == 1 && string.IsNullOrEmpty(queryBuilder.DpId[0].ToString())))
                queryBuilder.DpId = null;
            var modelList = needintegralservice.GetForPagingByCondition(this.WorkContext.CurrentUser, out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _userobj = _userService.FindByFeldName(u => u.UserId == item.UserId);
                var _uumobj = _uum_userinfoservice.FindByFeldName(u => u.employee == _userobj.EmployeeId);
                List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                listResult.Add(new
                {
                    Id = item.Id,
                    item.NeedIntegral,
                    item.UserId,
                    item.YearDate,
                    EmployeeId = _userobj.EmployeeId,
                    DpName = _tmpdplist[0],
                    DpmName = _tmpdplist[1],
                    PosiLevel = _uumobj != null? _uumobj.userPosiLevel:"",
                    //DpName = CommonFunction.getDeptNamesByIDs(_userobj.DpId),
                    RealName = _userobj.RealName,
                    Source = ShowSource(item.DoFlag.Value)
                });
            }
            return Json(new { items = listResult, count = count });
        }

        [SysOperation(OperationType.Save, "保存积分要求数据")]
        public ActionResult Save(HRNeedIntegral dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            double times = 0.00, c_integral = 0.00;
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
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
            if (dataObj.Id == Guid.Empty)//新增
            {
                dataObj.Id = Guid.NewGuid();
                result.IsSuccess = needintegralservice.Insert(dataObj);
            }
            else
            {//编辑                
                dataObj.DoFlag = 1;
                result.IsSuccess = needintegralservice.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }



        #region
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
        private string ShowSource(int flag)
        {
            string result = "";
            switch (flag)
            {
                case 1: result = "手工维护"; break;
                case 0: result = "数据同步"; break;
                default: break;
            }
            return result;
        }
        #endregion

    }
}