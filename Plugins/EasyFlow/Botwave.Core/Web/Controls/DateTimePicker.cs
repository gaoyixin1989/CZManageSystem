using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.Design;

namespace Botwave.Web.Controls
{
    /// <summary>
    /// [2008-3-28 (Frank)修正]
    /// 1.增加日期文本输入框的样式类属性.
    /// 2.修正一个页面有多个当前日期选择控件时，重复注册 CSS 样式表和引用日历 JavaScript.
    /// 3.修正 CalendarFolder 路径以"站点相对路径"格式显示.
    /// </summary>
    [
    ToolboxData("<{0}:DateTimePicker runat=\"server\"></{0}:DateTimePicker>"),
    DefaultProperty("DateType"),
    Designer(typeof(CompositeControlDesigner))
    ]
    public class DateTimePicker : WebControl, INamingContainer
    {
        /// <summary>
        /// 验证时间格式正确与否的正则表达式.
        /// </summary>
        protected const string DateTimeRegex = @"^\s*(([1-3]\d{3})|(\d{2}))([-./])((0?[1-9])|(1[0-2]))([-./])(([0-2]?[0-9])|(3[0-1]))\s*(\s((([0-1]?[0-9])|(2[0-3]))\:([0-5]?[0-9])((\s)|(\:([0-5]?[0-9])))))?$";
        /// <summary>
        /// 时间选择文本框控件格式({0}:名称;{1}:值).
        /// </summary>
        protected const string DatePickSpan = "<span class=\"ico_pickdate\" title=\"点击选择日期\" onclick=\"return showCalendar('{0}', '{1}', '24', true);\">&nbsp;</span>";

        #region getter/setter
        private string _calendarFolder = "calendar";
        private DateFormat _datetype;
        private bool _isRequired = false;
        private bool _isValidateExpression = true;
        private ValidatorDisplay _validatorDisplay = ValidatorDisplay.Dynamic;
        private string _requiredErrorMessage = "日期不能为空.";
        private string _expressionErrorMessage = "日期格式错误.";
        private string _requiredValidatorText;
        private string _expressionValidatorText;
        private string _inputBoxCssClass;
        private bool _setFocusOnError = false;
        private string _my97Options;

        private TextBox inputTextBox;
        private RequiredFieldValidator requiredValidator;  // 日期必须填写验证表达式.
        private RegularExpressionValidator regexValidator; // 日期验证正则表达式.

        /// <summary>
        /// 构造方法.
        /// </summary>
        public DateTimePicker()
        {
            this.inputTextBox = new TextBox();
            this.regexValidator = new RegularExpressionValidator();
        }

        /// <summary>
        /// 日期格式.
        /// </summary>
        public enum DateFormat
        {
            /// <summary>
            /// 如:"yyyy-MM-dd".
            /// </summary>
            OnlyDate,
            /// <summary>
            /// 如:"yyyy-MM-dd HH:mm:ss".
            /// </summary>
            Datetime
        }

        /// <summary>
        /// 选择后的日期
        /// </summary>
        [Browsable(false)]
        public string Text
        {
            get
            {
                EnsureChildControls();
                return inputTextBox.Text.Trim();
            }
            set
            {
                EnsureChildControls();
                inputTextBox.Text = value;
            }
        }

        /// <summary>
        /// 时间格式.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        public string My97Options
        {
            get { return _my97Options; }
            set { _my97Options = value; }
        }

        /// <summary>
        /// 是否验证日期是否填写.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("False")]
        [Description("是否验证日期是否填写.")]
        public bool IsRequired
        {
            get { return _isRequired; }
            set { _isRequired = value; }
        }

        /// <summary>
        /// 是否验证日期格式是否正确.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("是否验证日期格式是否正确.")]
        public bool IsValidateExpression
        {
            get { return _isValidateExpression; }
            set { _isValidateExpression = value; }
        }

        /// <summary>
        /// 验证显示的类型.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("验证显示的类型.")]
        public ValidatorDisplay ValidatorDisplay
        {
            get { return _validatorDisplay; }
            set { _validatorDisplay = value; }
        }

        /// <summary>
        /// 验证是否必须填写的错误信息.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("日期不能为空")]
        [Description("验证是否必须填写的错误信息.")]
        public string RequiredErrorMessage
        {
            get { return _requiredErrorMessage; }
            set { _requiredErrorMessage = value; }
        }

        /// <summary>
        /// 当错误是否将焦点放入日期文本框.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("当错误是否将焦点放入日期文本框.")]
        public bool SetFocusOnError
        {
            get { return _setFocusOnError; }
            set { _setFocusOnError = value; }
        }

