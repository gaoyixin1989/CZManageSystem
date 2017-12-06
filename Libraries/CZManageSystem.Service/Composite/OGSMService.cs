using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Core.Caching;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using System.Linq.Expressions;
using System.IO;
using System.Data;
using Aspose.Cells;
using CZManageSystem.Data.Domain.SysManger;
using System.Collections.Generic;

namespace CZManageSystem.Service.Composite
{
    public partial class OGSMService : BaseService<OGSM>, IOGSMService
    {
        static Users _user;
        static List<OGSM> listDic = new List<OGSM>();
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.Id) : this._entityStore.Table.OrderBy(c => c.Id).Where(ExpressionFactory(objs));
            PagedList<OGSM> pageList = new PagedList<OGSM>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList.Select(u => new
            {
                u.Id,
                u.Group_Name,
                u.Town,
                u.USR_NBR,
                u.PowerStation,
                u.BaseStation,
                u.PowerType,
                u.PropertyRight,
                u.Property,
                u.IsRemove,
                RemoveTime= u.RemoveTime.HasValue ? Convert.ToDateTime(u.RemoveTime).ToString("yyyy-MM-dd") : "",
                u.Price,
                u.LinkMan,
                u.IsShare,
                ContractStartTime = u.ContractStartTime.HasValue?Convert.ToDateTime(u.ContractStartTime).ToString("yyyy-MM-dd"):"",
                ContractEndTime = u.ContractEndTime.HasValue?Convert.ToDateTime( u.ContractEndTime).ToString("yyyy-MM-dd"):""
            });
        }        
        public IList<OGSM> GetForData(out int count, OGSMQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var curTable = this._entityStore.Table;
                if (objs.BaseStation != null && objs.BaseStation.Length > 0)
                {
                    curTable = curTable.Where(u => objs.BaseStation.Contains(u.BaseStation));
                }
                if (objs.Group_Name != null && objs.Group_Name.Length > 0)
                {
                    curTable = curTable.Where(u => objs.Group_Name.Contains(u.Group_Name));
                }
                if (objs.PropertyRight != null && objs.PropertyRight.Length > 0)
                {
                    curTable = curTable.Where(u => objs.PropertyRight.Contains(u.PropertyRight));
                }
                if (objs.USR_NBR != null && objs.USR_NBR != "")
                {
                    curTable = curTable.Where(u => u.USR_NBR.Contains(objs.USR_NBR));
                }
                if (objs.PowerType != null && objs.PowerType != "")
                {
                    curTable = curTable.Where(u => u.PowerType == objs.PowerType);
                }
                if (objs.IsShare != null && objs.IsShare != "")
                {
                    curTable = curTable.Where(u => u.IsShare == objs.IsShare);
                }
                if (objs.IsRemove != null && objs.IsRemove != "")
                {
                    curTable = curTable.Where(u => u.IsRemove == objs.IsRemove);
                }
                if (objs.ContractEndTime_Start != null && objs.ContractEndTime_End != null)
                {
                    curTable = curTable.Where(u => u.ContractEndTime >= objs.ContractEndTime_Start && u.ContractEndTime <= objs.ContractEndTime_End);
                }
                PagedList<OGSM> pageList = new PagedList<OGSM>(curTable.OrderBy(p => p.Id), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);

                return pageList.Select(u => new OGSM()
                {
                    Id = u.Id,
                    Group_Name = u.Group_Name,
                    Town = u.Town,
                    USR_NBR = u.USR_NBR,
                    PowerStation = u.PowerStation,
                    BaseStation = u.BaseStation,
                    PowerType = u.PowerType,
                    PropertyRight = u.PropertyRight,
                    Property = u.Property,
                    IsRemove = u.IsRemove,
                    RemoveTime = u.RemoveTime,
                    //u.RemoveTime.HasValue ? Convert.ToDateTime(u.RemoveTime).ToString("yyyy-MM-dd") : "",
                    Price = u.Price,
                    LinkMan = u.LinkMan,
                    IsShare = u.IsShare,
                    ContractStartTime = u.ContractStartTime,
                    ContractEndTime = u.ContractEndTime
                }).ToList();
            }
            catch(Exception ex)
            {
                throw;
            }

            
        }
        public IList<object> GetBaseStationListForPagingByCondition(out int count, string BaseStation = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var curData = from hrvc in this._entityStore.Table
                          select new
                          {
                              hrvc.BaseStation
                          };
            curData = curData.Distinct();
            if (BaseStation != null && BaseStation != "")
                curData = curData.Where(u => u.BaseStation.Contains(BaseStation));

            PagedList<object> pageList = new PagedList<object>(curData.OrderBy(p => ""), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = curData.Count();
            return pageList.ToList();
        }
        public dynamic ImportOGSMBase(Stream fileStream, Users user)
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
                listDic = this._entityStore.Table.ToList() as List<OGSM>;
                List<OGSM> list = new List<OGSM>();
                List<OGSM> listupdate = new List<OGSM>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    bool update = false;
                    var model = GetModel(item,out tip,out update);
                    if (model == null)
                    {
                        error += "第" + row + "行:" + string.Join(";", tip);
                        continue;
                    }
                    if(update)
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

        OGSM GetModel(DataRow dataRow, out List<string> tip,out bool update)
        {
            //验证数据是否合法 
            tip = new List<string>();
            DateTime? TimeNull = null;
            int? intnull = null;
            var _temp = dataRow.ItemArray;
            OGSM dic = new OGSM();
            OGSM temp = new OGSM();    


            dic = listDic.Find(f => f.USR_NBR == _temp[2]?.ToString().Trim());
            if (dic != null)
            {
                //tip.Add("户号存在；");
                var usr_nbr = _temp[2].ToString();
                temp = FindByFeldName(u => u.USR_NBR == usr_nbr);
                update = true;
            }               
            else
            {
                temp.USR_NBR = _temp[2].ToString();
                update = false;
            }
                

            if (tip.Count > 0)
            {
                update = false;
                return null;
            }
                

            temp.Group_Name = _temp[0].ToString();
            temp.Town = _temp[1].ToString();
            temp.PowerStation = _temp[3].ToString();
            temp.BaseStation = _temp[4].ToString();
            temp.PowerType = _temp[5].ToString();
            temp.PropertyRight = _temp[6].ToString();
            temp.IsRemove = _temp[7].ToString();
            var tmp = _temp[8]?.ToString();
            temp.RemoveTime = string.IsNullOrEmpty(_temp[8].ToString()) ? TimeNull : Convert.ToDateTime(_temp[8]);
            temp.IsShare = _temp[9].ToString();
            temp.ContractStartTime= string.IsNullOrEmpty(_temp[10].ToString()) ? TimeNull : Convert.ToDateTime(_temp[10]);
            temp.ContractEndTime= string.IsNullOrEmpty(_temp[11].ToString()) ? TimeNull : Convert.ToDateTime(_temp[11]);
            temp.Address= _temp[12].ToString();
            temp.Price= _temp[13].ToString();
            temp.PAY_CYC= string.IsNullOrEmpty(_temp[14]?.ToString()) ? intnull : Convert.ToInt32(_temp[14].ToString());
            temp.Property = _temp[15].ToString();
            temp.LinkMan = _temp[16].ToString();
            temp.Mobile = _temp[17].ToString();
            temp.IsWarn = _temp[18].ToString();
            temp.WarnCount= string.IsNullOrEmpty(_temp[19]?.ToString()) ? intnull : Convert.ToInt32(_temp[19]);
            temp.Remark = _temp[20].ToString();
            temp.IsNew = "0";
            temp.AttachmentId = Guid.NewGuid();
            return temp;
        }


        }
}
