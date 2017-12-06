using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Extension.IBatisNet;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Reflection;
using Botwave.Security;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text.RegularExpressions;
using Botwave.XQP.Domain;
using Botwave.XQP.Util;
using System.Web;
using Botwave.XQP.Commons;

namespace Botwave.XQP.Service.Support
{
    public class Ucs_ReportformsService : IUcs_ReportformsService
    {

        public Ucs_Reportforms GetReprotformsByid(Guid id, Guid userid)
        {
            if (id == Guid.Empty)
            {
                return new Ucs_Reportforms();
            }

            var item = IBatisMapper.Select<Ucs_Reportforms>("Get_ReportformsById", id).First();
            item.FieldList = IBatisMapper.Select<UCS_FromField>("Get_FromFieldById", item.id).ToList();
            return item;
        }


        public string GetTableHtml(System.Data.DataTable dt, int? type, string fieldtext, Ucs_Reportforms model, string LVL, string str)
        {
            // DataTable dt = new DataTable();
            //  MergeDataTable
            //   dt.Merge()

            if (string.IsNullOrEmpty(fieldtext))
            {
                return null;
            }
            StringBuilder result = new StringBuilder();
            //switch (type)
            //{
            //    case 1: result.Append(GetContext1(dt, fieldtext.Split(','), model, LVL)); break;
            //    case 2: result.Append(GetContext2(dt, fieldtext.Split(','), model, LVL)); break;
            //    case 3:
            //    case 4:
            //    case 5: result.Append(GetContext3(dt, fieldtext.Split(','), model, LVL)); break;
            //    case 6: result.Append(GetContext2(dt, fieldtext.Split(','), model, LVL)); break;
            //    default: result.Append(GetContext2(dt, fieldtext.Split(','), model, LVL)); break;
            //}
            result.Append(GetContext2(dt, fieldtext.Split(','), model, LVL));
            return result.ToString();
        }

        public void InsertForm(Ucs_Reportforms model, NameValueCollection from)
        {
            model.id = Guid.NewGuid();
            IBatisMapper.Insert("Reportforms_insert", model);
            //  model.id = i;
            UpdateFromField(model, from);
        }
        public void UpdateForm(Ucs_Reportforms model, NameValueCollection from)
        {

            IBatisMapper.Update("Reportforms_update", model);
            UpdateFromField(model, from);
        }
        void UpdateFromField(Ucs_Reportforms model, NameValueCollection from)
        {
            //新增的字段
            if (!string.IsNullOrEmpty(from["add_1" + "_field"]))
            {
                int i = 1;
                var str2 = from.AllKeys.Where(x => x.StartsWith("add_") && x.Contains("_field") && !x.Contains("_fieldname") && !x.Contains("_fieldorder"));
                foreach (var str in str2)
                {
                    if (string.IsNullOrEmpty(from[str])) { break; }
                    int ind = str.IndexOf("_", 5);
                    FromFieldUPdate(str.Substring(0, ind), from, model);
                    i++;
                }
            }
            if (model.FieldList != null)
            {
                foreach (UCS_FromField item in model.FieldList)
                {
                    FromFieldUPdate(item.id.ToString(), from, model);
                }
            }
        }

        void FromFieldUPdate(string guid, NameValueCollection from, Ucs_Reportforms model)
        {
            if (!string.IsNullOrEmpty(from[guid + "_field"]))
            {
                Type t = typeof(UCS_FromField);
                UCS_FromField field = (UCS_FromField)Activator.CreateInstance(t);// new Student()
                field.ReportformsID = model.id;
                Guid gid;
                gid = new Guid(guid.StartsWith("add_")?Guid.Empty.ToString():guid);
                field.id = gid;

                PropertyInfo[] properties = t.GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    if (!string.IsNullOrEmpty(from[guid + "_" + p.Name]))
                    {

                        string value = from[guid + "_" + p.Name];
                        //value =DataHelper.HtmlDecode(value);
                        value = XQPHelper.DecodeBase64("utf-8",value);
                        //value = value.Replace("?", " ");
                        decimal num = 0;


                        if (!p.PropertyType.IsValueType)
                        {
                            p.SetValue(field, value, null);
                        }
                        else
                        {
                            decimal.TryParse(value, out num);
                            object val = Convert.ChangeType(num, p.PropertyType);
                            p.SetValue(field, val, null);
                        }
                    }
                }
                //如果有预警更新字段
                if (field.EVA_ID > 0 && !field.field.Contains("dbo.fn_get_EVA_LVL"))
                {
                    field.field = "dbo.fn_get_EVA_LVL(" + field.EVA_Formula + "," + field.EVA_ID + ",TM_INTRVL_CD) as '" + Guid.NewGuid() + "'";
                    field.strWhere = "EWS_ID="+field.EVA_ID;
                }

                if (guid.Contains("add"))
                {
                    IBatisMapper.Insert("fromfield_insert", field);
                }
                else
                {
                    IBatisMapper.Update("fromfield_update", field);
                }

            }
            else
            {
                IBatisMapper.Delete("fromfield_del", guid);
            }
        }


        private void ExpendContext(Guid guid, StringBuilder sb, DataTable dt, string LVL, string str)
        {
            switch (guid.ToString())
            {

                case "910f8f2a-7e0b-460d-8ff2-cd3dfe9ad574":

                    if (false)
                    {
                        string where = "TM_INTRVL_CD=" + dt.Rows[0]["TM_INTRVL_CD"].ToString() + str;

                        var table = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format(@"select TM_INTRVL_CD,sum(INCOME1) as JTZTSR,
CONVERT(VARCHAR, CONVERT(DECIMAL(15,2), (case when SUM(INCOME2)>0 then sum(INCOME1-INCOME2)*100/SUM(INCOME2) else (case when SUM(INCOME1)>0 then 100 else 0 end) end)))+'%'  as TBZZ,CONVERT(VARCHAR, CONVERT(DECIMAL(15,2), (case when SUM(INCOME3)>0 then sum(INCOME1-INCOME3)*100/SUM(INCOME3) else (case when SUM(INCOME1)>0 then 100 else 0 end) end)))+'%'  as HBZZ,SUM(INCOME5) as JNLJSR,CONVERT(VARCHAR, CONVERT(DECIMAL(15,2), (case when SUM(INCOME7)>0 then sum(INCOME5-INCOME7)*100/SUM(INCOME7) else (case when SUM(INCOME5)>0 then 100 else 0 end) end)))+'%'  as LJTB,
sum(INCOME4) as PZSR,CONVERT(VARCHAR, CONVERT(DECIMAL(15,2), (case when SUM(INCOME4)>0 then sum(INCOME1)*100/SUM(INCOME4) else (case when SUM(INCOME1)>0 then 100 else 0 end) end)))+'%'  as PZSRBYL,dbo.fn_get_EVA_LVL(case when SUM(INCOME4)>0 then SUM(INCOME1)/sum(INCOME4)*100 else (case when SUM(INCOME1)>0 then 100 else 0 end) END,2,TM_INTRVL_CD) as 'tt'
from SHOW_GROUP_INCOME_STATISTICAL where  {0} group by TM_INTRVL_CD ", where)).Tables[0];
                        sb.Append("<tr><td>").Append(dt.Rows.Count + 1).Append("</td><td>").Append(table.Rows[0]["TM_INTRVL_CD"]).Append("</td><td>全市</td><td>").Append(table.Rows[0]["JTZTSR"]).Append("</td><td>").Append(table.Rows[0]["TBZZ"]).Append("</td><td>").Append(table.Rows[0]["HBZZ"]).Append("</td><td>").Append(table.Rows[0]["JNLJSR"]).Append("</td><td>").Append(table.Rows[0]["LJTB"]).Append("</td><td>").Append(table.Rows[0]["PZSR"]).Append("</td><td>").Append(table.Rows[0]["PZSRBYL"]).Append("</td><td>").Append(table.Rows[0]["tt"]).Append("</td></tr>");
                    }
                    break;
            }
        }
        decimal AmontColSum(DataTable dt, string colname)
        {


            decimal result = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                result += Convert.ToDecimal(dt.Rows[i][colname]);
            }
            return result;
        }


