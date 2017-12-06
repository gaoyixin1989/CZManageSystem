using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Botwave.XQP.Domain;


namespace Botwave.XQP.Service
{
    public interface IUCS_MarketManageService
    {
        void AddLableModle(UCS_MarketManage labelModle);
        void DeleteLabelModle(string labelId);
        void UpdateLabel(UCS_MarketManage labelModle);
        UCS_MarketManage GetLabelById(string labelId);
    }
}
