﻿using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.ITSupport
{
    public class ProjService : BaseService<Proj>, IProjService
    {
        static Users _user;
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
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.EditTime) : this._entityStore.Table.OrderBy(c => c.EditTime).Where(ExpressionFactory(objs));
            PagedList<Proj> pageList = new PagedList<Proj>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList;
        }
      /// <summary>
      /// 批量导入
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="user"></param>
      /// <returns></returns>
        public dynamic ImportProj(Stream fileStream, Users user)
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

                List<Proj> list = new List<Proj>();
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

        Proj GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            Proj temp = new Proj();
            //temp.ID = Guid.NewGuid();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("项目编号不能为空");
            else
                temp.ProjSn = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("项目名称不能为空");
            else
                temp.ProjName = _temp[1]?.ToString().Trim();
            //temp.ProjSn = _temp[0].ToString();
            //temp.ProjName = _temp[1].ToString();
            if (tip.Count > 0)
                return null;
            temp.EditTime = DateTime.Now;
            temp.Editor = _user.UserName;
            return temp;
        }
    }
}
