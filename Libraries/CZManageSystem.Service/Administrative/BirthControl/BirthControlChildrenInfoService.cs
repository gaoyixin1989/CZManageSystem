using Aspose.Cells;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.BirthControl
{
    public class BirthControlChildrenInfoService : BaseService<BirthControlChildrenInfo>, IBirthControlChildrenInfoService
    {
        static Users _user;
        static List<VW_Birthcontrol_Data> listDic = new List<VW_Birthcontrol_Data>();
        IBirthControlLogService _birthcontrollogService = new BirthControlLogService();
        BirthControlLog objLog = new BirthControlLog();
        public IList<BirthControlChildrenInfo> GetAllChildrenList(Guid id)
        {
            var query = this._entityStore.Table.Where(a => a.UserId == id);
            List<BirthControlChildrenInfo> List = new List<BirthControlChildrenInfo>(query);
            return List;
        }

        public dynamic ImportBirthControlChildrenInfo(Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                _user = user;
                int row = 1;
                string error = "";
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return false;
                listDic = new EfRepository<VW_Birthcontrol_Data>().Table.ToList() as List<VW_Birthcontrol_Data>;
                List<BirthControlChildrenInfo> list = new List<BirthControlChildrenInfo>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    bool update = false;
                    var model = GetModel(item, out tip);
                    if (model == null)
                    {
                        error += "第" + row + "行:" + string.Join(";", tip);
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
                int falCount = dataTable.Rows.Count - count;
                var result = new { IsSuccess = IsSuccess, Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：" + error };
                objLog.UserId = user.UserId;
                objLog.UserName = user.RealName;
                objLog.UserIp = System.Web.HttpContext.Current.Request.ServerVariables[System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "导入";
                objLog.Description = "导入" + "子女信息成功." + "成功导入" + count + "条，失败" + falCount + "条。其他提示：" + error;
                _birthcontrollogService.Insert(objLog);
                return result;
            }
            catch (Exception ex)
            {
                var result = new { IsSuccess = false, Message = "文件内容错误！" };
                objLog.UserId = user.UserId;
                objLog.UserName = user.RealName;
                objLog.UserIp = System.Web.HttpContext.Current.Request.ServerVariables[System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "导入";
                objLog.Description = "导入" + "子女信息失败.文件内容错误！";
                _birthcontrollogService.Insert(objLog);
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

        BirthControlChildrenInfo GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            int? intnull = 0;
            var _temp = dataRow.ItemArray;
            VW_Birthcontrol_Data dic = new VW_Birthcontrol_Data();
            BirthControlChildrenInfo temp = new BirthControlChildrenInfo();
            dic = listDic.Find(f => f.EmployeeId == _temp[0].ToString());
            if (dic == null)
            {
                tip.Add("未在系统中找到其用户信息。");
                return null;
            }
            if (string.IsNullOrEmpty(_temp[1].ToString().Trim()))
                tip.Add("子女-姓名不能为空");
            else
                temp.Name = _temp[1].ToString();

            if (string.IsNullOrEmpty(_temp[3].ToString().Trim()))
                tip.Add("子女-性别不能为空");
            else
                temp.Sex = _temp[3].ToString();

            if (string.IsNullOrEmpty(_temp[2].ToString().Trim()))
                tip.Add("子女-出生日期不能为空");
            else
                temp.Birthday = Convert.ToDateTime(_temp[2]);

            if (string.IsNullOrEmpty(_temp[4].ToString().Trim()))
                tip.Add("子女-政策内外不能为空");
            else
                temp.PolicyPostiton = _temp[4].ToString();

            if (string.IsNullOrEmpty(_temp[5].ToString().Trim()))
                tip.Add("子女-是否独生子女不能为空");
            else
                temp.CISingleChildren = _temp[5].ToString();

            if (tip.Count > 0)
            {
                return null;
            }
            temp.UserId = dic.UserId;
            temp.CISingleChildNum = _temp[6].ToString();
            temp.Treatment = _temp[7].ToString();
            temp.remark = _temp[8].ToString();
            temp.CreatedTime = DateTime.Now;
            temp.Creator = _user.UserName;
            temp.LastModifier = _user.UserName;
            temp.LastModTime = DateTime.Now;
            // update = false;
            return temp;
        }
    }
}
