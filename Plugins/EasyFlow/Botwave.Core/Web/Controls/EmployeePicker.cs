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
    /// 员工选择控件.
    /// </summary>
    [
    ToolboxData("<{0}:EmployeePicker runat=\"server\"></{0}:EmployeePicker>"),
    DefaultProperty("EmployeeFile"),
    Designer(typeof(CompositeControlDesigner))
    ]
    public class EmployeePicker : WebControl, INamingContainer
    {
        /// <summary>
        /// 员工选择块 html 格式.
        /// </summary>
        protected const string EmployeePickSpan = "<span id=\"spanPickUser\" onclick=\"window.open('{0}?pId={1}','SelectEmployee', 'width=600;height=600;');\" class=\"ico_pickdate\" title=\"点击选择人员\">&nbsp;</span>";

        #region getter/setter
        private string _EmployeFile = "SelectEmployee.aspx";
        private TextBox _dtTextBox;
        private HiddenField _hdTextBox;
        private RequiredFieldValidator _urValidator;
        private ValidateFormat _isValidate;
        private string _errorMessage = "用户不能为空";       

        /// <summary>
        /// 验证类型枚举
        /// </summary>
        public enum ValidateFormat
        {
            /// <summary>
            /// 是.
            /// </summary>
            True,
            /// <summary>
            /// 否.
            /// </summary>
            False
        }

        /// <summary>
        /// 用户名.
        /// </summary>
        [Browsable(false)]
        public string UserName
        {
            get
            {
                EnsureChildControls();
                return _dtTextBox.Text.Trim();
            }
            set
            {
                EnsureChildControls();
                _dtTextBox.Text = value;
            }
        }

        /// <summary>
        /// 用户ID.
        /// </summary>
        [Browsable(false)]
        public string HiddenUserID
        {
            get
            {
                EnsureChildControls();
                return _hdTextBox.Value.Trim();
            }
            set
            {
                EnsureChildControls();
                _hdTextBox.Value = value;
            }
        }

        /// <summary>
        /// 人员选择列表文件.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]       
        [Description("人员选择列表文件")]
        public string  EmployeeFile
        {
            get { return _EmployeFile; }
            set { _EmployeFile = value; }
        }

        /// <summary>
        /// 是否验证为空.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("True")]
        [Description("是否验证为空")]
        public ValidateFormat IsValidate
        {
            get { return _isValidate; }
            set { _isValidate = value; }
        }

        /// <summary>
        /// 验证错误信息.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("用户不能为空")]
        [Description("验证错误信息.")]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        #endregion

        #region override properties

        /// <summary>
        /// 重写子控件属性.
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
        /// 重写创建子控件的方法.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            _dtTextBox = new TextBox();
            _dtTextBox.ID = "txtUserName";

            _hdTextBox = new HiddenField();
            _hdTextBox.ID = "hdUserID";

            _urValidator = new RequiredFieldValidator();
            _urValidator.ControlToValidate = _dtTextBox.ID;
            _urValidator.ID = "validator";
            _urValidator.Text = "* " + _errorMessage;
            _urValidator.Display = ValidatorDisplay.Static;

            Controls.Add(_dtTextBox);
            Controls.Add(_hdTextBox);
            Controls.Add(_urValidator);
        }

        /// <summary>
        /// 重写呈现控件的方法.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            _dtTextBox.RenderControl(writer);
            writer.Write(string.Format(EmployeePickSpan, _EmployeFile, this.ClientID));
            _hdTextBox.RenderControl(writer);

            if (_isValidate.Equals(ValidateFormat.True))
                _urValidator.RenderControl(writer);
        }
        #endregion Overriden properties
    }
}
