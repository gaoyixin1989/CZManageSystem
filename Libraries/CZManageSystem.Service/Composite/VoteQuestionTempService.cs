using Aspose.Cells;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Composite
{
    public class VoteQuestionTempService : BaseService<VoteQuestionTemp>, IVoteQuestionTempService
    {
        static Users _user;
        public dynamic ImportQuestion(Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<VoteQuestionTemp> list = new List<VoteQuestionTemp>();
                List<VoteQuestionTemp> Questions = new List<VoteQuestionTemp>();

                foreach (DataRow item in dataTable.Rows)
                {
                    var model = GetQuestionModel(item);
                    if (model == null)
                        continue;
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
                // DataTable dt = excelop.ConvertToDataTable();
                int falCount = dataTable.Rows.Count - count;
                //result.Message = "导入成功";
                var result = new { IsSuccess = IsSuccess, Message = "成功导入" + count + "条，失败" + falCount + "条"
                    //, Questions = Questions
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
               
                DataTable dt = new DataTable("Workbook");
                DataColumnCollection columns = dt.Columns;
                for (int i = 0; i < cells.MaxDataColumn + 1; i++)
                    columns.Add(i.ToString (), typeof(System.String ));
                DataRow datarow = dt.NewRow();
                for (int i = 0; i < cells.MaxDataColumn + 1; i++) 
                    datarow[i] = i; 
               
                cells.ImportDataRow (datarow,1, 0);
                return   cells.ExportDataTableAsString(1, 0, cells.MaxDataRow, cells.MaxDataColumn+1, true);
                
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        VoteQuestionTemp GetQuestionModel(DataRow dataRow)
        {
            var question = dataRow.ItemArray;
            #region 增加
            if (string.IsNullOrEmpty(question[1]?.ToString()))
                return null;
            VoteQuestionTemp voteQuestionTemp = new VoteQuestionTemp();
            voteQuestionTemp.QuestionTitle = question[1]?.ToString();//题目
            voteQuestionTemp.CreatorID = _user?.UserId;
            voteQuestionTemp.Creator = _user?.RealName;
            voteQuestionTemp.AnswerNum = string.IsNullOrEmpty(question[3]?.ToString()) ? 0 : Convert .ToInt32 ( question[3]);//最多选择_个（注：仅限多选题，0为不限）
            voteQuestionTemp.SortOrder = string.IsNullOrEmpty(question[0]?.ToString()) ? 0 : Convert.ToInt32(question[0]);
            voteQuestionTemp.CreateTime = DateTime.Now;//时间
            voteQuestionTemp.QuestionType = string.IsNullOrEmpty(question[2]?.ToString()) ? "S" : GetQuestionType(question[2]?.ToString());//类型
            if (voteQuestionTemp.QuestionType == "Q") //为简答则直接返回
                return voteQuestionTemp;
            int sortOrder = 0;
            for (int i = 4; i < question.Length; i++)
            {
                if (string.IsNullOrEmpty(question[i]?.ToString()))//内容为空的直接跳过/中断
                    break;//continue;
                voteQuestionTemp.VoteAnserTemps.Add(
                    new VoteAnserTemp()
                    {
                        AnserContent = question[i].ToString(),
                        AnserScore = Convert.ToInt32(question[++i]),
                        MaxValue = 0,
                        MinValue = 0,
                        SortOrder = sortOrder
                    });
                sortOrder++;
            }
            #endregion
            return voteQuestionTemp;
        }
        string GetQuestionType(string type)
        {
            string _type = "S";
            switch (type)
            {
                case "单选":
                    _type = "S";
                    break;
                case "多选":
                    _type = "M";
                    break;
                case "简答":
                    _type = "Q";
                    break;
                default:
                    break;
            }
            return _type;
        }
    }
}
