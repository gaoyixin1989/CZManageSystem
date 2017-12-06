using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// [xqp_Help] 的实体类.
    /// 创建日期: 2009-04-16
    /// </summary>
    public class Help
    {
        #region Getter / Setter

        private int id;
        private string title = String.Empty;
        private string content = String.Empty;
        private int parentId;
        private int showOrder;

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ShowOrder
        {
            get { return showOrder; }
            set { showOrder = value; }
        }

        #endregion

        #region 数据操作

        public int Create()
        {
            IBatisMapper.Insert("xqp_Help_Insert", this);
            return this.Id;
        }

        public int Update()
        {
            return IBatisMapper.Update("xqp_Help_Update", this);
        }

        public int Delete()
        {
            return IBatisMapper.Delete("xqp_Help_Delete", this.Id);
        }

        public static Help LoadById(int id)
        {
            return IBatisMapper.Load<Help>("xqp_Help_Select", id);
        }

        public static IList<Help> Select()
        {
            return IBatisMapper.Select<Help>("xqp_Help_Select");
        }

        public static DataTable GetHelpList()
        {
            string sql = "SELECT [Id],[Title],[Content],[ParentId],[ShowOrder] FROM [dbo].[xqp_Help]";
            DataSet dsResult = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql);
            if (null != dsResult && dsResult.Tables.Count > 0)
                return dsResult.Tables[0];

            return null;
        }

        #endregion
    }
}
