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
using Aspose.Cells;
using System.Data;

namespace CZManageSystem.Service.Composite
{
    public partial class OGSMMonthService : BaseService<OGSMMonth>,IOGSMMonthService
    {
        static Users _user;

        static List<OGSM> listDic = new List<OGSM>();
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.Id) : this._entityStore.Table.OrderBy(c => c.Id).Where(ExpressionFactory(objs));

            PagedList<OGSMMonth> pageList= new PagedList<OGSMMonth>(source, pageIndex <= 0 ? 0 : (pageIndex-1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList.Select(u => new {
                u.Id,
                u.PAY_MON,
                u.USR_NBR,
                u.IsPayment,
                PaymentTime = u.PaymentTime.HasValue ? Convert.ToDateTime(u.PaymentTime).ToString("yyyy-MM-dd") : "",
                AccountTime = u.AccountTime.HasValue ? Convert.ToDateTime(u.AccountTime).ToString("yyyy-MM-dd") : "",
                u.AccountMoney,
                u.AccountNo,
                u.CMPower2G,
                u.CMPower3G,
                u.CMPower4G,
                u.CUPower2G,
                u.CUPower3G,
                u.CUPower4G,
                u.CTPower2G,
                u.CTPower3G,
                u.CTPower4G
            });
        }

        public IList<OGSMMonth> GetForPagingByCondition(out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMMonthQueryBuilder objs = null)
        {
            var curData = this._entityStore.Table;
            if (!string.IsNullOrEmpty(objs.USR_NBR))
                curData = curData.Where(u => u.USR_NBR.Contains(objs.USR_NBR));
            if (!string.IsNullOrEmpty(objs.IsPayment))
                curData = curData.Where(u => u.IsPayment.Contains(objs.IsPayment));
            if (objs.PaymentTime_Start !=null && objs.PaymentTime_End != null)
                curData = curData.Where(u => u.PaymentTime >= objs.PaymentTime_Start && u.PaymentTime <= objs.PaymentTime_End);
            if (objs.AccountTime_Start != null && objs.AccountTime_End != null)
                curData = curData.Where(u => u.AccountTime >= objs.AccountTime_Start && u.PaymentTime <= objs.AccountTime_End);
            if (!string.IsNullOrEmpty(objs.PAY_MON_Start) && !string.IsNullOrEmpty(objs.PAY_MON_End))
            {
                int pay_mon_st = 0, pay_mon_ed = 0;
                int.TryParse(objs.PAY_MON_Start, out pay_mon_st);
                int.TryParse(objs.PAY_MON_End, out pay_mon_ed);
                curData = curData.Where(u => u.PAY_MON >= pay_mon_st && u.PAY_MON <= pay_mon_ed);
            }                
            return new PagedList<OGSMMonth>(curData.OrderBy(c => c.Id), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count);
        }


        public IList<object> GetForExportData(OGSMMonthQueryBuilder objs = null)
        {
            string condition = " where 1=1 ";
            if (!string.IsNullOrEmpty(objs.USR_NBR))
                condition += " and ogm.USR_NBR LIKE '%" + objs.USR_NBR + "%' ";
            if (objs.PaymentTime_Start != null && objs.PaymentTime_End != null)
                condition += " and ogm.PaymentTime between  '" + objs.PaymentTime_Start + "' and '" + objs.PaymentTime_End + "' ";
            if (objs.AccountTime_Start != null && objs.AccountTime_End != null)
                condition += " and ogm.AccountTime between  '" + objs.AccountTime_Start + "' and '" + objs.AccountTime_End + "' ";
            if (!string.IsNullOrEmpty(objs.PAY_MON_Start) && !string.IsNullOrEmpty(objs.PAY_MON_End))
                condition += " and ogm.PAY_MON between '" + objs.PAY_MON_Start + "' and '" + objs.PAY_MON_End + "' ";
            if (!string.IsNullOrEmpty(objs.IsPayment))
                condition += " and ogm.IsPayment = '" + objs.IsPayment + "' ";
            string sql = string.Format(@"select
og.Group_Name,og.Town,og.PowerStation,og.BaseStation,og.PowerType,og.PropertyRight,og.IsRemove,og.RemoveTime,og.IsShare,og.ContractStartTime,og.ContractEndTime,ogm.PAY_MON,ogm.USR_NBR ,ogm.IsPayment,ogm.PaymentTime,ogm.AccountTime,ogm.AccountMoney,ogm.AccountNo,ogm.CMPower2G,ogm.CMPower3G,ogm.CMPower4G,ogm.CUPower2G,ogm.CUPower3G,ogm.CUPower4G,ogm.CTPower2G,ogm.CTPower3G,ogm.CTPower4G,og.Price,og.Address,og.PAY_CYC,og.Property,og.LinkMan,og.Mobile,og.IsWarn,og.WarnCount,ogm.Remark from ogsmmonth ogm left join ogsm og on ogm.usr_nbr=og.usr_nbr {0} order by ogm.USR_NBR,ogm.PAY_MON desc", condition);

            var ls = new EfRepository<OGSMMonth_Export>().ExecuteResT<OGSMMonth_Export>(sql);
            List<object> resultList = new List<object>();

            foreach (var x in ls)
            {
                resultList.Add(new
                {
                   x.Group_Name
                  ,x.Town
                  ,x.PowerStation
                  ,x.BaseStation
                  ,x.PowerType
                  ,x.PropertyRight
                  ,x.IsRemove
                  ,x.RemoveTime
                  ,x.IsShare
                  ,x.ContractStartTime
                  ,x.ContractEndTime                  
                  ,x.PAY_MON
                  ,x.USR_NBR
                  ,x.IsPayment
                  ,x.PaymentTime
                  ,x.AccountTime
                  ,x.AccountMoney
                  ,x.AccountNo
                  ,x.CMPower2G
                  ,x.CMPower3G
                  ,x.CMPower4G
                  ,x.CUPower2G
                  ,x.CUPower3G
                  ,x.CUPower4G
                  ,x.CTPower2G
                  ,x.CTPower3G
                  ,x.CTPower4G
                  ,x.Price
                  ,x.Address
                  ,x.PAY_CYC
                  ,x.Property
                  ,x.LinkMan
                  ,x.Mobile
                  ,x.IsWarn
                  ,x.WarnCount
                  ,x.Remark
                });
            }          
                      
            return new List<object>(resultList);
        }

        public override bool Insert(OGSMMonth entity)
        {
            if (entity == null)
                return false;
            return this._entityStore.Insert(entity);
        }
        public override bool Update(OGSMMonth entity)
        {
            if (entity == null)
                return false;
            var model = FindById(entity.Id);
            model.IsPayment = entity.IsPayment;
            model.PAY_MON = entity.PAY_MON;
            model.USR_NBR = entity.USR_NBR;
            model.PaymentTime = entity.PaymentTime ?? model.PaymentTime;
            model.AccountTime = entity.AccountTime ?? model.AccountTime;
            model.AccountMoney = entity.AccountMoney;
            model.AccountNo = entity.AccountNo;
            model.CMPower2G = entity.CMPower2G;
            model.CMPower3G = entity.CMPower3G;
            model.CMPower4G = entity.CMPower4G;
            model.CUPower2G = entity.CUPower2G;
            model.CUPower3G = entity.CUPower3G;
            model.CUPower4G = entity.CUPower4G;
            model.CTPower2G = entity.CTPower2G;
            model.CTPower3G = entity.CTPower3G;
            model.CTPower4G = entity.CTPower4G;
            model.Remark = entity.Remark == null ? model.Remark : entity.Remark;
            model.CreatedTime = entity.CreatedTime ?? model.CreatedTime;
            model.Creator = entity.Creator == null ? model.Creator : entity.Creator;
            model.LastModTime = entity.LastModTime ?? DateTime.Now;
            model.LastModifier = entity.LastModifier == null ? model.LastModifier : entity.LastModifier;

            return this._entityStore.Update(model);
        }


        public dynamic ImportOGSMMonth(Stream fileStream, Users user)
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
                listDic = new EfRepository<OGSM>().Table.ToList() as List<OGSM>;
                List<OGSMMonth> list = new List<OGSMMonth>();

                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
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

        OGSMMonth GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            DateTime? TimeNull = null;
            int? intnull = 0;
            var _temp = dataRow.ItemArray;
            OGSM dic = new OGSM();
            OGSMMonth temp = new OGSMMonth();
            dic = listDic.Find(f => f.USR_NBR == _temp[1]?.ToString().Trim());
            if (dic == null)
                tip.Add("户号不存在；");
            else
                temp.USR_NBR = _temp[1].ToString();

            if (tip.Count > 0)
                return null;

            temp.PAY_MON = string.IsNullOrEmpty(_temp[0].ToString()) ? intnull : Convert.ToInt32(_temp[0]);            
            temp.IsPayment = _temp[2].ToString();
            temp.PaymentTime = string.IsNullOrEmpty(_temp[3].ToString()) ? TimeNull : Convert.ToDateTime(_temp[3]);
            temp.AccountTime = string.IsNullOrEmpty(_temp[4].ToString()) ? TimeNull : Convert.ToDateTime(_temp[4]);
            temp.AccountMoney = string.IsNullOrEmpty(_temp[5].ToString()) ? intnull : Convert.ToInt32(_temp[5]);
            temp.AccountNo = _temp[6].ToString();
            temp.CMPower2G = string.IsNullOrEmpty(_temp[7]?.ToString()) ? intnull : Convert.ToInt32(_temp[7].ToString());
            temp.CMPower3G = string.IsNullOrEmpty(_temp[8].ToString()) ? intnull : Convert.ToInt32(_temp[8]);
            temp.CMPower4G = string.IsNullOrEmpty(_temp[9].ToString()) ? intnull : Convert.ToInt32(_temp[9]);
            temp.CUPower2G = string.IsNullOrEmpty(_temp[10].ToString()) ? intnull : Convert.ToInt32(_temp[10]);
            temp.CUPower3G = string.IsNullOrEmpty(_temp[11]?.ToString()) ? intnull : Convert.ToInt32(_temp[11].ToString());
            temp.CUPower4G = string.IsNullOrEmpty(_temp[12]?.ToString()) ? intnull : Convert.ToInt32(_temp[12].ToString());
            temp.CTPower2G = string.IsNullOrEmpty(_temp[13]?.ToString()) ? intnull : Convert.ToInt32(_temp[13].ToString());
            temp.CTPower3G = string.IsNullOrEmpty(_temp[14]?.ToString()) ? intnull : Convert.ToInt32(_temp[14].ToString());
            temp.CTPower4G = string.IsNullOrEmpty(_temp[15]?.ToString()) ? intnull : Convert.ToInt32(_temp[15].ToString()); ;            
            temp.Remark = _temp[16].ToString();
            temp.CreatedTime = DateTime.Now;
            temp.Creator = _user.UserName;
            return temp;
        }
    }
}
