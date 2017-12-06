using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Composite
{
    public class OGSMStaticService : BaseService<OGSMBasestationMonthStatic>,IOGSMStaticService
    {
        public IList<object> GetForPagingByCondition(out int count, int pageIndex, int pageSize, OGSMBasestationMonthStaticQueryBuilder obj)
        {
            string condition = " 1=1 ";

            if (string.IsNullOrEmpty(obj.PAY_MON_Start))
                obj.PAY_MON_Start = DateTime.Now.AddMonths(-12).ToString("yyyyMM");
            if (string.IsNullOrEmpty(obj.PAY_MON_End))
                obj.PAY_MON_End = DateTime.Now.ToString("yyyyMM");

            if (!string.IsNullOrEmpty(obj.Group_Name))
                condition += " and Group_Name = '" + obj.Group_Name + "' ";
            if (!string.IsNullOrEmpty(obj.BaseStation))
                condition += " and BaseStation = '" + obj.BaseStation + "' ";
            if (!string.IsNullOrEmpty(obj.PowerType))
                condition += " and PowerType = '" + obj.PowerType + "' ";
            if (!string.IsNullOrEmpty(obj.PAY_MON_Start) && !string.IsNullOrEmpty(obj.PAY_MON_End))
                condition += " and PAY_MON between '" + obj.PAY_MON_Start + "' and '" + obj.PAY_MON_End + "' ";
            
            System.Data.SqlClient.SqlParameter[] parameters = {
                new System.Data.SqlClient.SqlParameter("@WhereCondition",condition)
            };
            
            List<object> resultList = new List<object>();

            string sql = string.Format(@"select 
                                            PAY_MON,
                                            Group_Name,
                                            BaseStation,
                                            SUM(CHG) as CHG_CNT,
                                            convert(decimal(18,2),SUM(a.CHG)/[dbo].[ogsm_get_mon_num](a.pay_mon,a.BaseStation,a.Group_Name)) as Avg_CHG,
                                            Sum(Money) as Amount,
                                            convert(decimal(18,2),SUM(a.Money)/[dbo].[ogsm_get_mon_num](a.pay_mon,a.BaseStation,a.Group_Name)) as AvgMoney,
                                            sum(PrevMoney) as Prev_Amount,
                                            convert(varchar,convert(decimal(18,2),(Sum(Money)-sum(PrevMoney))/sum(case PrevMoney when 0 then 1 else PrevMoney end)*100))+'%' as Rate,
                                            sum(CMPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CMShare,
                                            sum(CUPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CUShare,
                                            sum(CTPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CTShare 
                                            from vw_ogsm_info_base_data a 
                                            where {0} 
                                            group by a.PAY_MON,a.Group_Name,a.BaseStation 
                                            order by (Sum(Money)-sum(PrevMoney))/sum(case PrevMoney when 0 then 1 else PrevMoney end) desc ", condition);

            var ls = new EfRepository<OGSMBasestationMonthStatic>().ExecuteResT<OGSMBasestationMonthStatic>(sql);

            foreach (var x in ls)
            {
                string u =  x.Avg_CHG.ToString();
                resultList.Add(new
                {
                    x.PAY_MON,
                    x.Group_Name,
                    x.BaseStation,
                    x.Amount,
                    x.AvgMoney,
                    x.CHG_CNT,
                    x.Prev_Amount,
                    x.Avg_CHG,
                    x.Rate,
                    x.CMShare,
                    x.CUShare,
                    x.CTShare
                });
            }           
               
            PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }


        public IList<object> GetBasestationYearForPagingByCondition(out int count, int pageIndex, int pageSize, OGSMBasestationMonthStaticQueryBuilder obj)
        {
            string condition = " 1=1 ";

            if (string.IsNullOrEmpty(obj.PAY_MON_Start))
                obj.PAY_MON_Start = DateTime.Now.ToString("yyyy");
            if (string.IsNullOrEmpty(obj.PAY_MON_End))
                obj.PAY_MON_End = DateTime.Now.ToString("yyyy");

            if (!string.IsNullOrEmpty(obj.Group_Name))
                condition += " and Group_Name = '" + obj.Group_Name + "' ";
            if (!string.IsNullOrEmpty(obj.BaseStation))
                condition += " and BaseStation = '" + obj.BaseStation + "' ";
            if (!string.IsNullOrEmpty(obj.PowerType))
                condition += " and PowerType = '" + obj.PowerType + "' ";
            if (!string.IsNullOrEmpty(obj.PAY_MON_Start) && !string.IsNullOrEmpty(obj.PAY_MON_End))
                condition += " and Year between '" + obj.PAY_MON_Start + "' and '" + obj.PAY_MON_End + "' ";

            System.Data.SqlClient.SqlParameter[] parameters = {
                new System.Data.SqlClient.SqlParameter("@WhereCondition",condition)
            };

            List<object> resultList = new List<object>();

            string sql = string.Format(@"select 
                                            Year,
                                            Group_Name,
                                            BaseStation,
                                            SUM(CHG) as CHG_CNT,
                                            convert(decimal(18,2),SUM(CHG)/case when count(distinct PAY_MON)=0 then 1 else  count(distinct PAY_MON) end) as Avg_CHG,
                                            SUM(Money) as Amount,
                                            convert(decimal(18,2),SUM(Money)/case when count(distinct PAY_MON)=0 then 1 else  count(distinct PAY_MON) end) as AvgMoney,
                                            sum(CMPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CMShare,
                                            sum(CUPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CUShare,
                                            sum(CTPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CTShare 
                                            from vw_ogsm_info_base_data 
                                            where {0} 
                                            group by Year,Group_Name,BaseStation 
                                            order by Year desc ", condition);

            var ls = new EfRepository<OGSMBasestationYearStatic>().ExecuteResT<OGSMBasestationYearStatic>(sql);

            foreach (var x in ls)
            {
                string u = x.Avg_CHG.ToString();
                resultList.Add(new
                {
                    x.Year,
                    x.Group_Name,
                    x.BaseStation,
                    x.Amount,
                    x.AvgMoney,
                    x.CHG_CNT,
                    x.Avg_CHG,
                    x.CMShare,
                    x.CUShare,
                    x.CTShare
                });
            }

            PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }


        public IList<object> GetGroupMonthForPagingByCondition(out int count, int pageIndex, int pageSize, OGSMGroupMonthStaticQueryBuilder obj)
        {
            string condition = " 1=1 ";

            if (string.IsNullOrEmpty(obj.PAY_MON_Start))
                obj.PAY_MON_Start = DateTime.Now.AddMonths(-12).ToString("yyyyMM");
            if (string.IsNullOrEmpty(obj.PAY_MON_End))
                obj.PAY_MON_End = DateTime.Now.ToString("yyyyMM");

            if (!string.IsNullOrEmpty(obj.Group_Name))
                condition += " and Group_Name = '" + obj.Group_Name + "' ";
            if (!string.IsNullOrEmpty(obj.PowerType))
                condition += " and PowerType = '" + obj.PowerType + "' ";
            if (!string.IsNullOrEmpty(obj.PAY_MON_Start) && !string.IsNullOrEmpty(obj.PAY_MON_End))
                condition += " and PAY_MON between '" + obj.PAY_MON_Start + "' and '" + obj.PAY_MON_End + "' ";            

            List<object> resultList = new List<object>();

            string sql = string.Format(@" select  
                                                PAY_MON, 
                                                Group_Name, 
                                                SUM(CHG) as CHG_CNT, 
                                                SUM(Money) as Amount, 
                                                convert(decimal(18,2),SUM(Money)/case when COUNT(distinct basestation)=0 then 1 else COUNT(distinct basestation) end) as AvgAmount, 
                                                sum(CMPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CMShare, 
                                                sum(CUPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CUShare, 
                                                sum(CTPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CTShare, 
                                                sum(case when propertyright='移动'  then Money else 0 end ) CMPropertyRightMoney, 
                                                sum(case when propertyright='铁塔'  then Money else 0 end ) CUPropertyRightMoney, 
                                                sum(case when propertyright='第三方'  then Money else 0 end ) CTPropertyRightMoney 
                                                from  dbo.vw_ogsm_info_base_data 
                                                where {0} 
                                                group by pay_mon,group_name 
                                                order by PAY_MON desc ", condition);

            var ls = new EfRepository<OGSMGroupMonthStatic>().ExecuteResT<OGSMGroupMonthStatic>(sql);

            foreach (var x in ls)
            {
                resultList.Add(new
                {
                    x.PAY_MON,
                    x.Group_Name,
                    x.Amount,
                    x.AvgAmount,
                    x.CHG_CNT,
                    x.CMShare,
                    x.CUShare,
                    x.CTShare,
                    x.CMPropertyRightMoney,
                    x.CUPropertyRightMoney,
                    x.CTPropertyRightMoney
                });
            }

            PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }


        public IList<object> GetGroupYearForPagingByCondition(out int count, int pageIndex, int pageSize, OGSMGroupMonthStaticQueryBuilder obj)
        {
            string condition = " 1=1 ";

            if (string.IsNullOrEmpty(obj.PAY_MON_Start))
                obj.PAY_MON_Start = DateTime.Now.ToString("yyyy");
            if (string.IsNullOrEmpty(obj.PAY_MON_End))
                obj.PAY_MON_End = DateTime.Now.ToString("yyyy");

            if (!string.IsNullOrEmpty(obj.Group_Name))
                condition += " and Group_Name = '" + obj.Group_Name + "' ";
            if (!string.IsNullOrEmpty(obj.PowerType))
                condition += " and PowerType = '" + obj.PowerType + "' ";
            if (!string.IsNullOrEmpty(obj.PAY_MON_Start) && !string.IsNullOrEmpty(obj.PAY_MON_End))
                condition += " and Year between '" + obj.PAY_MON_Start + "' and '" + obj.PAY_MON_End + "' ";

            System.Data.SqlClient.SqlParameter[] parameters = {
                new System.Data.SqlClient.SqlParameter("@WhereCondition",condition)
            };

            List<object> resultList = new List<object>();

            string sql = string.Format(@"select 
                                            year,
                                            Group_Name,
                                            SUM(CHG) as CHG_CNT,
                                            SUM(Money) as Amount,
                                            convert(decimal(18,2),SUM(Money)/case when COUNT(distinct basestation)=0 then 1 else COUNT(distinct basestation) end) as AvgAmount,
                                            sum(CMPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CMShare, 
                                            sum(CUPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CUShare, 
                                            sum(CTPower)*Sum(Money)/(case sum(CPower) when 0 then 1 else sum(CPower) end) as CTShare, 
                                            sum(case when propertyright='移动'  then Money else 0 end ) CMPropertyRightMoney, 
                                            sum(case when propertyright='铁塔'  then Money else 0 end ) CUPropertyRightMoney, 
                                            sum(case when propertyright='第三方'  then Money else 0 end ) CTPropertyRightMoney  
                                            from  dbo.vw_ogsm_info_base_data 
                                            where {0} 
                                            group by Group_Name,year 
                                            order by year,group_name ", condition);

            var ls = new EfRepository<OGSMGroupYearStatic>().ExecuteResT<OGSMGroupYearStatic>(sql);

            foreach (var x in ls)
            {
                resultList.Add(new
                {
                    x.Year,
                    x.Group_Name,
                    x.Amount,
                    x.AvgAmount,
                    x.CHG_CNT,
                    x.CMShare,
                    x.CUShare,
                    x.CTShare,
                    x.CMPropertyRightMoney,
                    x.CUPropertyRightMoney,
                    x.CTPropertyRightMoney
                });
            }

            PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }


        public IList<object> GetGroupBasestationForPagingByCondition(out int count, int pageIndex, int pageSize)
        {
            string condition = " 1=1 ";           

            List<object> resultList = new List<object>();

            string sql = string.Format(@"select 
                        Group_Name,
                        cast(sum(case when powertype='公电'  then 1 else 0 end ) as varchar) CommonCnt,
                        cast(sum(case when powertype='私电'  then 1 else 0 end ) as varchar) PrivateCnt,
                        cast(count(basestation) as varchar)  BasestationCnt,
                        cast(ROUND(cast(sum(case when powertype='私电'  then 1 else 0 end ) as float) /cast(case when count(powertype)=0 then 1 else count(powertype) end as float) ,2)*100 as varchar)+'%'  PrivatePercent 
                        from 
                        dbo.ogsm
                        group by group_name");

            var ls = new EfRepository<OGSMGroupBasestationStatic>().ExecuteResT<OGSMGroupBasestationStatic>(sql);

            foreach (var x in ls)
            {
                resultList.Add(new
                {
                    x.Group_Name,
                    x.CommonCnt,
                    x.PrivateCnt,
                    x.BasestationCnt,
                    x.PrivatePercent
                });
            }

            PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }


        public IList<object> GetBasestationChangeForPagingByCondition(out int count, int pageIndex, int pageSize, OGSMBasestationChangeQueryBuilder obj)
        {
            string condition = " 1=1 ";

            if (!string.IsNullOrEmpty(obj.USR_NBR))
                condition += " and USR_NBR LIKE '%" + obj.USR_NBR + "%' ";
            if (!string.IsNullOrEmpty(obj.PropertyRight))
                condition += " and PropertyRight = '" + obj.PropertyRight + "' ";
            if (!string.IsNullOrEmpty(obj.Group_Name))
                condition += " and Group_Name = '" + obj.Group_Name + "' ";
            if (!string.IsNullOrEmpty(obj.BaseStation))
                condition += " and BaseStation = '" + obj.BaseStation + "' ";
            if (!string.IsNullOrEmpty(obj.PowerType))
                condition += " and PowerType = '" + obj.PowerType + "' ";
            if (!string.IsNullOrEmpty(obj.PAY_MON_Start) && !string.IsNullOrEmpty(obj.PAY_MON_End))
                condition += " and PAY_MON between '" + obj.PAY_MON_Start + "' and '" + obj.PAY_MON_End + "' ";

            List<object> resultList = new List<object>();

            string sql = string.Format(@"select
                PAY_MON,
                Group_Name,
                Town,
                USR_NBR,
                BaseStation,
                PowerStation,
                PowerType,
                PropertyRight,
                Money,
                ISNULL(PrevMoney,0) PrevMoney ,
                ISNULL(PrevYearMonth,0) PrevYearMonth,
                case when PrevMoney is null or PrevMoney=0 then '无穷大' else 
                cast(cast((isnull(Money, 0)-isnull(PrevMoney,0))*100/isnull(PrevMoney, 0) as decimal(10,2)) as varchar(50))+'%' end ChainChanges,
                case when PrevYearMonth is null or PrevYearMonth=0 then '无穷大' else 
                cast(cast((isnull(Money, 0)-isnull(PrevYearMonth,0))*100/isnull(PrevYearMonth, 0) as decimal(10,2)) as varchar(50))+'%' end as YearBasis
                from vw_ogsm_info_base_data  where {0} ", condition);

            var ls = new EfRepository<OGSMBasestationChangeStatic>().ExecuteResT<OGSMBasestationChangeStatic>(sql);

            foreach (var x in ls)
            {
                resultList.Add(new
                {
                    x.PAY_MON,
                    x.Group_Name,
                    x.Town,
                    x.USR_NBR,
                    x.BaseStation,
                    x.PowerStation,
                    x.PowerType,
                    x.PropertyRight,
                    x.Money,
                    x.PrevMoney,
                    x.PrevYearMonth,
                    x.ChainChanges,
                    x.YearBasis
                });
            }

            PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }

        public IList<object> GetContractWarningForPagingByCondition(out int count, int pageIndex, int pageSize, OGSMContractWarningQueryBuilder obj)
        {
            string condition = " WarningSituation is not null ";

            if (!string.IsNullOrEmpty(obj.USR_NBR))
                condition += " and USR_NBR LIKE '%" + obj.USR_NBR + "%' ";
            if (!string.IsNullOrEmpty(obj.PropertyRight))
                condition += " and PropertyRight = '" + obj.PropertyRight + "' ";
            if (!string.IsNullOrEmpty(obj.Group_Name))
                condition += " and Group_Name = '" + obj.Group_Name + "' ";
            if (!string.IsNullOrEmpty(obj.BaseStation))
                condition += " and BaseStation = '" + obj.BaseStation + "' ";
            if (!string.IsNullOrEmpty(obj.PowerType))
                condition += " and PowerType = '" + obj.PowerType + "' ";
            if (!string.IsNullOrEmpty(obj.WarningSituation))
                condition += " and Warning in (" + obj.WarningSituation + ") ";
            //if(WarningSituation == "1M")
            //{
            //    condition += " and  datediff(dd,getdate(),ContractEndTime) <30 ";
            //}
            //else if (WarningSituation == "3M")
            //{
            //    condition += " and datediff(dd,getdate(),ContractEndTime)>=30 and datediff(dd,getdate(),ContractEndTime)<3*30 ";
            //}
            //else if (WarningSituation == "6M")
            //{
            //    condition += " and datediff(dd,getdate(),ContractEndTime)>=3*30 and datediff(dd,getdate(),ContractEndTime)<6*30 ";
            //}
            //else if (WarningSituation == "1Y")
            //{
            //    condition += " and datediff(dd,getdate(),ContractEndTime)>=6*30 and datediff(dd,getdate(),ContractEndTime)<12*30 ";
            //}

            List<object> resultList = new List<object>();

            string sql = string.Format(@"select * from (select 
                                            Group_Name,Town,USR_NBR,PowerStation,BaseStation,PowerType,PropertyRight,ContractStartTime ,ContractEndTime,
                                            case 
                                            when datediff(dd,getdate(),ContractEndTime)<0
                                            then '已到期'
                                            when datediff(dd,getdate(),ContractEndTime)>=0 and datediff(dd,getdate(),ContractEndTime)<30
                                            then '1个月内到期'
                                            when datediff(dd,getdate(),ContractEndTime)>=30 and datediff(dd,getdate(),ContractEndTime)<3*30
                                            then '3个月内到期'
                                            when datediff(dd,getdate(),ContractEndTime)>=3*30 and datediff(dd,getdate(),ContractEndTime)<6*30
                                            then '6个月内到期'
                                            when datediff(dd,getdate(),ContractEndTime)>=6*30 and datediff(dd,getdate(),ContractEndTime)<12*30
                                            then '1年内到期'
                                            end WarningSituation,
                                            case 
                                            when datediff(dd,getdate(),ContractEndTime)<0
                                            then '0'
                                            when datediff(dd,getdate(),ContractEndTime)>=0 and datediff(dd,getdate(),ContractEndTime)<30
                                            then '1'
                                            when datediff(dd,getdate(),ContractEndTime)>=30 and datediff(dd,getdate(),ContractEndTime)<3*30
                                            then '3'
                                            when datediff(dd,getdate(),ContractEndTime)>=3*30 and datediff(dd,getdate(),ContractEndTime)<6*30
                                            then '6'
                                            when datediff(dd,getdate(),ContractEndTime)>=6*30 and datediff(dd,getdate(),ContractEndTime)<12*30
                                            then '12'
                                            end Warning
                                            from ogsm where isnull(ContractEndTime,'')<>'' ) t where  {0}  order by Warning ", condition);

            var ls = new EfRepository<OGSMContractWarningStatic>().ExecuteResT<OGSMContractWarningStatic>(sql);

            foreach (var x in ls)
            {
                resultList.Add(new
                {
                    x.Town,
                    x.Group_Name,
                    x.USR_NBR,
                    x.PowerStation,
                    x.BaseStation,
                    x.PowerType,
                    x.PropertyRight,
                    ContractStartTime= x.ContractStartTime.HasValue? x.ContractStartTime.Value.ToString("yyyy-MM-dd") : "",
                    ContractEndTime = x.ContractEndTime.HasValue ? x.ContractEndTime.Value.ToString("yyyy-MM-dd") : "",
                    x.WarningSituation
                });
            }

            PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }


        public IList<object> GetBasestationNoPaymentWarningForPagingByCondition(out int count, int pageIndex, int pageSize, OGSMNoPaymentWarningQueryBuilder obj)
        {
            string condition = " 1=1 ";//分公司、户号、产权方、所属基站/服务厅、公电/私电、无缴费情况供

            if (!string.IsNullOrEmpty(obj.USR_NBR))
                condition += " and USR_NBR LIKE '%" + obj.USR_NBR + "%' ";
            if (!string.IsNullOrEmpty(obj.PropertyRight))
                condition += " and PropertyRight = '" + obj.PropertyRight + "' ";
            if (!string.IsNullOrEmpty(obj.Group_Name))
                condition += " and Group_Name = '" + obj.Group_Name + "' ";
            if (!string.IsNullOrEmpty(obj.BaseStation))
                condition += " and BaseStation = '" + obj.BaseStation + "' ";
            if (!string.IsNullOrEmpty(obj.PowerType))
                condition += " and PowerType = '" + obj.PowerType + "' ";
            if(!string.IsNullOrEmpty(obj.PaymentSituation))
                condition += " and CNT = '" + obj.PaymentSituation + "' ";


            List<object> resultList = new List<object>();

            string sql = string.Format(@"select PAY_MON,Group_Name,Town,USR_NBR,PowerStation,BaseStation,PowerType,PropertyRight 
                                            from (
                                            select max(PAY_MON) PAY_MON,Group_Name,Town,USR_NBR,PowerStation,BaseStation,PowerType,PropertyRight ,
                                            datediff(MM,substring(convert(varchar,max(PAY_MON)),1,4)+'-'+substring(convert(varchar,max(PAY_MON)),5,2)+'-01',getdate()) CNT
                                            from vw_ogsm_info_base_data 
                                            group by Group_Name,USR_NBR,BaseStation,PowerType,PropertyRight ,Town,PowerStation
                                            ) t 
                                            where {0}  order by PAY_MON desc", condition);

            var ls = new EfRepository<OGSMBasestationNoPaymentStatic>().ExecuteResT<OGSMBasestationNoPaymentStatic>(sql);

            foreach (var x in ls)
            {
                resultList.Add(new
                {
                    x.Town,
                    x.Group_Name,
                    x.USR_NBR,
                    x.PAY_MON,
                    x.PowerStation,
                    x.BaseStation,
                    x.PowerType,
                    x.PropertyRight
                });
            }

            PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }

    }
}
