using Aspose.Cells;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative
{
    public class BoardroomInfoService : BaseService<BoardroomInfo>, IBoardroomInfoService
    {

        static Users _user;

        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
        ISysUserService _sysUserService = new SysUserService();

        static List<DataDictionary> listDic = new List<DataDictionary>();
        public IList<BoardroomInfo> GetForPaging(out int count, BoardroomInfoQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = GetQueryTable(objs);
            count = curTable.Count();
            var list = curTable.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();
        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<BoardroomInfo> GetQueryTable(BoardroomInfoQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                BoardroomInfoQueryBuilder obj2 = (BoardroomInfoQueryBuilder)CloneObject(obj);
                if (obj.BoardroomID != null && obj.BoardroomID.Length > 0)
                {//会议室
                    curTable = curTable.Where(u => obj.BoardroomID.Contains(u.BoardroomID));
                    obj2.BoardroomID = null;
                }
                if (obj.State != null && obj.State.Length > 0)
                {//状态
                    curTable = curTable.Where(u => obj.State.Contains(u.State));
                    obj2.State = null;
                }
                if ((obj.MaxMan_min ?? 0) > 0)
                {//最大人数_下限
                    curTable = curTable.Where(u => u.MaxMan.HasValue && u.MaxMan.Value >= obj.MaxMan_min.Value);
                    obj2.MaxMan_min = null;
                }
                if ((obj.MaxMan_max ?? 0) > 0)
                {//最大人数_上限
                    curTable = curTable.Where(u => (u.MaxMan ?? 0) <= obj.MaxMan_max.Value);
                    obj2.MaxMan_max = null;
                }

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }


        public dynamic ImportBoardroomInfo(Stream fileStream, Users user)
        {
            try
            {

                bool IsSuccess = false;
                int count = 0;
                _user = user;
                string DDNames = "会议室设备,所属单位";
                string error = "";
                int row = 2;
                listDic = _dataDictionaryService.List().Where(w => DDNames.Contains(w.DDName)).ToList() as List<DataDictionary>;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<BoardroomInfo> list = new List<BoardroomInfo>();
                List<BoardroomInfo> Questions = new List<BoardroomInfo>();

                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    var model = GetBoardroomInfoModel(item, out tip);
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
                            Questions.AddRange(list);
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
                        Questions.AddRange(list);
                        count += list.Count;
                        list.Clear();
                        IsSuccess = true;
                    };
                }
                int falCount = dataTable.Rows.Count - count;
                //result.Message = "导入成功";
                var result = new
                {
                    IsSuccess = IsSuccess,
                    Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：" + error
                    //, Questions = Questions
                };
                return result;
            }
            catch (Exception ex)
            {
                return new { IsSuccess = false, Message = "文件内容错误！" };
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
                return cells.ExportDataTableAsString(1, 0, cells.MaxDataRow, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 会议室资料信息数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        BoardroomInfo GetBoardroomInfoModel(DataRow dataRow, out List<string> tip)
        {
            var dataObj = dataRow.ItemArray;
            DataDictionary dic = new DataDictionary();



            //验证数据是否合法 
            tip = new List<string>();
            BoardroomInfo boardroomInfo = new BoardroomInfo();
            //是否验证通过

            if (string.IsNullOrEmpty(dataObj[1]?.ToString().Trim()))
                tip.Add("所属单位不能为空");
            else
            {
                dic = listDic.Find(f => f.DDText == dataObj[1]?.ToString().Trim());
                if (dic == null)
                    tip.Add("所属单位不存在");
                else
                    boardroomInfo.CorpID = Convert.ToInt32(dic.DDValue);
            }
            
            if (string.IsNullOrEmpty(dataObj[2]?.ToString().Trim()))
                tip.Add("会议室名称不能为空");
            else
                boardroomInfo.Name = dataObj[2]?.ToString().Trim();

            if (string.IsNullOrEmpty(dataObj[3]?.ToString().Trim()))
                tip.Add("会议室地点不能为空");
            else
                boardroomInfo.Address = dataObj[3]?.ToString().Trim();

            int maxMan = 1;
            if (string.IsNullOrEmpty(dataObj[4]?.ToString().Trim()))
                tip.Add("最大人数不能为空");
            else if (!int.TryParse(dataObj[4]?.ToString().Trim(), out maxMan))
            {
                tip.Add("最大人数为正整数");
            }
            else
                boardroomInfo.MaxMan = maxMan;

            if (string.IsNullOrEmpty(dataObj[5].ToString().Trim()))
                tip.Add("会议室设备不能为空");
            else
            {
                var result = "";
                var strS = dataObj[5].ToString().Trim().Split(new char[] { ',', '，' });
                foreach (var item in strS)
                {
                    dic = listDic.Find(f => f.DDText == item.Trim());

                    if (dic == null)
                        tip.Add("会议室设备[" + item + "]不存在");
                    else
                        result += item + ",";
                }
                if (result.Substring(result.Length - 2, 1) == ",")
                {
                    result = result.Substring(0, result.Length - 1);
                }
                boardroomInfo.Equip = result;

            }        

            if (string.IsNullOrEmpty(dataObj[6].ToString().Trim()))
                tip.Add("管理单位不能为空");
            else
            {
                var depts = dataObj[6].ToString().Trim();
                var deptsList = _sysDeptmentService.List().Where(w => depts.Contains(w.DpName)).Select(s => s.DpId).ToList();
                if (deptsList.Count <= 0)
                    tip.Add("管理单位不存在");
                else
                    boardroomInfo.ManagerUnit = string.Join(",", deptsList);
            }

            if (string.IsNullOrEmpty(dataObj[7].ToString().Trim()))
                tip.Add("管理人不能为空");
            else
            {
                var depts = dataObj[7].ToString().Trim();
                var deptsList = _sysUserService.List().Where(w => depts.Contains(w.RealName)).Select(s => s.UserId);
                if (deptsList.Count() < 0)
                    tip.Add("管理人不存在");
                else
                    boardroomInfo.ManagerPerson = string.Join(",", deptsList);
            }


            if (string.IsNullOrEmpty(dataObj[8].ToString().Trim()))
                tip.Add("请选择正确的状态");
            else if (dataObj[8].ToString().Trim() != "在用" && dataObj[8].ToString().Trim() != "停用")
                tip.Add("请选择正确的状态");
            else
                boardroomInfo.State = dataObj[8].ToString().Trim();

            //DateTime star = DateTime.Now;
            //if (!DateTime.TryParse(dataObj[9]?.ToString().Trim(), out star))
            //{
            //    tip.Add("使用开始时间为时间类型");
            //}
            //else
            //    boardroomInfo.StartTime = star;
            //DateTime end = DateTime.Now;
            //if (!DateTime.TryParse(dataObj[10]?.ToString().Trim(), out end))
            //{
            //    tip.Add("使用结束时间为时间类型");
            //}
            //else
            //    boardroomInfo.EndTime = end;


            if (tip.Count > 0)
                return null;
            boardroomInfo.Editor = _user.RealName;
            boardroomInfo.EditTime = DateTime.Now;
            boardroomInfo.Remark = dataObj[9].ToString().Trim();


            return boardroomInfo;
        }


    }
}
