using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Botwave.Extension.IBatisNet;
using System.Web;
using Botwave.XQP.API.Util;


namespace Botwave.XQP.Util
{
    public class CommontUnit
    {
        public static CommontUnit Instance
        {
            get { return new CommontUnit(); }
        }

        #region 页面弹出消息-----void Message(string msg, HttpResponse Response)
        /// <summary>
        /// 页面弹出消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Response"></param>
        public void Message(string msg, HttpResponse Response)
        {
            Response.Write("<script>alert('" + msg + "')</script>");
        }

        public void Javascript(string js, HttpResponse Response)
        {
            Response.Write("<script>" + js + "</script>");

        }
        #endregion

        /// <summary>
        /// 获取已经分页列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldKey">主键字段</param>
        /// <param name="fieldShow">需要显示的字段</param>
        /// <param name="whereStr">查询条件</param>
        /// <param name="fieldOrder">排序字段</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页显示多少条记录</param>
        /// <param name="recordCount">返回总共多少条记录</param>
        /// <returns></returns>
        public DataTable GetUsersByPager(string tableName, string fieldKey, string fieldShow, string whereStr, string fieldOrder, int pageIndex, int pageSize, ref int recordCount)
        {
            string order = fieldOrder;
            Type = 0; Page = "";
            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, order, whereStr, ref recordCount);

        }
        static string page;
        string orderStr;

        public string OrderStr
        {
            get { object str = IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT TOP 1 orderStr FROM Ucs_TableField utf WHERE PAGE='{0}' and IsRequired='{1}'", Page, type));  if (str != null) { return str.ToString(); } else { return ""; } }

        }





        /// <summary>
        /// 获取已经分页列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldKey">主键字段</param>
        /// <param name="fieldShow">需要显示的字段</param>
        /// <param name="whereStr">查询条件</param>
        /// <param name="fieldOrder">排序字段</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页显示多少条记录</param>
        /// <param name="recordCount">返回总共多少条记录</param>
        /// <returns></returns>


        public static string Page
        {
            get { return page; }
            set { page = value; }
        }
        static int type;

        public static int Type
        {
            get { return CommontUnit.type; }
            set { CommontUnit.type = value; }
        }


        public DataTable GetUsersByPager(string tableName, string fieldKey, string fieldShow, string whereStr, string fieldOrder, int pageIndex, int pageSize, string groupStr, ref int recordCount)
        {
            string order;

                order = fieldOrder;
            Type = 0; Page = "";
            return APIServiceSQLHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, order, groupStr, whereStr, ref recordCount);
        }

        public List<PMSArea> GetAreaList()
        {
            List<PMSArea> areaList = new List<PMSArea>();
            areaList.Add(new PMSArea() { Code = "cz01", Text = "潮州分公司", Value = "cz01" });
            areaList.Add(new PMSArea() { Code = "cz02", Text = "潮安分公司", Value = "cz02" });
            areaList.Add(new PMSArea() { Code = "cz03", Text = "饶平分公司", Value = "cz03" });
            areaList.Add(new PMSArea() { Code = "czz1", Text = "未知", Value = "czz1" });
            areaList.Add(new PMSArea() { Code = "CZ", Text = "广东", Value = "CZ" });
            areaList.Insert(0, new PMSArea() { Code = "-100", Text = "全部", Value = "-100" });
            return areaList;
        }
    }
}


public class PMSArea
{
    public string Code { get; set; }
    public string Text { get; set; }
    public string Value { get; set; }
}

public class PMSBrnd
{
    public string Code { get; set; }
    public string Text { get; set; }
}
