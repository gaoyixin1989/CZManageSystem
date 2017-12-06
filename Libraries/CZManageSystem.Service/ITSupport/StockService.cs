using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.ITSupport
{
    public partial class StockService : BaseService<Stock>, IStockService
    {
        private readonly IRepository<Stock> _Stock;
        private readonly IRepository<StockAsset> _StockAsset;
        private readonly IRepository<EquipApp> _EquipApp;
        private readonly IRepository<Proj> _Proj;
        private readonly IRepository<EquipAsset> _EquipAsset;


        public StockService()
        {
            this._Stock = new EfRepository<Stock>();
            this._StockAsset = new EfRepository<StockAsset>();
            this._EquipApp = new EfRepository<EquipApp>();
            this._Proj = new EfRepository<Proj>();
            this._EquipAsset = new EfRepository<EquipAsset>();
        }

        static Users _user;

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetForPaging(out int count, StockQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._Stock.Table.Select(st => new
            {
                Id = st.Id,
                EquipNum = st.EquipNum,
                ProjSn = st.ProjSn,
                Stocktime = st.StockTime,
                EquipInfo = st.EquipInfo,
                EquipClass = st.EquipClass,
                LableNo = st.LableNo,
                Content = st.Content,
                StockType = st.StockType,
                EditTime = st.EditTime,
                outnum = _EquipApp.Table.Where(s =>s.EquipClass==st.EquipClass&&s.EquipInfo==st.EquipInfo&&s.ProjSn==st.ProjSn&&s.Status==3).Sum(r=>r.AppNum),//已调拨数量
                asscount = _StockAsset.Table.Where(w => st.Id == w.StockId).Count(),//已分配固定资产编码数
                totalnum = (st.EquipNum - _EquipApp.Table.Where(s => s.EquipClass == st.EquipClass && s.EquipInfo == st.EquipInfo && s.ProjSn == st.ProjSn && s.Status == 3).Sum(r => r.AppNum))?? st.EquipNum//库存
            });
            if (objs.Createdtime_Start != null)
                query = query.Where(a => a.Stocktime >= objs.Createdtime_Start);
            if (objs.Createdtime_End != null)
            {
                var dt = Convert.ToDateTime(objs.Createdtime_End).AddDays(1);
                query = query.Where(a => a.Stocktime <= dt);
            }
            if (objs.EquipClass != null)
                query = query.Where(a => objs.EquipClass.Contains(a.EquipClass));
            if (objs.LableNo != null)
                query = query.Where(a => a.LableNo == objs.LableNo);
            if (objs.ProjSn != null)
                query = query.Where(a => objs.ProjSn.Contains(a.ProjSn));
            if (objs.StockType != null)
                query = query.Where(a => a.StockType == objs.StockType);
            count = query.Count();
            return query.OrderByDescending(a => a.EditTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();


        }

        /// <summary>
        /// 设备库存管理
        /// </summary>
        /// <param name="count"></param>
        /// <param name="objs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> EquipStockNum(out int count, EquipAppQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
         

            var sql = @"select s.* ,b.ProjName,
t.enternum,t.totalnum from
(select EquipClass, EquipInfo, ProjSn, StockType from Stock group by EquipClass,
                                          EquipInfo, ProjSn, StockType)s
left join Proj b on s.ProjSn = b.ProjSn  join
(select EquipClass,EquipInfo,ProjSn,SUM(EquipNum) as enternum,SUM(AppNum) as outnum
, SUM(EquipNum)-SUM(AppNum) as totalnum
from (
select EquipClass,EquipInfo,ProjSn,EquipNum,0 as AppNum  from Stock
union all
select EquipClass,EquipInfo,ProjSn,0 as EquipNum,AppNum  from EquipApp 
) t group by EquipClass,EquipInfo,ProjSn) t on s.EquipClass=t.EquipClass 
and s.EquipInfo=t.EquipInfo and s.ProjSn=t.ProjSn";

            SqlParameter[] parameters = {
         new SqlParameter("@EquipClass",""),
         new SqlParameter("@ProjSn",""),
         new SqlParameter("@EquipInfo",""),
         new SqlParameter("@StockType", "")};

           
            string where = " where 1=1 ";
            if (objs.EquipClass != null)
            {
                parameters[0].Value = objs.EquipClass;
                where += " and s.EquipClass=@EquipClass";
               
            }
            if (objs.ProjSn != null)
            {
                parameters[1].Value = objs.ProjSn;
                where += " and s.ProjSn  in ('@ProjSn')";
              
            }
            if (objs.EquipInfo != null)
            {
                parameters[2].Value = objs.EquipInfo;
                where += " and s.EquipInfo=@EquipInfo";
               
            }
            if (objs.StockType != null)
            {
                parameters[3].Value = objs.StockType;
                where += " and s.StockType=@StockType";
           
            }
            if (objs.StockStatus != null)
            {
                if (objs.StockStatus == 1)//正常
                    where += " and t.totalnum>0";
                if (objs.StockStatus == 0)//归档
                    where += " and t.totalnum=0";
            }
            sql += where;
           // var list = this._EquipApp.ExecuteResT<OutStock>(sql,parameters).ToList();
            var list = new EfRepository<OutStock>().ExecuteResT<OutStock>(sql, parameters).ToList();

            count = list.Count();
            return list.OrderByDescending(a => a.EditTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<dynamic> Outmatinfo(out int count, List<Stock> stock, OutstockQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var EquipClass = "";
            var EquipInfo = "";
            var ProjSn = "";
            foreach (var item in stock)
            {
                EquipClass += item.EquipClass + ",";
                EquipInfo += item.EquipInfo + ",";
                ProjSn += item.ProjSn + ",";
            }
            var query = from a in this._EquipApp.Table
                        group a by new
                        {
                            a.ApplyId,
                            a.ApplySn,
                            a.ApplyName,
                            a.EditTime,
                            a.EquipClass,
                            a.EquipInfo,
                            a.ProjSn,
                            a.AssetSn,
                            a.Deptname
                        } into g where EquipClass.Contains(g.Key.EquipClass) && EquipInfo.Contains(g.Key.EquipInfo) && ProjSn.Contains(g.Key.ProjSn)
                        select new
                        {
                            g.Key.ApplyId,
                            g.Key.ApplySn,
                            g.Key.ApplyName,
                            g.Key.EditTime,
                            g.Key.EquipClass,
                            g.Key.EquipInfo,
                            g.Key.ProjSn,
                            g.Key.AssetSn,
                            g.Key.Deptname,
                            outnum = g.Count()
                        };
            if (objs.Createdtime_Start != null)
                query = query.Where(a => a.EditTime >= objs.Createdtime_Start);
            if (objs.Createdtime_End != null)
            {
               var dt= Convert.ToDateTime(objs.Createdtime_End).AddDays(1);
                query = query.Where(a => a.EditTime <= dt);
            }
            if (objs.ApplySn != null)
                query = query.Where(a => a.ApplySn == objs.ApplySn);
            if (objs.ApplyName != null)
                query = query.Where(a => a.ApplyName == objs.ApplyName);
            count = query.ToList().Count;
            return query.OrderByDescending(a => a.EditTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();

        }

        public IEnumerable<dynamic> InStockinfo(out int count, List<Stock> stock, StockQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var EquipClass = "";
            var EquipInfo = "";
            var ProjSn = "";
            foreach (var item in stock)
            {
                EquipClass += item.EquipClass + ",";
                EquipInfo += item.EquipInfo + ",";
                ProjSn += item.ProjSn + ",";
            }
            var query = from a in this._Stock.Table
                        where EquipClass.Contains(a.EquipClass) && EquipInfo.Contains(a.EquipInfo) && ProjSn.Contains(a.ProjSn)
                        select new
                        {
                            a.Id,
                            a.LableNo,
                            a.StockType,
                            a.EditTime,
                            a.EquipClass,
                            a.EquipInfo,
                            a.ProjSn,
                            a.StockTime,
                            a.EquipNum,
                            a.Content
                        };
            if (objs.Createdtime_Start != null)
                query = query.Where(a => a.StockTime >= objs.Createdtime_Start);
            if (objs.Createdtime_End != null)
            {
                var dt = Convert.ToDateTime(objs.Createdtime_End).AddDays(1);
                query = query.Where(a => a.StockTime <= dt);
            }
            if (objs.LableNo != null)
                query = query.Where(a => a.LableNo == objs.LableNo);
            if (objs.StockType != null)
                query = query.Where(a => a.StockType == objs.StockType);
            count = query.ToList().Count;
            return query.OrderByDescending(a => a.StockTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();

        }

        public IEnumerable<dynamic> EquipAsset(out int count, EquipAssetQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
           
            var sql = @"select EA.ApplyId,EA.EquipClass,EA.EquipInfo,EA.ProjSn,EA.AssetSn, EA.Deptname,EA.ApplyName,EA.ApplyReason,EA.BUsername,EA.ApplyTime,
t.enternum,EA.AppNum AS outnum,t.totalnum from EquipApp EA  join
(select EquipClass,EquipInfo,ProjSn,SUM(EquipNum) as enternum,SUM(AppNum) as outnum
, SUM(EquipNum)-SUM(AppNum) as totalnum
from (
select EquipClass,EquipInfo,ProjSn,EquipNum,0 as AppNum  from Stock
union all
select EquipClass,EquipInfo,ProjSn,0 as EquipNum,AppNum  from EquipApp WHERE Status=3
) t group by EquipClass,EquipInfo,ProjSn)t on  EA.EquipClass=t.EquipClass 
and ea.EquipInfo=t.EquipInfo and EA.ProjSn=t.ProjSn and ea.Status=3";

            SqlParameter[] parameters = {
            new SqlParameter("@EquipClass",""),
            new SqlParameter("@AssetSn",""),
            new SqlParameter("@EquipInfo",""),
            new SqlParameter("@StockType", "")};
            string where = " ";
            if (objs.EquipClass != null)
            {
                parameters[0].Value = objs.EquipClass;
                where += " and EA.EquipClass like '%@EquipClass%'";

            }
            if (objs.AssetSn != null)
            {
                parameters[1].Value = objs.ProjSn;
                where += " and EA.AssetSn like '%@AssetSn%'";

            }
            if (objs.EquipInfo != null)
            {
                parameters[2].Value = objs.EquipInfo;
                where += " and EA.EquipInfo=@EquipInfo";

            }
            sql += where;
            var list = this._EquipApp.ExecuteResT<EquipStcock>(sql, parameters).ToList();
            count = list.Count();
            return list.OrderByDescending(a => a.EditTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public dynamic ImportStock(Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                int row = 1;
                string error = "";
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<Stock> list = new List<Stock>();
                List<string> tip = new List<string>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
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
                var result = new
                {
                    IsSuccess = IsSuccess,
                    Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：" + error
                };
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
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        Stock GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            Stock temp = new Stock();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("标签号不能为空");
            else
                temp.LableNo = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("设备类型不能为空");
            else
                temp.EquipClass = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("设备型号不能为空");
            else
                temp.EquipInfo = _temp[2]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("投资项目编号不能为空");
            else
                temp.ProjSn = _temp[3]?.ToString().Trim();
            int ApplyCount = 1;
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("数量不能为空");
            else if (!int.TryParse(_temp[4]?.ToString().Trim(), out ApplyCount))
            {
                tip.Add("数量为正整数");
            }
            else
                temp.EquipNum = ApplyCount;
            int type = 1;
            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("资产类型不能为空");
            else if (!int.TryParse(_temp[5]?.ToString().Trim(), out type))
            {
                tip.Add("资产类型为正整数");
            }
            else
                temp.StockType = type;
            if (tip.Count > 0)
                return null;
            temp.EditTime = DateTime.Now;
            temp.StockTime = DateTime.Now;
            return temp;
        }
    }
}
