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
    /// [2008-3-28 (Frank)����]
    /// 1.���������ı���������ʽ������.
    /// 2.����һ��ҳ���ж����ǰ����ѡ��ؼ�ʱ���ظ�ע�� CSS ��ʽ����������� JavaScript.
    /// 3.���� CalendarFolder ·����"վ�����·��"��ʽ��ʾ.
    /// </summary>
    [
    ToolboxData("<{0}:DateTimePicker runat=\"server\"></{0}:DateTimePicker>"),
    DefaultProperty("DateType"),
    Designer(typeof(CompositeControlDesigner))
    ]
    public class DateTimePicker : WebControl, INamingContainer
    {
        /// <summary>
        /// ��֤ʱ���ʽ��ȷ����������ʽ.
        /// </summary>
        protected const string DateTimeRegex = @"^\s*(([1-3]\d{3})|(\d{2}))([-./])((0?[1-9])|(1[0-2]))([-./])(([0-2]?[0-9])|(3[0-1]))\s*(\s((([0-1]?[0-9])|(2[0-3]))\:([0-5]?[0-9])((\s)|(\:([0-5]?[0-9])))))?$";
        /// <summary>
        /// ʱ��ѡ���ı���ؼ���ʽ({0}:����;{1}:ֵ).
        /// </summary>
        protected const string DatePickSpan = "<span class=\"ico_pickdate\" title=\"���ѡ������\" onclick=\"return showCalendar('{0}', '{1}', '24', true);\">&nbsp;</span>";

        #region getter/setter
        private string _calendarFolder = "calendar";
        private DateFormat _datetype;
        private bool _isRequired = false;
        private bool _isValidateExpression = true;
        private ValidatorDisplay _validatorDisplay = ValidatorDisplay.Dynamic;
        private string _requiredErrorMessage = "���ڲ���Ϊ��.";
        private string _expressionErrorMessage = "���ڸ�ʽ����.";
        private string _requiredValidatorText;
        private string _expressionValidatorText;
        private string _inputBoxCssClass;
        private bool _setFocusOnError = false;
        private string _my97Options;

        private TextBox inputTextBox;
        private RequiredFieldValidator requiredValidator;  // ���ڱ�����д��֤���ʽ.
        private RegularExpressionValidator regexValidator; // ������֤������ʽ.

        /// <summary>
        /// ���췽��.
        /// </summary>
        public DateTimePicker()
        {
            this.inputTextBox = new TextBox();
            this.regexValidator = new RegularExpressionValidator();
        }

        /// <summary>
        /// ���ڸ�ʽ.
        /// </summary>
        public enum DateFormat
        {
            /// <summary>
            /// ��:"yyyy-MM-dd".
            /// </summary>
            OnlyDate,
            /// <summary>
            /// ��:"yyyy-MM-dd HH:mm:ss".
            /// </summary>
            Datetime
        }

        /// <summary>
        /// ѡ��������
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
        /// ʱ���ʽ.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        public string My97Options
        {
            get { return _my97Options; }
            set { _my97Options = value; }
        }

        /// <summary>
        /// �Ƿ���֤�����Ƿ���д.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("False")]
        [Description("�Ƿ���֤�����Ƿ���д.")]
        public bool IsRequired
        {
            get { return _isRequired; }
            set { _isRequired = value; }
        }

        /// <summary>
        /// �Ƿ���֤���ڸ�ʽ�Ƿ���ȷ.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("�Ƿ���֤���ڸ�ʽ�Ƿ���ȷ.")]
        public bool IsValidateExpression
        {
            get { return _isValidateExpression; }
            set { _isValidateExpression = value; }
        }

        /// <summary>
        /// ��֤��ʾ������.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("��֤��ʾ������.")]
        public ValidatorDisplay ValidatorDisplay
        {
            get { return _validatorDisplay; }
            set { _validatorDisplay = value; }
        }

        /// <summary>
        /// ��֤�Ƿ������д�Ĵ�����Ϣ.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("���ڲ���Ϊ��")]
        [Description("��֤�Ƿ������д�Ĵ�����Ϣ.")]
        public string RequiredErrorMessage
        {
            get { return _requiredErrorMessage; }
            set { _requiredErrorMessage = value; }
        }

        /// <summary>
        /// �������Ƿ񽫽�����������ı���.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("�������Ƿ񽫽�����������ı���.")]
        public bool SetFocusOnError
        {
            get { return _setFocusOnError; }
            set { _setFocusOnError = value; }
        }

        /// <summary>
        /// ��֤���ڸ�ʽ�Ĵ�����Ϣ.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("���ڸ�ʽ����.")]
        [Description("��֤���ڸ�ʽ�Ĵ�����Ϣ.")]
        public string ExpressionErrorMessage
        {
            get { return _expressionErrorMessage; }
            set { _expressionErrorMessage = value; }
        }

        /// <summary>
        /// ʱ�������֤����ʾ��Ϣ.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        public string RequiredValidatorText
        {
            get { return _requiredValidatorText; }
            set { _requiredValidatorText = value; }
        }

        /// <summary>
        /// ʱ���ʽ��֤����ʾ��Ϣ.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        public string ExpressionValidatorText
        {
            get { return _expressionValidatorText; }
            set { _expressionValidatorText = value; }
        }



        /// <summary>
        /// ʱ��������������ʾ����.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("OnlyDate")]
        [Description("������ʾ��ʽ")]
        public DateFormat DateType
        {
            get { return _datetype; }
            set { _datetype = value; }
        }

        /// <summary>
        /// ���ڵ�Ŀ¼·����Ϊ��ǰվ�����·�������� Calendar Ŀ¼����Ϊ��calendar;����\"../\"��.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("���ڵ�Ŀ¼·����Ϊ��ǰվ�����·�������� Calendar Ŀ¼����Ϊ��calendar;����\"../\"��.")]
        public string CalendarFolder
        {
            get { return _calendarFolder; }
            set { _calendarFolder = value; }
        }

        /// <summary>
        /// ���������input������ʽ������.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("���������input������ʽ������.")]
        public string InputBoxCssClass
        {
            get { return _inputBoxCssClass; }
            set { _inputBoxCssClass = value; }
        }

        #endregion

        #region override properties

        /// <summary>
        /// �����ı���Ŀ��.
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
        /// �����ı���ĸ߶�.
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
        /// �ӿؼ�.
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
        /// ���ֵ�ǰ������.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string calendarFolderUrl = WebUtils.GetAppPath() + this.CalendarFolder;
            // ����������ڿؼ��ظ�ע��ű��������������.
            ControlCollection headerControls = this.Page.Header.Controls;
            bool isRegistered = false;
            foreach (Control item in headerControls)
            {
                HtmlLink link = item as HtmlLink;
                if (link != null)
                {
                    if (link.Attributes["href"].EndsWith(this.CalendarFolder + "/skins/aqua/theme.css", StringComparison.OrdinalIgnoreCase)) // ���ڽű�����
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
        /// ��д�����ӿؼ�����.
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

            // ���ڸ�ʽ��֤
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

            // ���ڱ�����д��֤
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
        /// ��д���ֿؼ��ķ���.
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
        //���ÿؼ��޸�Ϊʹ��my97�����ɾ���Ϸ���д�ķ���-gyx201600714

        #endregion Overriden properties

        #region DateTimeScriptRegister

        /// <summary>
        /// ע������ѡ��ű�����.
        /// </summary>
        /// <param name="calendarFolderUrl"></param>
        private void RegisterCalendarScripts(string calendarFolderUrl)
        {
            RegisterScriptReference(this.Page, calendarFolderUrl + "/calendar_all.js");
            //RegisterScriptReference(this.Page, WebUtils.GetAppPath() + "/res/My97/WdatePicker.js");
            RegisterScriptReference(this.Page, WebUtils.GetAppPath() + "/res/DatePicker/WdatePicker.js");
        }

        /// <summary>
        /// ע������ѡ�����ʽ�ļ�����
        /// </summary>
        /// <param name="calendarFolderUrl"></param>
        private void RegisterCalendarSkin(string calendarFolderUrl)
        {
            RegisterCssReference(this.Page, calendarFolderUrl + "/skins/aqua/theme.css");
            RegisterCssReference(this.Page, WebUtils.GetAppPath() + "/App_Themes/font/iconfont.css");
        }

        /// <summary>
        /// ע����ʽ���ļ�����.
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
        /// ע��ű����õķ���.
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
    /// ���ʽ�ؼ������.
    /// </summary>
    public class CompositeControlDesigner : ControlDesigner
    {
        /// <summary>
        /// ��ȡ���ʱ Html.
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {
            ControlCollection controls = ((Control)Component).Controls;
            return base.GetDesignTimeHtml();
        }

        /// <summary>
        /// ��ʼ�������.
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
