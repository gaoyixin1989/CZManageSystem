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
    /// Ա��ѡ��ؼ�.
    /// </summary>
    [
    ToolboxData("<{0}:EmployeePicker runat=\"server\"></{0}:EmployeePicker>"),
    DefaultProperty("EmployeeFile"),
    Designer(typeof(CompositeControlDesigner))
    ]
    public class EmployeePicker : WebControl, INamingContainer
    {
        /// <summary>
        /// Ա��ѡ��� html ��ʽ.
        /// </summary>
        protected const string EmployeePickSpan = "<span id=\"spanPickUser\" onclick=\"window.open('{0}?pId={1}','SelectEmployee', 'width=600;height=600;');\" class=\"ico_pickdate\" title=\"���ѡ����Ա\">&nbsp;</span>";

        #region getter/setter
        private string _EmployeFile = "SelectEmployee.aspx";
        private TextBox _dtTextBox;
        private HiddenField _hdTextBox;
        private RequiredFieldValidator _urValidator;
        private ValidateFormat _isValidate;
        private string _errorMessage = "�û�����Ϊ��";       

        /// <summary>
        /// ��֤����ö��
        /// </summary>
        public enum ValidateFormat
        {
            /// <summary>
            /// ��.
            /// </summary>
            True,
            /// <summary>
            /// ��.
            /// </summary>
            False
        }

        /// <summary>
        /// �û���.
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
        /// �û�ID.
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
        /// ��Աѡ���б��ļ�.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]       
        [Description("��Աѡ���б��ļ�")]
        public string  EmployeeFile
        {
            get { return _EmployeFile; }
            set { _EmployeFile = value; }
        }

        /// <summary>
        /// �Ƿ���֤Ϊ��.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("True")]
        [Description("�Ƿ���֤Ϊ��")]
        public ValidateFormat IsValidate
        {
            get { return _isValidate; }
            set { _isValidate = value; }
        }

        /// <summary>
        /// ��֤������Ϣ.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue("�û�����Ϊ��")]
        [Description("��֤������Ϣ.")]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        #endregion

        #region override properties

        /// <summary>
        /// ��д�ӿؼ�����.
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
        /// ��д�����ӿؼ��ķ���.
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
        /// ��д���ֿؼ��ķ���.
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
