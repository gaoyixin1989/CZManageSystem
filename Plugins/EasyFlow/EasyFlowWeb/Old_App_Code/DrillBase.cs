using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using Botwave.Security.Web;
using System.Data;
using System.Reflection;
using System.Threading;
using Botwave.Extension.IBatisNet;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using Botwave.XQP.Domain;
using Botwave.Web;
/// <summary>
///DrillBase 的摘要说明
/// </summary>
public class DrillBase : Botwave.Security.Web.PageBase
{
    public DrillBase()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //WebUtils.RegisterCSSReference(this.Page, AppPath + "App_Themes/gmcc/default/style.css","");
        //WebUtils.RegisterCSSReference(this.Page, AppPath + "App_Themes/gmcc/div_sh.css", "");
        //WebUtils.RegisterCSSReference(this.Page, AppPath + "javascript/libs/css/import_basic.css", "");
        //WebUtils.RegisterCSSReference(this.Page, AppPath + "javascript/libs/js/tree/ztree/ztree.css","");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/common.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-latest.pack.js");
        //  WebUtils.RegisterScriptReference(this.Page, AppPath + "javascript/jquery-1.4.1.min.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-1.7.2.min.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/HideFieldJs.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery_custom.js");
        //WebUtils.RegisterScriptReference(this.Page, AppPath + "javascript/jquery132min.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Frienddetail.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery.showLoading.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/My97/WdatePicker.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery.validate.min.js");
        //WebUtils.RegisterScriptReference(this.Page, AppPath + "javascript/libs/js/framework.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery.easyui.min.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Base64.js");
        //WebUtils.RegisterScriptReference(this.Page, AppPath + "javascript/libs/js/tree/ztree/ztree.js");
        //WebUtils.RegisterScriptReference(this.Page, AppPath + "javascript/libs/js/form/selectTree.js");

    }
    protected string name = "";
    protected int pageIndex = 0;
    protected int total;
    protected int pageCount = 0;
    protected int pageSize = 8;
    protected string dataJOSN;
    private string field;
    protected DataTable dt;
    public string Title { get; set; }
    public string OrderStr { get; set; }
    protected Thread thred;
    protected string fromHtml;
    protected Ucs_Reportforms reportform;
    protected IDictionary<string, string> WhereDict { get; set; }

    /// 调用后台存储过程
    /// </summary>
    protected void Take_Back_PRO()
    {
        try
        {
            IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "PRO_Take_all");
        }
        catch
        {

        }

    }
    /// <summary>
    /// 获取最大月份
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    protected string GetMaxMonth(string datasource)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["TM_INTRVL_CD"])) { return Request.QueryString["TM_INTRVL_CD"]; }
        object obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("select max(TM_INTRVL_CD) from {0} ", datasource));
        if (obj != null)
        {
            return obj.ToString();
        }
        else
        {
            return null;
        }
    }

    protected string Field
    {
        get { return field; }
        set { field = value; }
    }

    public string GetField(List<UCS_FromField> list)
    {
        StringBuilder sb = new StringBuilder();
        if (list.Count == 0) { return null; }
        foreach (var item in list)
        {
            if (item.IsSelect)
            {
                sb.Append(item.field);
                sb.Append("|");
            }
        }
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }





    public string GetFieldText(List<UCS_FromField> list)
    {
        StringBuilder sb = new StringBuilder();
        if (list.Count == 0) { return null; }
        foreach (var item in list)
        {
            if (item.IsSelect&&item.IsShow)
            {
                sb.Append(item.fieldname);
                sb.Append(",");
            }
        }
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();

    }

    protected string VIPStr(string code)
    {
        string result = string.Empty;
        Dictionary<string, string> vipDict = new Dictionary<string, string>();
        if (code != string.Empty)
        {

            vipDict.Add("3", "银卡客户");
            vipDict.Add("4", "贵宾卡客户");
            vipDict.Add("99", "其它");
            vipDict.Add("100", "优先接入");
            vipDict.Add("-1", "不详");
            vipDict.Add("0", "非VIP客户");
            vipDict.Add("7", "优先接入");
            vipDict.Add("8", "银卡1");
            vipDict.Add("11", "b特殊登机业务");
        }
        vipDict.TryGetValue(code, out code);
        return code;
    }
    protected string UserStateStr(string code)
    {
        string result = string.Empty;
        Dictionary<string, string> vipDict = new Dictionary<string, string>();
        if (code != string.Empty)
        {

            vipDict.Add("US10", "正使用");
            vipDict.Add("US531", "集团预约销户");
            vipDict.Add("US532", "集团产品停用");
            vipDict.Add("US346", "欠费呼出限制");
            vipDict.Add("US24", "强制退网");
            vipDict.Add("US347", "欠费长途限制");
            vipDict.Add("US28", "临时生成资料");
            vipDict.Add("US301", "半停");
            vipDict.Add("US302", "全停");
            vipDict.Add("US21", "有效期到期销户");
            vipDict.Add("US27", "已转换品牌");
        }
        vipDict.TryGetValue(code, out code);
        return code;
    }
    protected string UserTYPStr(string code)
    {
        string result = string.Empty;
        Dictionary<string, string> vipDict = new Dictionary<string, string>();
        if (code != string.Empty)
        {

            vipDict.Add("ocut10", "公务员");
            vipDict.Add("ocut30", "教师");
            vipDict.Add("ocut50", "会计");
            vipDict.Add("ocut70", "军人");
            vipDict.Add("ocut80", "学生");
            vipDict.Add("ocut20", "工程师");
            vipDict.Add("ocut40", "农民");
            vipDict.Add("ocut60", "营销人员");
            vipDict.Add("ocut90", "公司职员");
            vipDict.Add("ocut00", "不详");
        }
        vipDict.TryGetValue(code, out code);
        return code;
    }

    public static List<T2> ExcuteList<T2>(DataTable dt)
    {
        if (dt != null && dt.Rows.Count > 0)
        {
            //6.创建泛型集合对象
            List<T2> list = new List<T2>();
            //7.遍历数据行，将行数据存入 实体对象中，并添加到 泛型集合中list
            foreach (DataRow row in dt.Rows)
            {
                //留言：等学完反射后再讲~~~~！
                //7.1先获得泛型的类型(里面包含该类的所有信息---有什么属性啊，有什么方法啊，什么字段啊....................)
                Type t = typeof(T2);
                //7.2根据类型创建该类型的对象
                T2 model = (T2)Activator.CreateInstance(t);// new Student()
                //7.3根据类型 获得 该类型的 所有属性定义
                PropertyInfo[] properties = t.GetProperties();
                //7.4遍历属性数组
                foreach (PropertyInfo p in properties)
                {
                    //7.4.1获得属性名，作为列名
                    string colName = p.Name;
                    //7.4.2根据列名 获得当前循环行对应列的值
                    object colValue = row[colName];
                    //7.4.3将列值 赋给 model对象的p属性
                    //model.Name=colValue;
                    p.SetValue(model, colValue, null);
                }
                //7.5将装好 了行数据的 实体对象 添加到 泛型集合中 O了！！！
                list.Add(model);
            }
            return list;
        }
        else
        {
            return null;
        }
    }

    protected string[] GetChartField(string field)
    {
        string[] s = field.Split('|');

        for (int i = 0; i < s.Length; i++)
        {
            int index = s[i].ToLower().IndexOf("as ");
            int index2 = s[i].ToLower().IndexOf("numeric(");
            if (index > 0 && index2 < 0)
            {
                s[i] = s[i].Substring(index + 3, s[i].Length - index - 3).Trim();
            }
            else if (index2 > 0)
            {
                s[i] = s[i].Substring(index2 + 18, s[i].Length - index2 - 18).Trim();
            }
        }
        return s;
    }

    /// <summary> 
    /// 将字符串使用base64算法解密 
    /// </summary> 
    /// <param name="code_type">编码类型</param> 
    /// <param name="code">已用base64算法加密的字符串</param> 
    /// <returns>解密后的字符串</returns> 
    public string DecodeBase64(string code_type, string code)
    {
        if (string.IsNullOrEmpty(code))
            return string.Empty;
        string decode = "";
        
        try
        {
            byte[] bytes = Convert.FromBase64String(code);  //将2进制编码转换为8位无符号整数数组. 
            decode = System.Text.Encoding.GetEncoding(code_type).GetString(bytes);  //将指定字节数组中的一个字节序列解码为一个字符串。 
        }
        catch
        {
            decode = code;
        }
        return decode;
    }
    #region 高级查询字段
    public string isshow { get; set; }
    public string usrcnt1 { get; set; }
    public string usrcnt2 { get; set; }
    public string cardbind1 { get; set; }
    public string cardbind2 { get; set; }
    public string cardcnt1 { get; set; }
    public string cardcnt2 { get; set; }
    public string ncardbind1 { get; set; }
    public string ncardbind2 { get; set; }
    public string drift1 { get; set; }
    public string drift2 { get; set; }
    public string ebox_typ11 { get; set; }
    public string ebox_typ12 { get; set; }
    public string ebox_typ21 { get; set; }
    public string ebox_typ22 { get; set; }
    public string ebox_typ31 { get; set; }
    public string ebox_typ32 { get; set; }
    public string chrg_amt1 { get; set; }
    public string chrg_amt2 { get; set; }
    public string Tot_chrg_amt1 { get; set; }
    public string Tot_chrg_amt2 { get; set; }
    public string thres_st { set; get; }
    #endregion
}