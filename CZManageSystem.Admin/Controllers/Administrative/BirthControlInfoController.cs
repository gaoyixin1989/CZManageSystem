using CZManageSystem.Service.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data;
using CZManageSystem.Service.Administrative;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using CZManageSystem.Admin.Base;
using Newtonsoft.Json;
using Aspose.Cells;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class BirthControlInfoController : BaseController
    {

        // GET: BirthControlInfo
        IVW_Birthcontrol_DataService _vwbirthcontroldataservice = new VW_Birthcontrol_DataService();
        IBirthControlInfoService _birthcontrolinfoService = new BirthControlInfoService();
        IBirthControlLogService _birthcontrollogService = new BirthControlLogService();
        IBirthControlChildrenInfoService _birthcontrolchildreninfoService = new BirthControlChildrenInfoService();
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


        public ActionResult GetVWBirthcontrolListData(int pageIndex = 1, int pageSize = 5, BirthControlInfoBuilder queryBuilder = null)
        {
            int count = 0;
            //当有部门的条件时，应该先查询出该条件下所包含的所有部门(自身和子级部门)
            if (queryBuilder.DpId != null)
                queryBuilder.DpId = Get_Subdept_ByDept(queryBuilder.DpId);
            if (queryBuilder.DpId == null || queryBuilder.DpId.Count == 0)
                queryBuilder.DpId = null;
            var modelList = _vwbirthcontroldataservice.GetForPagingByCondition(this.WorkContext.CurrentUser,out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize );
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.RealName,
                    item.EmployeeId,
                    item.Email,
                    item.Mobile,
                    item.InfoStatus,
                    item.UserId                    
                });
            }

            return Json(new { items = listResult, count = count });
        }

        public ActionResult Edit(Guid? userid,string show)
        {
            //有值=》编辑状态
            if(userid==null)
            {
                ViewData["userid"] = this.WorkContext.CurrentUser.UserId;
                userid = this.WorkContext.CurrentUser.UserId;
            }
            else
                ViewData["userid"] = userid;
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
            var list = _birthcontrolinfoService.List().Where(u => u.UserId == userid).ToList();
            if(list.Count>0)
                ViewData["id"] = list[0].id;
            else
                ViewData["id"] = "0";
            if (this.WorkContext.CurrentUser.UserId == userid)
                ViewData["IsSelf"] = true;
            else
                ViewData["IsSelf"] = false;
            if (this.WorkContext.CurrentUser.UserId == userid)
                if(list.Count > 0 && list[0].ConfirmStatus == "1")
                {
                    ViewData["IsComfirm"] = true;
                    ViewData["Show"] = true;
                }                    
                else
                    ViewData["IsComfirm"] = false;
            else
                ViewData["IsComfirm"] = false;

            if(userIdList.Contains(this.WorkContext.CurrentUser.UserId))
                ViewData["IsAdmin"] = true;
            else
                ViewData["IsAdmin"] = false;
            if(show=="T")
                ViewData["Show"] = true;
            else
                ViewData["Show"] = false;
            return View();
        }
        public ActionResult deleteChildren(int Id)
        {
            bool IsSuccess = false;
            int successCount = 0;
            var objs = _birthcontrolchildreninfoService.List().Where(u => u.id == Id).ToList();
            var Upguid = _birthcontrolchildreninfoService.FindById(Id).UserId.ToString();
            //var modelList = _birthcontrolchildreninfoService.GetAllChildrenList(new Guid(Upguid));
            if (objs.Count > 0)
            {
                IsSuccess = _birthcontrolchildreninfoService.DeleteByList(objs);
                successCount = IsSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                IsSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                data = _birthcontrolchildreninfoService.GetAllChildrenList(new Guid(Upguid))
            });

        }

        public ActionResult GetDataByID(Guid id)
        {
            //var BirthcontrolinfoList = new BirthControlInfo();
            DateTime? TimeNull = null;
            object BirthcontrolinfoList = null;
            object objbaseList = null;
            var objbaseinfo = _vwbirthcontroldataservice.FindByFeldName(u => u.UserId == id);
            if(objbaseinfo!=null)
            {
                objbaseList = new
                {
                    objbaseinfo.DpName,
                    objbaseinfo.EmployeeId,
                    objbaseinfo.RealName,
                    objbaseinfo.UserId,
                    JoinTime = objbaseinfo.JoinTime.HasValue ? objbaseinfo.JoinTime.Value.ToString("yyyy-MM-dd") : "",
                    objbaseinfo.IsFormal
                };
            }else
            {
                objbaseList = new
                {
                    DpName = "",
                    EmployeeId = "",
                    RealName = "",
                    UserId = id,
                    JoinTime =  "",
                    IsFormal = ""
                };
            }         
            var birthcontrollist = _birthcontrolinfoService.List().Where(u => u.UserId == id).ToList();
            if (birthcontrollist.Count > 0)
            {
                //BirthcontrolinfoList = birthcontrollist[0];
                BirthcontrolinfoList = new
                {
                    id=birthcontrollist[0].id,
                    userId=birthcontrollist[0].UserId ,
                    Sex=birthcontrollist[0].Sex ,
                    Birthday=birthcontrollist[0].Birthday.HasValue ? birthcontrollist[0].Birthday.Value.ToString("yyyy-MM-dd") : "",
                    Nation=birthcontrollist[0].Nation,
                    IdCardNum=birthcontrollist[0].IdCardNum,
                    StreetBelong=birthcontrollist[0].StreetBelong,
                    MaritalStatus=birthcontrollist[0].MaritalStatus,
                    PhoneNum=birthcontrollist[0].PhoneNum,
                    Lastupdatedate= birthcontrollist[0].Lastupdatedate.HasValue ? birthcontrollist[0].Lastupdatedate.Value.ToString("yyyy-MM-dd") : "",
                    Havebear=birthcontrollist[0].Havebear,
                    FirstMarryDate= birthcontrollist[0].FirstMarryDate.HasValue ? birthcontrollist[0].FirstMarryDate.Value.ToString("yyyy-MM-dd") : "",
                    DivorceDate= birthcontrollist[0].DivorceDate.HasValue ? birthcontrollist[0].DivorceDate.Value.ToString("yyyy-MM-dd") : "",
                    RemarryDate= birthcontrollist[0].RemarryDate.HasValue ? birthcontrollist[0].RemarryDate.Value.ToString("yyyy-MM-dd") : "",
                    WidowedDate= birthcontrollist[0].WidowedDate.HasValue ? birthcontrollist[0].WidowedDate.Value.ToString("yyyy-MM-dd") : "", 
                    LigationDate= birthcontrollist[0].LigationDate.HasValue ? birthcontrollist[0].LigationDate.Value.ToString("yyyy-MM-dd") : "",
                    BRemark=birthcontrollist[0].BRemark,
                    SpouseName=birthcontrollist[0].SpouseName ,
                    Spousesex=birthcontrollist[0].Spousesex,
                    SpouseBirthday= birthcontrollist[0].SpouseBirthday.HasValue ? birthcontrollist[0].SpouseBirthday.Value.ToString("yyyy-MM-dd") : "",
                    SpouseIdCardNum=birthcontrollist[0].SpouseIdCardNum,
                    SpouseAccountbelong=birthcontrollist[0].SpouseAccountbelong,
                    SpousePhone=birthcontrollist[0].SpousePhone,
                    SpouseMaritalStatus=birthcontrollist[0].SpouseMaritalStatus ,
                    fixedjob=birthcontrollist[0].FixedJob,
                    SpouseWorkingAddress=birthcontrollist[0].SpouseWorkingAddress,
                    SpouseLigationDate= birthcontrollist[0].SpouseLigationDate.HasValue ? birthcontrollist[0].SpouseLigationDate.Value.ToString("yyyy-MM-dd") : "",
                    organizeGE =birthcontrollist[0].OrganizeGE,
                    sameworkplace=birthcontrollist[0].SameWorkPlace ,
                    Latemarriage=birthcontrollist[0].Latemarriage ,
                    foremarriagebore=birthcontrollist[0].ForeMarriageBore
                };
            }
            else
            {
                BirthcontrolinfoList = new
                {
                    id = 0,
                    userId = id,
                    Sex = "",
                    Birthday = TimeNull,
                    Nation = "",
                    IdCardNum = "",
                    StreetBelong = "",
                    MaritalStatus = "",
                    PhoneNum = "",
                    Lastupdatedate = TimeNull,
                    Havebear = "",
                    FirstMarryDate = TimeNull,
                    DivorceDate = TimeNull,
                    RemarryDate = TimeNull,
                    WidowedDate = TimeNull,
                    LigationDate = TimeNull,
                    BRemark = "",
                    SpouseName = "",
                    Spousesex = "",
                    SpouseBirthday = TimeNull,
                    SpouseIdCardNum = "",
                    SpouseAccountbelong = "",
                    SpousePhone = "",
                    SpouseMaritalStatus = "",
                    fixedjob = "",
                    SpouseWorkingAddress = "",
                    SpouseLigationDate = TimeNull,
                    organizeGE = "",
                    sameworkplace = "",
                    Latemarriage = "",
                    foremarriagebore = ""
                };
            }
            var modelList = _birthcontrolchildreninfoService.GetAllChildrenList(id);
            return Json(new { birthcontrolbaseinfo= objbaseList, birthcontrolinfo = BirthcontrolinfoList, childreninfo = modelList });
            
        }
        public ActionResult Update(Guid[] Ids)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            BirthControlLog objLog = new BirthControlLog();
            List<BirthControlInfo> list = new List<BirthControlInfo>();
            var objs = _birthcontrolinfoService.List().Where(u => Ids.Contains(u.UserId.Value)).ToList();

            foreach (Guid id in Ids)
            {
                var _obj = _birthcontrolinfoService.FindByFeldName(u => u.UserId == id);
                if (_obj == null)
                {
                    result.IsSuccess = false;
                    result.Message = "所选的数据中存在未编辑状态，不能进行操作！";
                    return Json(result);
                }
                _obj.ConfirmStatus = "0";
                list.Add(_obj);
            }
            result.IsSuccess = _birthcontrolinfoService.UpdateByList(list);          
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                objLog.UserId = this.WorkContext.CurrentUser.UserId;
                objLog.UserName = this.WorkContext.CurrentUser.RealName;
                objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "编辑";
                objLog.Description = "修改计划生育人员信息采集卡状态成功";
                _birthcontrollogService.Insert(objLog);
            }
            return Json(result);
        }

        public ActionResult Save(BirthControlInfo dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            BirthControlInfo obj = new BirthControlInfo();
            BirthControlLog objLog = new BirthControlLog();
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            if (dataObj.Sex == null)
            {
                tip = "性别不能为空";
            }
            else if (dataObj.Birthday == null)
            {
                tip = "出生日期不能为空";
            }
            else if (dataObj.Nation == null)
            {
                tip = "民族不能为空";
            }
            else if (dataObj.IdCardNum == null)
            {
                tip = "身份证号码不能为空";
            }
            else if (dataObj.StreetBelong == null)
            {
                tip = "户口所属街道不能为空";
            }
            else if (dataObj.MaritalStatus == null)
            {
                tip = "婚姻状况不能为空";
            }
            else if (dataObj.PhoneNum == null)
            {
                tip = "联系电话不能为空";
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


            if (dataObj.id == 0)//新增
            {
                dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                dataObj.CreatedTime = DateTime.Now;
                result.IsSuccess = _birthcontrolinfoService.Insert(dataObj);
                objLog.UserId = this.WorkContext.CurrentUser.UserId;
                objLog.UserName = this.WorkContext.CurrentUser.RealName;
                objLog.UserIp= Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "新增";
                objLog.Description = "新增" + CommonFunction.getUserRealNamesByIDs(dataObj.UserId.ToString()) + "的计划生育人员信息采集卡成功";
                _birthcontrollogService.Insert(objLog);
            }
            else
            {//编辑
                dataObj.LastModifier = this.WorkContext.CurrentUser.UserName;
                dataObj.LastModTime = DateTime.Now;
                result.IsSuccess = _birthcontrolinfoService.Update(dataObj);
                objLog.UserId = this.WorkContext.CurrentUser.UserId;
                objLog.UserName = this.WorkContext.CurrentUser.RealName;
                objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "编辑";
                objLog.Description = "编辑" + CommonFunction.getUserRealNamesByIDs(dataObj.UserId.ToString()) + "的计划生育人员信息采集卡成功";
                _birthcontrollogService.Insert(objLog);
            }           
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        public ActionResult Export(string id)
        {

            var t = Guid.NewGuid();
            var tempid = id;
            var userid = new Guid(id);
            try
            {

                WorkbookDesigner designer = new WorkbookDesigner();
                var objbaseinfo = _vwbirthcontroldataservice.FindByFeldName(u => u.UserId == userid);
                designer.SetDataSource("DpName", objbaseinfo.DpName);
                designer.SetDataSource("EmployeeId", objbaseinfo.EmployeeId);
                designer.SetDataSource("RealName", objbaseinfo.RealName);
                designer.SetDataSource("JoinTime", objbaseinfo.JoinTime);
                designer.SetDataSource("IsFormal", objbaseinfo.IsFormal);
                if(objbaseinfo.Sex=="1")
                    designer.SetDataSource("Sex", "男");
                else if (objbaseinfo.Sex == "2")
                    designer.SetDataSource("Sex", "女");
                designer.SetDataSource("Birthday", objbaseinfo.Birthday);
                designer.SetDataSource("Nation", objbaseinfo.Nation);
                designer.SetDataSource("IdCardNum", objbaseinfo.IdCardNum);
                designer.SetDataSource("StreetBelong", objbaseinfo.StreetBelong);
                designer.SetDataSource("MaritalStatus", objbaseinfo.MaritalStatus);
                designer.SetDataSource("PhoneNum", objbaseinfo.PhoneNum);
                designer.SetDataSource("RealName", objbaseinfo.RealName);
                designer.SetDataSource("JoinTime", objbaseinfo.JoinTime);
                designer.SetDataSource("Havebear", objbaseinfo.Havebear);
                designer.SetDataSource("FirstMarryDate", objbaseinfo.FirstMarryDate);
                designer.SetDataSource("DivorceDate", objbaseinfo.DivorceDate);
                designer.SetDataSource("RemarryDate", objbaseinfo.RemarryDate);
                designer.SetDataSource("WidowedDate", objbaseinfo.WidowedDate);
                designer.SetDataSource("LigationDate", objbaseinfo.LigationDate);

                designer.SetDataSource("BRemark", objbaseinfo.BRemark);
                designer.SetDataSource("SpouseName", objbaseinfo.SpouseName);
                designer.SetDataSource("Spousesex", objbaseinfo.Spousesex);
                designer.SetDataSource("SpouseBirthday", objbaseinfo.SpouseBirthday);
                designer.SetDataSource("SpouseIdCardNum", objbaseinfo.SpouseIdCardNum);
                designer.SetDataSource("SpouseAccountbelong", objbaseinfo.SpouseAccountbelong);
                designer.SetDataSource("SpousePhone", objbaseinfo.SpousePhone);
                designer.SetDataSource("SpouseMaritalStatus", objbaseinfo.SpouseMaritalStatus);
                designer.SetDataSource("fixedjob", objbaseinfo.fixedjob);
                designer.SetDataSource("SpouseWorkingAddress", objbaseinfo.SpouseWorkingAddress);
                designer.SetDataSource("SpouseLigationDate", objbaseinfo.SpouseLigationDate);
                designer.SetDataSource("organizeGE", objbaseinfo.organizeGE);
                designer.SetDataSource("sameworkplace", objbaseinfo.sameworkplace);
                designer.SetDataSource("Latemarriage", objbaseinfo.Latemarriage);
                designer.SetDataSource("foremarriagebore", objbaseinfo.foremarriagebore);

                var modelList = _birthcontrolchildreninfoService.GetAllChildrenList(userid);
                if (modelList.Count<1 )
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.BirthControlInfo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                
                //打开模板
                designer.Open(ExportTempPath.BirthControlInfo);
                //设置集合变量
                designer.SetDataSource(ImportFileType.BirthControlInfo, modelList);
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

        public ActionResult SaveConfirm(BirthControlInfo dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            BirthControlInfo obj = new BirthControlInfo();
            BirthControlLog objLog = new BirthControlLog();
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            if (dataObj.Sex == null)
            {
                tip = "性别不能为空";
            }
            else if (dataObj.Birthday == null)
            {
                tip = "出生日期不能为空";
            }
            else if (dataObj.Nation == null)
            {
                tip = "民族不能为空";
            }
            else if (dataObj.IdCardNum == null)
            {
                tip = "身份证号码不能为空";
            }
            else if (dataObj.StreetBelong == null)
            {
                tip = "户口所属街道不能为空";
            }
            else if (dataObj.MaritalStatus == null)
            {
                tip = "婚姻状况不能为空";
            }
            else if (dataObj.PhoneNum == null)
            {
                tip = "联系电话不能为空";
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


            if (dataObj.id == 0)//新增
            {
                dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                dataObj.CreatedTime = DateTime.Now;
                dataObj.Lastupdatedate= DateTime.Now;
                dataObj.ConfirmStatus = "1";
                result.IsSuccess = _birthcontrolinfoService.Insert(dataObj);
            }
            else
            {//编辑
                dataObj.LastModifier = this.WorkContext.CurrentUser.UserName;
                dataObj.LastModTime = DateTime.Now;
                dataObj.Lastupdatedate = DateTime.Now;
                dataObj.ConfirmStatus = "1";
                result.IsSuccess = _birthcontrolinfoService.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                objLog.UserId = this.WorkContext.CurrentUser.UserId;
                objLog.UserName = this.WorkContext.CurrentUser.RealName;
                objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "确认";
                objLog.Description = "确认" + CommonFunction.getUserRealNamesByIDs(dataObj.UserId.ToString()) + "的计划生育人员信息采集卡成功";
                _birthcontrollogService.Insert(objLog);
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
            for(int i=0;i<temp.Length;i++)
            {
                var mm = new EfRepository<string>().Execute<string>(string.Format("select * from  dbo.Get_Subdept_ByDept ('{0}')", temp[i].ToString())).ToList();
                listResult.AddRange(mm);
            }
            return listResult;
        }
        #endregion
    }
}