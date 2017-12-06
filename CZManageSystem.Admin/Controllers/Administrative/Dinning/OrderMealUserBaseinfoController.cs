using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative.Dinning;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative.Dinning
{
    public class OrderMealUserBaseinfoController : BaseController
    {
        // GET: OrderMeal_UserBaseinfo
        IOrderMeal_UserBaseinfoService _userbaseinfoservice = new OrderMeal_UserBaseinfoService();
        ISysUserService _userService = new SysUserService();
        IOrderMeal_DinningRoomService _diningroomservice = new OrderMeal_DinningRoomService();
        IOrderMeal_UserDinningRoomService _userdinningroomservice = new OrderMeal_UserDinningRoomService();
        [SysOperation(OperationType.Browse, "访问食堂用户信息页面")]
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, OrderMeal_UserBaseinfoQueryBuilder queryBuilder = null,string DpId =  null)
        {
            int count = 0;
            //if (queryBuilder.DpId != null)
            //    queryBuilder.DpId = Get_Subdept_ByDept(queryBuilder.DpId);
            //if (queryBuilder.DpId == null || queryBuilder.DpId.Count == 0 || (queryBuilder.DpId.Count == 1 && string.IsNullOrEmpty(queryBuilder.DpId[0].ToString())))
            //    queryBuilder.DpId = null;
            if (DpId != "")
                queryBuilder.DpId = Get_Subdept_ByDept(DpId);
            else
                queryBuilder.DpId = null;
            var modelList = _userbaseinfoservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _tmpstate = "";
                if (item.State == 1)
                    _tmpstate = "启用";
                else if(item.State== 0 )
                    _tmpstate = "停用";
                else
                    _tmpstate = "未开启";
                var _userobj = _userService.FindByFeldName(u => u.UserName == item.LoginName);
                List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                listResult.Add(new
                {
                    item.RealName,
                    item.LoginName,
                    item.Telephone,
                    State = _tmpstate,
                    DpName = _tmpdplist[0]//DpName = CommonFunction.getDeptNamesByIDs(item.DeptId),

                });
            }
            return Json(new { items = listResult, count = count });
        }

        public ActionResult Edit(string id ,string type= "edit")
        {
            if (id == null)
                id = "";
            ViewData["id"] = id;
            ViewData["type"] = type;
            return View();
        }
        public ActionResult GetDataByID(string id = "")
        {
            //var obj = _userbaseinfoservice.FindByFeldName(u => u.LoginName == id);
            var InfoList = new List<OrderMeal_UserDinningRoomTmp>();
            var UserBaseInfo = new List<OrderMeal_UserDinningRoom>();
            var BaseInfo = new OrderMeal_UserBaseinfoTmp();

            var roomlist = _diningroomservice.List();            
            var _tmpuserbaseInfo = _userbaseinfoservice.FindByFeldName(u => u.LoginName == id);

            //var _tmpsysuserInfo = _userService.FindByFeldName(u => u.UserName == id);
            //if (_tmpsysuserInfo != null)
            //{
            //    BaseInfo.LoginName = _tmpsysuserInfo.UserName;
            //    BaseInfo.RealName = _tmpsysuserInfo.RealName;
            //    BaseInfo.DeptName = CommonFunction.getDeptNamesByIDs(_tmpsysuserInfo.DpId);
            //    BaseInfo.Telephone = _tmpsysuserInfo.Mobile;
            //    BaseInfo.EmployId = _tmpsysuserInfo.EmployeeId;
            //    BaseInfo.DeptId = _tmpsysuserInfo.DpId;
            //} 
            
            
            if(id != "")
            {
                BaseInfo.LoginName = _userService.FindByFeldName(u => u.UserName == id).UserName;
                BaseInfo.RealName = _userService.FindByFeldName(u => u.UserName == id).RealName;
                BaseInfo.DeptName = CommonFunction.getDeptNamesByIDs(_userService.FindByFeldName(u => u.UserName == id).DpId);
            }                      
            if (_tmpuserbaseInfo != null)
            {
                BaseInfo.State = _tmpuserbaseInfo.State;
                UserBaseInfo = _userdinningroomservice.List().Where(u => u.UserBaseinfoID == _tmpuserbaseInfo.Id).ToList();
            }
            foreach (var roomitem in roomlist)
            {
                var tmpstate = 0;
                var tmpid = new object();
                Guid tmpuserbaseinfoid = Guid.Parse("00000000-0000-0000-0000-000000000000");
                var tmpgetsms = 0;
                var tmp1 = roomitem.Id;
                var tmplist = UserBaseInfo.Where(u => u.DinningRoomID == tmp1).ToList();
                if (tmplist.Count > 0)
                {
                    tmpgetsms = tmplist[0].GetSms;
                    tmpstate = 1;
                    tmpid = tmplist[0].Id;
                    tmpuserbaseinfoid= tmplist[0].UserBaseinfoID;
                }
                else
                    tmpid = Guid.NewGuid();
                InfoList.Add(new OrderMeal_UserDinningRoomTmp {
                    Id = Guid.Parse(tmpid.ToString()),
                    UserBaseinfoID = tmpuserbaseinfoid,
                    DinningRoomID = roomitem.Id,
                    DinningRoomName = roomitem.DinningRoomName,
                    DinningRoomState = tmpstate,
                    GetSms = tmpgetsms
                });
            }
            return Json(new
            {
                baseinfo = BaseInfo,
                info = InfoList,
            });
        }

        public ActionResult UpdateState(string[] ids)
        {
            List<OrderMeal_UserBaseinfo> list = new List<OrderMeal_UserBaseinfo>();
            foreach (string id in ids)
            {
                var _obj = _userbaseinfoservice.FindByFeldName(u => u.LoginName == id);
                if (_obj == null)
                {
                    SystemResult result = new SystemResult() { IsSuccess = false, Message = "选中有未开启的用户，无法修改状态！" };
                    return Json(result);
                }
                list.Add(_obj);
            }
            if (list.Count > 0)
            {
                foreach(var item in list)
                {
                    if (item.State == 1)
                        item.State = 0;
                    else
                        item.State = 1;
                    _userbaseinfoservice.Update(item);
                }
                return Json(new { IsSuccess = true, Message = "修改账户状态成功", SuccessCount = list.Count });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "没有可修改账户状态的数据" });
            }
        }

        public ActionResult DeleteSms(Guid[] ids)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                var objs = _userdinningroomservice.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    if (_userdinningroomservice.DeleteByList(objs))
                    {                        
                        result.IsSuccess = true;
                        result.Message = "删除成功";
                        result.data = objs.Count;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "删除失败";
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "没有可删除的数据";
                }
                _sysLogService.WriteSysLog<SysOperationLog>(new SysOperationLog()
                {
                    Operation = OperationType.Delete,
                    OperationDesc = "删除成功！",
                    OperationPage = Request.RawUrl,
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = "删除失败！";// ex.Message;
                //记录错误日志
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "删除失败",
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                return Json(result);
            }

        }
        [SysOperation(OperationType.Save, "保存食堂用户归属食堂信息")]
        public ActionResult Save(string Type, string LoginName, List<OrderMeal_UserDinningRoomTmp> dataList)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            SysOperationLog _log = new SysOperationLog();
            try
            {
                #region 验证数据是否合法
                string tip = "";
                bool isValid = false;//是否验证通过 
                OrderMeal_UserBaseinfo dataObj = new OrderMeal_UserBaseinfo();

                var DBSysuserInfo = _userService.FindByFeldName(u => u.UserName == LoginName);
                var DBdataObj = _userbaseinfoservice.FindByFeldName(u => u.LoginName == LoginName);
                if (DBSysuserInfo == null)
                    tip = "根据您所输入的[用户登录账号]无法查找到用户信息，请核实后再试";
                if (Type == "add")
                {
                    var _tmplist = _userbaseinfoservice.List().Where(u => u.LoginName == LoginName).ToList();
                    if (_tmplist.Count > 0)
                    {
                        tip = "用户已经加入食堂了，请勿重复添加！";
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
                if (DBdataObj == null)
                {
                    dataObj = new OrderMeal_UserBaseinfo();
                    dataObj.Id = Guid.NewGuid();
                    dataObj.RealName = DBSysuserInfo.RealName;
                    dataObj.LoginName = DBSysuserInfo.UserName;
                    dataObj.MealCardID = DBSysuserInfo.UserName;
                    dataObj.State = 1;
                    dataObj.DeptId = DBSysuserInfo.DpId;
                    dataObj.Telephone = DBSysuserInfo.Mobile;
                    dataObj.EmployId = DBSysuserInfo.EmployeeId;
                    dataObj.Balance = 0;
                }
                else
                {
                    dataObj = DBdataObj;
                    dataObj.EmployId = DBSysuserInfo.EmployeeId;
                    dataObj.DeptId = DBSysuserInfo.DpId;
                }

                #endregion
                if (DBdataObj == null)//新增
                {
                    _log.Operation = OperationType.Add;
                    result.IsSuccess = _userbaseinfoservice.Insert(dataObj);
                    var m1 = _userdinningroomservice.List().Where(m => m.UserBaseinfoID == dataObj.Id).ToList();
                    _userdinningroomservice.DeleteByList(m1);
                    foreach (var item in dataList)
                    {
                        OrderMeal_UserDinningRoom obj = new OrderMeal_UserDinningRoom();
                        var tmp1 = item.DinningRoomState;
                        var tmp2 = item.DinningRoomID;
                        obj.DinningRoomID = item.DinningRoomID;
                        obj.Id = item.Id;
                        obj.UserBaseinfoID = dataObj.Id;
                        obj.GetSms = item.GetSms.Value;
                        if (item.DinningRoomState == 1)
                            _userdinningroomservice.Insert(obj);
                    }
                }
                else
                {//编辑
                    _log.Operation = OperationType.Edit;
                    result.IsSuccess = _userbaseinfoservice.Update(dataObj);
                    var m1 = _userdinningroomservice.List().Where(m => m.UserBaseinfoID == dataObj.Id).ToList();
                    _userdinningroomservice.DeleteByList(m1);
                    foreach (var item in dataList)
                    {
                        OrderMeal_UserDinningRoom obj = new OrderMeal_UserDinningRoom();
                        var tmp1 = item.DinningRoomState;
                        var tmp2 = item.DinningRoomID;
                        obj.Id = item.Id;
                        obj.UserBaseinfoID = dataObj.Id;
                        obj.DinningRoomID = item.DinningRoomID;
                        obj.GetSms = item.GetSms.Value;
                        if (item.DinningRoomState == 1)
                            _userdinningroomservice.Insert(obj);
                    }
                }
                result.Message = tip;
                _log.OperationDesc = "保存成功！";
                _log.OperationPage = Request.RawUrl;
                _log.RealName = this.WorkContext.CurrentUser.RealName;
                _log.UserName = this.WorkContext.CurrentUser.UserName;
                _sysLogService.WriteSysLog<SysOperationLog>(_log);
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = "保存失败！";// ex.Message;
                //记录错误日志
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "保存失败",
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                return Json(result);
            }           
        }
        [SysOperation(OperationType.Browse, "访问食堂用户归属食堂信息页面")]
        public ActionResult Belong(Guid? DinningRoomID)
        {
            ViewData["DinningRoomID"] = DinningRoomID;
            return View();
        }
        public ActionResult SaveSms(Guid? DinningRoomID, Guid? UserID,int getsms)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };


            var _tmpuser = _userService.FindById(UserID.Value);
            var _tmpbaseuser = _userbaseinfoservice.FindByFeldName(u => u.LoginName == _tmpuser.UserName);
            
            if (_tmpbaseuser==null)
            {
                result.IsSuccess = false;
                result.Message = "该用户不存在或已被禁用";
            }else
            {
                var _tmpbasebelong = _userdinningroomservice.List().Where(u => u.DinningRoomID == DinningRoomID.Value && u.UserBaseinfoID == _tmpbaseuser.Id).ToList();
                if(_tmpbasebelong.Count>0)
                {
                    result.IsSuccess = false;
                    result.Message = "该用户已添加";
                }
                else
                {
                    OrderMeal_UserDinningRoom _tmp = new OrderMeal_UserDinningRoom();
                    _tmp.Id = Guid.NewGuid();
                    _tmp.UserBaseinfoID = _tmpbaseuser.Id;
                    _tmp.DinningRoomID = DinningRoomID.Value;
                    _tmp.GetSms = getsms;
                    result.IsSuccess = _userdinningroomservice.Insert(_tmp);
                }
            }            
            return Json(result);
        }
        public ActionResult UpdateSms(List<OrderMeal_UserDinningRoom> dataList)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            foreach (var item in dataList)
            {
                var _tmp = _userdinningroomservice.FindById(item.Id);
                _tmp.GetSms = item.GetSms;
                _userdinningroomservice.Update(_tmp);
            }
            result.IsSuccess = true;            
            return Json(result);
        }
        public ActionResult GetForBelongList(int pageIndex = 1, int pageSize = 5, OrderMeal_UserDinningRoomQueryBuilder queryBuilder = null, string DpId = null)
        {
            int count = 0;
            var modelList = _userbaseinfoservice.GetForBelongPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _tmpuserbaseInfo = _userbaseinfoservice.FindById(item.UserBaseinfoID);
                listResult.Add(new
                {

                    item.GetSms,
                    item.Id,
                    item.DinningRoomID,
                    item.UserBaseinfoID,
                    RealName = _tmpuserbaseInfo.RealName
                });
            }
            return Json(new { items = listResult, count = count });
        }

        [SysOperation(OperationType.Export, "导出食堂用户信息")]
        public ActionResult Export(string queryBuilder = null,string DpId=null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OrderMeal_UserBaseinfoQueryBuilder>(queryBuilder);
                //if (QueryBuilder.DpId != null)
                //    QueryBuilder.DpId = Get_Subdept_ByDept(QueryBuilder.DpId);
                //if (QueryBuilder.DpId == null || QueryBuilder.DpId.Count == 0 || (QueryBuilder.DpId.Count == 1 && string.IsNullOrEmpty(QueryBuilder.DpId[0].ToString())))
                //    QueryBuilder.DpId = null;
                //if (queryBuilder.DpId != null)
                //    queryBuilder.DpId = Get_Subdept_ByDept(queryBuilder.DpId);
                //if (queryBuilder.DpId == null || queryBuilder.DpId.Count == 0 || (queryBuilder.DpId.Count == 1 && string.IsNullOrEmpty(queryBuilder.DpId[0].ToString())))
                //    queryBuilder.DpId = null;
                if (DpId != "")
                    QueryBuilder.DpId = Get_Subdept_ByDept(DpId);
                else
                    QueryBuilder.DpId = null;
                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _userbaseinfoservice.GetForPagingByCondition(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();

                List<object> listResult = new List<object>();

                foreach (var item in modelList)
                {
                    var _tmpstate = "";
                    var _tmpid = "";

                    var _tmpUserBaseinfoID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    var _tmpDinningRoomID =new List<Guid>();
                    var _tmpdinningname = new List<string>();
                    var _userobj = _userService.FindByFeldName(u => u.EmployeeId == item.EmployId);
                    List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                    if (item.State == 1)
                        _tmpstate = "启用";
                    else if (item.State == 0)
                        _tmpstate = "停用";
                    else
                        _tmpstate = "未开启";
                    var _tmpUserBaseinfoIDmodel = _userbaseinfoservice.FindByFeldName(u => u.LoginName == item.LoginName);
                    if(_tmpUserBaseinfoIDmodel != null)
                    {
                        _tmpUserBaseinfoID = _tmpUserBaseinfoIDmodel.Id;
                        _tmpDinningRoomID = _userdinningroomservice.List().Where(u => u.UserBaseinfoID == _tmpUserBaseinfoID).Select(u => u.DinningRoomID).ToList();
                        _tmpid = string.Join(",", _tmpDinningRoomID);
                        _tmpdinningname = _diningroomservice.List().Where(u => _tmpDinningRoomID.Contains(u.Id)).Select(u => u.DinningRoomName).ToList();
                    }
                    
                    listResult.Add(new
                    {
                        item.RealName,
                        item.LoginName,
                        State = _tmpstate,
                        DpName = _tmpdplist[0],//DpName = CommonFunction.getDeptNamesByIDs(item.DeptId),
                        DinningRoomName = string.Join(",", _tmpdinningname)
                    });
                }
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.OrderMealUserBaseinfo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();

                
                //打开模板
                designer.Open(ExportTempPath.OrderMealUserBaseinfo);
                //设置集合变量
                designer.SetDataSource(ImportFileType.OrderMealUserBaseinfo, listResult);
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

        /// <summary>
        /// 根据部门组织Ids获取包括自身的所有子节点的id
        /// </summary>
        /// <param name="ids">组织Ids</param>
        /// <returns></returns>
        private List<string> Get_Subdept_ByDept(string ids)
        {
            List<string> listResult = new List<string>();
            if (ids == null)
                return listResult;
            string[] temp = ids.Split(',');
            for (int i = 0; i < temp.Length; i++)
            {
                var mm = new EfRepository<string>().Execute<string>(string.Format("select * from  dbo.Get_Subdept_ByDept ('{0}')", temp[i].ToString())).ToList();
                listResult.AddRange(mm);
            }
            return listResult;
        }

    }
}