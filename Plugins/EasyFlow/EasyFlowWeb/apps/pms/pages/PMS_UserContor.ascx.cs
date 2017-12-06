using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class apps_pms_pages_PMS_UserContor : System.Web.UI.UserControl
{
    #region 事件
    //定义委托   
    public delegate void DelegateSeachHander(object serder, EventArgs e);
    //添加一个事件
    public event DelegateSeachHander Seach;//添加事件到句柄
    #endregion

    #region 属性
    //开始时间
    public DateTime StartDate
    {
        get
        {
          return  Convert.ToDateTime(this.txt_StartData.Value);
        }
        set
        {
            this.txt_StartData.Value = value.ToString();
        }
    }

    public DateTime StartDate2
    {
        get
        {
            return Convert.ToDateTime(this.txt_StartData2.Value);
        }
        set
        {
            this.txt_StartData2.Value = value.ToString();
        }
    }
    public string strt
    {
        get { return this.txt_StartData2.Value; }
        set { this.txt_StartData2.Value = value; }
    }
    public string endt
    {
        get { return this.txt_EndData2.Value; }
        set { this.txt_EndData2.Value = value; }
    }

    //结束时间
    public DateTime EndDate
    {
        get
        {
      return  Convert.ToDateTime(this.txt_EndData.Value);
        }
        set
        {
            this.txt_EndData.Value = value.ToString();
        }
    }

    public DateTime EndDate2
    {
        get
        {
            return Convert.ToDateTime(this.txt_EndData2.Value);
        }
        set
        {
            this.txt_EndData2.Value = value.ToString();
        }
    }
    public string Chnl_cd {
        get
        {
            return this.txt_Chnl_cd.Value.Trim();
        }
        set
        {
            this.txt_Chnl_cd.Value = value.Trim();
        }
    }
    public string Chnl_nam
    {
        get
        {
            return this.txt_Chnl_nam.Value.Trim();
        }
        set
        {
            this.txt_Chnl_nam.Value = value.Trim();
        }
    }
    public string BArea
    {
        get
        {
            return this.Area2.Value.Trim();
        }
        set
        {
            this.Area2.Value = value.Trim();
        }
    }
    public string BBrnd
    {
        get
        {
            return this.Brnd2.Value.Trim();
        }
        set
        {
            this.Brnd2.Value = value.Trim();
        }
    }
    //营销编码
    public string Code
    {
        get
        {
            return this.txt_Code.Value.Trim();
        }
        set
        {
            this.txt_Code.Value = value.Trim();
        }
    }
        public string Num
    {
        get
        {
            return this.select_Num.Value.Trim();
        }
        set
        {
            this.select_Num.Value = value.Trim();
        }
    }
    public string City
    {
        get { return this.select_city.Value.Trim(); }
        set { this.select_city.Value = value.Trim(); }
    }
    public string BatchName
    {
        get { return this.bitchname.Value.Trim(); }
        set { this.bitchname.Value = value.Trim(); }
    }
    //营销名称
    public string Name
    {
        get
        {
            return this.txt_Name.Value.Trim();
        }
        set
        {
            this.txt_Name.Value = value.Trim();
        }
    }
    public string Ebox_typ
    {
        get {
         return   this.ebox_typ.Value.Trim();
        }
        set {
            this.ebox_typ.Value = value.Trim();
        }
    }
    //地区
    public string Area
    {
        get
        {
            return this.select_Area.Value.Trim();
        }
        set
        {
            this.select_Area.Value = value.Trim();
        }
    }
    public string Thres
    {
        get {
            return this.thres.Value;
        }
        set {
            this.thres.Value = value;
        }
    }
    //品牌
    public string Brnd
    {
     
          get
        {
            return this.select_Brnd.Value.Trim();
        }
        set
        {
            this.select_Brnd.Value = value.Trim();
        }
    }
    #endregion

    #region 后台方法
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txt_Code.MaxLength = int.MaxValue;

            try
            {
                this.txt_StartData2.Value = strt;
                this.txt_EndData2.Value = endt;
            }
            catch { }
            //LoadAreaData();
            //LoadBrndData();
        }
    }
    protected void btn_Seach_Click(object sender, EventArgs e)
    {
        if (Seach != null)
        {
            Seach(this, new EventArgs());
        }
    }
    #endregion

    #region 私有方法
    ///// <summary>
    ///// 加载品牌数据
    ///// </summary>
    //private void LoadBrndData()
    //{
    //    this.select_Brnd.DataSource = CommontUnit.Instance.GetBrndList();
    //    this.select_Brnd.DataTextField = "Text";
    //    this.select_Brnd.DataValueField = "Code";
    //    this.DataBind();
    //}

    // <summary>
    // 加载地区数据
    // </summary>
    //private void LoadAreaData()
    //{
    //    this.select_Area.DataSource = CommontUnit.Instance.GetAreaList();
    //    this.select_Area.DataTextField = "Text";
    //    this.select_Area.DataValueField = "Value";
    //    this.select_Area.DataBind();
    //}
    #endregion
}