        /// <summary>
        /// 验证日期格式的错误信息.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("日期格式错误.")]
        [Description("验证日期格式的错误信息.")]
        public string ExpressionErrorMessage
        {
            get { return _expressionErrorMessage; }
            set { _expressionErrorMessage = value; }
        }

        /// <summary>
        /// 时间必填验证的提示信息.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        public string RequiredValidatorText
        {
            get { return _requiredValidatorText; }
            set { _requiredValidatorText = value; }
        }

        /// <summary>
        /// 时间格式验证的提示信息.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        public string ExpressionValidatorText
        {
            get { return _expressionValidatorText; }
            set { _expressionValidatorText = value; }
        }



        /// <summary>
        /// 时间输入框的日期显示类型.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("OnlyDate")]
        [Description("日期显示格式")]
        public DateFormat DateType
        {
            get { return _datetype; }
            set { _datetype = value; }
        }

        /// <summary>
        /// 日期的目录路径（为当前站点相对路径，如在 Calendar 目录下则为：calendar;不含\"../\"）.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("日期的目录路径（为当前站点相对路径，如在 Calendar 目录下则为：calendar;不含\"../\"）.")]
        public string CalendarFolder
        {
            get { return _calendarFolder; }
            set { _calendarFolder = value; }
        }

        /// <summary>
        /// 日期输入框（input）的样式类名称.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("日期输入框（input）的样式类名称.")]
        public string InputBoxCssClass
        {
            get { return _inputBoxCssClass; }
            set { _inputBoxCssClass = value; }
        }

        #endregion

        #region override properties

        /// <summary>
        /// 设置文本框的宽度.
        /// </summary>
        public override Unit Width
        {
            get
            {
                if (this.inputTextBox != null)
                    return this.inputTextBox.Width;
                return 0;
            }
            set
            {
                if (this.inputTextBox != null)
                    this.inputTextBox.Width = value;

            }
        }

        /// <summary>
        /// 设置文本框的高度.
        /// </summary>
        public override Unit Height
        {
            get
            {
                if (this.inputTextBox != null)
                    return this.inputTextBox.Height;
                return 0;
            }
            set
            {
                if (this.inputTextBox != null)
                    this.inputTextBox.Height = value;
            }
        }

        /// <summary>
        /// 子控件.
        /// </summary>
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        /// <summary>
        /// 呈现的前续处理.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string calendarFolderUrl = WebUtils.GetAppPath() + this.CalendarFolder;
            // 修正多个日期控件重复注册脚本引用所引发溢出.
            ControlCollection headerControls = this.Page.Header.Controls;
            bool isRegistered = false;
            foreach (Control item in headerControls)
            {
                HtmlLink link = item as HtmlLink;
                if (link != null)
                {
                    if (link.Attributes["href"].EndsWith(this.CalendarFolder + "/skins/aqua/theme.css", StringComparison.OrdinalIgnoreCase)) // 存在脚本引用
                    {
                        isRegistered = true;
                        break;
                    }
                }
            }
            if (!isRegistered)
            {
                this.RegisterCalendarSkin(calendarFolderUrl);
                this.RegisterCalendarScripts(calendarFolderUrl);
            }
        }

        /// <summary>
        /// 重写创建子控件方法.
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.inputTextBox.ID = "txtDate";
            if (!string.IsNullOrEmpty(this.InputBoxCssClass))
                this.inputTextBox.CssClass = this.InputBoxCssClass;
            string strOnclickEvent = "WdatePicker()";
            if (!string.IsNullOrEmpty(this._my97Options))
                strOnclickEvent = "WdatePicker({" + this._my97Options + "})";
            this.inputTextBox.Attributes.Add("onclick", strOnclickEvent);
            if (this.inputTextBox.Width.Value < 95)
                this.inputTextBox.Width = 95;
            this.inputTextBox.Style.Add(HtmlTextWriterStyle.PaddingLeft, "10px");

            this.Controls.Add(inputTextBox);
            this.Controls.Add(new LiteralControl("<i class='icon-data'></i>"));

            // 日期格式验证
            if (this._isValidateExpression)
            {
                this.regexValidator.ID = "rev" + this.ID;
                this.regexValidator.ControlToValidate = inputTextBox.ID;
                this.regexValidator.ValidationExpression = DateTimeRegex;
                this.regexValidator.Display = this._validatorDisplay;
                this.regexValidator.Text = string.IsNullOrEmpty(this._expressionValidatorText) ? this._expressionErrorMessage : this._expressionValidatorText;
                this.regexValidator.ErrorMessage = this._expressionErrorMessage;
                this.regexValidator.SetFocusOnError = this._setFocusOnError;
                this.Controls.Add(this.regexValidator);
            }

