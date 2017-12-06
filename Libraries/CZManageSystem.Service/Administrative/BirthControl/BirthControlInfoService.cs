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
    public class BirthControlInfoService: BaseService<BirthControlInfo>, IBirthControlInfoService
    {
        static Users _user;
        static List<VW_Birthcontrol_Data> listDic = new List<VW_Birthcontrol_Data>();
        IBirthControlLogService _birthcontrollogService = new BirthControlLogService();
        BirthControlLog objLog = new BirthControlLog();
        public IList<BirthControlInfo> GetListByUserid(Guid id)
        {
            var query = this._entityStore.Table.Where(a => a.UserId == id);
            List<BirthControlInfo> List = new List<BirthControlInfo>(query);
            return List;
        }


        public dynamic ImportBirthControlInfo(Stream fileStream, Users user)
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
                List<BirthControlInfo> list = new List<BirthControlInfo>();
                List<BirthControlInfo> listupdate = new List<BirthControlInfo>();                
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    bool update = false;
                    var model = GetModel(item, out tip, out update);
                    if (model == null)
                    {
                        error += "第" + row + "行:" + string.Join(";", tip);
                        continue;
                    }
                    if (update)
                        listupdate.Add(model);
                    else
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
                    if (listupdate.Count == 100) //足够100的
                    {
                        if (this.UpdateByList(listupdate))
                        {
                            count += listupdate.Count;
                            listupdate.Clear();
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
                if (listupdate.Count > 0)//不足100的
                {
                    if (this.UpdateByList(listupdate))
                    {
                        count += listupdate.Count;
                        listupdate.Clear();
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
                objLog.Description = "导入" + "计划信息成功." + "成功导入" + count + "条，失败" + falCount + "条。其他提示：" + error;
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
                objLog.Description = "导入" + "计划信息失败.文件内容错误！";
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

        BirthControlInfo GetModel(DataRow dataRow, out List<string> tip, out bool update)
        {
            tip = new List<string>();
            DateTime? TimeNull = null;
            int? intnull = 0;
            var _temp = dataRow.ItemArray;
            VW_Birthcontrol_Data dic = new VW_Birthcontrol_Data();
            BirthControlInfo temp = new BirthControlInfo();
            dic = listDic.Find(f => f.EmployeeId == _temp[0].ToString());
            update = false;
            if(dic==null)
            {
                tip.Add("未在系统中找到其用户信息。");
                update = false;
                return null;
            }
            if (dic.InfoStatus == "已编辑")
            {
                temp = FindByFeldName(u => u.UserId == dic.UserId);
                update = true;
            }
            else if (dic.InfoStatus == "未编辑")
            {
                temp.UserId = dic.UserId;
                temp.CreatedTime = DateTime.Now;
                temp.Creator = _user.UserName;
                update = false;
            }


            if (string.IsNullOrEmpty(_temp[2].ToString().Trim()))
                tip.Add("性别不能为空");
            else
                if (_temp[2].ToString() == "女")
                temp.Sex = "2";
            else if (_temp[2].ToString() == "男")
                temp.Sex = "1";

            if (string.IsNullOrEmpty(_temp[3].ToString().Trim()))
                tip.Add("出生日期不能为空");
            else
                temp.Birthday = string.IsNullOrEmpty(_temp[3].ToString()) ? TimeNull : Convert.ToDateTime(_temp[3]);
            if (string.IsNullOrEmpty(_temp[4].ToString().Trim()))
                tip.Add("民族不能为空");
            else
                temp.Nation = _temp[4].ToString();
            if (string.IsNullOrEmpty(_temp[5].ToString().Trim()))
                tip.Add("身份证号码不能为空");
            else
                temp.IdCardNum = _temp[5].ToString().Trim();
            if (string.IsNullOrEmpty(_temp[6].ToString().Trim()))
                tip.Add("户口所属街道不能为空");
            else
                temp.StreetBelong = _temp[6].ToString();
            if (string.IsNullOrEmpty(_temp[8].ToString().Trim()))
                tip.Add("联系电话不能为空");
            else
                temp.PhoneNum = _temp[8].ToString();
            if (string.IsNullOrEmpty(_temp[7].ToString().Trim()))
                tip.Add("婚姻状况不能为空");
            else
                temp.MaritalStatus = _temp[7]?.ToString();

            if (_temp[7]?.ToString()== "初婚" || _temp[7]?.ToString()== "再婚")
            {
                if (string.IsNullOrEmpty(_temp[11].ToString().Trim()))
                    tip.Add("初婚日期不能为空");
                else
                    temp.FirstMarryDate = Convert.ToDateTime(_temp[11]);

                if (string.IsNullOrEmpty(_temp[17].ToString().Trim()))
                    tip.Add("配偶-姓名不能为空");
                else
                    temp.SpouseName = _temp[17]?.ToString();
                if (string.IsNullOrEmpty(_temp[18].ToString().Trim()))
                    tip.Add("配偶-性别不能为空");
                else
                    temp.Spousesex = _temp[18]?.ToString();
                if (string.IsNullOrEmpty(_temp[19].ToString().Trim()))
                    tip.Add("配偶-出生日期不能为空");
                else
                    temp.SpouseBirthday = string.IsNullOrEmpty(_temp[19]?.ToString()) ? TimeNull : Convert.ToDateTime(_temp[19].ToString());
                if (string.IsNullOrEmpty(_temp[20].ToString().Trim()))
                    tip.Add("配偶-身份证号码不能为空");
                else
                    temp.SpouseIdCardNum = _temp[20]?.ToString();
                if (string.IsNullOrEmpty(_temp[21].ToString().Trim()))
                    tip.Add("配偶-户口所属街道不能为空");
                else
                    temp.SpouseAccountbelong = _temp[21]?.ToString();
                if (string.IsNullOrEmpty(_temp[22].ToString().Trim()))
                    tip.Add("配偶-联系电话不能为空");
                else
                    temp.SpousePhone = _temp[22]?.ToString();
                if (string.IsNullOrEmpty(_temp[23].ToString().Trim()))
                    tip.Add("配偶-婚姻状况不能为空");
                else
                    temp.SpouseMaritalStatus = _temp[23]?.ToString();
                if (_temp[24]?.ToString()== "是")
                    if (string.IsNullOrEmpty(_temp[25]?.ToString()))
                        tip.Add("配偶-工作单位不能为空");
                    else
                    {
                        temp.SpouseWorkingAddress = _temp[25]?.ToString();  
                    }
                else if(_temp[24]?.ToString() == "否")
                    if (string.IsNullOrEmpty(_temp[28]?.ToString()))
                        tip.Add("配偶-是否参加公司组织的妇检不能为空");
                    else
                    {
                        temp.OrganizeGE = _temp[28]?.ToString();
                    }
                else
                {
                    temp.FixedJob = _temp[24]?.ToString();
                    temp.SpouseWorkingAddress = _temp[25]?.ToString();
                    temp.OrganizeGE = _temp[28]?.ToString();
                }


            }
            else
            {
                temp.FirstMarryDate = TimeNull;
                temp.SpouseName = _temp[17]?.ToString();
                temp.Spousesex = _temp[18]?.ToString();
                temp.SpouseBirthday = TimeNull;
                temp.SpouseIdCardNum = _temp[20]?.ToString();
                temp.SpouseAccountbelong = _temp[21]?.ToString();
                temp.SpousePhone = _temp[22]?.ToString();
                temp.SpouseMaritalStatus = _temp[23]?.ToString();
                temp.FixedJob = _temp[24]?.ToString();
                temp.SpouseWorkingAddress = _temp[25]?.ToString();
                temp.OrganizeGE = _temp[28]?.ToString();
            }
            if (tip.Count > 0)
            {
                update = false;
                return null;
            }
            temp.Latemarriage = _temp[9].ToString();
            temp.Havebear = _temp[10].ToString();            
            temp.DivorceDate = string.IsNullOrEmpty(_temp[12].ToString()) ? TimeNull : Convert.ToDateTime(_temp[12]);
            temp.RemarryDate = string.IsNullOrEmpty(_temp[13].ToString()) ? TimeNull : Convert.ToDateTime(_temp[13]);
            temp.WidowedDate = string.IsNullOrEmpty(_temp[14].ToString()) ? TimeNull : Convert.ToDateTime(_temp[14]);
            temp.LigationDate = string.IsNullOrEmpty(_temp[15].ToString()) ? TimeNull : Convert.ToDateTime(_temp[15]);     
            temp.BRemark =_temp[16]?.ToString();
            
            temp.SpouseLigationDate = string.IsNullOrEmpty(_temp[26].ToString()) ? TimeNull : Convert.ToDateTime(_temp[26]);
            temp.SameWorkPlace = _temp[27]?.ToString();
           
            temp.ForeMarriageBore = _temp[29]?.ToString();

            temp.LastModifier= _user.UserName;
            temp.LastModTime = DateTime.Now;
           // update = false;
            return temp;
        }
    }
}
