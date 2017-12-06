using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Core.Caching;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using System.IO;
using Aspose.Cells;
using System.Data;

namespace CZManageSystem.Service.Composite
{
    public partial class OGSMInfoService : BaseService<OGSMInfo>,IOGSMInfoService
    {
        static Users _user;        
        public override bool Insert(OGSMInfo entity)
        {
            var tmpOGSM = new EfRepository<OGSM>().Table.Where(u => u.USR_NBR == entity.USR_NBR).ToList();
            if (entity == null)
                return false;            
            if (tmpOGSM[0].PowerType == "私电")
            {
                int j = 0;
                int[] pay_mon_list = new int[Convert.ToInt32(entity.PSubPayMonth)]; 
                int FinalPSubPayMonth = 0;
                List<OGSMInfo> list = new List<OGSMInfo>();               
                DateTime endTime = Convert.ToDateTime(entity.PAY_MON.ToString().Substring(0, 4) + "-" + entity.PAY_MON.ToString().Substring(4, 2) + "-" + "01");
                for (j = 0; j < entity.PSubPayMonth; j++)
                {
                    int.TryParse(endTime.AddMonths(0 - j).ToString("yyyyMM"), out pay_mon_list[j]);
                }
                var tmpobjlist = this._entityStore.Table.Where(u => u.USR_NBR == entity.USR_NBR).OrderByDescending(u => u.PAY_MON).ToList();
                //判断私电情况下，以往有没有缴费记录,有：比较做处理，没有：直接分缴处理
                if (this._entityStore.Table.Where(u => u.USR_NBR == entity.USR_NBR).OrderByDescending(u => u.PAY_MON).ToList().Count > 0)
                {
                    var tmpobj = tmpobjlist[0];
                    DateTime startTime = Convert.ToDateTime(tmpobj.PAY_MON.ToString().Substring(0, 4) + "-" + tmpobj.PAY_MON.ToString().Substring(4, 2) + "-" + "01");
                    int cnt = (endTime.Year - startTime.Year) * 12 + (endTime.Month - startTime.Month);
                    if(cnt< entity.PSubPayMonth)
                    {
                        FinalPSubPayMonth = cnt;
                    }
                    else
                    {
                        FinalPSubPayMonth = Convert.ToInt32(entity.PSubPayMonth);
                    }
                }
                else
                {
                    FinalPSubPayMonth = Convert.ToInt32(entity.PSubPayMonth);                    
                }
                for (j = 0; j < FinalPSubPayMonth; j++)
                {
                    OGSMInfo OGSMInfoTemp = new OGSMInfo();
                    Double arvMoney = Convert.ToDouble(entity.Money) / Convert.ToDouble(FinalPSubPayMonth);
                    OGSMInfoTemp.USR_NBR = entity.USR_NBR;
                    OGSMInfoTemp.MF = entity.MF;
                    OGSMInfoTemp.PreKwh = entity.PreKwh;
                    OGSMInfoTemp.NowKwh = entity.NowKwh;
                    OGSMInfoTemp.Adjustment = entity.Adjustment;//调整电费
                    OGSMInfoTemp.Price = entity.Price;
                    OGSMInfoTemp.New_Meter = entity.New_Meter;
                    OGSMInfoTemp.RTime = entity.RTime;
                    OGSMInfoTemp.Payee = entity.Payee;
                    OGSMInfoTemp.Mobile1 = entity.Mobile1;
                    OGSMInfoTemp.Mobile2 = entity.Mobile2;
                    OGSMInfoTemp.BankAcount = entity.BankAcount;
                    OGSMInfoTemp.Bank = entity.Bank;
                    OGSMInfoTemp.Address = entity.Address;
                    OGSMInfoTemp.PSubPayMonth = entity.PSubPayMonth;
                    OGSMInfoTemp.Remark = entity.Remark;
                    OGSMInfoTemp.Creator = entity.Creator;
                    OGSMInfoTemp.CreatedTime = DateTime.Now;
                    OGSMInfoTemp.PAY_MON = pay_mon_list[j];
                    OGSMInfoTemp.Money = Convert.ToDecimal(Math.Round(arvMoney,2));
                    OGSMInfoTemp.CHG = entity.CHG;//计费电度
                    list.Add(OGSMInfoTemp);
                }                
                return this.InsertByList(list);
            }
            else
            {
                return this._entityStore.Insert(entity);
            }
                
        }
        public override bool Update(OGSMInfo entity)
        {
            if (entity == null)
                return false;
            var model = FindById(entity.Id);
            model.PAY_MON = entity.PAY_MON;
            model.USR_NBR = entity.USR_NBR;
            model.PreKwh = entity.PreKwh;
            model.NowKwh = entity.NowKwh;
            model.MF = entity.MF;
            model.CHG = entity.CHG;
            model.Price = entity.Price;
            model.Adjustment = entity.Adjustment;
            model.Money = entity.Money;
            model.CHG_COMPARE = entity.CHG_COMPARE;
            model.Money_COMPARE = entity.Money_COMPARE;
            model.New_Meter = entity.New_Meter;
            model.RTime = entity.RTime;
            model.Payee = entity.Payee;
            model.Mobile1 = entity.Mobile1;
            model.Mobile2 = entity.Mobile2;
            model.BankAcount = entity.BankAcount;
            model.Bank = entity.Bank;
            model.Address = entity.Address;
            model.PSubPayMonth = entity.PSubPayMonth;
            model.Remark = entity.Remark == null ? model.Remark : entity.Remark;
            model.CreatedTime = entity.CreatedTime ?? model.CreatedTime;
            model.Creator = entity.Creator == null ? model.Creator : entity.Creator;
            model.LastModTime = entity.LastModTime ?? DateTime.Now;
            model.LastModifier = entity.LastModifier == null ? model.LastModifier : entity.LastModifier;

            return this._entityStore.Update(model);
        }


