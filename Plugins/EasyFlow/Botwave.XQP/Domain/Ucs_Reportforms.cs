using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// Ucs_Reportforms:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Ucs_Reportforms
    {
        public Ucs_Reportforms()
        { }
        #region Model
        private Guid _id;
        private string _formname;
        private string _datasource;
        private int? _type;
        private DateTime? _createtime = DateTime.Now;
        private DateTime? _updatetime = DateTime.Now;
        private string _lasthandlers;
        private string _strorder;
        public string strGroup { get; set; }
        public string strWhere { get; set; }
        public int formtype { get; set; }

        /// <summary>
        /// 是否有合计行
        /// </summary>
        public bool Statistics { get; set; }

        /// <summary>
        /// 权限过滤规则
        /// </summary>
        public string FilterRule { get; set; }

        /// <summary>
        /// 是否只显示图表
        /// </summary>
        public bool ImageOnly { get; set; }

        public string[] groupbystr
        {
            get { return strGroup.Split(','); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string formname
        {
            set { _formname = value; }
            get { return _formname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string datasource
        {
            set { _datasource = value; }
            get { return _datasource; }
        }
        /// <summary>
        /// 1:有数据分层显示 2:无数据分层显示
        /// </summary>
        public int? type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? createtime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 最后操作者
        /// </summary>
        public string lasthandlers
        {
            set { _lasthandlers = value; }
            get { return _lasthandlers; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string strOrder
        {
            set { _strorder = value; }
            get { return _strorder; }
        }

        #endregion Model
        public List<UCS_FromField> FieldList { get; set; }
        public string FromHead { set; get; }
        public void GetFromHead(string[] fieldtext, Ucs_Reportforms model, StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(FromHead))
            {
                sb.Append(FromHead);
            }
            else
            {
                for (int i = 0; i < fieldtext.Length; i++)
                {
                    if (!fieldtext[i].StartsWith("thres_"))
                        sb.Append("<th>").Append(fieldtext[i]).Append("</th>");
                }
            }
        }
    }
    [Serializable]
    public class Pet
    {
        public string name { get; set; }
        public decimal y { get; set; }
        public string color { set; get; }
    }
}
