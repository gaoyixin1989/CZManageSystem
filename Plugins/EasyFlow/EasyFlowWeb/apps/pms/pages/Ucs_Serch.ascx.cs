using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Botwave.Extension.IBatisNet;
using System.Data;
using Botwave.XQP.Service;
using Botwave.XQP.Domain;
public partial class apps_pms_pages_Ucs_Serch : System.Web.UI.UserControl
{
    #region 事件
    //定义委托   
    public delegate void DelegateSeachHander(object serder, EventArgs e);
    //添加一个事件
    public event DelegateSeachHander Seach;//添加事件到句柄
    #endregion

    private IUcs_ReportformsService ucs_ReportformsService;

    protected IUcs_ReportformsService Ucs_ReportformsService
    {
        get { return ucs_ReportformsService; }
        set { ucs_ReportformsService = value; }
    }

    #region 后台方法
    protected void Page_Load(object sender, EventArgs e)
    {
    

          //  Guid id;
          //  Guid.TryParse(Request.QueryString["formid"],out id);
          //  Ucs_Reportforms reportforms=Ucs_ReportformsService.GetReprotformsByid(id,)
            //LoadAreaData();
            //LoadBrndData();
        //if(!IsPostBack)
            Bind();
            
        
    }
    public Ucs_Reportforms reportforms { get; set; }
    protected StringBuilder sbUp = new StringBuilder();
    protected StringBuilder sbDown = new StringBuilder();
    protected StringBuilder sbHightSeach = new StringBuilder();
    protected IDictionary<string, string> whereDict = new Dictionary<string, string>();
    protected void btn_Seach_Click(object sender, EventArgs e)
    {
        if (Seach != null)
        {
            if (Session["wheredict"] != null)
                Session.Remove("wheredict");
            Seach(this, new EventArgs());
        }
    }
    public void ShowCheckBox()
    {
        chkPeople.Visible = true;
    }
    public bool IsChecked()
    {
        return chkPeople.Checked;
    }
    #endregion
    private void Bind()
    {
        if (Session["wheredict"] != null && !IsPostBack)
            whereDict = (Dictionary<string, string>)Session["wheredict"];
        foreach (var item in reportforms.FieldList.OrderBy(x=>x.whereOrder))
        {

            switch (item.whereStrtype)
            {
                case 1: BindCommonSeach(item); break;
                case 2: CreateHightSeachDiv(item); if (item.Fieldtype == 4) { BindCommonSeach(item); }; break;
                default: break;
            }
        } 
        //if (Session["wheredict"] != null)
        //    Session.Remove("wheredict");
    }

    /// <summary>
    /// 生成普通查询html
    /// </summary>
    /// <param name="model"></param>
    private void BindCommonSeach(UCS_FromField model)
    {
        StringBuilder sb = model.wherePositionType == 1 ? sbUp : sbDown;
        
        switch (model.Fieldtype)
        {
            case 4:
            case 1: CreateInput(model, sb); break;
            case 2:
            case 3: CreateDateInput3(model, sb); break;
            
            case 5: CreateEve_Select(model, sb); break;
            case 6: CreateDateInput2(model, sb); break;
            case 7: CreateDateInput(model, sb);   break;
            default: break;
        }
        
    }

