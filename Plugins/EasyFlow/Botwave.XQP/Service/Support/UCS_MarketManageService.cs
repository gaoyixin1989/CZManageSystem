using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Extension.IBatisNet;
using System.Data;
using Botwave.XQP.Domain;


namespace Botwave.XQP.Service.Support
{
    public class UCS_MarketManageService : IUCS_MarketManageService
    {
        public static UCS_MarketManageService Instance
        {
            get { return new UCS_MarketManageService(); }
        }

        public void AddLableModle(UCS_MarketManage labelModle)
        {
            IBatisMapper.Insert("UCS_MarketManageService_Add", labelModle);
        }

        public void DeleteLabelModle(string labelId)
        {
            IBatisMapper.Delete("UCS_MarketManageService_Delete", labelId);
        }

        public void UpdateLabel(UCS_MarketManage labelModle)
        {
            IBatisMapper.Update("UCS_MarketManageService_Update", labelModle);
        }


        public UCS_MarketManage GetLabelById(string labelId)
        {
            var obj = IBatisMapper.Select<UCS_MarketManage>("UCS_MarketManageService_Select", labelId);
            UCS_MarketManage modle = null;
            if (obj != null&&obj.Count!=0)
            {
                modle = obj[0];
            }
            return modle;
        }
    }
}
