using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Commons
{
    public class SMSHelper
    {
        public static DataTable GetSMSView(DateTime? beginDate, DateTime? endDate, string receiver, string sheetId, int pageIndex, int pageSize, ref int recordCount)
        {
            receiver = receiver == null ? null : Botwave.Commons.DbUtils.FilterSQL(receiver);
            sheetId = sheetId == null ? null : Botwave.Commons.DbUtils.FilterSQL(sheetId);

            string tableName = "vw_cz_SMSDetails";
            string fieldKey = "Id";
            string fieldShow = "ID, SheetId, CreatedTime, ReceivedTime, Sender, Receiver, ReceiverName, Content,  SendStatus, ProcessStatus, ProcessContent"; //ContentDescription,
            string fieldOrder = "ID desc";
            StringBuilder where = new StringBuilder(" (1=1) ");
            if (beginDate.HasValue)
                where.AppendFormat(" AND (CreatedTime>='{0:yyyy-MM-dd}')", beginDate);
            if (endDate.HasValue)
                where.AppendFormat(" AND (CreatedTime<='{0:yyyy-MM-dd}')", endDate.Value.AddDays(1));

            if (!string.IsNullOrEmpty(receiver))
                where.AppendFormat(" AND (Receiver like '%{0}%' or ReceiverName like '%{0}%')", receiver);
            if (!string.IsNullOrEmpty(sheetId))
                where.AppendFormat(" AND (SheetId like '%{0}%')", sheetId);

            try
            {
                return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            }
            catch (Exception ex)
            {
                throw new IBatisNet.Common.Exceptions.IBatisNetException(string.Format("ConnectionString:{0}>{1}", IBatisDbHelper.ConnectionString, ex), ex);
            }
        }
    }
}