        public IList<object> GetForPagingByCondition(out int count, int pageIndex, int pageSize, OGSMInfoQueryBuilder objs = null)
        {
            string condition = " where 1=1 ";
            if (!string.IsNullOrEmpty(objs.USR_NBR))
                condition += " and oi.USR_NBR LIKE '%" + objs.USR_NBR + "%' ";
            if (!string.IsNullOrEmpty(objs.Group_Name))
                condition += " and o.Group_Name = '" + objs.Group_Name + "' ";
            if (!string.IsNullOrEmpty(objs.BaseStation))
                condition += " and o.BaseStation = '" + objs.BaseStation + "' ";
            if (!string.IsNullOrEmpty(objs.PowerType))
                condition += " and o.PowerType = '" + objs.PowerType + "' ";
            if (!string.IsNullOrEmpty(objs.PAY_MON_Start) && !string.IsNullOrEmpty(objs.PAY_MON_End))
                condition += " and oi.PAY_MON between '" + objs.PAY_MON_Start + "' and '"+ objs.PAY_MON_End + "' ";
            if (!string.IsNullOrEmpty(objs.CHG_COMPARE))
                condition += " and oi.CHG_COMPARE = '" + objs.CHG_COMPARE + "' ";
            if (!string.IsNullOrEmpty(objs.Money_COMPARE))
                condition += " and oi.Money_COMPARE = '" + objs.Money_COMPARE + "' ";
            if (!string.IsNullOrEmpty(objs.IsRemove))
                condition += " and o.IsRemove = '" + objs.IsRemove + "' ";

            //System.Data.SqlClient.SqlParameter[] parameters = {
            //    new System.Data.SqlClient.SqlParameter("@WhereCondition",condition)
            //};
            //var query = this._entityStore.ExecuteResT<OGSM_Info>("exec PRO_GET_OGSMINFO @WhereCondition", parameters);

            string sql = string.Format(@"select
oi.Id,oi.USR_NBR,oi.PAY_MON,oi.PreKwh,oi.NowKwh,oi.MF,oi.CHG,oi.Price,oi.Adjustment,oi.Money,oi.CHG_COMPARE,oi.Money_COMPARE,oi.New_Meter,oi.RTime,oi.Payee,oi.Mobile1,oi.Mobile2,oi.BankAcount,oi.Bank,oi.Address,oi.PSubPayMonth,oi.Remark,oi.Creator,oi.CreatedTime,oi.LastModifier,oi.LastModTime,o.BaseStation,o.PowerType,o.IsRemove,o.Group_Name from ogsminfo oi left join ogsm o on oi.usr_nbr=o.usr_nbr {0} order by oi.PAY_MON desc", condition);

            var ls = new EfRepository<OGSM_Info>().ExecuteResT<OGSM_Info>(sql);
            List<object> resultList = new List<object>();
            foreach (var x in ls)
            {
                resultList.Add(new
                {
                    x.Id,
                    x.USR_NBR
                    ,x.PAY_MON
                    ,x.PreKwh
                    ,x.NowKwh
                    ,x.MF
                    ,x.CHG
                    ,x.Price
                    ,x.Adjustment
                    ,x.Money
                    ,x.CHG_COMPARE
                    ,x.Money_COMPARE
                    ,New_Meter = x.New_Meter==1? "是": "否"
                    ,x.RTime
                    ,x.Payee
                    ,x.Mobile1
                    ,x.Mobile2
                    ,x.BankAcount
                    ,x.Bank
                    ,x.Address
                    ,x.PSubPayMonth
                    ,x.Remark
                    ,x.BaseStation
                    ,x.Group_Name
                });
            }
            PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }

        public IList<object> FindOGSMInfoById(int id)
        {
            string condition = " where  1=1  and Id = '" + id + "' ";

            //System.Data.SqlClient.SqlParameter[] parameters = {
            //    new System.Data.SqlClient.SqlParameter("@WhereCondition",condition)
            //};
            //var query = this._entityStore.ExecuteResT<OGSM_Info>("exec PRO_GET_OGSMINFO @WhereCondition", parameters);

            string sql = string.Format(@"select
oi.Id,oi.USR_NBR,oi.PAY_MON,oi.PreKwh,oi.NowKwh,oi.MF,oi.CHG,oi.Price,oi.Adjustment,oi.Money,oi.CHG_COMPARE,oi.Money_COMPARE,oi.New_Meter,oi.RTime,oi.Payee,oi.Mobile1,oi.Mobile2,oi.BankAcount,oi.Bank,oi.Address,oi.PSubPayMonth,oi.Remark,oi.Creator,oi.CreatedTime,oi.LastModifier,oi.LastModTime,o.BaseStation,o.PowerType,o.IsRemove,o.Group_Name from ogsminfo oi left join ogsm o on oi.usr_nbr=o.usr_nbr {0}", condition);

            var ls = new EfRepository<OGSM_Info>().ExecuteResT<OGSM_Info>(sql);

            List<object> resultList = new List<object>();           
            foreach (var x in ls)
            {
                resultList.Add(new
                {
                    x.Id,
                    x.USR_NBR
                    ,x.PAY_MON
                    ,x.PreKwh
                    ,x.NowKwh
                    ,x.MF
                    ,x.CHG
                    ,x.Price
                    ,x.Adjustment
                    ,x.Money
                    ,x.CHG_COMPARE
                    ,x.Money_COMPARE
                    ,x.New_Meter
                    ,x.RTime
                    ,x.Payee
                    ,x.Mobile1
                    ,x.Mobile2
                    ,x.BankAcount
                    ,x.Bank
                    ,x.Address
                    ,x.PSubPayMonth
                    ,x.Remark
                    ,x.BaseStation
                    ,x.Group_Name
                });
            }
            return resultList;
        }

        public object ImportOGSMPInfo(Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0, compareCnt = 0, allcount = 0,Cnt = 0;
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return false;

                List<OGSMInfo> list = new List<OGSMInfo>();

                foreach (DataRow item in dataTable.Rows)
                {
                    var USR_NBR = item[0].ToString();
                    var tmpOGSM = new EfRepository<OGSM>().Table.Where(u => u.USR_NBR == USR_NBR).ToList();
                    if (tmpOGSM.Count() == 0)
                    {
                        OGSM ogsmobj = new OGSM();
                        ogsmobj.IsNew = "1";
                        ogsmobj.USR_NBR = item[0].ToString();
                        ogsmobj.BaseStation = item[1].ToString();
                        new EfRepository<OGSM>().Insert(ogsmobj);
                    }
                    DataRow[] preDw = dataTable.Select(string.Format("户号='{0}' and 缴费月份<{1}", item[0].ToString(), Convert.ToInt32(item[9])), "缴费月份 desc");
                    //var model = GetModel(item);
                    var modellist = GETModelList(item, preDw,out Cnt);
                    allcount += modellist.Count;
                    compareCnt += Cnt;
                    list.AddRange(modellist);
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
                int falCount = allcount - count;
                var result = new { IsSuccess = IsSuccess, Message = "成功导入" + count + "条，有" + compareCnt + "条不符合“电量校验”、“电价校验”，失败" + falCount + "条" };
                return result;
            }
            catch (Exception ex)
            {
                var result = new { IsSuccess = false, Message = "文件内容错误！" };
                return result;
            }
        }
        List<OGSMInfo> GETModelList(DataRow item, DataRow[] preDw,out int compareCnt)
        {
            int j = 0;
            int[] pay_mon_list = new int[Convert.ToInt32(item[18].ToString())];
            int FinalPSubPayMonth = 0;
            int LastPAY_MON = 0;
            int comparecnt = 0;
            List<OGSMInfo> list = new List<OGSMInfo>();
            DateTime endTime = Convert.ToDateTime(item[9].ToString().Substring(0, 4) + "-" + item[9].ToString().Substring(4, 2) + "-" + "01");
            for (j = 0; j < Convert.ToInt32(item[18].ToString()); j++)
            {
                int.TryParse(endTime.AddMonths(0 - j).ToString("yyyyMM"), out pay_mon_list[j]);
            }
            if(preDw.Length>0)//如果在文件中存在上一次的缴费记录，上一次缴费月份在文件取得
            {
                LastPAY_MON = Convert.ToInt32(item[18].ToString());
            }//如果在文件中不存在上一次的缴费记录，上一次缴费月份在数据库取得
            else
            {
                var tmpUSR_NBR = item[0].ToString();
                var tmpobjlist = this._entityStore.Table.Where(u => u.USR_NBR == tmpUSR_NBR).OrderByDescending(u => u.PAY_MON).ToList();
                if (tmpobjlist.Count > 0)
                {
                    LastPAY_MON = Convert.ToInt32(tmpobjlist[0].PAY_MON);
                }
            }
            if(LastPAY_MON!=0)//存在上一次缴费记录，则判断缴费时间差与分缴数的差别
            {
                DateTime startTime = Convert.ToDateTime(LastPAY_MON.ToString().Substring(0, 4) + "-" + LastPAY_MON.ToString().Substring(4, 2) + "-" + "01");
                int cnt = (endTime.Year - startTime.Year) * 12 + (endTime.Month - startTime.Month);
                if (cnt < Convert.ToInt32(item[18]))
                {
                    FinalPSubPayMonth = cnt;
                }
                else
                {
                    FinalPSubPayMonth = Convert.ToInt32(item[18]);
                }
            }
            else
            {
                FinalPSubPayMonth = Convert.ToInt32(item[18]);
            }            
            for (j = 0; j < FinalPSubPayMonth; j++)
            {
                DateTime? TimeNull = null;
                string StringNull = null;
                int intnull = 0;
                decimal decimalnull = 0;
                OGSMInfo OGSMInfoTemp = new OGSMInfo();
                Double arvMoney = Convert.ToDouble(item[8]) / Convert.ToDouble(FinalPSubPayMonth);
                OGSMInfoTemp.USR_NBR = item[0].ToString();
                OGSMInfoTemp.PreKwh = string.IsNullOrEmpty(item[2].ToString()) ? decimalnull : Convert.ToDecimal(item[2]);
                OGSMInfoTemp.NowKwh = string.IsNullOrEmpty(item[3].ToString()) ? decimalnull : Convert.ToDecimal(item[3]);
                OGSMInfoTemp.MF = string.IsNullOrEmpty(item[4].ToString()) ? intnull : Convert.ToInt32(item[4]);
                OGSMInfoTemp.CHG = string.IsNullOrEmpty(item[5].ToString()) ? decimalnull : Convert.ToDecimal(item[5]);
                OGSMInfoTemp.Price = string.IsNullOrEmpty(item[6].ToString()) ? decimalnull : Convert.ToDecimal(item[6]);
                OGSMInfoTemp.Adjustment = string.IsNullOrEmpty(item[7]?.ToString()) ? decimalnull : Convert.ToDecimal(item[7]);
                OGSMInfoTemp.Money = Convert.ToDecimal(Math.Round(arvMoney, 2));
                OGSMInfoTemp.PAY_MON = pay_mon_list[j];
                OGSMInfoTemp.New_Meter = string.IsNullOrEmpty(item[10].ToString()) ? intnull : item[10].ToString() == "是" ? 1 : 0;
                OGSMInfoTemp.RTime = string.IsNullOrEmpty(item[11]?.ToString()) ? TimeNull : Convert.ToDateTime(item[11].ToString());
                OGSMInfoTemp.Payee = string.IsNullOrEmpty(item[12]?.ToString()) ? StringNull : item[12].ToString();
                OGSMInfoTemp.Mobile1 = string.IsNullOrEmpty(item[13]?.ToString()) ? StringNull : item[13].ToString();
                OGSMInfoTemp.Mobile2 = string.IsNullOrEmpty(item[14]?.ToString()) ? StringNull : item[14].ToString();
                OGSMInfoTemp.BankAcount = string.IsNullOrEmpty(item[15]?.ToString()) ? StringNull : item[15].ToString();
                OGSMInfoTemp.Bank = string.IsNullOrEmpty(item[16]?.ToString()) ? StringNull : item[16].ToString();
                OGSMInfoTemp.Address = string.IsNullOrEmpty(item[17]?.ToString()) ? StringNull : item[17].ToString();
                OGSMInfoTemp.PSubPayMonth = string.IsNullOrEmpty(item[18].ToString()) ? intnull : Convert.ToInt32(item[18]);
                OGSMInfoTemp.Remark = item[19].ToString();
                OGSMInfoTemp.CHG_COMPARE = "准确";
                OGSMInfoTemp.Money_COMPARE = "准确";
                //(有功本期-有功上期)*倍率 是否等于 计费电度
                if (Convert.ToDouble(item[3]) > Convert.ToDouble(item[2]) && ((Convert.ToDouble(item[3]) - Convert.ToDouble(item[2])) * Convert.ToInt32(item[4])).ToString("f2") != Convert.ToDouble(item[5]).ToString("f2"))
                {
                    OGSMInfoTemp.CHG_COMPARE = "不准确";
                    comparecnt++;
                }
                //计费电度*电度电价+调整电费 是否等于 实收金额，如电价填0（即阶梯电价），则不做校验。
                if (Convert.ToDouble(item[6]) > 0 && (Math.Round(Convert.ToDouble(item[6]), 4) * Math.Round(Convert.ToDouble(item[5]), 2) + Math.Round(Convert.ToDouble(item[7]), 2)).ToString("f2") != Convert.ToDouble(item[8]).ToString("f2"))
                {
                    OGSMInfoTemp.Money_COMPARE = "不准确";
                    comparecnt++;
                }                
                OGSMInfoTemp.CreatedTime = DateTime.Now;
                OGSMInfoTemp.Creator = _user.UserName;
                list.Add(OGSMInfoTemp);
            }
            compareCnt = comparecnt;
            return list;
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

        public object ImportOGSMInfo(Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0, compareCnt = 0, allcount = 0, Cnt = 0;
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return false;

                List<OGSMInfo> list = new List<OGSMInfo>();

                foreach (DataRow item in dataTable.Rows)
                {
                    var USR_NBR = item[0].ToString();
                    var tmpOGSM = new EfRepository<OGSM>().Table.Where(u => u.USR_NBR == USR_NBR).ToList();
                    if (tmpOGSM.Count() == 0)
                    {
                        OGSM ogsmobj = new OGSM();
                        ogsmobj.IsNew = "1";
                        ogsmobj.USR_NBR = item[0].ToString();
                        ogsmobj.BaseStation = item[1].ToString();
                        new EfRepository<OGSM>().Insert(ogsmobj);
                    }
                    var model = GetModel(item,out Cnt );
                    compareCnt += Cnt;
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
                var result = new { IsSuccess = IsSuccess, Message = "成功导入" + count + "条，有" + compareCnt + "条不符合“电量校验”、“电价校验”，失败" + falCount + "条" };
                return result;
            }
            catch (Exception ex)
            {
                var result = new { IsSuccess = false, Message = "文件内容错误！" };
                return result;
            }
        }


        OGSMInfo GetModel(DataRow dataRow,out int cnt)
        {
            DateTime? TimeNull = null;
            int intnull = 0;
            decimal decimalnull = 0;
            string StringNull = null;
            int comparecnt = 0;
            var _temp = dataRow.ItemArray;
            OGSMInfo OGSMInfoTemp = new OGSMInfo();
            OGSMInfoTemp.USR_NBR = _temp[0].ToString();
            OGSMInfoTemp.PreKwh = string.IsNullOrEmpty(_temp[2].ToString()) ? decimalnull : Convert.ToDecimal(_temp[2]);
            OGSMInfoTemp.NowKwh = string.IsNullOrEmpty(_temp[3].ToString()) ? decimalnull : Convert.ToDecimal(_temp[3]);
            OGSMInfoTemp.MF = string.IsNullOrEmpty(_temp[4].ToString()) ? intnull : Convert.ToInt32(_temp[4]);
            OGSMInfoTemp.CHG = string.IsNullOrEmpty(_temp[5].ToString()) ? decimalnull : Convert.ToDecimal(_temp[5]);
            OGSMInfoTemp.Price = string.IsNullOrEmpty(_temp[6].ToString()) ? decimalnull : Convert.ToDecimal(_temp[6]);
            OGSMInfoTemp.Adjustment = string.IsNullOrEmpty(_temp[7]?.ToString()) ? decimalnull : Convert.ToDecimal(_temp[7]);
            OGSMInfoTemp.Money = string.IsNullOrEmpty(_temp[8]?.ToString()) ? decimalnull : Convert.ToDecimal(_temp[8]);
            OGSMInfoTemp.PAY_MON = string.IsNullOrEmpty(_temp[9].ToString()) ? intnull : Convert.ToInt32(_temp[9]);
            OGSMInfoTemp.New_Meter = string.IsNullOrEmpty(_temp[10].ToString()) ? intnull : _temp[10].ToString() == "是" ? 1 : 0;
            OGSMInfoTemp.RTime = string.IsNullOrEmpty(_temp[11]?.ToString()) ? TimeNull : Convert.ToDateTime(_temp[11].ToString());
            OGSMInfoTemp.Payee = string.IsNullOrEmpty(_temp[12]?.ToString()) ? StringNull : _temp[12].ToString();
            OGSMInfoTemp.Mobile1 = string.IsNullOrEmpty(_temp[13]?.ToString()) ? StringNull : _temp[13].ToString();
            OGSMInfoTemp.Mobile2 = string.IsNullOrEmpty(_temp[14]?.ToString()) ? StringNull : _temp[14].ToString();
            OGSMInfoTemp.BankAcount = string.IsNullOrEmpty(_temp[15]?.ToString()) ? StringNull : _temp[15].ToString();
            OGSMInfoTemp.Bank = string.IsNullOrEmpty(_temp[16]?.ToString()) ? StringNull : _temp[16].ToString();
            OGSMInfoTemp.Address = string.IsNullOrEmpty(_temp[17]?.ToString()) ? StringNull : _temp[17].ToString();
            OGSMInfoTemp.PSubPayMonth = string.IsNullOrEmpty(_temp[18].ToString()) ? intnull : Convert.ToInt32(_temp[18]);
            OGSMInfoTemp.Remark = _temp[19].ToString();
            OGSMInfoTemp.CHG_COMPARE = "准确";
            OGSMInfoTemp.Money_COMPARE = "准确";
            //(有功本期-有功上期)*倍率 是否等于 计费电度
            if (Convert.ToDouble(_temp[3]) > Convert.ToDouble(_temp[2]) && ((Convert.ToDouble(_temp[3]) - Convert.ToDouble(_temp[2])) * Convert.ToInt32(_temp[4])).ToString("f2") != Convert.ToDouble(_temp[5]).ToString("f2"))
            {
                OGSMInfoTemp.CHG_COMPARE = "不准确";
                comparecnt++;
            }
            //计费电度*电度电价+调整电费 是否等于 实收金额，如电价填0（即阶梯电价），则不做校验。
            if (Convert.ToDouble(_temp[6]) > 0 && (Math.Round(Convert.ToDouble(_temp[6]), 4) * Math.Round(Convert.ToDouble(_temp[5]), 2) + Math.Round(Convert.ToDouble(_temp[7]), 2)).ToString("f2") != Convert.ToDouble(_temp[8]).ToString("f2"))
            {
                OGSMInfoTemp.Money_COMPARE = "不准确";
                comparecnt++;
            }
            OGSMInfoTemp.CreatedTime = DateTime.Now;
            OGSMInfoTemp.Creator = _user.UserName;
            cnt = comparecnt;
            return OGSMInfoTemp;
        }
    }
}
