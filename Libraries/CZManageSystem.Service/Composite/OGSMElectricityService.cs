using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Core.Caching;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using System.Linq.Expressions;
using CZManageSystem.Data.Domain.SysManger;
using System.IO;
using System.Data;
using Aspose.Cells;

namespace CZManageSystem.Service.Composite
{
    public partial class OGSMElectricityService : BaseService<OGSMElectricity>, IOGSMElectricityService
    {
        static Users _user;
        private readonly IRepository<OGSMInfo> _OGSMInfo = new EfRepository<OGSMInfo>();
        private readonly IRepository<OGSM> _OGSM = new EfRepository<OGSM>();
        public IList<OGSMElectricity> GetForPagingByCondition(out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMElectricityQueryBuilder obj = null)
        {
            var curData = this._entityStore.Table;
            if (!string.IsNullOrEmpty(obj.USR_NBR))
                curData = curData.Where(u => u.USR_NBR.Contains(obj.USR_NBR));
            if (!string.IsNullOrEmpty(obj.ElectricityMeter))
                curData = curData.Where(u => u.ElectricityMeter == obj.ElectricityMeter);
            if (obj.PAY_MON != 0 )
                curData = curData.Where(u => u.PAY_MON == obj.PAY_MON);
            //object condition = new
            //{
            //    USR_NBR = USR_NBR,
            //    ElectricityMeter = ElectricityMeter,
            //    PAY_MON = PAY_MON
            //};
            return new PagedList<OGSMElectricity>(curData.OrderBy(c => c.Id), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count);
        }
        public IList<OGSMElectricity> GetForPagingIByCondition(OGSMInfoQueryBuilder obj = null)
        {
            var curData = from t1 in this._entityStore.Table
                          from t2 in this._OGSMInfo.Table
                          where t1.USR_NBR == t2.USR_NBR && t1.PAY_MON == t2.PAY_MON
                          from t3 in this._OGSM.Table
                          where t2.USR_NBR == t3.USR_NBR
                          select new
                          {
                              t1.USR_NBR,
                              t1.PAY_MON,
                              t1.ElectricityMeter,
                              t1.Electricity,
                              t2.CHG_COMPARE,
                              t2.Money_COMPARE,
                              t3.Group_Name,
                              t3.BaseStation,
                              t3.PowerType,
                              t3.IsRemove
                          };
            if (!string.IsNullOrEmpty(obj.USR_NBR))
                curData = curData.Where(u => u.USR_NBR.Contains(obj.USR_NBR));
            if (!string.IsNullOrEmpty(obj.CHG_COMPARE))
                curData = curData.Where(u => u.CHG_COMPARE == obj.CHG_COMPARE);
            if (!string.IsNullOrEmpty(obj.Money_COMPARE))
                curData = curData.Where(u => u.Money_COMPARE == obj.Money_COMPARE);
            if (!string.IsNullOrEmpty(obj.Group_Name))
                curData = curData.Where(u => u.Group_Name == obj.Group_Name);
            if (!string.IsNullOrEmpty(obj.BaseStation))
                curData = curData.Where(u => u.BaseStation == obj.BaseStation);
            if (!string.IsNullOrEmpty(obj.PowerType))
                curData = curData.Where(u => u.PowerType == obj.PowerType);
            if (!string.IsNullOrEmpty(obj.IsRemove))
                curData = curData.Where(u => u.IsRemove == obj.IsRemove);
            if (!string.IsNullOrEmpty(obj.PAY_MON_End) || !string.IsNullOrEmpty(obj.PAY_MON_Start))
            {
                int pay_mon_st = 0, pay_mon_ed = 0;
                int.TryParse(obj.PAY_MON_Start, out pay_mon_st);
                int.TryParse(obj.PAY_MON_End, out pay_mon_ed);
                curData = curData.Where(u => u.PAY_MON >= pay_mon_st && u.PAY_MON <= pay_mon_ed);
            }
            

            var list = curData.OrderByDescending(p => p.PAY_MON).AsEnumerable().Select(x => new OGSMElectricity()
            {
                USR_NBR = x.USR_NBR,
                PAY_MON = x.PAY_MON,
                ElectricityMeter = x.ElectricityMeter,
                Electricity = x.Electricity
            });
            return list.ToList();
        }

        public override bool Insert(OGSMElectricity entity)
        {
            if (entity == null)
                return false;
            return this._entityStore.Insert(entity);
        }
        public override bool Update(OGSMElectricity entity)
        {
            if (entity == null)
                return false;
            var model = FindById(entity.Id);
            model.ElectricityMeter = entity.ElectricityMeter;
            model.Electricity = entity.Electricity;
            model.PAY_MON = entity.PAY_MON;
            model.USR_NBR = entity.USR_NBR;
            model.Remark = entity.Remark == null ? model.Remark : entity.Remark;
            model.CreatedTime = entity.CreatedTime ?? model.CreatedTime;
            model.Creator = entity.Creator == null ? model.Creator : entity.Creator;
            model.LastModTime = entity.LastModTime ?? DateTime.Now;
            model.LastModifier = entity.LastModifier == null ? model.LastModifier : entity.LastModifier;

            return this._entityStore.Update(model);
        }


        public dynamic ImportOGSMBase(Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                int row = 1;
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return false;

                List<OGSMElectricity> list = new List<OGSMElectricity>();
                List<string> tip = new List<string>();
                string noexisterror = "", error = "";

                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    var model = GetModel(item,out tip);
                    if (model == null)
                    {
                        noexisterror += "," + row;
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
                if (noexisterror.Length > 0)
                    error += "第" + noexisterror.Remove(0, 1) + "行:不存在对应的缴费明细;";
                int falCount = dataTable.Rows.Count - count;
                var result = new { IsSuccess = IsSuccess, Message = "成功导入" + count + "条，失败" + falCount + "条,其他提示：" + error  };
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

        OGSMElectricity GetModel(DataRow dataRow, out List<string> tip)
        {
            int? intnull = null;
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            OGSMElectricity temp = new OGSMElectricity();
            IOGSMInfoService _ogsminfoservice = new OGSMInfoService();
            var tmp0 = string.IsNullOrEmpty(_temp[0]?.ToString()) ? intnull : Convert.ToInt32(_temp[0].ToString());
            var tmp1= _temp[1].ToString();
            var list = _ogsminfoservice.List().Where(u => u.USR_NBR == tmp1 && u.PAY_MON == tmp0).ToList();
            if(list.Count<=0)
            {
                tip.Add("不存在缴费明细");
            }
            else
            {
                temp.PAY_MON = string.IsNullOrEmpty(_temp[0]?.ToString()) ? intnull : Convert.ToInt32(_temp[0].ToString());
                temp.USR_NBR = _temp[1].ToString();
                temp.ElectricityMeter = _temp[2].ToString();
                temp.Electricity = _temp[3].ToString();
                temp.Remark = _temp[4].ToString();
                temp.CreatedTime = DateTime.Now;
                temp.Creator = _user.UserName;
            }

            if (tip.Count > 0)
            {
                return null;
            }            
            return temp;
        }
    }
}
