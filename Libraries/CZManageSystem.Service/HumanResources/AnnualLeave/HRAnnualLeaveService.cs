using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.AnnualLeave;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.AnnualLeave
{
    public class HRAnnualLeaveService : BaseService<HRAnnualLeave>, IHRAnnualLeaveService
    {
        static Users _user;
        private readonly IRepository<Users> _bw_Users = new EfRepository<Users>();
        public IList<HRAnnualLeave> GetForPagingByCondition(Users user, out int count, HRAnnualLeaveQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._bw_Users.Table
                          on hrvc.UserId equals bwu.UserId
                          select new
                          {
                              hrvc.Id,
                              DpId = bwu.DpId,
                              UserName = bwu.UserName,
                              RealName = bwu.RealName,
                              hrvc.VYears,
                              hrvc.VDays,
                              hrvc.UserId,
                              hrvc.FdYearVDays,
                              hrvc.FdLastYearVDays,
                              hrvc.BcYearVDays,
                              bwu.EmployeeId
                          };
            if (objs.DpId != null)
                curData = curData.Where(u => objs.DpId.Contains(u.DpId));
            if (objs.VYears != null && objs.VYears != "")
                curData = curData.Where(u => objs.VYears.Contains(u.VYears));
            if (objs.EmployeeID != null && objs.EmployeeID != "")
                curData = curData.Where(u => u.EmployeeId.Contains(objs.EmployeeID));
            if (objs.RealName != null && objs.RealName != "")
                curData = curData.Where(u => u.RealName.Contains(objs.RealName));
            _user = user;
            var mm = new EfRepository<string>().Execute<string>(string.Format("exec HR_GetUser @UserName='{0}'", _user.UserName)).ToList();
            curData = curData.Where(u => mm.Contains(u.UserName.ToString()));
            count = curData.Count();
            var list = curData.OrderByDescending(p => p.VYears).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new HRAnnualLeave()
            {
                Id = x.Id,
                UserName = x.UserName,
                UserId = x.UserId,
                VYears = x.VYears,
                FdYearVDays = x.FdYearVDays,
                VDays = x.VDays,
                FdLastYearVDays = x.FdLastYearVDays,
                BcYearVDays = x.BcYearVDays
            });
            return list.ToList();
        }


        public dynamic ImportHRAnnualLeave(string filename, Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                _user = user;
                int row = 1;
                string error = "", niimport = "", dbniimport = "";
                string repeaterror = "",usererror="",enougherror="", userrepeaterror="";
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return false;
                List<HRAnnualLeave> list = new List<HRAnnualLeave>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    var model = GetModel(item, out tip,  out niimport);
                    dbniimport += niimport;
                    if (model == null)
                    {
                        switch(tip[0])
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
                        }                          
                        continue;
                    }
                    list.Add(model);
                    if (list.Count == 100) //足够100的
                    {
                        if (this.InsertByList(list))
                        {
                            count += list.Count;
                            list.Clear();
                            IsSuccess = true;
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
                }
                if (repeaterror.Length > 0)
                    error += "第" + repeaterror.Remove(0, 1) + "行:有重复的记录;";
                if (enougherror.Length > 0)
                    error += "第" + enougherror.Remove(0, 1) + "行:信息不完整;";
                if (usererror.Length > 0)
                    error += "第" + usererror.Remove(0, 1) + "行:用户不存在或数据出错;";
                if (userrepeaterror.Length > 0)
                    error += "第" + userrepeaterror.Remove(0, 1) + "行:名字重复;";
                int falCount = dataTable.Rows.Count - count;
                var result = new { IsSuccess = IsSuccess, Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：" + error };
                HRAnnualleaveImport impobj = new HRAnnualleaveImport();
                IHRAnnualleaveImportService _importservice = new HRAnnualleaveImportService();
                impobj.Importor = user.RealName;
                impobj.ImportTime = DateTime.Now;
                impobj.ImportMsg = error;
                if(dbniimport.Length>0)
                    impobj.ImportInformation = dbniimport.Remove(dbniimport.Length - 1, 1);
                impobj.ImportTitle = filename;
                impobj.ImportType = "AnnualLeave";
                _importservice.Insert(impobj);
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

        HRAnnualLeave GetModel(DataRow dataRow, out List<string> tip, out string shtmw)
        {
            //验证数据是否合法 
            tip = new List<string>();
            shtmw = "";
            Users dicUsers = new Users();
            var _temp = dataRow.ItemArray;
            HRAnnualLeave temp = new HRAnnualLeave();
            ISysUserService _sysuserservice = new SysUserService();

            var CurrDataBase = this._entityStore.Table;


            if (string.IsNullOrEmpty(_temp[0].ToString().Trim()) || string.IsNullOrEmpty(_temp[1].ToString().Trim()) || string.IsNullOrEmpty(_temp[3].ToString().Trim()) )
                tip.Add("信息不完整");
            else
            {
                //检测是否有重复的记录
                var tmp0 = _temp[0].ToString();
                var tmp1 = _temp[1].ToString();
                var checkexsitlist = CurrDataBase.Where(u => u.VYears == tmp1 && u.UserName == tmp0).ToList();
                if (checkexsitlist.Count > 0)
                {
                    tip.Add("重复记录");
                    shtmw += _temp[0].ToString() + "*" +
                             _temp[1].ToString() + "*" +
                             _temp[2].ToString() + "*" +
                             _temp[3].ToString() + "*" +
                             _temp[4].ToString() + "$";
                }
                else
                {
                    //导入时检查用户表中是否有同名的人，有则不导入此用户的信息。
                    var UsersDataList = _bw_Users.Table.Where(u => u.RealName == tmp0 && u.Status == 0).ToList();
                    if (UsersDataList.Count > 1)
                        tip.Add("名字重复");
                    else if(UsersDataList.Count == 0 )
                        tip.Add("用户不存在或数据出错");
                    else
                    {
                        temp.UserId = UsersDataList[0].UserId;
                        temp.UserName = _temp[0].ToString();
                    }

                }
            }
            if (tip.Count > 0)
            {
                return null;
            }
            temp.FdYearVDays = Convert.ToDecimal(_temp[3].ToString());
            temp.VYears = _temp[1].ToString();
            temp.FdLastYearVDays = string.IsNullOrEmpty(_temp[2].ToString().Trim()) ? 0 : Convert.ToDecimal(_temp[2].ToString());
            temp.BcYearVDays = string.IsNullOrEmpty(_temp[4].ToString().Trim()) ? 0 : Convert.ToDecimal(_temp[4].ToString());
            return temp;
        }

    }
}