    void CreateDateInput2(UCS_FromField model, StringBuilder sb)
    {
        if (Request.Form[model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()] == null && Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)] != null)
            sb.Append(@"时间:<input type='text' datatype='date' style='width: 70px' value='").Append(Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)]).Append("' name='").Append(model.whereFieldValue).Append("' onclick='WdatePicker({dateFmt:\"").Append("yyyyMM").Append("\"});'").Append("/> ");
        else if (whereDict.ContainsKey(model.field))
            sb.Append(@"时间:<input type='text' datatype='date' style='width: 70px' value='").Append(string.IsNullOrEmpty(whereDict[model.field]) ? string.Empty : whereDict[model.field]).Append("' name='").Append(model.whereFieldValue).Append("' onclick='WdatePicker({dateFmt:\"").Append("yyyyMM").Append("\"});'").Append("/> ");
        else
            sb.Append(@"时间:<input type='text' datatype='date' style='width: 70px' value='").Append(string.IsNullOrEmpty(Request.Form[model.whereFieldValue]) ? string.Empty : Request.Form[model.whereFieldValue]).Append("' name='").Append(model.whereFieldValue).Append("' onclick='WdatePicker({dateFmt:\"").Append("yyyyMM").Append("\"});'").Append("/> ");
    }
    void CreateDateInput3(UCS_FromField model, StringBuilder sb)
    {
        if (Request.Form[model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()] == null && Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)] != null)
            sb.Append(@"时间:<input type='text' datatype='date' style='width: 70px' value='").Append(Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)]).Append("' name='").Append(model.whereFieldValue).Append("' onclick='WdatePicker({dateFmt:\"").Append("yyyyMMdd").Append("\"});'").Append("/> ");
        else if (whereDict.ContainsKey(model.field))
            sb.Append(@"时间:<input type='text' datatype='date' style='width: 70px' value='").Append(string.IsNullOrEmpty(whereDict[model.field]) ? string.Empty : whereDict[model.field]).Append("' name='").Append(model.whereFieldValue).Append("' onclick='WdatePicker({dateFmt:\"").Append("yyyyMMdd").Append("\"});'").Append("/> ");
        else
            sb.Append(@"时间:<input type='text' datatype='date' style='width: 70px' value='").Append(string.IsNullOrEmpty(Request.Form[model.whereFieldValue]) ? string.Empty : Request.Form[model.whereFieldValue]).Append("' name='").Append(model.whereFieldValue).Append("' onclick='WdatePicker({dateFmt:\"").Append("yyyyMMdd").Append("\"});'").Append("/> ");
    }
    /// <summary>
    /// 获取最大月份
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    protected string GetMaxMonth(string datasource)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["TM_INTRVL_CD"])) { return Request.QueryString["TM_INTRVL_CD"]; }
        //object obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("select max(TM_INTRVL_CD) from {0} ", datasource));
        //if (obj != null)
        //{
        //    return obj.ToString();
        //}
        //else
        //{
        //    return null;
        //}
        return null;
    }
    void CreateEve_Select(UCS_FromField model, StringBuilder sb)
    {
         
    }
    //string GetEveDetailId(string str)
    //{
    //  int index1=str.IndexOf("");
    //    return 
    //}
    /// <summary>
    /// 生成预警选择下拉框
    /// </summary>
    /// <param name="model"></param>
    /// <param name="sb"></param>
    void CreateSelect(UCS_FromField model, StringBuilder sb)
    {


        sb.Append(model.fieldname).Append("<select name='thres' val='").Append(Request.Form["thres"]).Append("'><option value=''>全部</option><option value='1'>预警</option><option value='2'>未预警</option></select>");
    }

    /// <summary>
    /// 生成日期查询框框
    /// </summary>
    /// <param name="model"></param>
    /// <param name="sb"></param>
    private void CreateDateInput(UCS_FromField model,StringBuilder sb)
    {
        string dateFmt = model.Fieldtype== 2 ? "yyyy-MM" : "yyyy-MM-dd";
        if (whereDict.ContainsKey(model.field + "1") && whereDict.ContainsKey(model.field + "2"))
            sb.Append(model.fieldname).Append(@"：<input type='text' datatype='date' style='width: 70px' value='").Append(whereDict[model.field + "1"]).Append("' name='").Append(model.whereFieldValue).Append(@"1' onclick=""WdatePicker({dateFmt:'").Append(dateFmt).Append(@"'});"" id='").Append(model.whereFieldValue).Append(@"1' />至<input
                                type='text' datatype='date' style='width: 70px' name='").Append(model.whereFieldValue).Append("2'  value='").Append(whereDict[model.field + "2"]).Append("' id='").Append(model.whereFieldValue).Append("2' onclick=\"WdatePicker({dateFmt:'").Append(dateFmt).Append("'});\" /> ");
        else
            sb.Append(model.fieldname).Append(@"：<input type='text' datatype='date' style='width: 70px' value='").Append(Request.Form[model.whereFieldValue + "1"]).Append("' name='").Append(model.whereFieldValue).Append(@"1' onclick=""WdatePicker({dateFmt:'").Append(dateFmt).Append(@"'});"" id='").Append(model.whereFieldValue).Append(@"1' />至<input
                                type='text' datatype='date' style='width: 70px' name='").Append(model.whereFieldValue).Append("2'  value='").Append(Request.Form[model.whereFieldValue + "2"]).Append("' id='").Append(model.whereFieldValue).Append("2' onclick=\"WdatePicker({dateFmt:'").Append(dateFmt).Append("'});\" /> ");
    }
    /// <summary>
    /// 生成多选下拉框查询
    /// </summary>
    /// <param name="model"></param>
    /// <param name="sb"></param>
    private void CreateInput(UCS_FromField model, StringBuilder sb)
    {

        if (//Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)] != Request.Form["text_" + model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()] 
            Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)]!=null
            && Request.Form[model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()]==null)
            sb.Append(model.fieldname).Append(@"：<input style='width: 70px;' type='text' name='").Append(model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("' value='").Append(Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)]).Append("' id='").Append(model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("' /><input type='button' style='vertical-align: middle;cursor: pointer; background: url(" + Botwave.Web.WebUtils.GetAppPath() + "App_Themes/gmcc/img/btnse01.jpg);border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;'onclick=\"showDiv({isorganization:'").Append(model.isorganization).Append("',tableName:'").Append(model.datasource).Append("',text:'").Append(model.whereFieldText).Append("',value:'").Append(model.whereFieldValue).Append("',fieldWhere:'").Append(model.strWhere).Append("'},'GetMarkData.aspx',{hide:'").Append(model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("',text:'").Append(model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("'})\" /> ");
        //  value =  model.whereFieldValue.Substring(( model.whereFieldValue.ToLower().IndexOf(" as") > 0 ?  model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim();
        else if (whereDict.ContainsKey(model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)))
            sb.Append(model.fieldname).Append(@"：<input style='width: 70px;' type='text' name='").Append(model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("' value='").Append(whereDict[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)].Replace("'",string.Empty)).Append("' id='").Append(model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("' /><input type='button' style='vertical-align: middle;cursor: pointer; background: url(" + Botwave.Web.WebUtils.GetAppPath() + "App_Themes/gmcc/img/btnse01.jpg);border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;'onclick=\"showDiv({isorganization:'").Append(model.isorganization).Append("',tableName:'").Append(model.datasource).Append("',text:'").Append(model.whereFieldText).Append("',value:'").Append(model.whereFieldValue).Append("',fieldWhere:'").Append(model.strWhere).Append("'},'GetMarkData.aspx',{hide:'").Append(model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("',text:'").Append(model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("'})\" /> ");
        else
            sb.Append(model.fieldname).Append(@"：<input style='width: 70px;' type='text' name='").Append(model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("' value='").Append(DecodeBase64("utf-8", Request.Form[model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()])).Append("' id='").Append(model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("' /><input type='button' style='vertical-align: middle;cursor: pointer; background: url(" + Botwave.Web.WebUtils.GetAppPath() + "App_Themes/gmcc/img/btnse01.jpg);border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;'onclick=\"showDiv({isorganization:'").Append(model.isorganization).Append("',tableName:'").Append(model.datasource).Append("',text:'").Append(model.whereFieldText).Append("',value:'").Append(model.whereFieldValue).Append("',fieldWhere:'").Append(model.strWhere).Append("'},'GetMarkData.aspx',{hide:'").Append(model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("',text:'").Append(model.whereFieldText.Substring((model.whereFieldText.ToLower().IndexOf(" as") > 0 ? model.whereFieldText.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("'})\" /> ");
       // CreateHideInput(model, sb);
    }
    /// <summary>
    /// 生成隐藏域
    /// </summary>
    /// <param name="model"></param>
    /// <param name="sb"></param>
    private void CreateHideInput(UCS_FromField model, StringBuilder sb)
    {
        if (//Request.Form[model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()] != Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)] 
            Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)]!=null
            && Request.Form[model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()]==null)//如果搜索条件跟url参数的值相同时，则取页面参数的值
            sb.Append("<input type='hidden' value=\"'").Append(Request.QueryString[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)]).Append("'\" name='").Append(model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("' id='").Append(model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("'").Append(" />");
        else if (whereDict.ContainsKey(model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)))
            sb.Append("<input type='hidden' value=\"").Append(whereDict[model.field.Substring(0, model.field.ToLower().LastIndexOf(" as") > 0 ? model.field.ToLower().LastIndexOf(" as") : model.field.Length)]).Append("\" name='").Append(model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("' id='").Append(model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("'").Append(" />");
        else
        sb.Append("<input type='hidden' value=\"").Append(DecodeBase64("utf-8",Request.Form[model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()])).Append("\" name='").Append(model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("' id='").Append(model.whereFieldValue.Substring((model.whereFieldValue.ToLower().IndexOf(" as") > 0 ? model.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim()).Append("'").Append(" />");
    }

    private void CreateHightSeachDiv(UCS_FromField model)
    {
        if (sbHightSeach.Length == 0)
        {
            sbHightSeach.Append(@"<input type='hidden' id='isshow' name='isshow' onkeyup=""this.value=this.value.replace(/[^\d+.]/g,'')""  value='").Append(Request.Form["isshow"]).Append("' />");
        }
      
     	
           sbHightSeach.Append("<td> &nbsp;").Append(model.fieldname).Append(":</td>").Append(@"<td>
                    <input name='").Append(model.whereFieldText).Append(@"1' type='text' onkeyup=""this.value=this.value.replace(/[^\d+.]/g,'')""
                        value='").Append(Request.Form[model.whereFieldText + "1"]).Append(@"' />至<input type='text' name='").Append(model.whereFieldText).Append(@"2' onkeyup=""this.value=this.value.replace(/[^\d+.]/g,'')""
                            value='").Append(Request.Form[model.whereFieldText + "2"]).Append(@"' />%
                </td>");

      	
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
        byte[] bytes = Convert.FromBase64String(code);  //将2进制编码转换为8位无符号整数数组. 
        try
        {
            decode = System.Text.Encoding.GetEncoding(code_type).GetString(bytes);  //将指定字节数组中的一个字节序列解码为一个字符串。 
        }
        catch
        {
            decode = code;
        }
        return decode;
    }
}   