using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.XQP.API.Util;
using System.Collections;
using System.Data;

namespace Botwave.XQP.API.Service
{
    public class SaveAsAPIService : ISaveAsAPIService
    {
        public int SelectSystemPage(string workflowinstanceid,string page)
        {
            int rtn_val = 0;
            {
                string sql = string.Format("select count(0) from xqp_Customize_System where workflowinstanceid='{0}' and page='{1}'",workflowinstanceid,page);
                rtn_val =Convert.ToInt32(IBatisDbHelper.ExecuteScalar(CommandType.Text,sql).ToString());
            }
            return rtn_val;

        }

        public void InsertSystemPage(string workflowinstanceid, string page,int state)
        {
            string insertsql = string.Format("insert into xqp_Customize_System values('{0}','{1}','{2}')",workflowinstanceid,page,state);
            IBatisDbHelper.ExecuteNonQuery(CommandType.Text,insertsql);
        }

        public void UpdateSystemPage(string workflowinstanceid, string page,int state)
        {
            string updatesql = string.Format("update xqp_Customize_System set state='{0}' where workflowinstanceid='{1}' and page='{2}'",state,workflowinstanceid,page);
            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, updatesql);
        }
    }
}
