using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public class OrderMeal_UserBaseinfoService : BaseService<OrderMeal_UserBaseinfo>, IOrderMeal_UserBaseinfoService
    {
        private readonly IRepository<Users> _bw_Users = new EfRepository<Users>();
        private readonly IRepository<OrderMeal_UserDinningRoom> _or_Users_dr = new EfRepository<OrderMeal_UserDinningRoom>();
        static List<OrderMeal_UserBaseinfo> listDic = new List<OrderMeal_UserBaseinfo>();
        static List<Users> listDicUsers = new List<Users>();
        IOrderMeal_UserDinningRoomService _userdinningroomservice = new OrderMeal_UserDinningRoomService();
        public IList<OrderMeal_UserBaseinfoTmp> GetForPagingByCondition(out int count, OrderMeal_UserBaseinfoQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var curData = from bwu in this._bw_Users.Table
                          join ub in this._entityStore.Table                            
                          on bwu.UserName equals ub.LoginName into ubc
                          from ubci in ubc.DefaultIfEmpty()
                          where bwu.Status == 0
                          select new
                          {
                              bwu.UserType,
                              bwu.UserId,
                              bwu.UserName,
                              bwu.Mobile,
                              bwu.RealName,
                              bwu.DpId,
                              ubci.State
                          };
            curData = curData.Where(u => u.UserType == 1);
            if (objs.RealName != null && objs.RealName != "")
                curData = curData.Where(u => u.RealName.Contains(objs.RealName));
            if (objs.DpId != null && objs.DpId.Count > 0 )
                curData = curData.Where(u => objs.DpId.Contains(u.DpId));
            if (objs.Tel != null && objs.Tel != "")
                curData = curData.Where(u => u.Mobile.Contains(objs.Tel));

            var list = curData.OrderByDescending(p => p.UserId).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new OrderMeal_UserBaseinfoTmp()
            {
                RealName = x.RealName,
                LoginName = x.UserName,
                Telephone = x.Mobile,
                DeptId = x.DpId,
                State = x.State
            });
            count = curData.Count();
            return list.ToList();
        }


        public IList<OrderMeal_UserDinningRoom> GetForBelongPagingByCondition(out int count, OrderMeal_UserDinningRoomQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curData = from ub in this._or_Users_dr.Table
                          join bwu in this._entityStore.Table
                          on ub.UserBaseinfoID equals bwu.Id
                          where bwu.State == 1
                          select new
                          {
                              ub.UserBaseinfoID,
                              ub.Id,
                              ub.DinningRoomID,
                              ub.GetSms
                          };

            if (objs.DinningRoomID != null )
                curData = curData.Where(u => objs.DinningRoomID==u.DinningRoomID);
            
            var list = curData.OrderByDescending(p => p.Id).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new OrderMeal_UserDinningRoom()
            {
                UserBaseinfoID = x.UserBaseinfoID,
                Id = x.Id,
                DinningRoomID = x.DinningRoomID,
                GetSms = x.GetSms
            });
            count = curData.Count();
            return list.ToList();
        }

        public dynamic ImportOrderMealUserBaseinfo(string filename, Stream fileStream)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                int row = 1;
                string error = "", niimport = "", dbniimport = "";
                string repeaterror = "", usererror = "", enougherror = "", userrepeaterror = "", configerror = "";
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return false;
                listDic = this._entityStore.Table.ToList() as List<OrderMeal_UserBaseinfo>;
                listDicUsers = new EfRepository<Users>().Table.ToList() as List<Users>;
                List<OrderMeal_UserBaseinfo> list = new List<OrderMeal_UserBaseinfo>();
                List<OrderMeal_UserDinningRoom> listHRIntegral = new List<OrderMeal_UserDinningRoom>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    List<OrderMeal_UserDinningRoom> HRIntegralModel = new List<OrderMeal_UserDinningRoom>();
                    var model = GetModel(item, out tip, out HRIntegralModel, out niimport);
                    dbniimport += niimport;
                    if (model == null)
                    {
                        switch (tip[0])
                        {
                            case "重复记录":
                                repeaterror += "," + row;
                                break;
                            case "信息不完整":
                                enougherror += "," + row;
                                break;
                            case "用户不存在或数据出错":
                                usererror += "," + row;
                                break;
                            case "名字重复":
                                userrepeaterror += "," + row;
                                break;
                            case "找不到对应的食堂":
                                configerror += "," + row;
                                break;
                            case "存在多个相同名字的食堂":
                                configerror += "," + row;
                                break;
                        }
                        continue;
                    }
                    list.Add(model);
                    listHRIntegral.AddRange(HRIntegralModel);
                    if (list.Count == 100) //足够100的
                    {
                        if (this.InsertByList(list))
                        {
                            count += list.Count;
                            list.Clear();
                            IsSuccess = true;
                        };
                        if (_userdinningroomservice.InsertByList(listHRIntegral))
                        {
                            listHRIntegral.Clear();
                        };
                    }
                    

                }
                if (list.Count > 0)//不足100的
                {                    
                    if (this.InsertByList(list))
                    {
                        count += list.Count;
                        list.Clear();
                        IsSuccess = true;
                    };
                    if (_userdinningroomservice.InsertByList(listHRIntegral))
                    {
                        listHRIntegral.Clear();
                    };
                }                
                if (repeaterror.Length > 0)
                    error += "第" + repeaterror.Remove(0, 1) + "行:有重复的记录;";
                if (enougherror.Length > 0)
                    error += "第" + enougherror.Remove(0, 1) + "行:信息不完整;";
                if (usererror.Length > 0)
                    error += "第" + usererror.Remove(0, 1) + "行:用户不存在或数据出错;";
                if (userrepeaterror.Length > 0)
                    error += "第" + userrepeaterror.Remove(0, 1) + "行:名字重复;";
                if (configerror.Length > 0)
                    error += "第" + configerror.Remove(0, 1) + "行:没有找到对应的食堂或存在多个相同名字的食堂;";
                int falCount = dataTable.Rows.Count - count;
                var result = new { IsSuccess = IsSuccess, Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：" + error };                
                return result;
            }
            catch (Exception ex)
            {
                var result = new { IsSuccess = false, Message = "文件内容错误！" };
                return result;
            }
        }

        DataTable ExcelToDatatable(Stream fileStream)
        {
            try
            {
                Workbook book = new Workbook(fileStream);
                Worksheet sheet = book.Worksheets[0];
                Cells cells = sheet.Cells;

                DataTable dt = new DataTable("Workbook");
                DataColumnCollection columns = dt.Columns;
                for (int i = 0; i < cells.MaxDataColumn + 1; i++)
                    columns.Add(i.ToString(), typeof(System.String));
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        OrderMeal_UserBaseinfo GetModel(DataRow dataRow, out List<string> tip, out List<OrderMeal_UserDinningRoom> HRIntegralModel, out string shtmw)
        {
            //验证数据是否合法 
            tip = new List<string>();
            shtmw = "";
            HRIntegralModel = new List<OrderMeal_UserDinningRoom>();
            Users dicUsers = new Users();
            var _temp = dataRow.ItemArray;
            //HRVacationCourses dic = new HRVacationCourses();
            OrderMeal_UserBaseinfo temp = new OrderMeal_UserBaseinfo();
            ISysUserService _sysuserservice = new SysUserService();
            IOrderMeal_DinningRoomService _diningroomservice = new OrderMeal_DinningRoomService();


            var CurrDataBase = this._entityStore.Table;


            if (string.IsNullOrEmpty(_temp[0].ToString().Trim()) || string.IsNullOrEmpty(_temp[1].ToString().Trim()) || string.IsNullOrEmpty(_temp[2].ToString().Trim()))
                tip.Add("信息不完整");
            else
            {
                //检测是否有重复的记录
                var tmp0 = _temp[0].ToString();
                var tmp1 = _temp[1].ToString();
                var tmp4 = _temp[4].ToString();
                string[] DRName = tmp1.Split(',');
                var checkexsitlist = CurrDataBase.Where(u => u.LoginName == tmp0).ToList();
                if (checkexsitlist.Count > 0)
                {
                    tip.Add("重复记录");
                }
                else
                {
                    //导入时检查用户表中是否有同名的人，有则不导入此用户的信息。
                    var UsersDataList = _bw_Users.Table.Where(u => u.UserName == tmp0).ToList();
                    if (UsersDataList.Count > 1)
                        tip.Add("名字重复");
                    else if (UsersDataList.Count == 0)
                        tip.Add("用户不存在或数据出错");
                    else
                    {
                        temp.Id = Guid.NewGuid();
                        temp.EmployId = UsersDataList[0].EmployeeId;
                        temp.LoginName = _temp[0].ToString();
                        temp.MealCardID = _temp[0].ToString();
                        temp.RealName = UsersDataList[0].RealName;
                        temp.Telephone = UsersDataList[0].Mobile;
                        temp.DeptId = UsersDataList[0].DpId;
                        temp.Balance = 0;
                        temp.State = 1;
                        foreach (var drn in DRName)
                        {
                            var test = false;
                            OrderMeal_UserDinningRoom temproom = new OrderMeal_UserDinningRoom();
                            var configlist = _diningroomservice.List().Where(u => u.DinningRoomName == drn).ToList();
                            if (configlist.Count == 0)
                            {
                                tip.Add("找不到对应的食堂");
                                test = true;
                            }    
                            else if (configlist.Count > 1)
                            {
                                tip.Add("存在多个相同名字的食堂");
                                test = true;
                            }                            
                            else
                            {
                                temproom.Id = Guid.NewGuid();
                                temproom.DinningRoomID = configlist[0].Id;
                                temproom.UserBaseinfoID = temp.Id;
                                temproom.GetSms = 0;
                            }
                            if(test)
                            {
                                temp = null;
                                break;
                            }
                            else
                            {
                                HRIntegralModel.Add(temproom);
                            }
                            
                        }
                    }
                }
            }
            if (tip.Count > 0)
            {
                return null;
            }            
            return temp;
        }

    }
}