            // 日期必须填写验证
            if (this._isRequired)
            {
                this.requiredValidator = new RequiredFieldValidator();
                this.requiredValidator.ControlToValidate = inputTextBox.ID;
                this.requiredValidator.ID = "validator" + this.ID;
                this.requiredValidator.Text = string.IsNullOrEmpty(this._requiredValidatorText) ? this._requiredErrorMessage : this._requiredValidatorText;
                this.requiredValidator.ErrorMessage = this._requiredErrorMessage;
                this.requiredValidator.Display = _validatorDisplay;
                this.requiredValidator.SetFocusOnError = this._setFocusOnError;
                this.Controls.Add(requiredValidator);
            }
        }

        /// <summary>
        /// 重写呈现控件的方法.
        /// </summary>
        /// <param name="writer"></param>
        //public override void RenderControl(HtmlTextWriter writer)
        //{
        //    this.inputTextBox.RenderControl(writer);
        //    if (this.Enabled)
        //    {
        //        writer.Write(string.Format(DatePickSpan, inputTextBox.ClientID, DateType.Equals(DateFormat.Datetime) ? "%Y-%m-%d %H:%M:%S" : "%Y-%m-%d"));
        //    }
        //    if (this._isValidateExpression)
        //    {
        //        this.regexValidator.RenderControl(writer);
        //    }
        //    if (this._isRequired)
        //    {
        //        this.requiredValidator.RenderControl(writer);
        //    }
        //}
        //将该控件修改为使用my97，因此删除上方重写的方法-gyx201600714

        #endregion Overriden properties

        #region DateTimeScriptRegister

        /// <summary>
        /// 注册日期选择脚本引用.
        /// </summary>
        /// <param name="calendarFolderUrl"></param>
        private void RegisterCalendarScripts(string calendarFolderUrl)
        {
            RegisterScriptReference(this.Page, calendarFolderUrl + "/calendar_all.js");
            //RegisterScriptReference(this.Page, WebUtils.GetAppPath() + "/res/My97/WdatePicker.js");
            RegisterScriptReference(this.Page, WebUtils.GetAppPath() + "/res/DatePicker/WdatePicker.js");
        }

        /// <summary>
        /// 注册日期选择的样式文件引用
        /// </summary>
        /// <param name="calendarFolderUrl"></param>
        private void RegisterCalendarSkin(string calendarFolderUrl)
        {
            RegisterCssReference(this.Page, calendarFolderUrl + "/skins/aqua/theme.css");
            RegisterCssReference(this.Page, WebUtils.GetAppPath() + "/App_Themes/font/iconfont.css");
        }

        /// <summary>
        /// 注册样式表文件引用.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="cssFile"></param>
        static void RegisterCssReference(Page currentPage, string cssFile)
        {
            currentPage.Header.Controls.Add(new LiteralControl("\r\n"));
            HtmlLink cssLink = new HtmlLink();
            cssLink.Attributes["type"] = "text/css";
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["href"] = cssFile;
            currentPage.Header.Controls.Add(cssLink);
        }

        /// <summary>
        /// 注册脚本引用的方法.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="scriptFilePath"></param>
        static void RegisterScriptReference(Page currentPage, string scriptFilePath)
        {
            currentPage.Header.Controls.Add(new LiteralControl("\r\n"));
            HtmlGenericControl scriptReference = new HtmlGenericControl();
            scriptReference.TagName = "script";
            scriptReference.Attributes["type"] = "text/javascript";
            scriptReference.Attributes["language"] = "javascript";
            scriptReference.Attributes["src"] = scriptFilePath;
            currentPage.Header.Controls.Add(scriptReference);
        }
        #endregion 
    }

    #region Designer

    /// <summary>
    /// 组合式控件设计器.
    /// </summary>
    public class CompositeControlDesigner : ControlDesigner
    {
        /// <summary>
        /// 获取设计时 Html.
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {
            ControlCollection controls = ((Control)Component).Controls;
            return base.GetDesignTimeHtml();
        }

        /// <summary>
        /// 初始化设计器.
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            if (!(component is Control) &&
                !(component is INamingContainer))
            {
                throw new ArgumentException(
                    "Component must be a container control.", "component");
            }
            base.Initialize(component);
        }
    }
    #endregion
}
