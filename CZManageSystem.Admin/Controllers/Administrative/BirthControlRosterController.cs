using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative.BirthControl;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class BirthControlRosterController : BaseController
    {
        // GET: BirthControlRoster
        IBirthControlLogService _birthcontrollogService = new BirthControlLogService();
        IBirthControlRosterService _birthcontrolrosterService = new BirthControlRsterService();
        IVW_BirthcontrolRoster_DataService _vwbirthcontrolrosterdataservice = new VW_BirthcontrolRoster_DataService();
        public ActionResult Index()
        {
            var _sysRolesInResourcesService = new RolesInResourcesService();
            var userIdList = new List<object>();
            var objs = new EfRepository<Roles>().Table.ToList().Where(u => "计划生育管理员".Contains(u.RoleName)).Select(u => u.RoleId).ToList();
            for (int i = 0; i < objs.Count; i++)
            {
                var roleid = objs[i].ToString();
                var resourceslist = _sysRolesInResourcesService.GetUserIdByroleId(new Guid(roleid));
                for (int j = 0; j < resourceslist.Count; j++)
                {
                    userIdList.Add(resourceslist[j]);
                }
            }
            if (userIdList.Contains(this.WorkContext.CurrentUser.UserId))
                ViewData["IsAdmin"] = true;
            else
                ViewData["IsAdmin"] = false;
            return View();
        }

        public ActionResult GetVWBirthcontrolListData(int pageIndex = 1, int pageSize = 5, BirthControlRosterBuilder queryBuilder = null)
        {
            int count = 0;
            //当有部门的条件时，应该先查询出该条件下所包含的所有部门(自身和子级部门)
            if (queryBuilder.DpId != null)
                queryBuilder.DpId = Get_Subdept_ByDept(queryBuilder.DpId);
            if (queryBuilder.DpId == null || queryBuilder.DpId.Count == 0)
                queryBuilder.DpId = null;
            var modelList = _vwbirthcontrolrosterdataservice.GetForPagingByCondition(this.WorkContext.CurrentUser, out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.RealName,
                    item.FemaleName,
                    item.FemaleBirthday,
                    item.FemaleWorkingPlace,
                    item.MaleName,
                    item.MaleWorkingPlace,
                    item.InfoStatus,
                    item.UserId
                });
            }

            return Json(new { items = listResult, count = count });
        }
        public ActionResult Edit(Guid? userid)
        {
            //有值=》编辑状态
            ViewData["userid"] = userid;
            return View();
        }
        public ActionResult GetDataByID(Guid id)
        {
            //var BirthcontrolinfoList = new BirthControlInfo();
            DateTime? TimeNull = null;
            object BirthcontrolinfoList = null;
            var objbaseinfo = _vwbirthcontrolrosterdataservice.FindByFeldName(u => u.UserId == id);
            var objbaseList = new
            {
                objbaseinfo.EmployeeId,
                objbaseinfo.RealName,
                objbaseinfo.UserId
            };
            var birthcontrollist = _birthcontrolrosterService.List().Where(u => u.UserId == id).ToList();
            if (birthcontrollist.Count > 0)
            {
                //BirthcontrolinfoList = birthcontrollist[0];
                BirthcontrolinfoList = new
                {
                    id = birthcontrollist[0].id,
                    userId = birthcontrollist[0].UserId,
                    FemaleName = birthcontrollist[0].FemaleName,
                    FemaleBirthday = birthcontrollist[0].FemaleBirthday==null ? "": birthcontrollist[0].FemaleBirthday.ToString("yyyy-MM-dd")  ,
                    FemaleWorkingPlace = birthcontrollist[0].FemaleWorkingPlace,
                    MaleName = birthcontrollist[0].MaleName,
                    MaleWorkingPlace = birthcontrollist[0].MaleWorkingPlace,
                    FirstEmbryoSex = birthcontrollist[0].FirstEmbryoSex,
                    FirstEmbryoBirthday = birthcontrollist[0].FirstEmbryoBirthday == null ? "" : birthcontrollist[0].FirstEmbryoBirthday.Value.ToString("yyyy-MM-dd"),
                    SecondEmbryoSex = birthcontrollist[0].SecondEmbryoSex,
                    Remark = birthcontrollist[0].Remark,
                    SecondEmbryoBirthday = birthcontrollist[0].SecondEmbryoBirthday == null ? "" : birthcontrollist[0].SecondEmbryoBirthday.Value.ToString("yyyy-MM-dd"),
                    OverThreeChildrenFemele = birthcontrollist[0].OverThreeChildrenFemele,
                    OverThreeChildrenMele = birthcontrollist[0].OverThreeChildrenMele,
                    MeleLigation = birthcontrollist[0].MeleLigation,
                    FemeleLigation = birthcontrollist[0].FemeleLigation,
                    PutAnnulus = birthcontrollist[0].PutAnnulus,
                    Others = birthcontrollist[0].Others
                };
            }
            else
            {
                BirthcontrolinfoList = new
                {
                    id = 0,
                    userId = objbaseinfo.UserId,
                    FemaleName = "",
                    FemaleBirthday = "",
                    FemaleWorkingPlace = "",
                    MaleName = "",
                    MaleWorkingPlace = "",
                    FirstEmbryoSex = "",
                    FirstEmbryoBirthday = "",
                    SecondEmbryoSex  = "",
                    Remark = "",
                    SecondEmbryoBirthday = "",
                    OverThreeChildrenFemele = "",
                    OverThreeChildrenMele = "",
                    MeleLigation = "",
                    FemeleLigation = "",
                    PutAnnulus  = "",
                    Others  = ""
                };
            }
            return Json(new { birthcontrolbaseinfo = objbaseList, BirthControlRoster = BirthcontrolinfoList });

        }


        public ActionResult Save(BirthControlRoster dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            BirthControlRoster obj = new BirthControlRoster();
            BirthControlLog objLog = new BirthControlLog();
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            #endregion


            if (dataObj.id == 0)//新增
            {
                dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                dataObj.CreatedTime = DateTime.Now;
                result.IsSuccess = _birthcontrolrosterService.Insert(dataObj);
                objLog.UserId = this.WorkContext.CurrentUser.UserId;
                objLog.UserName = this.WorkContext.CurrentUser.RealName;
                objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "新增";
                objLog.Description = "新增" + CommonFunction.getUserRealNamesByIDs(dataObj.UserId.ToString()) + "的计划生育花名册信息成功";
                _birthcontrollogService.Insert(objLog);
            }
            else
            {//编辑
                dataObj.LastModifier = this.WorkContext.CurrentUser.UserName;
                dataObj.LastModTime = DateTime.Now;
                result.IsSuccess = _birthcontrolrosterService.Update(dataObj);
                objLog.UserId = this.WorkContext.CurrentUser.UserId;
                objLog.UserName = this.WorkContext.CurrentUser.RealName;
                objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "编辑";
                objLog.Description = "编辑" + CommonFunction.getUserRealNamesByIDs(dataObj.UserId.ToString()) + "的计划生育花名册信息成功";
                _birthcontrollogService.Insert(objLog);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        public ActionResult Export(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<BirthControlRosterBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                if (QueryBuilder.DpId != null)
                    QueryBuilder.DpId = Get_Subdept_ByDept(QueryBuilder.DpId);
                if (QueryBuilder.DpId == null || QueryBuilder.DpId.Count == 0)
                    QueryBuilder.DpId = null;
                var modelList = _vwbirthcontrolrosterdataservice.GetForPagingByCondition(this.WorkContext.CurrentUser, out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.BirthControlRoster + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.BirthControlRoster);
                //设置集合变量
                designer.SetDataSource(ImportFileType.BirthControlRoster, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
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