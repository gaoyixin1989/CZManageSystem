using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Specialized;
using Botwave.Security;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service
{
    public interface IUcs_ReportformsService
    {
        Ucs_Reportforms GetReprotformsByid(Guid id, Guid userid);
        string GetTableHtml(System.Data.DataTable dt, int? type, string fieldtext, Ucs_Reportforms model, string Lvl,string str);
        void InsertForm(Ucs_Reportforms model, System.Collections.Specialized.NameValueCollection from);
        void UpdateForm(Ucs_Reportforms model, NameValueCollection from);
        DataTable GetAllTableName();
        DataTable GetAllFieldName(string tablename);
        string GetNextLvL(string LvL, string tableName);
        string GetPreLvL(string LvL);


        string PreView(Ucs_Reportforms model, NameValueCollection from);
        string GetIndexImgHtml(List<Ucs_Reportforms> list,  StringBuilder javacript);
        string GetImgHtml(Ucs_Reportforms model, StringBuilder javacript, string wherestr, string lvl);
        //#region 预警管理
        //int Insert_UCS_EWS_MANAGE(UCS_EWS_MANAGE model);
        //void Update_UCS_EWS_MANAGE(UCS_EWS_MANAGE model);
        //void Insert_EWS_DETAIL(UCS_EWS_DETAIL model);
        //void Update_EWS_DETAIL(List<UCS_EWS_DETAIL> list, NameValueCollection from, int id, int TM_INTRVL_CD);
        //UCS_EWS_DETAIL  Get_UCS_EWS_DETAIL_BYID(string id);
        //List<UCS_EWS_DETAIL> Get_UCS_EWS_DETAIL_BYEWSID(string id, string TM_INTRVL_CD);
        
        //#endregion 
    }
}