        string defaultArea = ConfigurationManager.AppSettings["DefaultArea"];
        string defaultBrnd = ConfigurationManager.AppSettings["DefaultBrnd"];
        /// <summary>
        /// 带数据分层的模板
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fieldtext"></param>
        /// <returns></returns>
        private string GetContext1(DataTable dt, string[] fieldtext, Ucs_Reportforms model, string lvl)
        {
            decimal d = 0;
            StringBuilder sb = new StringBuilder();
            
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("<tr><th>序号</th>");
                for (int i = 0; i < fieldtext.Length; i++)
                {
                    if (!fieldtext[i].StartsWith("thres_"))
                        sb.Append("<th>").Append(fieldtext[i]).Append("</th>");
                }
                sb.Append("</tr>");
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    sb.Append("<tr  style=\"text-align:center;\">");
                    foreach (DataColumn col in dt.Columns)
                    {
                        UCS_FromField field = new UCS_FromField();
                        if (col.ColumnName != "rowId")
                        {
                            field = model.FieldList.Where(x => x.field.Substring((x.field.ToLower().IndexOf(" as") > 0 ? x.field.ToLower().IndexOf(" as") : -3) + 3).Trim() == col.ColumnName.Trim()).First();
                            if (!field.IsShow) { continue; }
                        }
                        if (col.ColumnName.StartsWith("thres"))
                        {
                            sb.Append("<td style='display: none' thres='ture'>");
                        }
                        else
                        {
                            sb.Append("<td>");
                        }

                        if (col.ColumnName == "code")
                        {
                            sb.Append(@"<span style='cursor: pointer' class='span_link' onclick='get_data(this)' defaultarea='").Append(defaultArea).Append("'  no='").Append(row["MARK_PLAN_CD"]).Append("' by='1' cid='").Append(row[col].ToString() == defaultArea ? 1 : 2).Append("' bid='").Append(row["brand"]).Append("' aid='").Append(row[col].ToString()).Append("'>").Append(row[col].ToString()).Append("</span>");
                        }
                        else if (col.ColumnName == "brand" && row[col].ToString() == defaultBrnd)
                        {
                            sb.Append(@"<span style='cursor: pointer' class='span_link' onclick='get_data(this)' defaultarea='").Append(defaultArea).Append("'  no='").Append(row["MARK_PLAN_CD"]).Append("' by='2' cid='").Append(dt.Columns.Contains("code") ? 1 : 2).Append("' bid='").Append(dt.Columns.Contains("code") ? row["code"] : row["name"]).Append("' aid='").Append(row[col].ToString()).Append("'>").Append(row[col].ToString()).Append("</span>");
                        }
                        else
                        {
                            GetCommonHtml(col, row, sb, model, field, lvl);
                        }
                        sb.Append("</td>");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 直接显示的表格模板
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fieldtext"></param>
        /// <returns></returns>
        private string GetContext2(DataTable dt, string[] fieldtext, Ucs_Reportforms model, string lvl)
        {
            decimal d = 0;
            StringBuilder sb = new StringBuilder();
            
            if (dt != null && dt.Rows.Count > 0)
            {
                    sb.Append("<tr><th>序号</th>");
                    for (int i = 0; i < fieldtext.Length; i++)
                    {
                        if (!fieldtext[i].StartsWith("thres_"))
                            sb.Append("<th>").Append(fieldtext[i]).Append("</th>");
                    }
                    sb.Append("</tr>");
                #region 合计行统计
                if (model.Statistics)
                {
                    object[] foot = new object[dt.Columns.Count];
                    foot[0] = null;
                    int sindex = 0;
                    foreach (DataColumn col in dt.Columns)
                    {
                        UCS_FromField field = null;
                        Regex rx = new Regex(" as ");
                        var obj = model.FieldList.Where(x => (x.field.ToLower().IndexOf(" as") > 0 ? rx.Split(x.field.ToLower())[rx.Split(x.field.ToLower()).Length - 1] : x.field.ToLower()).Trim().Trim('\'') == col.ColumnName.ToLower().Trim());
                        if (obj.Count() > 0)
                            field = obj.First();
                        else
                            continue;
                        string name = rx.Split(field.field.ToLower())[rx.Split(field.field.ToLower()).Length - 1].Trim().Trim('\'');
                        if (col.ColumnName.ToLower().Trim() == name && model.strGroup.Contains(col.ColumnName))
                        {
                            foot[col.Ordinal] = "合计";
                            sindex++;
                        }
                        else if ( col.ColumnName.ToLower().Trim() == name && (col.DataType.Name == "Int32" || col.DataType.Name == "Decimal" || col.DataType.Equals(typeof(Double))))
                        {
                            object sumObject = null;
                            switch (model.FieldList[sindex].StatisticsType)
                            {
                                case 1:
                                    sumObject = dt.Compute("sum(" + col.ColumnName + ")", "TRUE");
                                    break;
                                case 2:
                                    sumObject = dt.Compute("avg(" + col.ColumnName + ")", "TRUE");
                                    break;
                                default:
                                    sumObject = dt.Compute("sum(" + col.ColumnName + ")", "TRUE");
                                    break;
                            }
                            foot[col.Ordinal] = model.FieldList[sindex].IsCount ? sumObject : null;
                            sindex++;
                        }
                        else if (col.ColumnName.ToLower().Trim() == name)
                            sindex++;
                    }

                    dt.Rows.Add(foot);
                }
                #endregion
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    if(Botwave.Commons.DbUtils.ToInt32(row["rowid"])%2==0)
                        sb.Append("<tr class=\"trClass\" style=\"text-align:center;\">");
                    else
                        sb.Append("<tr  style=\"text-align:center;\">");
                    foreach (DataColumn col in dt.Columns)
                    {
                        UCS_FromField field = new UCS_FromField();
                        if (col.ColumnName != "rowId")
                        {
                            //field = model.FieldList.Where(x => x.field.Substring((x.field.ToLower().IndexOf(" as") > 0 ? x.field.ToLower().IndexOf(" as") : -3) + 3).Trim() == col.ColumnName.Trim()).First();
                            //if (!field.IsShow) { continue; }
                            Regex rx = new Regex(" as ");
                            var obj = model.FieldList.Where(x => (x.field.ToLower().IndexOf(" as") > 0 ? rx.Split(x.field.ToLower())[rx.Split(x.field.ToLower()).Length - 1] : x.field.ToLower()).ToLower().Trim().Trim('\'') == col.ColumnName.ToLower().Trim());
                            if (obj.Count() > 0)
                                field = obj.First();
                            else
                            {
                                continue;
                            }
                            if (!field.IsShow) { continue; }
                        }
                            sb.Append("<td>");
                        GetCommonHtml(col, row, sb, model, field, lvl);

                        sb.Append("</td>");
                    }
                }
            }
            else
            {
                sb.Append("<tr><th>序号</th></tr><tr><td style='text-align:left; font-weight:bold'> 暂无查询结果！！！</td></tr>");
            }
            return sb.ToString();
        }

        private string GetContext3(DataTable dt, string[] fieldtext, Ucs_Reportforms model, string Lvl)
        {
            string nextLvl = GetNextLvL(Lvl.Split(',')[0], model.datasource);
            decimal d = 0;
            StringBuilder sb = new StringBuilder();
            
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("<tr><th>序号</th>");
                model.GetFromHead(fieldtext, model, sb);
                sb.Append("</tr>");
                #region 合计行统计
                if (model.Statistics)
                {
                    object[] foot = new object[dt.Columns.Count];
                    foot[0] = null;
                    int sindex = 0;
                    foreach (DataColumn col in dt.Columns)
                    {
                        UCS_FromField field = null;
                        Regex rx = new Regex(" as ");
                        if (!(col.ColumnName == "rowId"))
                        {
                            var obj = model.FieldList.Where(x => (x.field.ToLower().IndexOf(" as") > 0 ? rx.Split(x.field.ToLower())[rx.Split(x.field.ToLower()).Length - 1] : x.field.ToLower()).Trim().Trim('\'') == col.ColumnName.ToLower().Trim());
                            if (obj.Count() > 0)
                            {
                                field = obj.First();
                            }
                            else
                                continue;
                        }
                        string name = field == null ? string.Empty : rx.Split(field.field.ToLower())[rx.Split(field.field.ToLower()).Length - 1].Trim().Trim('\'');
                        if (col.ColumnName.ToLower() != "rowid")
                        {
                            foot[col.Ordinal] = "合计";
                            if (!col.ColumnName.ToUpper().Contains("HIDE"))
                                sindex++;
                        }
                        else if (col.ColumnName.ToLower().Trim() == name && (col.DataType.Name == "Int32" || col.DataType.Name == "Decimal" || col.DataType.Equals(typeof(Double))))
                        {
                            object sumObject = null;
                            switch (model.FieldList[sindex].StatisticsType)
                            {
                                case 1:
                                    sumObject = dt.Compute("sum(" + col.ColumnName + ")", "TRUE");
                                    break;
                                case 2:
                                    sumObject = dt.Compute("avg(" + col.ColumnName + ")", "TRUE");
                                    break;
                                default:
                                    sumObject = dt.Compute("sum(" + col.ColumnName + ")", "TRUE");
                                    break;
                            }
                            foot[col.Ordinal] = model.FieldList[sindex].IsCount ? sumObject : null;
                            sindex++;
                        }
                        else if (col.ColumnName.ToLower().Trim() == name)
                            sindex++;
                        if (sindex == model.FieldList.Count)
                            break ;
                    }

                    dt.Rows.Add(foot);
                }
                #endregion
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn col in dt.Columns)
                    {
                        UCS_FromField field = null;
                        if (!(col.ColumnName == "rowId"))
                        {
                            Regex rx = new Regex(" as ");
                            var obj = model.FieldList.Where(x => (x.field.ToLower().IndexOf(" as") > 0 ? rx.Split(x.field.ToLower())[rx.Split(x.field.ToLower()).Length - 1] : x.field.ToLower()).ToLower().Trim().Trim('\'') == col.ColumnName.ToLower().Trim());
                            if (obj.Count() > 0)
                                field = obj.First();
                            else
                            {
                                continue;
                            }
                            if (!field.IsShow) { continue; }
                        }
                        if (col.ColumnName.StartsWith("thres"))
                        {
                            sb.Append("<td style='display: none' thres='ture'>");
                        }
                        else if (model.groupbystr.Contains(col.ColumnName))
                        {

                            sb.Append("<td colname='").Append(col.ColumnName).Append("' colval='").Append(row[col]).Append("'/>");
                        }
                        else
                        {
                            sb.Append("<td>");
                        }
                        if (col.ColumnName.ToUpper().StartsWith("GROUP_VEST_LVL_") && !string.IsNullOrEmpty(nextLvl) && (!dt.Columns.Contains("type_lvl_2") || !string.IsNullOrEmpty(row["type_lvl_2"].ToString())))
                        {
                            string[] prelvl = Lvl.Split(',');
                            string colname = col.ColumnName;
                            if (dt.Columns.Contains("HIDE" + col.ColumnName.Substring(5) + "_CD"))
                            {
                                colname = "HIDE" + col.ColumnName.Substring(5) + "_CD";
                            }
                            if (row[col].ToString().Contains("合计") || row[col].ToString().Contains("全市"))
                            {
                                sb.Append(row[col]);
                            }
                            else
                            {
                                sb.Append(@"<span style='cursor: pointer' class='span_link' onclick='get_NextLVL(this)' val='").Append(row[colname]).Append("' lvl_name='").Append(nextLvl).Append("' pre_lvl='").Append(prelvl[prelvl.Length - 1]).Append("'>").Append(row[col].ToString()).Append("</span>");
                            }
                        }
                        else
                        {
                            GetCommonHtml(col, row, sb, model, field, Lvl);
                        }
                        sb.Append("</td>");
                    }
                }
            }
            else
            {
                sb.Append("<tr><th>序号</th></tr><tr><td style='text-align:left; font-weight:bold'> 暂无查询结果！！！</td></tr>");
            }

            return sb.ToString();
        }

        void GetCommonHtml(DataColumn col, DataRow row, StringBuilder sb, Ucs_Reportforms model, UCS_FromField field, string lvl)
        {
            decimal d = 0;
            if (col.ColumnName == "rowId")
            {
                sb.Append(row[col]);
                return;
            }



            if (field != null && field.IsShow == false) { return; }
            if (field == null || string.IsNullOrEmpty(field.LinkUrl))
            {
                sb.Append(decimal.TryParse(row[col].ToString(), out d) ? Math.Round(d, 2) : row[col]);
            }
            else
            {
                //sb.Append("<span style='cursor: pointer' class='span_link' onclick='goLink(\"").Append(field.LinkUrl);
                sb.Append("<a href=\"").Append(field.LinkUrl);
                if (!string.IsNullOrEmpty(field.parameter))
                {
                    string[] paralist = field.parameter.Split(',');
                    for (var i = 0; i < paralist.Length; i++)
                    {
                        sb.Append("&").Append(paralist[i]).Append("=").Append(System.Web.HttpUtility.UrlEncode(System.Web.HttpUtility.HtmlEncode(Botwave.Commons.DbUtils.ToString(row[paralist[i]]))));
                    }
                }
                
                sb.Append("\"").Append(" onclick=\"putin();\" >").Append(row[col]).Append("</a>");
            }
        }

        public DataTable GetAllTableName()
        {
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, "select NAME,xtype from sysobjects where xtype='u' OR xtype='v' ORDER BY xtype,name").Tables[0];

        }




        public string GetNextLvL(string LvL, string tableName)
        {
            int l = 0;
            if (LvL.ToUpper().IndexOf("GROUP_VEST_LVL_") < 0)
            {
                throw new Exception("数据分层字段命名不合规则");
            }
            else
            {
                if (!int.TryParse(LvL.Substring(15), out l))
                {
                    throw new Exception("数据分层字段命名不合规则");
                }
            }
            int i = (int)IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("select count(*) from syscolumns where id=(select max(id) from sysobjects where (xtype='v' OR xtype='u') and name='{0}') and name='{1}'", tableName, "GROUP_VEST_LVL_" + (l + 1).ToString()));
            return i > 0 ? "GROUP_VEST_LVL_" + (l + 1).ToString() : null;
        }

        string GetGroupLvL(string LvL, string tableName)
        {
            int l = 0;
            if (LvL.ToUpper().IndexOf("GROUP_VEST_LVL_") < 0)
            {
                throw new Exception("数据分层字段命名不合规则");
            }
            else
            {
                if (!int.TryParse(LvL.Substring(15), out l))
                {
                    throw new Exception("数据分层字段命名不合规则");
                }
            }
            int i = (int)IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("select count(*) from syscolumns where id=(select max(id) from sysobjects where (xtype='v' OR xtype='u') and name='{0}') and name='{1}'", tableName, "GROUP_VEST_LVL_" + (l + 1).ToString()));
            return i > 0 ? "GROUP_VEST_LVL_" + (l + 1).ToString() : null;
        }


        public string GetPreLvL(string LvL)
        {
            int l = 0;
            if (LvL.ToUpper().IndexOf("GROUP_VEST_LVL_") < 0)
            {
                throw new Exception("数据分层字段命名不合规则");
            }
            else
            {
                if (!int.TryParse(LvL.Substring(15), out l))
                {
                    throw new Exception("数据分层字段命名不合规则");
                }
            }
            if (l == 1)
            {
                l++;
            }
            return "GROUP_VEST_LVL_" + (l - 1).ToString();
        }


        public DataTable GetAllFieldName(string tablename)
        {
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format("select   name   from   syscolumns   where   id=object_id('{0}')", tablename)).Tables[0];
        }

        public string PreView(Ucs_Reportforms model, NameValueCollection from)
        {
            StringBuilder sb = new StringBuilder();

            var keys = from.AllKeys.Where(x => x.EndsWith("field")).OrderBy(x => "fieldorder"); model.FieldList = new List<UCS_FromField>();
            foreach (string k in keys)
            {
                string id;
                string[] str = k.Split('_');
                if (str.Length == 3)
                {
                    id = str[0] + "_" + str[1];
                }
                else
                {
                    id = str[0];
                }
                UCS_FromField FromField = new UCS_FromField() { fieldname = from[id + "_fieldname"], field = from[id + "_field"], imgtype = from[id + "_imgtype"], IsCount = from[id + "_IsCount"]=="1"?true:false };
                if (!string.IsNullOrEmpty(from[id + "_Axis"]))
                {
                    FromField.Axis = Convert.ToInt32(from[id + "_Axis"]);
                }
                model.FieldList.Add(FromField);
            }

            sb.Append("<tr><td valign=\"top\" style=\"border-bottom-style: none ; \"><div class=\"div_content1\"><div class=\"div_title\"><div style=\"float:left; \" class=\"div_inco\"><img src=\"../../../App_Themes/gmcc/icons/small05.png\" /></div>").Append(model.formname);
            sb.AppendFormat("</div><table class=\"tb_content2\" cellpadding=\"0\" cellspacing=\"0\" {0}><tr>",model.ImageOnly ? "style=\"display:none\"":string.Empty);
            if (model.formtype != 3)
            {
                sb.Append("<th>").Append("序号").Append("</th>");
                foreach (var field in model.FieldList)
                {
                    sb.Append("<th>").Append(field.fieldname).Append("</th>");
                }
                sb.Append("</tr>");
            }
            StringBuilder sb2 = new StringBuilder();
            if (model.FieldList.Count == 0) { return null; }
            foreach (var item in model.FieldList)
            {
                sb2.Append(item.field);
                sb2.Append(",");
            }
            sb2.Remove(sb2.Length - 1, 1);

            int i = 0;
            var dt = CommontUnit.Instance.GetUsersByPager(model.datasource, "", sb2.ToString(), "", model.strOrder, 0, 100, model.strGroup, ref i);
            if (model.formtype != 3)
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn col in dt.Columns)
                        {
                            sb.Append("<td>").Append(decimal.TryParse(row[col].ToString(), out d) ? Math.Round(d, 2) : row[col]).Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                }
            }
            sb.Append("</table></div></td></tr><tr><td style=\"border-top-style: none\"><div class=\"div_content4 left\"><div class=\"div_img\" id=\"div_img1\"></div></div></td><tr>");



            sb.Append("|").Append(GetHighcharts(model, dt, "div_img1"));




            return sb.ToString();

        }
        string GetFields(Ucs_Reportforms model)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(Ucs_ReportformsService));

            if (model == null)
                log.Info("1");
            if (model.FieldList == null)
                log.Info("2");
            log.Info(model.formname);
            log.Info(model.id);
            StringBuilder sb2 = new StringBuilder();

            foreach (var item in model.FieldList)
            {
                sb2.Append(item.field);
                sb2.Append(",");
            }
            sb2=sb2.Length>0?sb2.Remove(sb2.Length - 1, 1):sb2;
            return sb2.ToString();
        }

        string GetHighcharts(Ucs_Reportforms model, DataTable dt, string targid)
        {
            switch (model.formtype)
            {
                case 1:
                    return string.Empty;
                case 4:
                case 5:
                    return GetHighchartsByLine(model, dt, targid);
                default:
                    StringBuilder sb = new StringBuilder();
                    Highcharts highcharts = new Highcharts();
                    List<string> xname = new List<string>(); bool flag = false; int ycnt = 0;
                    int petname = 0;
                    for (int i = 0; i < model.FieldList.Count; i++)
                    {
                        UCS_FromField field = model.FieldList[i];
                        if (field.Axis == 1)
                        {
                            //if (model.formtype == 3)
                            //{
                            //    int l = 0;
                            //    foreach (DataRow row in dt.Rows)
                            //    {
                            //        petname = i + 1;
                            //        if (!xname.Contains(row[l].ToString()))
                            //            xname.Add(row[l].ToString());
                            //        l++;
                            //    }
                            //}
                            //else
                            //{
                            foreach (DataRow row in dt.Rows)
                            {

                                petname = i + 1;
                                if (!xname.Contains(row[i + 1].ToString()))
                                    xname.Add(row[i + 1].ToString());
                            }
                            // }
                        }
                        if (field.Axis == 2)
                        {
                            highcharts.yAxis.Add(new yAxis() { title = field.fieldname, opposite = flag });
                            flag = !flag;
                        }
                        if (model.formtype == 3 || !string.IsNullOrEmpty(field.imgtype))
                        {
                            if (model.formtype != 3)
                            {
                                series series = new series() { type = field.imgtype, name = field.fieldname, yAxis = ycnt };
                                int j = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (j == highcharts.colors.Count) { j = 0; }
                                    int index = i + 1;
                                    if (dt.Columns[index].ColumnName.ToUpper().Contains("HIDE")) { index++; }
                                    series.data.Add(new data() { name = row[petname].ToString(), color = highcharts.colors[j], y = Convert.ToInt32(row[index]) }); j++;
                                }
                                highcharts.series.Add(series);
                                ycnt++;
                            }
                            else if (!string.IsNullOrEmpty(field.imgtype))
                            {
                                var lineName = model.FieldList.Where(x => x.Axis == 0).First(); int j = 0;

                                foreach (DataRow row in dt.Rows)
                                {

                                    string name = row[lineName.field].ToString();
                                    if (highcharts.series.Where(x => x.name == name).Count() == 0)
                                    {
                                        highcharts.series.Add(new series() { name = name, type = field.imgtype, yAxis = 0 });
                                    }


                                }
                                int k = -1;
                                var yfield = model.FieldList.Where(x => x.Axis == 2).First();
                                string[] str = yfield.field.Split(' ');
                                string yname = str[str.Length - 1];
                                string str1 = "";
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (str1 != row[lineName.field].ToString())
                                    {
                                        str1 = row[lineName.field].ToString(); k++;
                                    }

                                    highcharts.series[k].data.Add(new data() { name = yfield.fieldname, y = Convert.ToInt32(row[yname]) });

                                }

                            }
                        }
                    }

                    highcharts.chart.renderTo = targid;
                    highcharts.title.text = "<b>" + model.formname + "<b>";
                    highcharts.title.style.color = "#3E576F";

                    highcharts.style.color = "#3E576F";

                    highcharts.xAxis.categories = xname;

                    sb.Append(new JavaScriptSerializer().Serialize(highcharts));
                    return sb.ToString();
            }
        }

        /// <summary>
        /// 以行为维度显示图表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dt"></param>
        /// <param name="targid"></param>
        /// <returns></returns>
        string GetHighchartsByLine(Ucs_Reportforms model, DataTable dt, string targid)
        {
            StringBuilder sb = new StringBuilder();
            Highcharts highcharts = new Highcharts();
            List<string> xname = new List<string>(); bool flag = false; int ycnt = 0;
            int petname = 0;
            highcharts.yAxis.Add(new yAxis() { title = model.formname, opposite = flag });
            foreach (UCS_FromField field in model.FieldList)
            {
                if (!xname.Contains(field.fieldname) && field.IsCount)
                    xname.Add(field.fieldname);
            }
            foreach (UCS_FromField field in model.FieldList)
            {
                List<UCS_FromField> lineName = model.FieldList.Where(x => x.Axis == 1).ToList();
                if (lineName.Count > 0)
                {
                    petname++;
                    if (dt.Columns[petname].ColumnName.ToUpper().Contains("HIDE")) { petname++; }
                    break;
                }
            }
            foreach (DataRow dw in dt.Rows)
            {
                series series = new series() { type = "spline", name = dw[petname].ToString(), yAxis = ycnt };
                switch (model.formtype)
                {
                    case 4:
                        series = new series() { type = "spline", name = dw[petname].ToString(), yAxis = ycnt };
                        break;
                    case 5:
                        series = new series() { type = "column", name = dw[petname].ToString(), yAxis = ycnt };
                        break;
                }
                int index = 0;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataColumn col = dt.Columns[i];
                    UCS_FromField field = null;
                    int j = 0;
                    if (j == highcharts.colors.Count) { j = 0; }
                    index = i + 1;
                    Regex rx = new Regex(" as ");

                    var obj = model.FieldList.Where(x => (x.field.ToLower().IndexOf(" as") > 0 ? rx.Split(x.field.ToLower())[rx.Split(x.field.ToLower()).Length - 1] : x.field.ToLower()).Trim().Trim('\'') == col.ColumnName.ToLower().Trim());
                    if (obj.Count() > 0)
                        field = obj.First();
                    else
                        continue;
                    string name = field == null ? string.Empty : rx.Split(field.field.ToLower())[rx.Split(field.field.ToLower()).Length - 1].Trim().Trim('\'');
                    //if (dt.Columns[index].ColumnName.ToUpper().Contains("HIDE")) { index++; }
                    if (field.IsCount)
                    {
                        series.data.Add(new data() { name = field.fieldname, color = highcharts.colors[j], y = decimal.TryParse(Botwave.Commons.DbUtils.ToString(dw[name]), out d) ? Math.Round(d, 2) : 0 }); j++;
                    }
                    //ycnt++;
                } highcharts.series.Add(series);
            }
            highcharts.chart.renderTo = targid;
            highcharts.title.text = "<b>" + model.formname + "<b>";
            highcharts.title.style.color = "#3E576F";

            highcharts.style.color = "#3E576F";

            highcharts.xAxis.categories = xname;

            sb.Append(new JavaScriptSerializer().Serialize(highcharts));
            return sb.ToString();
        }

        /// <summary>
        /// 生成首页表格html，及图表javascript代码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="javascript"></param>
        /// <returns></returns>
        public string GetIndexImgHtml(List<Ucs_Reportforms> list, StringBuilder javascript)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < list.Count; i = i + 2)
            {
                sb.Append("<tr>");
                if (list[i].formtype != 3)
                {
                    sb.Append(GetTrHtml(list, i, javascript));
                }
                else
                {
                    sb.Append(GetLineImgHtml(list, i, javascript));
                }
                if (i + 1 != list.Count)
                {
                    if (list[i + 1].formtype != 3)
                    {
                        sb.Append(GetTrHtml(list, i + 1, javascript));
                    }
                    else
                    {
                        sb.Append(GetLineImgHtml(list, i + 1, javascript));
                    }
                }
                else
                {
                    sb.Append(GetTrHtml(list, i + 1, javascript));
                }
                if (list[i].formtype != 3)
                {
                    sb.Append("</tr><tr>");
                }
                else if (i + 1 != list.Count)
                {
                    if (list[i + 1].formtype != 3)
                    {
                        sb.Append("</tr><tr>");
                    }
                }
                if (list[i].formtype != 3)
                {

                    sb.Append(GetTrImgHtml(list, i));
                }

                if (i + 1 != list.Count)
                {
                    if (list[i + 1].formtype != 3)
                    {

                        sb.Append(GetTrImgHtml(list, i + 1));
                    }
                }
                else
                {

                    sb.Append(GetTrImgHtml(list, i + 1));

                }



            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成首页表格html，及图表javascript代码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="javascript"></param>
        /// <returns></returns>
        public string GetIndexImgHtml(List<Ucs_Reportforms> list,ArrayList whereArr, StringBuilder javascript)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < list.Count; i = i + 2)
            {
                sb.Append("<tr>");
                if (list[i].formtype != 3)
                {
                    sb.Append(GetTrHtml(list, i, javascript,whereArr,null));
                }
                else
                {
                    sb.Append(GetLineImgHtml(list, i, javascript, whereArr, null));
                }
                if (i + 1 != list.Count)
                {
                    if (list[i + 1].formtype != 3)
                    {
                        sb.Append(GetTrHtml(list, i + 1, javascript, whereArr, null));
                    }
                    else
                    {
                        sb.Append(GetLineImgHtml(list, i + 1, javascript, whereArr, null));
                    }
                }
                else
                { if(list.Count >i+1)
                    sb.Append(GetTrHtml(list, i + 1, javascript, whereArr, null));
                }
                if (list[i].formtype != 3)
                {
                    sb.Append("</tr><tr>");
                }
                else if (i + 1 != list.Count)
                {
                    if (list[i + 1].formtype != 3)
                    {
                        sb.Append("</tr><tr>");
                    }
                }
                if (list[i].formtype != 3)
                {

                    sb.Append(GetTrImgHtml(list, i, whereArr));
                }

                if (i + 1 != list.Count)
                {
                    if (list[i + 1].formtype != 3)
                    {

                        sb.Append(GetTrImgHtml(list, i + 1, whereArr));
                    }
                }
                else
                {

                    sb.Append(GetTrImgHtml(list, i + 1, whereArr));

                }



            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成首页表格html
        /// </summary>
        /// <param name="model"></param>
        /// <param name="javascript"></param>
        /// <returns></returns>
        public string GetIndexFormHtml(List<Ucs_Reportforms> list, ArrayList whereArr, StringBuilder javascript, ArrayList lvlArr)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < list.Count; i ++)
            {
                
                sb.Append("<tr>");
                if (list[i].formtype != 3)
                {
                    sb.Append(GetTrHtml(list, i, javascript, whereArr, lvlArr));
                }
                else
                {
                    //if (list[i].formtype != 1)
                    sb.Append(GetLineImgHtml(list, i, javascript, whereArr, lvlArr));
                }
                if (list[i].formtype != 3)
                {
                    sb.Append("</tr><tr>");
                }
               
                if (list[i].formtype != 3)
                {
                    //if (list[i].formtype != 1)
                    sb.Append(GetTrImgHtml(list, i, whereArr));
                }

                /*if (i + 1 != list.Count)
                {
                    if (list[i + 1].formtype != 3)
                    {
                        sb.Append(GetTrHtml(list, i + 1, javascript, whereArr, lvlArr));
                    }
                    else
                    {
                        //if (list[i].formtype != 1)
                        sb.Append(GetLineImgHtml(list, i + 1, javascript, whereArr, lvlArr));
                    }
                }
                else
                {
                    if (list.Count > i + 1)
                        sb.Append(GetTrHtml(list, i + 1, javascript, whereArr, lvlArr));
                }
                if (list[i].formtype != 3)
                {
                    sb.Append("</tr><tr>");
                }
                else if (i + 1 != list.Count)
                {
                    if (list[i + 1].formtype != 3)
                    {
                        sb.Append("</tr><tr>");
                    }
                }
                if (list[i].formtype != 3)
                {
                    //if (list[i].formtype != 1)
                    sb.Append(GetTrImgHtml(list, i, whereArr));
                }

                if (i + 1 != list.Count)
                {
                    if (list[i + 1].formtype != 3)
                    {
                        //if (list[i+1].formtype != 1)
                        sb.Append(GetTrImgHtml(list, i + 1, whereArr));
                    }
                }
                else
                {
                    //if (list[i].formtype != 1)
                    sb.Append(GetTrImgHtml(list, i + 1, whereArr));

                }


                */
            }
            return sb.ToString();
        }

        string GetLineImgHtml(List<Ucs_Reportforms> list, int i, StringBuilder javascript)
        {
            if (i == list.Count)
            {
                return "<td  style=\"border-top-style: none\"></td>";
            }
            bool flag = i % 2 == 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("<td rowspan='2' valign=\"top\" style=\"border-bottom-style: none ; width: 50%;\">");
            sb.AppendFormat("<div class=\"div_content1 {0}\" style='height:100% ;border:0px;'><div class=\"div_title\"> <div style=\"float:left; \" class=\"div_inco\"><img src=\"../../../App_Themes/gmcc/icons/small05.png\" /></div>", flag ? "left" : "right").Append(list[i].formname);
            sb.Append("</div>");
            sb.AppendFormat("<div class=\"div_content4 {0}\" style='margin:0px;height:360px;'>", flag ? "left" : "right");
            sb.AppendFormat(" <div class=\"div_img\" id=\"div_img{0}\" style='margin:auto;height:90%;width:90%'> </div> </div>  </td>", i);
            var dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format("select ROW_NUMBER() OVER (ORDER BY {3}) AS rowid,{0} from {1}    {2} order by {3}", GetFields(list[i]), list[i].datasource, string.IsNullOrEmpty(list[i].strGroup) ? "" : "group by " + list[i].strGroup, list[i].strOrder)).Tables[0];
            javascript.Append(GetJavaScript("div_img" + i, list[i], dt));
            return sb.ToString();

        }

        string GetLineImgHtml(List<Ucs_Reportforms> list, int i, StringBuilder javascript, ArrayList whereArr,  ArrayList lvlArr)
        {
            string lvl = Botwave.Commons.DbUtils.ToString(lvlArr[i]);
            if (i == list.Count)
            {
                return "<td  style=\"border-top-style: none\"></td>";
            }
            bool flag = i % 2 == 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("<td rowspan='2' valign=\"top\" style=\"border-bottom-style: none ; width: 50%;\">");
            sb.AppendFormat("<div class=\"div_content1 {0}\" style='height:100% ;border:0px;'><div class=\"div_title\"> <div style=\"float:left; \" class=\"div_inco\"><img src=\"../../../App_Themes/gmcc/icons/small05.png\" /></div>", flag ? "left" : "right").Append(list[i].formname);
            sb.Append("</div>");
            sb.AppendFormat("<div class=\"div_content4 {0}\" style='margin:0px;height:360px;'>", flag ? "left" : "right");
            sb.AppendFormat(" <div class=\"div_img\" id=\"div_img{0}\" style='margin:auto;height:90%;width:90%'> </div> </div>  </td>", i);
            if (list[i].formtype != 1)
            {
                var dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format("select ROW_NUMBER() OVER (ORDER BY {3}) AS rowid,{0} from {1}    {2} order by {3}", !string.IsNullOrEmpty(lvl) ? GetFields(list[i]).Replace("GROUP_VEST_LVL_1", lvl).Replace("${diqu}", lvl.Split(',')[lvl.Split(',').Length - 1]) : GetFields(list[i]), list[i].datasource + " " + whereArr[i], string.IsNullOrEmpty(list[i].strGroup) ? "" : "group by " + (string.IsNullOrEmpty(lvl) ? list[i].strGroup : list[i].strGroup.Replace("GROUP_VEST_LVL_1", lvl)), list[i].strOrder.Replace("GROUP_VEST_LVL_1", lvl))).Tables[0];
                javascript.Append(GetJavaScript("div_img" + i, list[i], dt));
            }
            return sb.ToString();

        }
        /// <summary>
        /// 生成产生图表的javascript代码
        /// </summary>
        /// <param name="targid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        string GetJavaScript(string targid, Ucs_Reportforms model, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            return sb.AppendFormat("new Highcharts.Chart(eval('('+'{0}'+')')); ", GetHighcharts(model, dt, targid)).ToString();
        }
        string GetTrImgHtml(List<Ucs_Reportforms> list, int i)
        {
            if (i == list.Count)
            {
                return "<td style=\"border-top-style: none\"></td>";
            }
            bool flag = i % 2 == 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<td style=\"border-top-style: none\"><div class=\"div_content4 {0}\">", flag ? "left" : "right");
            sb.AppendFormat(" <div class=\"div_img\" id=\"div_img{0}\"> </div> </div>  </td>", i);

            return sb.ToString();
        }
        string GetTrHtml(List<Ucs_Reportforms> list, int i, StringBuilder javascript)
        {

            StringBuilder sb = new StringBuilder();
            if (i == list.Count)
            {
                return "<td valign=\"top\" style=\"border-bottom-style: none ; width: 50%;\"></td>";
            }


            bool flag = i % 2 == 0;

            var dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format("select ROW_NUMBER() OVER (ORDER BY {3}) AS rowid,{0} from {1}    {2} order by {3}", GetFields(list[i]), list[i].datasource, string.IsNullOrEmpty(list[i].strGroup) ? "" : "group by " + list[i].strGroup, list[i].strOrder)).Tables[0];
            sb.Append("<td valign=\"top\" style=\"border-bottom-style: none ; width: 50%;\">");
            sb.AppendFormat("<div class=\"div_content1 {0}\"><div class=\"div_title\"> <div style=\"float:left; \" class=\"div_inco\"><img src=\"../../../App_Themes/gmcc/icons/small05.png\" /></div>", flag ? "left" : "right").Append(list[i].formname);
            sb.Append("</div><table class=\"tb_content2\" cellpadding=\"0\" cellspacing=\"0\">");
            sb.Append("<th>").Append("序号").Append("</th>");
            foreach (var field in list[i].FieldList)
            {
                sb.Append("<th>").Append(field.fieldname).Append("</th>");
            }
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn col in dt.Columns)
                {
                    sb.Append("<td>").Append(decimal.TryParse(row[col].ToString(), out d) ? Math.Round(d, 2) : row[col]).Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table> </div></td>");
            javascript.Append(GetJavaScript("div_img" + i, list[i], dt));
            return sb.ToString();
        }

        string GetTrImgHtml(List<Ucs_Reportforms> list, int i, ArrayList whereArr)
        {
            if (i == list.Count)
            {
                return "<td style=\"border-top-style: none\"></td>";
            }
            bool flag = i % 2 == 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<td style=\"border-top-style: none\"><div class=\"div_content4 {0}\">", flag ? "left" : "right");
            sb.AppendFormat(" <div class=\"div_img\" id=\"div_img{0}\"> </div> </div>  </td>", i);

            return sb.ToString();
        }
        string GetTrHtml(List<Ucs_Reportforms> list, int i, StringBuilder javascript, ArrayList whereArr, ArrayList lvlArr)
        {
            string lvl=Botwave.Commons.DbUtils.ToString(lvlArr[i]);
            Ucs_Reportforms model = list[i];
            StringBuilder sb = new StringBuilder();
            if (i == list.Count)
            {
                return "<td valign=\"top\" style=\"border-bottom-style: none ; width: 50%;\"></td>";
            }


            bool flag = i % 2 == 0;

            var dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format("select ROW_NUMBER() OVER (ORDER BY {3}) AS rowid,{0} from {1}    {2} order by {3}", !string.IsNullOrEmpty(lvl) ? GetFields(list[i]).Replace("GROUP_VEST_LVL_1", lvl).Replace("${diqu}", lvl.Split(',')[lvl.Split(',').Length - 1]) : GetFields(list[i]), list[i].datasource + " " + whereArr[i], string.IsNullOrEmpty(list[i].strGroup) ? "" : "group by " + (string.IsNullOrEmpty(lvl) ? list[i].strGroup : list[i].strGroup.Replace("GROUP_VEST_LVL_1", lvl)), list[i].strOrder.Replace("GROUP_VEST_LVL_1", lvl.Split(',')[lvl.Split(',').Length - 1]))).Tables[0];
            if(model.formtype!=1)
            javascript.Append(GetJavaScript("div_img" + i, list[i], dt));
            sb.Append("<td valign=\"top\" style=\"border-bottom-style: none ; width: 50%;\">");
            sb.AppendFormat("<div class=\"div_content1 {0}\"><div class=\"div_title\"> <div style=\"float:left; \" class=\"div_inco\"><img src=\"../../../App_Themes/gmcc/icons/small05.png\" /></div>", flag ? "left" : "right").Append(list[i].formname);
            sb.AppendFormat("</div><table class=\"tb_content2\" cellpadding=\"0\" cellspacing=\"0\" {0}><tr>", model.ImageOnly&&list[i].formtype>1 ? "style=\"display:none\"" : "");
            sb.Append("<th>").Append("序号").Append("</th>");
            foreach (var field in list[i].FieldList)
            {
                if (!field.IsShow && list[i].formtype==1) continue;
                sb.Append("<th>").Append(field.fieldname).Append("</th>");
            }
            sb.Append("</tr>");

             #region 合计行统计
                if (model.Statistics)
                {
                    object[] foot = new object[dt.Columns.Count];
                    foot[0] = null;
                    int sindex = 0;
                    foreach (DataColumn col in dt.Columns)
                    {
                        UCS_FromField field = null;
                        Regex rx = new Regex(" as ");
                        if (!(col.ColumnName == "rowId" || col.ColumnName.Contains("GROUP_VEST_LVL_")))
                        {
                            var obj = model.FieldList.Where(x => (x.field.ToLower().IndexOf(" as") > 0 ? rx.Split(x.field.ToLower())[rx.Split(x.field.ToLower()).Length - 1] : x.field.ToLower()).Trim().Trim('\'') == col.ColumnName.ToLower().Trim());
                            if (obj.Count() > 0)
                            {
                                field = obj.First();
                            }
                            else
                                continue;
                        }
                        string name = field == null ? string.Empty : rx.Split(field.field.ToLower())[rx.Split(field.field.ToLower()).Length - 1].Trim().Trim('\'');
                        if (col.ColumnName.ToUpper().Contains("GROUP_VEST_LVL_") && col.ColumnName.ToLower() != "rowid")
                        {
                            foot[col.Ordinal] = "合计";
                            if (!col.ColumnName.ToUpper().Contains("HIDE"))
                                sindex++;
                        }
                        else if (!col.ColumnName.ToLower().Contains("byl") && !col.ColumnName.ToLower().Contains("zb") && col.ColumnName.ToLower().Trim() == name && (col.DataType.Name == "Int32" || col.DataType.Name == "Decimal" || col.DataType.Equals(typeof(Double))))
                        {
                            object sumObject = null;
                            switch (model.FieldList[sindex].StatisticsType)
                            {
                                case 1:
                                    sumObject = dt.Compute("sum(" + col.ColumnName + ")", "TRUE");
                                    break;
                                case 2:
                                    sumObject = dt.Compute("avg(" + col.ColumnName + ")", "TRUE");
                                    break;
                                default:
                                    sumObject = dt.Compute("sum(" + col.ColumnName + ")", "TRUE");
                                    break;
                            }
                            foot[col.Ordinal] = model.FieldList[sindex].IsCount ? sumObject : null;
                            sindex++;
                        }
                        else if (col.ColumnName.ToLower().Trim() == name)
                            sindex++;
                        if (sindex == model.FieldList.Count)
                            break ;
                    }

                    dt.Rows.Add(foot);
                }
                #endregion
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                int index = 0;

                foreach (DataColumn col in dt.Columns)
                {
                    if (!(col.ColumnName.ToLower() == "rowid" || col.ColumnName.Contains("GROUP_VEST_LVL_")))
                    {
                        //UCS_FromField field = list[i].FieldList[index];
                        UCS_FromField field = null;
                        Regex rx = new Regex(" as ");
                        var obj = model.FieldList.Where(x => (x.field.ToLower().IndexOf(" as") > 0 ? rx.Split(x.field.ToLower())[rx.Split(x.field.ToLower()).Length - 1] : x.field.ToLower()).ToLower().Trim().Trim('\'') == col.ColumnName.ToLower().Trim());
                        if (obj.Count() > 0)
                            field = obj.First();
                        else
                        {
                            continue;
                        }
                        if (!field.IsShow && list[i].formtype==1) continue;
                        if (!string.IsNullOrEmpty(field.LinkUrl))
                        {
                            sb.Append("<td>");
                            sb.Append("<span style='cursor: pointer' class='span_link' onclick='goLink(\"").Append(field.LinkUrl);
                            if (!string.IsNullOrEmpty(field.parameter))
                            {
                                sb.Append("&");
                                string[] paralist = field.parameter.Split(',');
                                for (var j = 0; j < paralist.Length; j++)
                                {
                                    paralist[j] = paralist[j].Replace("${diqu}",lvl.Split(',').Length>1?lvl.Split(',')[1]:lvl);
                                    sb.Append(paralist[j]).Append("=").Append(System.Web.HttpUtility.UrlEncode(Botwave.Commons.DbUtils.ToString(row[paralist[j]]))).Append("&");
                                }
                                sb = sb.Remove(sb.Length - 1, 1);
                            }
                            sb.Append("\")").Append("' >").Append(decimal.TryParse(row[col].ToString(), out d) ? Math.Round(d, 2) : row[col]).Append("</span>");
                            sb.Append("</td>");
                        }
                        else
                            sb.Append("<td>").Append(decimal.TryParse(row[col].ToString(), out d) ? Math.Round(d, 2) : row[col]).Append("</td>");
                        index++;
                    }
                    else
                    sb.Append("<td>").Append(decimal.TryParse(row[col].ToString(), out d) ? Math.Round(d, 2) : row[col]).Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table> </div></td>");
            
            return sb.ToString();
        }
        decimal d;


        public string GetImgHtml(Ucs_Reportforms model, StringBuilder javascript, string wherestr, string lvl)
        {
           // lvl = lvl.Split(',')[0];
            var dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format("select ROW_NUMBER() OVER (ORDER BY {3}) AS rowid,{0} from   {1}  where {4}  {2} order by {3}", GetFields(model).Replace("GROUP_VEST_LVL_1", lvl), model.datasource, string.IsNullOrEmpty(model.strGroup) ? "" : "group by " + model.strGroup.Replace("GROUP_VEST_LVL_1", lvl), model.strOrder.Replace("GROUP_VEST_LVL_1", lvl.Split(',')[lvl.Split(',').Length - 1]), wherestr)).Tables[0];
            javascript.Append(GetJavaScript("container", model, dt));
            //     dt.Rows.Add(new object[] { null, "a", "b", "c" });
            if (model.Statistics)
            {
                object[] foot = new object[dt.Columns.Count];
                foot[0] = null;
                int sindex = 0;
                foreach (DataColumn col in dt.Columns)
                {

                    if (col.ColumnName.ToUpper() != "TM_INTRVL_CD" && model.strGroup.Replace("GROUP_VEST_LVL_1", lvl).Contains(col.ColumnName))
                    {
                        foot[col.Ordinal] = "合计";
                        if (!col.ColumnName.ToUpper().Contains("HIDE"))
                            sindex++;
                    }
                    else if (!col.ColumnName.ToLower().Contains("byl") && !col.ColumnName.ToLower().Contains("zb") && col.ColumnName.ToUpper() != "TM_INTRVL_CD" && (col.DataType.Name == "Int32" || col.DataType.Name == "Decimal" || col.DataType.Equals(typeof(Double))))
                    {
                        object sumObject = null;
                        switch (model.FieldList[sindex].StatisticsType)
                        {
                            case 1:
                                sumObject = dt.Compute("sum(" + col.ColumnName + ")", "TRUE");
                                break;
                            case 2:
                                sumObject = dt.Compute("avg(" + col.ColumnName + ")", "TRUE");
                                break;
                            default:
                                sumObject = dt.Compute("sum(" + col.ColumnName + ")", "TRUE");
                                break;
                        }
                        foot[col.Ordinal] = model.FieldList[sindex].IsCount ? sumObject : null;
                        sindex++;
                    }
                    else
                    {
                        if (col.ColumnName.ToLower() != "rowid")
                            sindex++;
                        foot[col.Ordinal] = null;
                    }

                }
                dt.Rows.Add(foot);
            }
            StringBuilder sb = new StringBuilder();
            if (model.formtype != 3)
            {
                sb.Append("<fieldset style=\"width: 99%\" class=\"img_fieldset\"><legend>").Append(model.formname);
                sb.AppendFormat("</legend><div class=\"libox\"  ><div><div id=\"container\" style=\"width: 50%; float: left;   margin-top: 20px;height:300px\"></div> <div style=\"float: right; width: 35%; height: 250px; margin: 50px 20px;\"> <table width=\"95%\" cellpadding=\"0\" cellspacing=\"0\" class=\"tblClass\" {0} ><tr >", model.ImageOnly ? "style=\"display:none\"" : string.Empty); 
                sb.Append("<th style=\"text-align:center\"> 序号</th>");
                foreach (var field in model.FieldList)
                {
                    sb.Append("<th style=\"text-align:center\">").Append(field.fieldname).Append("</th>");
                }
                sb.Append("</tr>");
                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");
                    int index=0;
                    foreach (DataColumn col in dt.Columns)
                    {

                        if (!col.ColumnName.ToUpper().Contains("HIDE"))
                        {
                            if (col.ColumnName.ToLower() != "rowid")
                            {
                                
                                UCS_FromField field = model.FieldList[index];
                                if (!string.IsNullOrEmpty(field.LinkUrl))
                                {
                                    sb.Append("<td>");
                                    sb.Append("<span style='cursor: pointer' class='span_link' onclick='goLink(\"").Append(field.LinkUrl + "?");
                                    if (!string.IsNullOrEmpty(field.parameter))
                                    {
                                        string[] paralist = field.parameter.Split(',');
                                        for (var i = 0; i < paralist.Length; i++)
                                        {
                                            sb.Append(paralist[i]).Append("=").Append(System.Web.HttpUtility.UrlEncode(Botwave.Commons.DbUtils.ToString(row[paralist[i]]))).Append("&");
                                        }
                                        sb = sb.Remove(sb.Length - 1, 1);
                                    }
                                    sb.Append("\")").Append("' >").Append(row[col]).Append("</span>"); 
                                    sb.Append("</td>");
                                }
                                else
                                    sb.Append("<td>").Append(decimal.TryParse(row[col].ToString(), out d) ? Math.Round(d, 2) : row[col]).Append("</td>");
                                index++;
                            }
                            else
                            sb.Append("<td>").Append(decimal.TryParse(row[col].ToString(), out d) ? Math.Round(d, 2) : row[col]).Append("</td>");
                        }
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table></div> </div></div></fieldset>");
            }
            else
            {
                sb.Append("<fieldset style=\"width: 99%\" class=\"img_fieldset\"><legend>").Append(model.formname).Append("</legend><div class=\"libox\"  ><div><div id=\"container\" style=\"width: 90%; float: left;   margin-top: 20px;height:300px\"></div> "); sb.Append("  </div></div></fieldset>");
            }

            return sb.ToString();
        }
    }

}
