
using CZManageSystem.Admin.Base;
using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Admin.Models;
using CZManageSystem.Service.HumanResources.ShiftManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Service.SysManger;
using System.Reflection;

/// <summary>
/// 排班管理
/// </summary>
namespace CZManageSystem.Admin.Controllers.HumanResources.ShiftManages
{
    public class ShiftController : BaseController
    {
        IShiftZhibanService _zhibanService = new ShiftZhibanService();//排班信息表
        IShiftBanciService _banciService = new ShiftBanciService();//班次信息
        IShiftLunbanService _lunbanService = new ShiftLunbanService();//轮班信息
        IShiftLbuserService _lbuserService = new ShiftLbuserService();//轮班用户
        IShiftRichService _richService = new ShiftRichService();//班次值班安排

        ISysUserService _userService = new SysUserService();//用户
        IUsersGrounpService _groupService = new UsersGrounpService();//群组
        ISysDeptmentService _deptService = new SysDeptmentService();


        /// <summary>
        /// 排班查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PaiBanSearch()
        {
            return View();
        }

        /// <summary>
        /// 排班管理列表信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ZhiBanList()
        {
            ViewData["curDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        /// <summary>
        /// 排班管理编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ZhiBanEdit(Guid? Id, string type = "look")
        {
            List<string> result = new List<string>();
            ViewData["type"] = type;
            ViewData["Id"] = Id.HasValue ? Id.ToString() : null;
            ViewData["deptId"] = this.WorkContext.CurrentUser.DpId;
            ViewData["deptName"] = this.WorkContext.CurrentUser.Dept.DpName;
            //List<string> depts = GetDeptIdByRole(this.WorkContext.CurrentUser.UserId);
            //depts.Add(this.WorkContext.CurrentUser.DpId);
            //depts = depts.Distinct().ToList();
            //ViewData["deptForDeal"] = string.Join(",", depts);
            return View();
        }
        public ActionResult BanCiDetail(Guid? ZhiBanId)
        {
            ViewData["ZhiBanId"] = ZhiBanId;
            return View();
        }

        //获取用户在排班管理功能中所能安排的部门id
        private List<string> GetDeptIdByRole(Guid userId)
        {
            List<string> result = new List<string>();
            if (userId != null && userId != Guid.Empty)
            {
                var curUser = _userService.FindById(userId);
                if (curUser != null && curUser.UserId != Guid.Empty)
                {
                    var roles = curUser.UsersInRoles.Select(u => u.Roles).Where(u => u.RoleName.Contains("人力资源管理员")).ToList();
                    if (roles.Count > 0)
                    {
                        if (roles.Where(u => u.RoleName == "人力资源管理员").Count() > 0)
                            result.AddRange(_deptService.List().Where(u => u.DpName == u.DpFullName).Select(u => u.DpId).ToList());
                        else if (roles.Where(u => u.RoleName == "部门人力资源管理员").Count() > 0)
                        {

                        }
                        else if (roles.Where(u => u.RoleName == "科室人力资源管理员").Count() > 0)
                        {

                        }
                        else if (roles.Where(u => u.RoleName == "服务厅人力资源管理员").Count() > 0)
                        {

                        }

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 排班方式一
        /// </summary>
        /// <param name="ZhiBanId"></param>
        /// <returns></returns>
        public ActionResult SetRichMode1(Guid? ZhiBanId)
        {
            ViewData["ZhiBanId"] = ZhiBanId;
            ViewData["curDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        /// <summary>
        /// 排班方式二
        /// </summary>
        /// <param name="ZhiBanId"></param>
        /// <returns></returns>
        public ActionResult SetRichMode2(Guid? ZhiBanId)
        {
            ViewData["ZhiBanId"] = ZhiBanId;
            ViewData["curDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <param name="isSearch">是否用于查询功能页面，默认false，</param>
        /// <returns></returns>
        public ActionResult GetListData_ZhiBan(int pageIndex = 1, int pageSize = 5, ZhibanQueryBuilder queryBuilder = null, bool isSearch = false)
        {
            if (queryBuilder == null) queryBuilder = new ZhibanQueryBuilder();
            if (!isSearch)
            {//只查询当前用户创建修改的数据
                queryBuilder.Editor = this.WorkContext.CurrentUser.UserId;
            }
            //当有部门的条件时，应该先查询出该条件下所包含的所有部门(自身和子级部门)
            if (queryBuilder.DeptId != null)
                queryBuilder.DeptId = GetDpIdFromIds(queryBuilder.DeptId);
            if (queryBuilder.DeptId == null || queryBuilder.DeptId.Count == 0)
                queryBuilder.DeptId = null;

            int count = 0;
            var modelList = _zhibanService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (ShiftZhiban)u)
                .Select(u => new
                {
                    u.Id,
                    u.Title,
                    u.Editor,
                    Editor_RealName = u.EditorObj.RealName,
                    u.EditTime,
                    u.DeptId,
                    DeptId_DpFullName = u.DeptObj.DpFullName,
                    DeptId_DpName = u.DeptObj.DpName,
                    u.Year,
                    u.Month,
                    u.State,
                    u.Remark
                }).ToList();

            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 根据排班ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_ZhiBan(Guid id)
        {
            var obj = _zhibanService.FindById(id);
            return Json(new
            {
                zhibanInfo = new
                {
                    obj.Id,
                    obj.DeptId,
                    DeptId_Text = obj.DeptObj.DpName,
                    obj.Year,
                    obj.Month,
                    obj.Remark,
                    obj.State,
                    obj.Title,
                    Bc_Num = obj.ShiftBancis.Count,
                    Lb_Num = obj.ShiftLunbans.Count
                },
                banciList = obj.ShiftBancis.OrderBy(u => u.OrderNo ?? 0).Select(u => new
                {
                    u.Id,
                    u.ZhibanId,
                    u.BcName,
                    u.StartHour,
                    u.StartMinute,
                    u.EndHour,
                    u.EndMinute,
                    u.OrderNo,
                    u.Remark
                }),
                lunbanList = obj.ShiftLunbans.OrderBy(u => u.StartDay).Select(u => new
                {
                    u.Id,
                    u.ZhibanId,
                    u.StartDay,
                    u.EndDay,
                    ShiftLbusers = u.ShiftLbusers.Select(a => new
                    {
                        a.Id,
                        a.LunbanId,
                        a.UserId,
                        UserRealName = a.UserObj?.RealName
                    })
                })
            });
        }

        /// <summary>
        /// 查询数据用于排班方式一
        /// </summary>
        /// <param name="ZhiBanId"></param>
        /// <returns></returns>
        public ActionResult GetDataForRichMode1(Guid ZhiBanId)
        {
            var obj = _zhibanService.FindById(ZhiBanId);

            return Json(new
            {
                zhibanInfo = new
                {
                    obj.Id,
                    obj.Year,
                    obj.Month,
                    obj.State,
                    obj.Title,
                },
                banciList = obj.ShiftBancis.OrderBy(u => u.OrderNo ?? 0).Select(u => new
                {
                    u.Id,
                    u.ZhibanId,
                    u.BcName,
                    u.StartHour,
                    u.StartMinute,
                    u.EndHour,
                    u.EndMinute,
                    u.OrderNo,
                    u.Remark,
                    RichData = u.ShiftRichs.ToList().FirstOrDefault() ?? new ShiftRich() { BanciId = u.Id }
                }),
                userDatas = GetDayUserInfoForRichMode1(obj)
            }, JsonRequestBehavior.AllowGet);
        }
        private List<DayUserInfoForRichMode1> GetDayUserInfoForRichMode1(ShiftZhiban zhibanObj)
        {
            List<DayUserInfoForRichMode1> list = new List<DayUserInfoForRichMode1>();
            DateTime sDate = DateTime.Parse(string.Format("{0}-{1}-01", zhibanObj.Year, zhibanObj.Month)).Date;
            DateTime eDate = sDate.AddMonths(1).AddDays(-1).Date;
            for (DateTime curDate = sDate; curDate <= eDate; curDate = curDate.AddDays(1))
            {
                DayUserInfoForRichMode1 temp = new DayUserInfoForRichMode1();
                temp.day = curDate.Day;
                temp.week = (int)curDate.DayOfWeek;
                list.Add(temp);
            }

            foreach (var item in zhibanObj.ShiftLunbans)
            {
                int startDay = int.Parse(item.StartDay);
                int endDay = int.Parse(item.EndDay);
                for (int i = startDay - 1; i < endDay && i < list.Count; i++)
                    list[i].userList.AddRange(item.ShiftLbusers.Select(u => u.UserObj.RealName).ToList());
            }

            foreach (var item in list)
                item.userList = item.userList.Distinct().ToList();

            return list;
        }
        public class DayUserInfoForRichMode1
        {
            public int day = 0;
            public int week = 0;
            public List<string> userList = new List<string>();
        }

        /// <summary>
        /// 查询数据用于排班方式二
        /// </summary>
        /// <param name="ZhiBanId"></param>
        /// <returns></returns>
        public ActionResult GetDataForRichMode2(Guid ZhiBanId)

        {
            var obj = _zhibanService.FindById(ZhiBanId);

            return Json(new
            {
                zhibanInfo = new
                {
                    obj.Id,
                    obj.Year,
                    obj.Month,
                    obj.State,
                    obj.Title,
                },
                banciList = obj.ShiftBancis.OrderBy(u => u.OrderNo ?? 0).Select(u => new
                {
                    u.Id,
                    u.ZhibanId,
                    u.BcName,
                    u.StartHour,
                    u.StartMinute,
                    u.EndHour,
                    u.EndMinute,
                    u.OrderNo,
                    u.Remark,
                    RichData = u.ShiftRichs.ToList().FirstOrDefault() ?? new ShiftRich() { BanciId = u.Id }
                }),
                userDatas = GetAllUserForRichMode2(obj),
                dayAndWeek = GetDayAndWeek(int.Parse(obj.Year), int.Parse(obj.Month))
            }, JsonRequestBehavior.AllowGet);
        }
        private List<string> GetAllUserForRichMode2(ShiftZhiban zhibanObj)
        {
            List<string> result = new List<string>();
            foreach (var item in zhibanObj.ShiftLunbans)
            {
                foreach (var item2 in item.ShiftLbusers)
                {
                    result.Add(item2.UserObj.RealName);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取某个月的日期和星期，日期为1-31，星期为0-6
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private List<object> GetDayAndWeek(int year, int month)
        {
            List<object> result = new List<object>();
            DateTime sDate = new DateTime(year, month, 1).Date;
            DateTime eDate = sDate.AddMonths(1).AddDays(-1).Date;
            for (DateTime curDate = sDate; curDate <= eDate; curDate = curDate.AddDays(1))
            {
                result.Add(new
                {
                    day = curDate.Day,
                    week = (int)curDate.DayOfWeek
                });
            }

            return result;
        }

        /// <summary>
        /// 排班方式一保存数据
        /// </summary>
        /// <param name="richs"></param>
        /// <returns></returns>
        public ActionResult Save_RichMode1(List<ShiftRich> richs)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            #region 验证数据是否合法，可以保存，并补全信息
            string tip = "";
            bool isValid = true;//是否验证通过
            foreach (var item in richs)
            {
                if (item.BanciId == Guid.Empty)
                {
                    isValid = false;
                    tip = "数据相应的班次Id不合法";
                    break;
                }
                var temp = _banciService.FindById(item.BanciId);
                if (temp == null || temp.Id == Guid.Empty)
                {
                    isValid = false;
                    tip = "找不到对应的班次信息";
                    break;
                }

                item.Id = Guid.NewGuid();
                item.Editor = this.WorkContext.CurrentUser.UserId;
                item.EditTime = DateTime.Now;
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            result.IsSuccess = _richService.InsertByList(richs);

            if (result.IsSuccess)
            {
                List<Guid> banciIds = richs.Select(u => u.BanciId).ToList();
                var oldDatas = _richService.List().Where(u => banciIds.Contains(u.BanciId)).ToList();
                oldDatas = oldDatas.Where(u => !richs.Select(a => a.Id).ToList().Contains(u.Id)).ToList();
                _richService.DeleteByList(oldDatas);
            }
            else
            {
                result.Message = "保存失败";
            }

            return Json(result);
        }

        /// <summary>
        /// 排班方式二保存数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Save_RichMode2(string userName, List<richDataForRichMode2> richDatas)
        {
            SystemResult result = new SystemResult() { IsSuccess = true };

            PropertyInfo curP;
            object objValue;
            string strValue;
            List<string> listValue = new List<string>();
            foreach (var item in richDatas)
            {
                ShiftRich curRich = _richService.FindByFeldName(u => u.BanciId == item.banciId);
                if (curRich == null || curRich.Id == Guid.Empty)
                    curRich = new ShiftRich() { BanciId = item.banciId };

                #region 更新数据
                for (int i = 1; i <= 31; i++)
                {
                    string day = i.ToString().PadLeft(2, '0');
                    curP = curRich.GetType().GetProperty("Day" + day);
                    objValue = curP.GetValue(curRich);
                    if (objValue == null) strValue = "";
                    else strValue = objValue.ToString();
                    listValue = new List<string>();
                    if (!string.IsNullOrEmpty(strValue))
                        listValue = strValue.Split(',').ToList();

                    if (item.days != null && item.days.Contains(day))
                    {//需要添加
                        if (!listValue.Contains(userName))
                            listValue.Add(userName);
                    }
                    else
                    {//需要删除
                        if (listValue.Contains(userName))
                            listValue.Remove(userName);
                    }
                    curP.SetValue(curRich, string.Join(",", listValue));
                }
                #endregion

                curRich.Editor = this.WorkContext.CurrentUser.UserId;
                curRich.EditTime = DateTime.Now;
                if (curRich.Id == Guid.Empty)
                {
                    curRich.Id = Guid.NewGuid();
                    result.IsSuccess = _richService.Insert(curRich);
                }
                else
                {
                    result.IsSuccess = _richService.Update(curRich);
                }
                if (!result.IsSuccess)
                    break;

            }

            if (!result.IsSuccess)
                result.Message = "保存失败";
            return Json(result);
        }
        public class richDataForRichMode2
        {
            public Guid banciId { get; set; }
            public List<string> days { get; set; }
        }

        /// <summary>
        /// 查询当前用户的班次信息，作为模板提供给页面引用
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBanciListAsModel()
        {
            List<object> result = new List<object>();
            var condition = new
            {
                Editor = this.WorkContext.CurrentUser.UserId
            };
            int count = 0;
            List<ShiftZhiban> zhibanList = _zhibanService.GetForPaging(out count, condition).Select(u => (ShiftZhiban)u).ToList();

            foreach (var item in zhibanList)
            {
                result.Add(new
                {
                    zhibanTitle = item.Title,
                    banciList = item.ShiftBancis.OrderBy(u => u.OrderNo ?? 0).Select(u => new
                    {
                        u.BcName,
                        u.StartHour,
                        u.StartMinute,
                        u.EndHour,
                        u.EndMinute
                    })
                });

            }

            return Json(result);
        }

        /// <summary>
        /// 保存排班数据
        /// </summary>
        /// <param name="zhibanInfo">排班信息</param>
        /// <param name="banciList">班次信息列表</param>
        /// <param name="lunbanList">轮班设置列表</param>
        /// <returns></returns>
        public ActionResult Save_ZhiBan(ShiftZhiban zhibanInfo, List<ShiftBanci> banciList, List<ShiftLunban> lunbanList)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法，可以保存
            string tip = "";
            bool isValid = false;//是否验证通过

            if (zhibanInfo.Year == null || string.IsNullOrEmpty(zhibanInfo.Year.Trim()))
                tip = "年度不能为空";
            else if (zhibanInfo.Month == null || string.IsNullOrEmpty(zhibanInfo.Month.Trim()))
                tip = "月度不能为空";
            else if (zhibanInfo.Title == null || string.IsNullOrEmpty(zhibanInfo.Title.Trim()))
                tip = "班次名称不能为空";
            else if (zhibanInfo.DeptId == null || string.IsNullOrEmpty(zhibanInfo.DeptId.Trim()))
                tip = "部门不能为空";
            else
            {
                isValid = true;
                zhibanInfo.Title = zhibanInfo.Title.Trim();
            }

            if (isRepeatCheck(zhibanInfo)) {
                isValid = false;
                tip = "该部门的当前时间已经存在排班信息";
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            #region 保存排班信息zhibanInfo
            zhibanInfo.Editor = this.WorkContext.CurrentUser.UserId;
            zhibanInfo.EditTime = DateTime.Now;
            if (zhibanInfo.Id == Guid.Empty)
            {//新增
                zhibanInfo.Id = Guid.NewGuid();
                zhibanInfo.State = "0";
                result.IsSuccess = _zhibanService.Insert(zhibanInfo);
            }
            else
            {//修改
                result.IsSuccess = _zhibanService.Update(zhibanInfo);
            }
            #endregion

            #region 保存更新班次信息banciList
            if (banciList == null)
                banciList = new List<ShiftBanci>();

            List<Guid> ids = banciList.Select(a => a.Id).ToList();
            List<ShiftBanci> oldBanciList = _banciService.List().Where(u => u.ZhibanId == zhibanInfo.Id && !ids.Contains(u.Id)).ToList();//查询出需要删除的数据

            foreach (var item in oldBanciList)
            {//删除需要删除的数据
                _richService.DeleteByList(item.ShiftRichs.ToList());
                _banciService.Delete(item);
            }
            //更新或新增数据
            for (int i = 0; i < banciList.Count; i++)
            {
                banciList[i].Editor = this.WorkContext.CurrentUser.UserId;
                banciList[i].EditTime = DateTime.Now;
                banciList[i].ZhibanId = zhibanInfo.Id;
                banciList[i].OrderNo = i + 1;
                if (banciList[i].Id == Guid.Empty)
                {//新增
                    banciList[i].Id = Guid.NewGuid();
                    _banciService.Insert(banciList[i]);

                    ShiftRich rich = new ShiftRich();
                    rich.Id = Guid.NewGuid();
                    rich.Editor = this.WorkContext.CurrentUser.UserId;
                    rich.EditTime = DateTime.Now;
                    rich.BanciId = banciList[i].Id;
                    _richService.Insert(rich);
                }
                else
                {//修改
                    _banciService.Update(banciList[i]);
                }
            }
            #endregion

            #region 保存轮班信息
            //删除原有轮班信息数据
            List<ShiftLunban> listtemp1 = _lunbanService.List().Where(u => u.ZhibanId == zhibanInfo.Id).ToList();
            List<ShiftLbuser> listtemp2 = new List<ShiftLbuser>();
            foreach (var item in listtemp1)
            {
                listtemp2.AddRange(item.ShiftLbusers);
            }
            _lbuserService.DeleteByList(listtemp2);
            _lunbanService.DeleteByList(listtemp1);

            if (lunbanList == null)
                lunbanList = new List<ShiftLunban>();
            foreach (var item in lunbanList)
            {
                item.Id = Guid.NewGuid();
                item.ZhibanId = zhibanInfo.Id;
                foreach (var item2 in item.ShiftLbusers)
                {
                    item2.Id = Guid.NewGuid();
                }
            }
            if (lunbanList.Count > 0)
                _lunbanService.InsertByList(lunbanList);

            #endregion

            if (result.IsSuccess)
                result.Message = zhibanInfo.Id.ToString();
            else
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        /// <summary>
        /// 查询是否已经存在
        /// </summary>
        /// <returns></returns>
        public bool isRepeatCheck(ShiftZhiban obj) {
            var count=_zhibanService.List().Where(u => u.Id != obj.Id && u.DeptId == obj.DeptId && u.Year == obj.Year && u.Month == obj.Month).Count();
            if (count > 0)
                return true;
            else
                return false;
        }

        public ActionResult Submit_ZhiBan(Guid ZhiBanId)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            ShiftZhiban zhibanInfo = _zhibanService.FindById(ZhiBanId);
            if (zhibanInfo == null || zhibanInfo.Id == Guid.Empty)
            {
                result.IsSuccess = false;
                result.Message = "该数据不存在";
            }
            else
            {
                if (zhibanInfo.State == "1")
                {
                    result.IsSuccess = false;
                    result.Message = "该数据已经提交";
                }
                else
                {
                    zhibanInfo.State = "1";
                    result.IsSuccess = _zhibanService.Update(zhibanInfo);
                    if (!result.IsSuccess)
                    {
                        result.Message = "提交失败";
                    }
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 删除排班信息
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        public ActionResult Delete_ZhiBan(Guid[] ids)
        {
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            List<ShiftZhiban> listZhibanDatas = new List<ShiftZhiban>();//排班信息
            List<ShiftBanci> listBanciDatas = new List<ShiftBanci>();//班次信息
            List<ShiftLunban> listLunbanDatas = new List<ShiftLunban>();//轮班信息
            List<ShiftLbuser> listLbuserDatas = new List<ShiftLbuser>();//轮班用户
            List<ShiftRich> listRichDatas = new List<ShiftRich>();//班次值班安排

            listZhibanDatas = _zhibanService.List().Where(u => ids.Contains(u.Id)).ToList();//排班信息
            allCount = listZhibanDatas.Count;
            DateTime curMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (listZhibanDatas.Where(u => u.State == "1" && curMonth > new DateTime(int.Parse(u.Year), int.Parse(u.Month), 1)).Count() > 0)
            {
                strMsg = "其中存在当前月及之前的已提交的记录，不能删除";
            }
            else if (listZhibanDatas.Count > 0)
            {
                foreach (var item_zhiban in listZhibanDatas)
                {
                    listBanciDatas.AddRange(item_zhiban.ShiftBancis.ToList());
                    listLunbanDatas.AddRange(item_zhiban.ShiftLunbans.ToList());
                    foreach (var item_banci in item_zhiban.ShiftBancis.ToList())
                    {
                        listRichDatas.AddRange(item_banci.ShiftRichs.ToList());
                    }
                    foreach (var item_lunban in item_zhiban.ShiftLunbans.ToList())
                    {
                        listLbuserDatas.AddRange(item_lunban.ShiftLbusers.ToList());
                    }
                }

                if (listRichDatas != null && listRichDatas.Count > 0)
                    _richService.DeleteByList(listRichDatas);
                if (listLbuserDatas != null && listLbuserDatas.Count > 0)
                    _lbuserService.DeleteByList(listLbuserDatas);
                if (listLunbanDatas != null && listLunbanDatas.Count > 0)
                    _lunbanService.DeleteByList(listLunbanDatas);
                if (listBanciDatas != null && listBanciDatas.Count > 0)
                    _banciService.DeleteByList(listBanciDatas);

                if (listZhibanDatas != null && listZhibanDatas.Count > 0)
                {
                    successCount = listZhibanDatas.Count;
                    _zhibanService.DeleteByList(listZhibanDatas);
                }

            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });

        }

        /// <summary>
        /// 获取年月信息
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetDropList()
        {
            var YearList = GetDictListByDDName("年");
            var MonthList = GetDictListByDDName("月份");
            return Json(new
            {
                YearList = YearList,
                MonthList = MonthList
            });
        }

        public ActionResult GetZhiBanDetail(Guid ZhiBanId)
        {
            var zhiban = _zhibanService.FindById(ZhiBanId);
            var BanciDetail = _banciService.List().Where(b => b.ZhibanId == ZhiBanId).OrderBy(a => a.OrderNo).ToList();
            var mm = BanciDetail.Select(s =>
         new
         {
             s.OrderNo,
             s.BcName,
             richList = s.ShiftRichs.ToList().FirstOrDefault() ?? new ShiftRich()
         });

            return Json(new { Year = zhiban.Year, Month = zhiban.Month, Title = zhiban.Title, items = mm });

        }


        #region 其他的方法
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
                temp = _sysDeptmentService.List().Where(u => temp.Contains(u.ParentDpId)).Select(u => u.DpId).ToList();
            }
            listResult = listResult.Distinct().ToList();
            return listResult;
        }
        #endregion

    }
}