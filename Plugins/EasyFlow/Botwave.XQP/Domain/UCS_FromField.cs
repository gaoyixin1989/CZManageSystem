using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// UCS_FromField:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UCS_FromField
    {
        public UCS_FromField()
        { }
        #region Model
        private Guid _id;
        private Guid _reportformsid;
        private string _fieldname;
        private string _field;
        private bool _isshow;
        private decimal _fieldorder;
        private bool _isselect;
        private int _wherestrtype = 0;
        private int _fieldtype;
        private string _datasource;
        private string _wherefieldtext;
        private string _wherefieldvalue;
        public string way { get; set; }
        public int EVA_ID { get; set; }
        public string strWhere { get; set; }
        public string EVA_Formula { get; set; }
        public string userId { set; get; }
        public string imgtype { set; get; } 
        public int Axis { set; get; }
        /// <summary>
        /// 能否统计
        /// </summary>
        public bool IsCount { get; set; }

        /// <summary>
        /// 计算类型 1：累加 2：平均数
        /// </summary>
        public int StatisticsType { get; set; }
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
        public Guid ReportformsID
        {
            set { _reportformsid = value; }
            get { return _reportformsid; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string fieldname
        {
            set { _fieldname = value; }
            get { return _fieldname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string field
        {
            set { _field = value; }
            get { return _field; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsShow
        {
            set { _isshow = value; }
            get { return _isshow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal fieldorder
        {
            set { _fieldorder = value; }
            get { return _fieldorder; }
        }
        /// <summary>
        /// 是否查询，即是否显示在select 语句中
        /// </summary>
        public bool IsSelect
        {
            set { _isselect = value; }
            get { return _isselect; }
        }
        /// <summary>
        /// 0:不是查询条件;1:普通查询;2:高级查询(having 条件)
        /// </summary>
        public int whereStrtype
        {
            set { _wherestrtype = value; }
            get { return _wherestrtype; }
        }
        /// <summary>
        /// 1:多选下拉;2:日期(月);3:日期(日);4:单选下拉
        /// </summary>
        public int Fieldtype
        {
            set { _fieldtype = value; }
            get { return _fieldtype; }
        }
        /// <summary>
        /// 查询条件的数据源；
        /// </summary>
        public string datasource
        {
            set { _datasource = value; }
            get { return _datasource; }
        }
        /// <summary>
        /// 查询字段显示的中文
        /// </summary>
        public string whereFieldText
        {
            set { _wherefieldtext = value; }
            get { return _wherefieldtext; }
        }
        /// <summary>
        /// 查询字段现实的值
        /// </summary>
        public string whereFieldValue
        {
            set { _wherefieldvalue = value; }
            get { return _wherefieldvalue; }
        }
        /// <summary>
        /// 1:上行显示2:下行显示
        /// </summary>
        public int wherePositionType { set; get; }
        public decimal whereOrder { set; get; }
        public bool isorganization { set; get; }
        public string LinkUrl { set; get; }
        public string parameter { set; get; }
        #endregion Model

    }
}
