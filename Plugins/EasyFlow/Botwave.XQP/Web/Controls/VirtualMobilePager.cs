using System;
using System.IO;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace Botwave.XQP.Web.Controls
{
    #region PagingMode enum

    /// <summary>
    /// 可选取的的分页模式
    /// </summary>
    public enum PagingMode
    {
        /// <summary>
        /// 使用页面缓存
        /// </summary>
        Cached,
        /// <summary>
        /// 没缓存，用SQL分页
        /// </summary>
        NonCached
    }
    #endregion

    #region PagerStyle enum

    /// <summary>
    /// 可选取的的分页样式
    /// </summary>
    public enum PagerStyle
    {
        /// <summary>
        /// 上/下一页
        /// </summary>
        NextPrev,

        /// <summary>
        /// 以图片显示上/下一页
        /// </summary>
        NextPrevImage,

        /// <summary>
        /// 数字形式
        /// </summary>
        NumericPages,

        /// <summary>
        /// 下拉框形式
        /// </summary>
        DropDownList
    }

    #endregion

    #region VirtualRecordCount class
    /// <summary>
    /// 虚拟分页类
    /// </summary>
    public class VirtualRecordCount
    {
        /// <summary>
        /// 记录总数
        /// </summary>
        public int RecordCount;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount;
        /// <summary>
        /// Last一页的记录数
        /// </summary>
        public int RecordsInLastPage;
    }
    #endregion

    #region PageChangedEventArgs class

    /// <summary>
    /// 
    /// </summary>
    public class PageChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public int OldPageIndex;
        /// <summary>
        /// 
        /// </summary>
        public int NewPageIndex;
    }
    #endregion

    #region VirtualPager Control

    /// <summary>
    /// VirtualPager
    /// </summary>
    [DefaultProperty("DataSource")]
    [DefaultEvent("PageIndexChanged")]
    [ToolboxData("<{0}:VirtualMobilePager runat=\"server\" />")]
    public class VirtualMobilePager : WebControl, INamingContainer
    {
        #region  PRIVATE DATA MEMBERS
        // ***********************************************************************
        // PRIVATE members
        //private int _totalRecordCount;
        private DataTable _dataSource1;
        private PagedDataSource _dataSource;
        //private Control _controlToPaginate;
        private string CacheKeyName
        {
            get { return Page.Request.FilePath + "_" + UniqueID + "_Data"; }
        }
        private string CurrentPageText = "&nbsp;页次: {0} / {1} 页";//<b>Page</b> {0} <b>of</b> {1}";
        private string NoPageSelectedText = "";//"No page selected.";
        private string TotalRecordCountText = "&nbsp;总记录数: {0} 条&nbsp;";


        //		private string QueryCountCommandText = "SELECT COUNT(*) FROM ({0}) AS t0";
        //		private string QueryPageCommandText = "SELECT * FROM " + 
        //			"(SELECT TOP {0} * FROM " + 
        //			"(SELECT TOP {1} * FROM ({2}) AS t0 ORDER BY {3} {4}) AS t1 " + 
        //			"ORDER BY {3} {5}) AS t2 " + 
        //			"ORDER BY {3}";

        // ***********************************************************************
        #endregion

        #region CTOR(s)
        // ***********************************************************************
        /// <summary>
        /// Ctor
        /// </summary>
        public VirtualMobilePager()
            : base()
        {
            _dataSource1 = null;
            _dataSource = null;
            //_controlToPaginate = null;

            Font.Name = "verdana";
           // Font.Size = FontUnit.Point(11);
            //BackColor = Color.Gainsboro; 
            BackColor = Color.Empty;
            ForeColor = Color.Black;
            BorderStyle = BorderStyle.None;
            BorderWidth = Unit.Parse("1px");
            PagingMode = PagingMode.NonCached;
            PagerStyle = PagerStyle.NextPrev;
            //SelectCommand = "";
            //ConnectionString = "";
            PageButtonCount = 6;
            CurrentPageIndex = 0;
            ItemsPerPage = 20;
            TotalPages = -1;
            CacheDuration = 60;
            DropDownListWidth = Unit.Point(35);
            DisplayCurrentPage = true;

        }
        // ***********************************************************************
        #endregion

        #region PUBLIC PROGRAMMING INTERFACE
        // ***********************************************************************
        // METHOD ClearCache
        // Removes any data cached for paging
        /// <summary>
        /// Removes any data cached for paging.
        /// </summary>
        public void ClearCache()
        {
            if (PagingMode == PagingMode.Cached)
                Page.Cache.Remove(CacheKeyName);
        }

        // EVENT PageIndexChanged
        // Fires when the pager is about to switch to a new page
        /// <summary>
        /// Fires when the pager is about to switch to a new page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);
        /// <summary>
        /// 页面索引改变事件.
        /// </summary>
        public event PageChangedEventHandler PageIndexChanged;

        /// <summary>
        /// 页面索引改变事件的处理.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPageIndexChanged(PageChangedEventArgs e)
        {
            if (PageIndexChanged != null)
                PageIndexChanged(this, e);
        }
        // ***********************************************************************

        // ***********************************************************************

        /// <summary>
        /// PROPERTY CacheDuration
        /// </summary>
        [Description("Gets and sets for how many seconds the data should stay in the cache"),
        Browsable(false)]
        public int CacheDuration
        {
            get { return Convert.ToInt32(ViewState["CacheDuration"]); }
            set { ViewState["CacheDuration"] = value; }
        }
        // ***********************************************************************

        // ***********************************************************************
        /// <summary>
        /// PROPERTY PagingMode
        /// </summary>
        [Description("Indicates whether the data are retrieved page by page or can be cached"),
        Browsable(false)]
        public PagingMode PagingMode
        {
            get { return (PagingMode)ViewState["PagingMode"]; }
            set { ViewState["PagingMode"] = value; }
        }
        // ***********************************************************************

        // ***********************************************************************
        // PROPERTY PagerStyle
        /// <summary>
        /// Indicates the style of the pager's navigation bar.
        /// </summary>
        [Description("Indicates the style of the pager's navigation bar")]
        public PagerStyle PagerStyle
        {
            get { return (PagerStyle)ViewState["PagerStyle"]; }
            set { ViewState["PagerStyle"] = value; }
        }
        // ***********************************************************************

        // ***********************************************************************
        // PROPERTY ControlToPaginate
        /// <summary>
        /// Gets and sets the name(id) of the control to paginate
        /// </summary>
        [Description("Gets and sets the name(id) of the control to paginate"),
        Browsable(false)]
        public string ControlToPaginate
        {
            get { return Convert.ToString(ViewState["ControlToPaginate"]); }
            set { ViewState["ControlToPaginate"] = value; }
        }
        // ***********************************************************************

        // ***********************************************************************
        // PROPERTY ItemsPerPage
        /// <summary>
        /// 设置或获取每页记录数.
        /// </summary>
        [Description("设置或获取每页记录数")]
        public int ItemsPerPage
        {
            get { return Convert.ToInt32(ViewState["ItemsPerPage"]); }
            set { ViewState["ItemsPerPage"] = value; }
        }
        // ***********************************************************************

        // ***********************************************************************
        // PROPERTY CurrentPageIndex
        /// <summary>
        /// Gets and sets the index of the currently displayed page.
        /// </summary>
        [Description("Gets and sets the index of the currently displayed page")]
        public int CurrentPageIndex
        {
            get { return Convert.ToInt32(ViewState["CurrentPageIndex"]); }
            set
            {
                ViewState["CurrentPageIndex"] = value;
                PageGroupCount = (int)ViewState["CurrentPageIndex"] / PageButtonCount;
            }
        }
        // ***********************************************************************

        // ***********************************************************************
        // PROPERTY ConnectionString
        //		[Description("Gets and sets the connection string to access the database")]
        //		public string ConnectionString
        //		{
        //			get {return Convert.ToString(ViewState["ConnectionString"]);}
        //			set {ViewState["ConnectionString"] = value;}
        //		}
        // ***********************************************************************

        // ***********************************************************************
        // PROPERTY SelectCommand
        //		[Description("Gets and sets the SQL query to get data")]
        //		public string SelectCommand
        //		{
        //			get {return Convert.ToString(ViewState["SelectCommand"]);}
        //			set {ViewState["SelectCommand"] = value;}
        //		}
        // ***********************************************************************

        // ***********************************************************************
        // PROPERTY SortField
        /// <summary>
        /// Gets and sets the sort-by field. It is mandatory in NonCached mode.
        /// </summary>
        [Description("Gets and sets the sort-by field. It is mandatory in NonCached mode."),
        Browsable(false)]
        public string SortField
        {
            get { return Convert.ToString(ViewState["SortKeyField"]); }
            set { ViewState["SortKeyField"] = value; }
        }
        // ***********************************************************************

        // ***********************************************************************
        // PROPERTY PageCount
        // Gets the number of displayable pages 
        /// <summary>
        /// Gets the number of displayable pages.
        /// </summary>
        [Browsable(false)]
        public int PageCount
        {
            get { return TotalPages; }
        }
        // ***********************************************************************

        // ***********************************************************************
        // PROPERTY TotalPages
        // Gets and sets the number of pages to display 
        /// <summary>
        /// Gets and sets the number of pages to display.
        /// </summary>
        protected int TotalPages
        {
            get { return Convert.ToInt32(ViewState["TotalPages"]); }
            set { ViewState["TotalPages"] = value; }
        }
        // ***********************************************************************

        /// <summary>
        /// 获取或设置数据源
        /// </summary>
        [Description("获取或设置数据源"), Browsable(false)]
        public DataTable DataSource
        {
            get
            {
                return (_dataSource1);
            }
            set
            {
                _dataSource1 = value;
            }
        }

        /// <summary>
        /// 总记录数.
        /// </summary>
        [DefaultValue(0), Browsable(false)]
        public int TotalRecordCount
        {
            get { return Convert.ToInt32(ViewState["TotalRecordCount"]); }

            set { ViewState["TotalRecordCount"] = value; }
        }

        /// <summary>
        /// 设置或获取按钮数目,仅当PagerStyle设为NumericPages时生效
        /// </summary>
        [Description("设置或获取按钮数目,仅当PagerStyle设为NumericPages时生效")]
        public int PageButtonCount
        {
            get { return Convert.ToInt32(ViewState["PageButtonCount"]); }

            set
            {
                if (value <= 0) throw new Exception();
                ViewState["PageButtonCount"] = value;
            }
        }

        /// <summary>
        /// 当分页样式为DropDownList时,可设置其宽度
        /// </summary>
        [Description("当分页样式为DropDownList时,可设置其宽度")]
        public Unit DropDownListWidth
        {
            get { return Unit.Parse(ViewState["CboWidth"].ToString()); }

            set { ViewState["CboWidth"] = value; }
        }

        /// <summary>
        /// 获取首页按钮的图片URL
        /// </summary>
        [Description("获取首页按钮的图片URL")]
        public string FirstImageButtonUrl
        {
            get { return (string)ViewState["FirstImageButtonUrl"]; }
            set { ViewState["FirstImageButtonUrl"] = value; }
        }

        /// <summary>
        /// 获取上一页按钮的图片URL
        /// </summary>
        [Description("获取上一页按钮的图片URL")]
        public string PrevImageButtonUrl
        {
            get { return (string)ViewState["PrevImageButtonUrl"]; }
            set { ViewState["PrevImageButtonUrl"] = value; }
        }

        /// <summary>
        /// 获取下一页按钮的图片URL
        /// </summary>
        [Description("获取下一页按钮的图片URL")]
        public string NextImageButtonUrl
        {
            get { return (string)ViewState["NextImageButtonUrl"]; }
            set { ViewState["NextImageButtonUrl"] = value; }
        }

        /// <summary>
        /// 获取末尾页按钮的图片URL
        /// </summary>
        [Description("获取末尾页按钮的图片URL")]
        public string LastImageButtonUrl
        {
            get { return (string)ViewState["LastImageButtonUrl"]; }
            set { ViewState["LastImageButtonUrl"] = value; }
        }

        /// <summary>
        /// 确定是否显示页次
        /// </summary>
        [Description("确定是否显示页次")]
        public bool DisplayCurrentPage
        {
            get { return (bool)ViewState["DisplayCurrentPage"]; }
            set { ViewState["DisplayCurrentPage"] = value; }
        }

        // ***********************************************************************
        // OVERRIDE DataBind
        // Fetches and stores the data
        /// <summary>
        /// Fetches and stores the data.
        /// </summary>
        public override void DataBind()
        {
            // Fires the data binding event
            base.DataBind();

            // Controls must be recreated after data binding 
            ChildControlsCreated = false;

            // Ensures the control exists and is a list control
            //			if (ControlToPaginate == "")
            //				return;

            //			_controlToPaginate = Page.FindControl(ControlToPaginate);
            //			if (_controlToPaginate == null)
            //				return;
            //			if (!(_controlToPaginate is BaseDataList || _controlToPaginate is ListControl))
            //				return;

            // Ensures enough info to connect and query is specified
            //			if (ConnectionString == "" || SelectCommand == "")
            //				return;
            //if (DataSource == null) return;


            // Fetch data
            if (PagingMode == PagingMode.Cached)
                FetchAllData();
            else
            {
                //if (SortField == "")
                //	return;
                FetchPageData();
            }


            //return;

            //// Bind data to the buddy control  绑定相关控件
            //BaseDataList baseDataListControl = null;
            //ListControl listControl = null;
            //if (_controlToPaginate is BaseDataList)
            //{
            //    baseDataListControl = (BaseDataList)_controlToPaginate;
            //    baseDataListControl.DataSource = _dataSource;
            //    baseDataListControl.DataBind();
            //    return;
            //}
            //if (_controlToPaginate is ListControl)
            //{
            //    listControl = (ListControl)_controlToPaginate;
            //    listControl.Items.Clear();
            //    listControl.DataSource = _dataSource;
            //    listControl.DataBind();
            //    return;
            //}
        }
        // ***********************************************************************

        // ***********************************************************************
        // OVERRIDE Render
        // Writes the content to be rendered on the client
        /// <summary>
        /// Writes the content to be rendered on the client.
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            // If in design-mode ensure that child controls have been created.
            // Child controls are not created at this time in design-mode because
            // there's no pre-render stage. Do so for composite controls like this 
            if (Site != null && Site.DesignMode)
                CreateChildControls();

            base.Render(output);
        }
        // ***********************************************************************

        // ***********************************************************************
        // OVERRIDE CreateChildControls
        // Outputs the HTML markup for the control
        /// <summary>
        /// Outputs the HTML markup for the control.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            ClearChildViewState();

            BuildControlHierarchy();
        }
        // ***********************************************************************
        #endregion

        #region PRIVATE HELPER METHODS

        /// <summary>
        /// 页组索引
        /// </summary>
        [DefaultValue(0)]
        private int PageGroupCount
        {
            get { return Convert.ToInt32(ViewState["PageGroupCount"]); }

            set { ViewState["PageGroupCount"] = value; }
        }

        // ***********************************************************************
        // PRIVATE BuildControlHierarchy
        // Control the building of the control's hierarchy
        private void BuildControlHierarchy()
        {
            // Build the surrounding table (one row, two cells)
            Table t = new Table();
            t.CellPadding = 0;
            t.CellSpacing = 0;
            t.Font.Name = Font.Name;
            t.Font.Size = Font.Size;
            t.BorderStyle = BorderStyle;
            t.BorderWidth = BorderWidth;
            t.BorderColor = BorderColor;
            t.Width = Width;
            t.Height = Height;
            t.BackColor = BackColor;
            t.ForeColor = ForeColor;
            HtmlGenericControl  htmlUl =  new HtmlGenericControl ("ul");
            htmlUl.Attributes.Add("class", "pagination");
            // Build the table row
            TableRow row = new TableRow();
            t.Rows.Add(row);
            Control ctLi = new Control();
            // Build the cell with navigation bar
            TableCell cellNavBar = new TableCell();
            if (PagerStyle == PagerStyle.NextPrev)
            {
                Font.Name = "verdana";
                // Font.Size = FontUnit.Point(11);
                //BackColor = Color.Gainsboro; 
                BackColor = Color.Empty;
                ForeColor = Color.Empty;
                BorderStyle = BorderStyle.None;
                BuildNextPrevUI(ctLi);
            }
            //else
            //{
            //    if (PagerStyle == PagerStyle.NextPrevImage)
            //        BuildNextPrevImageUI(cellNavBar);
            //    else
            //    {
            //        if (PagerStyle == PagerStyle.DropDownList)
            //            BuildDropDownListUI(cellNavBar);
            //        else
            //            BuildNumericPagesUI(cellNavBar);
            //    }
            //}

            //BuildGOTOPage(cellNavBar);

            //row.Cells.Add(cellNavBar);
            htmlUl.Controls.Add(ctLi);
            //TableCell celltotalRecord = new TableCell();
            //BuildRecordCount(celltotalRecord);
            //row.Cells.Add(celltotalRecord);
            

            // Build the cell with the page index
            //if (DisplayCurrentPage)
            //{
            //    TableCell cellPageDesc = new TableCell();
            //    cellPageDesc.HorizontalAlign = HorizontalAlign.Right;
            //    BuildCurrentPage(cellPageDesc);
            //    row.Cells.Add(cellPageDesc);
            //}

           
            // Add the table to the control tree
            Controls.Add(htmlUl);
        }
        // ***********************************************************************

        /// <summary>
        ///显示总记录数
        /// </summary>
        /// <update>Ivan于2007-8-23</update>
        /// <param name="cell"></param>
        private void BuildRecordCount(TableCell cell)
        { 
             cell.ForeColor = ForeColor;
             cell.Text = String.Format(TotalRecordCountText, TotalRecordCount);
             cell.Font.Size = Font.Size;
            
        }

        /// <summary>
        /// 跳转输入框
        /// </summary>
        /// <updater>Gavin于2007-8-6修改</updater>
        /// <param name="cell"></param>
        private void BuildGOTOPage(TableCell cell)
        {
            //加个空格
            cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

            TextBox goToPageTxt = new TextBox();
            goToPageTxt.ID = "txtGoToPage";
            goToPageTxt.Font.Size = Font.Size;
            goToPageTxt.Width = 25;
       
            cell.Controls.Add(goToPageTxt);

            //加个空格
            cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

            Button goToPageBtn = new Button();
            goToPageBtn.ID = "btnGoToPage";
            goToPageBtn.Click += new EventHandler(goToPageBtn_Click);
            //nextGroup.Font.Name = "webdings";
            goToPageBtn.Font.Size = Font.Size;//FontUnit.Medium;
            goToPageBtn.ForeColor = ForeColor;
            goToPageBtn.ToolTip = "跳转";
            goToPageBtn.Text = "跳转";
            goToPageBtn.CssClass = "btn2";
            //nextGroup.Enabled = isValidPage && canMoveForward;
            cell.Controls.Add(goToPageBtn);
        }

        /// <summary>
        /// 生成数字形式按钮
        /// </summary>
        /// <param name="cell"></param>
        private void BuildNumericPagesUI(Control ctLi)
        {
            bool isValidPage = (CurrentPageIndex >= 0 && CurrentPageIndex <= TotalPages - 1);
            bool canMoveBack = (CurrentPageIndex > 0) && (PageButtonCount * PageGroupCount > 0);
            bool canMoveForward = (CurrentPageIndex < TotalPages - 1) && (TotalPages > PageButtonCount * (PageGroupCount + 1));
            HtmlGenericControl li = new HtmlGenericControl("li");

            //			bool canMoveBackA = (CurrentPageIndex>0);
            //			bool canMoveForwardA = (CurrentPageIndex<TotalPages-1);

            // Render the First button
            //LinkButton first = new LinkButton();
            //first.ID = "first";
            //first.Click += new EventHandler(first1_Click);
            //first.Font.Name = "webdings";
            //first.Font.Size = Font.Size;//FontUnit.Medium;
            //first.ForeColor = ForeColor;
            //first.ToolTip = "First";
            //first.Text = "<<";
            //first.Enabled = isValidPage && canMoveBack;
            //cell.Controls.Add(first);

            //// Add a separator
            //cell.Controls.Add(new LiteralControl("&nbsp;"));

            // Render the << button
            //if (isValidPage && canMoveBack)
            //{
                
            //    LinkButton prevGroup = new LinkButton();
            //    prevGroup.ID = "prevGroup";
            //    prevGroup.Click += new EventHandler(prevGroup_Click);
            //    //prevGroup.Font.Name = "webdings";
            //    prevGroup.Font.Size = Font.Size;//FontUnit.Medium;
            //    prevGroup.ForeColor = ForeColor;
            //    prevGroup.ToolTip = "上一页";
            //    prevGroup.Text = "‹";
            //    //prevGroup.Enabled = isValidPage && canMoveBack;
            //    li.Controls.Add(prevGroup);
            //    ctLi.Controls.Add(li);
            //}            

            // Add a separator
            //cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

            // Add NumericButton
                       
            li = new HtmlGenericControl("li");
            LinkButton Btn1;
            if (TotalPages <= 0 || CurrentPageIndex == -1)
            {
                Btn1 = new LinkButton();
                Btn1.ID = "A1";
                Btn1.Font.Name = "Arial";
                Btn1.Font.Size = Font.Size;//FontUnit.Small;
                Btn1.ForeColor = ForeColor;
                Btn1.ToolTip = "No page";
                Btn1.Text = "1";
                Btn1.Enabled = false;
                li.Attributes.Add("class", "disabled");
                li.Controls.Add(Btn1);
                ctLi.Controls.Add(li);
                //cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            }
            else
            {
                int i = PageGroupCount * PageButtonCount + 1;
                while (i <= TotalPages & i <= (PageGroupCount * PageButtonCount + PageButtonCount))
                {
                    li = new HtmlGenericControl("li");
                    Btn1 = new LinkButton();
                    Btn1.ID = "A" + i.ToString();
                    Btn1.Font.Name = "Arial";
                    Btn1.Font.Size = Font.Size;//FontUnit.Small;
                    Btn1.ForeColor = ForeColor;
                    Btn1.ToolTip = "Page" + i.ToString();
                    Btn1.Text = i.ToString();
                    if (CurrentPageIndex == i - 1) { 
                        Btn1.Enabled = false;
                        li.Attributes.Add("class", "active");
                    }
                    Btn1.Click += new EventHandler(Btn1_Click);

                    li.Controls.Add(Btn1);

                    //cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    ctLi.Controls.Add(li);
                    i++;
                }

                //throw new Exception(PageGroupCount + " " + PageButtonCount + " " + TotalPages);

            }

            // Render the >> button            
            //if (isValidPage && canMoveForward)
            //{
            //    li = new HtmlGenericControl("li");
            //    LinkButton nextGroup = new LinkButton();
            //    nextGroup.ID = "nextGroup";
            //    nextGroup.Click += new EventHandler(nextGroup_Click);
            //    //nextGroup.Font.Name = "webdings";
            //    nextGroup.Font.Size = Font.Size;//FontUnit.Medium;
            //    nextGroup.ForeColor = ForeColor;
            //    nextGroup.ToolTip = "下一页";
            //    nextGroup.Text = "›";
            //    //nextGroup.Enabled = isValidPage && canMoveForward;
            //    li.Controls.Add(nextGroup);
            //    ctLi.Controls.Add(li);
            //}

            


            //// Add a separator
            //cell.Controls.Add(new LiteralControl("&nbsp;"));

            //// Render the Last button
            //LinkButton last = new LinkButton();
            //last.ID = "last";
            //last.Click += new EventHandler(last1_Click);
            //last.Font.Name = "webdings";
            //last.Font.Size = Font.Size;//FontUnit.Medium;
            //last.ForeColor = ForeColor;
            //last.ToolTip = "Last";
            //last.Text = ">>";
            //last.Enabled = isValidPage && canMoveForward;
            //cell.Controls.Add(last);
        }

        /// <summary>
        /// 生成以图片显示的分页UI
        /// </summary>
        /// <param name="cell"></param>
        private void BuildNextPrevImageUI(TableCell cell)
        {
            bool isValidPage = (CurrentPageIndex >= 0 && CurrentPageIndex <= TotalPages - 1);
            bool canMoveBack = (CurrentPageIndex > 0);
            bool canMoveForward = (CurrentPageIndex < TotalPages - 1);

            if (FirstImageButtonUrl != null && FirstImageButtonUrl != "" && LastImageButtonUrl != null && LastImageButtonUrl != "" || Site != null && Site.DesignMode)
            {
                // Render the << button
                ImageButton firstImg = new ImageButton();
                firstImg.ID = "FirstImg";
                firstImg.ImageUrl = FirstImageButtonUrl;
                firstImg.Click += new ImageClickEventHandler(firstImg_Click); ;
                firstImg.ToolTip = "First page";
                firstImg.Enabled = isValidPage && canMoveBack;
                cell.Controls.Add(firstImg);

                // Add a separator
                cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            }

            // Render the < button
            ImageButton prevImg = new ImageButton();
            prevImg.ID = "PrevImg";
            prevImg.ImageUrl = PrevImageButtonUrl;
            prevImg.Click += new ImageClickEventHandler(prevImg_Click);
            prevImg.ToolTip = "Previous page";
            prevImg.Enabled = isValidPage && canMoveBack;
            cell.Controls.Add(prevImg);

            // Add a separator
            cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

            // Render the > button
            ImageButton nextImg = new ImageButton();
            nextImg.ID = "NextImg";
            nextImg.ImageUrl = NextImageButtonUrl;
            nextImg.Click += new ImageClickEventHandler(nextImg_Click);
            nextImg.ToolTip = "Next page";
            nextImg.Enabled = isValidPage && canMoveForward;
            cell.Controls.Add(nextImg);

            if (FirstImageButtonUrl != null && FirstImageButtonUrl != "" && LastImageButtonUrl != null && LastImageButtonUrl != "" || Site != null && Site.DesignMode)
            {
                // Add a separator
                cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

                // Render the >> button
                ImageButton lastImg = new ImageButton();
                lastImg.ID = "LastImg";
                lastImg.ImageUrl = LastImageButtonUrl;
                lastImg.Click += new ImageClickEventHandler(lastImg_Click);
                lastImg.ToolTip = "Last page";
                lastImg.Enabled = isValidPage && canMoveForward;
                cell.Controls.Add(lastImg);
            }
        }

        // ***********************************************************************
        // PRIVATE BuildNextPrevUI
        // Generates the HTML markup for the Next/Prev navigation bar
        private void BuildNextPrevUI(Control ctLi)
        {
            bool isValidPage = (CurrentPageIndex >= 0 && CurrentPageIndex <= TotalPages - 1);
            bool canMoveBack = (CurrentPageIndex > 0);
            bool canMoveForward = (CurrentPageIndex < TotalPages - 1);
            HtmlGenericControl li = new HtmlGenericControl("li");
            // Render the << button
            LinkButton first = new LinkButton();
            first.ID = "First";
            first.Click += new EventHandler(first_Click);
            //first.Font.Name = "webdings";
            first.Font.Size = Font.Size;//FontUnit.Medium;
            first.ForeColor = ForeColor;
            first.ToolTip = "第一页";
            first.Text = "«";
            if (!(isValidPage && canMoveBack))
            {
                first.Enabled = false;
                li.Attributes.Add("class", "disabled");
            }
            li.Controls.Add(first);

            // Add a separator
            //cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            ctLi.Controls.Add(li);
            // Render the < button
            li = new HtmlGenericControl("li");
            LinkButton prev = new LinkButton();
            prev.ID = "Prev";
            prev.Click += new EventHandler(prev_Click);
            //prev.Font.Name = "webdings";
            prev.Font.Size = Font.Size;//FontUnit.Medium;
            prev.ForeColor = ForeColor;
            prev.ToolTip = "上一页";
            prev.Text = "‹";
            if (!(isValidPage && canMoveBack))
            {
                prev.Enabled = false;
                li.Attributes.Add("class", "disabled");
            }
            li.Controls.Add(prev);
            ctLi.Controls.Add(li);
            BuildNumericPagesUI(ctLi);
            // Add a separator
            //cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            
            // Render the > button
            li = new HtmlGenericControl("li");
            LinkButton next = new LinkButton();
            next.ID = "Next";
            next.Click += new EventHandler(next_Click);
            //next.Font.Name = "webdings";
            next.Font.Size = Font.Size;//FontUnit.Medium;
            next.ForeColor = ForeColor;
            next.ToolTip = "下一页";
            next.Text = "›";
            if (!(isValidPage && canMoveForward))
            {
                next.Enabled = false;
                li.Attributes.Add("class", "disabled");
            }
            li.Controls.Add(next);

            // Add a separator
            //cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            ctLi.Controls.Add(li);
            // Render the >> button
            li = new HtmlGenericControl("li");
            LinkButton last = new LinkButton();
            last.ID = "Last";
            last.Click += new EventHandler(last_Click);
            //last.Font.Name = "webdings";
            last.Font.Size = Font.Size;//FontUnit.Medium;
            last.ForeColor = ForeColor;
            last.ToolTip = "最后页";
            last.Text = "»";
            if (!(isValidPage && canMoveForward))
            {
                last.Enabled = false;
                li.Attributes.Add("class", "disabled");
            }
            li.Controls.Add(last);
            ctLi.Controls.Add(li);
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE BuildDropDownListUI
        // Generates the HTML markup for the DropDownList Pages button bar
        private void BuildDropDownListUI(TableCell cell)
        {
            // Render a drop-down list  
            DropDownList pageList = new DropDownList();
            pageList.ID = "PageList";
            pageList.Width = DropDownListWidth;
            pageList.AutoPostBack = true;
            pageList.SelectedIndexChanged += new EventHandler(PageList_Click);
            pageList.Font.Name = Font.Name;
            pageList.Font.Size = Font.Size;
            pageList.ForeColor = ForeColor;

            // Embellish the list when there are no pages to list 
            if (TotalPages <= 0 || CurrentPageIndex == -1)
            {
                pageList.Items.Add("No pages");
                pageList.Enabled = false;
                pageList.SelectedIndex = 0;
            }
            else // Populate the list
            {
                for (int i = 1; i <= TotalPages; i++)
                {
                    ListItem item = new ListItem(i.ToString(), (i - 1).ToString());
                    pageList.Items.Add(item);
                }
                pageList.SelectedIndex = CurrentPageIndex;
            }
            cell.Controls.Add(pageList);

        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE BuildCurrentPage
        // Generates the HTML markup to describe the current page (0-based)
        private void BuildCurrentPage(TableCell cell)
        {
            // Use a standard template: Page X of Y
            //cell.VerticalAlign = VerticalAlign.Bottom;

            cell.ForeColor = ForeColor;

            if (CurrentPageIndex < 0 || CurrentPageIndex >= TotalPages)
            {
                cell.Text = NoPageSelectedText;
            }
            else
            {
                cell.Text = String.Format(CurrentPageText, (CurrentPageIndex + 1), TotalPages);
            }
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE ValidatePageIndex
        // Ensures the CurrentPageIndex is either valid [0,TotalPages) or -1
        private void ValidatePageIndex()
        {
            if (TotalPages <= 0)
                CurrentPageIndex = -1;
            else
            {
                if (CurrentPageIndex == -1) CurrentPageIndex = 0;

                if (!(CurrentPageIndex >= 0 && CurrentPageIndex < TotalPages))
                {
                    throw new Exception("CurrentPageIndex小于零或已超出范围!");
                }
            }
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE FetchAllData
        // Runs the query for all data to be paged and caches the resulting data
        private void FetchAllData()
        {
            // Looks for data in the ASP.NET Cache
            DataTable data;
            data = (DataTable)Page.Cache[CacheKeyName];
            if (data == null)
            {
                // Fix SelectCommand with order-by info
                //AdjustSelectCommand(true);

                // If data expired or has never been fetched, go to the database
                //SqlDataAdapter adapter = new SqlDataAdapter(SelectCommand, ConnectionString);

                data = DataSource;
                //adapter.Fill(data);

                Page.Cache.Insert(CacheKeyName, data, null,
                    DateTime.Now.AddSeconds(CacheDuration),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }

            // Configures the paged data source component
            if (_dataSource == null)
                _dataSource = new PagedDataSource();
            //_dataSource.DataSource = data.DefaultView; // must be IEnumerable!
            _dataSource.AllowPaging = true;
            _dataSource.PageSize = ItemsPerPage;
            TotalPages = _dataSource.PageCount;

            // Ensures the page index is valid 
            ValidatePageIndex();
            if (CurrentPageIndex == -1)
            {
                _dataSource = null;
                return;
            }

            // Selects the page to view
            _dataSource.CurrentPageIndex = CurrentPageIndex;
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE FetchPageData
        // Runs the query to get only the data that fit into the current page
        private void FetchPageData()
        {
            // Need a validated page index to fetch data.
            // Also need the virtual page count to validate the page index
            //AdjustSelectCommand(false);
            VirtualRecordCount countInfo = CalculateVirtualRecordCount();
            TotalPages = countInfo.PageCount;

            // Validate the page number (ensures CurrentPageIndex is valid or -1)
            ValidatePageIndex();
            if (CurrentPageIndex == -1)
                return;

            // Prepare and run the command
            //			SqlCommand cmd = PrepareCommand(countInfo);
            //			if (cmd == null)
            //				return;

            //			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable data = DataSource;
            //			adapter.Fill(data);

            // Configures the paged data source component
            if (_dataSource == null)
                _dataSource = new PagedDataSource();
            _dataSource.AllowCustomPaging = true;
            _dataSource.AllowPaging = true;
            _dataSource.CurrentPageIndex = 0;
            _dataSource.PageSize = ItemsPerPage;
            _dataSource.VirtualCount = countInfo.RecordCount;
            //_dataSource.DataSource = data.DefaultView;	
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE AdjustSelectCommand
        // Strips ORDER-BY clauses from SelectCommand and adds a new one based
        // on SortKeyField
        private void AdjustSelectCommand(bool addCustomSortInfo)
        {
            // Truncate where ORDER BY is found
            //			string temp = SelectCommand.ToLower();
            //			int pos = temp.IndexOf("order by"); 
            //			if (pos > -1)
            //				SelectCommand = SelectCommand.Substring(0, pos);
            //
            // Add new ORDER BY info if SortKeyField is specified
            //			if (SortField != "" && addCustomSortInfo)
            //				SelectCommand += " ORDER BY " + SortField;
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE CalculateVirtualRecordCount
        // Calculates record and page count for the specified query
        private VirtualRecordCount CalculateVirtualRecordCount()
        {
            VirtualRecordCount count = new VirtualRecordCount();

            // Calculate the virtual number of records from the query
            count.RecordCount = GetQueryVirtualCount();
            count.RecordsInLastPage = ItemsPerPage;

            // Calculate the correspondent number of pages
            int lastPage = count.RecordCount / ItemsPerPage;
            int remainder = count.RecordCount % ItemsPerPage;
            if (remainder > 0)
                lastPage++;
            count.PageCount = lastPage;

            // Calculate the number of items in the last page
            if (remainder > 0)
                count.RecordsInLastPage = remainder;

            return count;
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE PrepareCommand
        // Prepares and returns the command object for the reader-based query
        private SqlCommand PrepareCommand(VirtualRecordCount countInfo)
        {
            // No sort field specified: figure it out
            if (SortField == "")
            {
                // Get metadata for all columns and choose either the primary key
                // or the 
                //string text = "SET FMTONLY ON;"; //+ SelectCommand + ";SET FMTONLY OFF;";
                //SqlDataAdapter adapter;//= new SqlDataAdapter(text, ConnectionString);
                DataTable t = new DataTable();
                //				adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                //				adapter.Fill(t);
                DataColumn col = null;
                if (t.PrimaryKey.Length > 0)
                    col = t.PrimaryKey[0];
                else
                    col = t.Columns[0];
                SortField = col.ColumnName;
            }

            // Determines how many records are to be retrieved.
            // The last page could require less than other pages
            int recsToRetrieve = ItemsPerPage;
            if (CurrentPageIndex == countInfo.PageCount - 1)
                recsToRetrieve = countInfo.RecordsInLastPage;

            //			string cmdText; 
            //= String.Format(QueryPageCommandText, 
            //				recsToRetrieve,						// {0} --> page size
            //				ItemsPerPage*(CurrentPageIndex+1),	// {1} --> size * index
            //				SelectCommand,						// {2} --> base query
            //				SortField,							// {3} --> key field in the query
            //				"ASC",								// Default to ascending order
            //				"DESC");

            //			SqlConnection conn = new SqlConnection(ConnectionString);
            //			SqlCommand cmd = new SqlCommand(cmdText, conn);
            return null;//cmd;
        }

        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE GetQueryVirtualCount
        // Run a query to get the record count
        private int GetQueryVirtualCount()
        {
            //			string cmdText = String.Format(QueryCountCommandText, SelectCommand);
            //			SqlConnection conn = new SqlConnection(ConnectionString);
            //			SqlCommand cmd = new SqlCommand(cmdText, conn);
            //
            //			cmd.Connection.Open();
            //			int recCount = (int) cmd.ExecuteScalar(); 
            //			cmd.Connection.Close();

            //			return recCount;

            return TotalRecordCount;
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE GoToPage
        // Sets the current page index
        private void GoToPage(int pageIndex)
        {
            // Prepares event data
            PageChangedEventArgs e = new PageChangedEventArgs();
            e.OldPageIndex = CurrentPageIndex;
            e.NewPageIndex = pageIndex;

            // Updates the current index
            CurrentPageIndex = pageIndex;

            // Fires the page changed event
            OnPageIndexChanged(e);

            // Binds new data
            DataBind();

        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE first_Click
        // Event handler for the << button
        private void first_Click(object sender, EventArgs e)
        {
            GoToPage(0);
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE prev_Click
        // Event handler for the < button
        private void prev_Click(object sender, EventArgs e)
        {
            GoToPage(CurrentPageIndex - 1);
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE next_Click
        // Event handler for the > button
        private void next_Click(object sender, EventArgs e)
        {
            GoToPage(CurrentPageIndex + 1);
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE last_Click
        // Event handler for the >> button
        private void last_Click(object sender, EventArgs e)
        {
            GoToPage(TotalPages - 1);
        }
        // ***********************************************************************

        // ***********************************************************************
        // PRIVATE PageList_Click
        // Event handler for any page selected from the drop-down page list 
        private void PageList_Click(object sender, EventArgs e)
        {
            DropDownList pageList = (DropDownList)sender;
            GoToPage(Convert.ToInt32(pageList.SelectedValue));
        }
        // ***********************************************************************

        private void Btn1_Click(object sender, EventArgs e)
        {
            LinkButton Lbtn = (LinkButton)sender;
            //Lbtn.Enabled = false;
            GoToPage(int.Parse(Lbtn.Text) - 1);
        }

        private void nextGroup_Click(object sender, EventArgs e)
        {
            //PageGroupCount += 1;
            GoToPage((PageGroupCount + 1) * PageButtonCount);
        }

        private void prevGroup_Click(object sender, EventArgs e)
        {
            //PageGroupCount -= 1;
            GoToPage((PageGroupCount - 1) * PageButtonCount);
        }

        private void first1_Click(object sender, EventArgs e)
        {
            //PageGroupCount = 0;
            GoToPage(0);
        }

        private void last1_Click(object sender, EventArgs e)
        {
            //PageGroupCount = (TotalPages -1) / PageButtonCount;
            GoToPage(TotalPages - 1);
        }

        /// <summary>
        /// 跳转页面事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToPageBtn_Click(object sender, EventArgs e)
        {

            string gotoPageNum = ((TextBox)this.FindControl("txtGoToPage")).Text.Trim();
            int intPage = 0;

            if (int.TryParse(gotoPageNum,out intPage))
            {
                if (intPage <= 1)
                {
                    GoToPage(0);
                }
                else if (intPage >= TotalPages)
                {
                    GoToPage(TotalPages - 1);
                }
                else
                {
                    GoToPage(intPage - 1);
                }
            }
            else
            {
                GoToPage(CurrentPageIndex);
            }

           
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void firstImg_Click(object sender, ImageClickEventArgs e)
        {
            GoToPage(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prevImg_Click(object sender, ImageClickEventArgs e)
        {
            GoToPage(CurrentPageIndex - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextImg_Click(object sender, ImageClickEventArgs e)
        {
            GoToPage(CurrentPageIndex + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lastImg_Click(object sender, ImageClickEventArgs e)
        {
            GoToPage(TotalPages - 1);
        }

        #endregion

    }
    #endregion
}
