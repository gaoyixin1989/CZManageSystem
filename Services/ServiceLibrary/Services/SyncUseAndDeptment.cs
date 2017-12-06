using CZManageSystem.Core;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Data.Models;
using CZManageSystem.Service.SysManger;
using ServiceLibrary.Base;
using ServiceLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 同步组织架构信息_当前使用
/// </summary>
namespace ServiceLibrary
{
    public class SyncUseAndDeptment : ServiceJob
    {
        private ISysUserService _sysUserService = new SysUserService();
        private ISysDeptmentService _sysDeptService = new SysDeptmentService();
        private IUum_Organizationinfo_LogService _uumOrgLogService = new Uum_Organizationinfo_LogService();
        private IUum_OrganizationinfoService _uumOrgInfoService = new Uum_OrganizationinfoService();
        private IUum_Userinfo_LogService _uumUserLogService = new Uum_Userinfo_LogService();
        private IUum_UserinfoService _uumUserInfoService = new Uum_UserinfoService();
        private ICzUumLeadersService _uumLeadersService = new CzUumLeadersService();
        private ICzUumPostsService _uumPostsService = new CzUumPostsService();

        private const string packageName = "webservice";
        private const string unitID = "0";
        private const string password = "";
        private const string automata_OrgHis = "biz.bizQueryOrgChgHistory";
        private const string automata_UserHis = "biz.bizQueryEmpChgHistory";
        private const decimal logSpan = 400;//组织用户信息异动获取数据的日志跨度

        public override bool Execute()
        {
            #region 查询当前服务策略信息
            string sTemp = "";
            if (!SetStrategyInfo(out sTemp))
            {
                sMessage = sTemp;
                return false;
            }
            #endregion

            SystemResult _result = new SystemResult();
            bool boolResult = true;

            #region 同步组织信息
            LogRecord.WriteLog(string.Format("{0}:开始同步组织信息", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:开始同步组织信息", strCurStrategyInfo), true);
            _result = DealOrg();
            if (_result.IsSuccess)
            {
                LogRecord.WriteLog(string.Format("{0}:同步组织信息成功", strCurStrategyInfo), LogResult.normal);
                AddStrategyLog(string.Format("{0}:同步组织信息成功", strCurStrategyInfo), true);
            }
            else
            {
                LogRecord.WriteLog(string.Format("{0}:同步组织信息失败", strCurStrategyInfo), LogResult.fail);
                AddStrategyLog(string.Format("{0}:同步组织信息失败", strCurStrategyInfo), false);
                sMessage += (string.IsNullOrEmpty(sMessage) ? "" : ";") + _result.Message;
                boolResult = false;
            }
            #endregion
            #region 同步用户信息
            LogRecord.WriteLog(string.Format("{0}:开始同步用户信息", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:开始同步用户信息", strCurStrategyInfo), true);
            _result = DealUser();
            if (_result.IsSuccess)
            {
                LogRecord.WriteLog(string.Format("{0}:同步用户信息成功", strCurStrategyInfo), LogResult.normal);
                AddStrategyLog(string.Format("{0}:同步用户信息成功", strCurStrategyInfo), true);
            }
            else
            {
                LogRecord.WriteLog(string.Format("{0}:同步用户信息失败", strCurStrategyInfo), LogResult.fail);
                AddStrategyLog(string.Format("{0}:同步用户信息失败", strCurStrategyInfo), false);
                sMessage += (string.IsNullOrEmpty(sMessage) ? "" : ";") + _result.Message;
                boolResult = false;

            }
            #endregion

            ExecuteProcedure_Update_Depts_DpFullName();
            SaveStrategyLog();//保存日志到数据库
            if (boolResult)
                sMessage = "服务执行成功";
            return boolResult;
        }

        /// <summary>
        /// 处理组织异动信息
        /// </summary>
        /// <returns></returns>
        private SystemResult DealOrg()
        {
            SystemResult result = new SystemResult();
            result = GetOrgTranXml();//获取组织异动信息


            if (result.IsSuccess)
            {
                //解析组织异动信息xml字符串，转换为对象
                orgRoot rootValue = (orgRoot)XmlUtil.Deserialize(typeof(orgRoot), result.Message);
                if (rootValue == null)
                {
                    LogRecord.WriteLog(string.Format("{0}:解析组织异动信息xml字符串失败", strCurStrategyInfo), LogResult.fail);
                    AddStrategyLog(string.Format("{0}:解析组织异动信息xml字符串失败", strCurStrategyInfo), false);
                    result.IsSuccess = false;
                    result.Message = "解析组织异动信息xml字符串失败";
                    return result;
                }

                if (rootValue.data.list.Count > 0)
                {
                    List<Uum_Organizationinfo_Log> listLogData = new List<Uum_Organizationinfo_Log>();
                    foreach (var item in rootValue.data.list)
                    {
                        Uum_Organizationinfo_Log temp = new Uum_Organizationinfo_Log();
                        CommonFun.AutoMapping<orgRoot.UumOrgLog, Uum_Organizationinfo_Log>(item, temp);
                        DateTime dtemp = new DateTime();
                        if (!string.IsNullOrEmpty(item.createTime))
                        {
                            if (DateTime.TryParseExact(item.createTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtemp))
                                temp.createTime = dtemp;
                        }
                        if (!string.IsNullOrEmpty(item.lastModifyTime))
                        {
                            if (DateTime.TryParseExact(item.lastModifyTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtemp))
                                temp.lastModifyTime = dtemp;
                        }
                        listLogData.Add(temp);
                    }
                    result = UpdataOrgByLogData(listLogData);
                }
                if (result.IsSuccess)
                {
                    if (rootValue.data.list.Count > 0 || (rootValue.data.continueFlag ?? false))
                    {//有数据或继续标识为true，则更新logid，继续下一次循环
                        decimal nextLogStart = 0;
                        if (rootValue.data.list.Count > 0)
                            nextLogStart = rootValue.data.list.Max(u => u.logID ?? 0);
                        else
                            nextLogStart = ConfigData.UIMinfo_lastOrgLogId + logSpan;

                        ConfigData.UIMinfo_lastOrgLogId = nextLogStart;
                        result = DealOrg();//rootValue.data.continueFlag==true，程序没出错，则继续循环
                    }

                }


            }

            return result;
        }
        /// <summary>
        /// 处理用户异动信息
        /// </summary>
        /// <returns></returns>
        private SystemResult DealUser()
        {
            SystemResult result = new SystemResult();
            result = GetUserTranXml();//获取用户异动信息


            if (result.IsSuccess)
            {
                //解析用户异动信息xml字符串，转换为对象
                userRoot rootValue = (userRoot)XmlUtil.Deserialize(typeof(userRoot), result.Message);
                if (rootValue == null)
                {
                    LogRecord.WriteLog(string.Format("{0}:解析用户异动信息xml字符串失败", strCurStrategyInfo), LogResult.fail);
                    AddStrategyLog(string.Format("{0}:解析用户异动信息xml字符串失败", strCurStrategyInfo), false);
                    result.IsSuccess = false;
                    result.Message = "解析用户异动信息xml字符串失败";
                    return result;
                }


                if (rootValue.data.list.Count > 0)
                {
                    List<Uum_Userinfo_Log> listLogData = new List<Uum_Userinfo_Log>();
                    foreach (var item in rootValue.data.list)
                    {
                        Uum_Userinfo_Log temp = new Uum_Userinfo_Log();
                        CommonFun.AutoMapping<userRoot.UumUserLog, Uum_Userinfo_Log>(item, temp);
                        DateTime dtemp = new DateTime();
                        if (!string.IsNullOrEmpty(item.userBirthday))
                        {
                            if (DateTime.TryParseExact(item.userBirthday, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtemp))
                                temp.userBirthday = dtemp;
                        }
                        if (!string.IsNullOrEmpty(item.userJoinInDate))
                        {
                            if (DateTime.TryParseExact(item.userJoinInDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtemp))
                                temp.userJoinInDate = dtemp;
                        }
                        if (!string.IsNullOrEmpty(item.userQuitDate))
                        {
                            if (DateTime.TryParseExact(item.userQuitDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtemp))
                                temp.userQuitDate = dtemp;
                        }
                        if (!string.IsNullOrEmpty(item.createTime))
                        {
                            if (DateTime.TryParseExact(item.createTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtemp))
                                temp.createTime = dtemp;
                        }
                        if (!string.IsNullOrEmpty(item.lastModifyTime))
                        {
                            if (DateTime.TryParseExact(item.lastModifyTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtemp))
                                temp.lastModifyTime = dtemp;
                        }
                        listLogData.Add(temp);
                    }
                    result = UpdataUserByLogData(listLogData);
                }
                if (result.IsSuccess)
                {
                    if (rootValue.data.list.Count > 0 || (rootValue.data.continueFlag ?? false))
                    {//有数据或继续标识为true，则更新logid，继续下一次循环
                        decimal nextLogStart = 0;
                        if (rootValue.data.list.Count > 0)
                            nextLogStart = rootValue.data.list.Max(u => u.logID ?? 0);
                        else
                            nextLogStart = ConfigData.UIMinfo_lastUserLogId + logSpan;

                        ConfigData.UIMinfo_lastUserLogId = nextLogStart;
                        result = DealUser();//rootValue.data.continueFlag==true，程序没出错，则继续循环
                    }

                }

            }

            return result;
        }

        /// <summary>
        /// 获取组织异动信息xml
        /// </summary>
        /// <returns></returns>
        private SystemResult GetOrgTranXml()
        {
            LogRecord.WriteLog(string.Format("{0}：开始获取组织异动信息", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}：开始获取组织异动信息", strCurStrategyInfo), true);
            SystemResult result = new SystemResult();
            try
            {
                decimal intStart = ConfigData.UIMinfo_lastOrgLogId + 1;
                decimal intEnd = ConfigData.UIMinfo_lastOrgLogId + logSpan;
                string strLogInfo = string.Format("{0}：尝试获取组织异动信息,logid:{1}-{2}"
                    , strCurStrategyInfo
                    , intStart
                    , intEnd);
                LogRecord.WriteLog(strLogInfo, LogResult.normal);
                AddStrategyLog(strLogInfo, true);

                string objectXML = "<?xml version=\"1.0\" encoding=\"GBK\"?>"
                                        + "<root><data>"
                                            + "<APPLICATION>"
                                                + "<APPID>{0}</APPID>"
                                                + "<WEBSERVICEPWD>{1}</WEBSERVICEPWD>"
                                            + "</APPLICATION >"
                                            + "<EOSORG_T_ORGANIZATION_LOG>"
                                                + "<logID criteria=\"between\">{2}:{3}</logID>"
                                            + "</EOSORG_T_ORGANIZATION_LOG>"
                                        + "</data></root>";
                objectXML = string.Format(objectXML
                    , ConfigData.UIMinfo_appid
                    , ConfigData.UIMinfo_webservicepwd
                    , intStart
                    , intEnd);
                string[] args = new string[5];
                args[0] = packageName;//packageName 
                args[1] = unitID;//unitID
                args[2] = automata_OrgHis;//automata
                args[3] = password;//password
                args[4] = objectXML;//inputXML 
                result.Message = WebServicesHelper.InvokeWebService(ConfigData.UIMinfo_url, ConfigData.UIMinfo_classname, ConfigData.UIMinfo_methodname, args).ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "获取组织异动信息失败：" + ex.ToString();
            }

            if (result.IsSuccess)
            {
                LogRecord.WriteLog(string.Format("{0}：获取组织异动信息成功", strCurStrategyInfo), LogResult.normal);
                AddStrategyLog(string.Format("{0}：获取组织异动信息成功", strCurStrategyInfo), true);
            }
            else
            {
                LogRecord.WriteLog(string.Format("{0}：获取组织异动信息失败", strCurStrategyInfo), LogResult.fail);
                AddStrategyLog(string.Format("{0}：获取组织异动信息失败", strCurStrategyInfo), false);
            }

            //result.IsSuccess = true;
            //result.Message = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><data><APPLICATION><APPID>2c818836ff1c4003a8ca50db67088280</APPID><WEBSERVICEPWD>abcd1234</WEBSERVICEPWD><ERRMESSAGE><ERRCODE>88</ERRCODE><ERRDESC>验证通过</ERRDESC></ERRMESSAGE></APPLICATION><list type=\"ORANIZATION_LOG\" rowNum=\"2\"><EOSORG_V_ORGANIZATION_LOG><companyID>63A107F1CC13452D9349CA6F45A49DCD</companyID><orgSEQ></orgSEQ><OUOrder>93</OUOrder><departmentID>b178f8a0526b4c4aa54e9b622a1a457a</departmentID><region>gmcc</region><hrCompanyID></hrCompanyID><userGroup>潮州家宽组</userGroup><parentOUGUID>b178f8a0526b4c4aa54e9b622a1a457a</parentOUGUID><company>潮州分公司</company><department>第三方人员</department><parentOUID>24400000021</parentOUID><teams>PN_OT_1459155104990,kejingjuan,kejingjuan</teams><hrDepartmentID></hrDepartmentID><branch></branch><orgTypeID>dw</orgTypeID><OUID>24400191867</OUID><hall></hall><parentOrgID>24400000021</parentOrgID><createTime>20161014103828</createTime><orgDN>o=潮州家宽组,o=第三方人员,o=潮州分公司,dc=gmcc,dc=net</orgDN><regionID>85D176CCBC8942F3B54DE050BE748A58</regionID><lastModifyTime>20161014103828</lastModifyTime><orgCode></orgCode><hrBranchID></hrBranchID><OUGUID>9844882dfe740c538eb145e19ee27814</OUGUID><hrRegionID></hrRegionID><OUFullName>潮州家宽组</OUFullName><hrSalepointID></hrSalepointID><orgState>1</orgState><hrUserGroupID></hrUserGroupID><OULevel>5</OULevel><branchID></branchID><orgLevel></orgLevel><userGroupID>9844882dfe740c538eb145e19ee27814</userGroupID><salepoint></salepoint><hrHallID></hrHallID><operationType>ADD</operationType><orgID>24400191867</orgID><salepointID></salepointID><hrLogID></hrLogID><OUName>潮州家宽组</OUName><orgName>潮州家宽组</orgName><logID>1340329</logID></EOSORG_V_ORGANIZATION_LOG><EOSORG_V_ORGANIZATION_LOG><companyID>63A107F1CC13452D9349CA6F45A49DCD</companyID><orgSEQ></orgSEQ><OUOrder>94</OUOrder><departmentID>b178f8a0526b4c4aa54e9b622a1a457a</departmentID><region>gmcc</region><hrCompanyID></hrCompanyID><userGroup>凤新东厅</userGroup><parentOUGUID>b178f8a0526b4c4aa54e9b622a1a457a</parentOUGUID><company>潮州分公司</company><department>第三方人员</department><parentOUID>24400000021</parentOUID><teams>temp765,zhanzewen,zhanzewen</teams><hrDepartmentID></hrDepartmentID><branch></branch><orgTypeID>dw</orgTypeID><OUID>24400191932</OUID><hall></hall><parentOrgID>24400000021</parentOrgID><createTime>20161019164834</createTime><orgDN>o=凤新东厅,o=第三方人员,o=潮州分公司,dc=gmcc,dc=net</orgDN><regionID>85D176CCBC8942F3B54DE050BE748A58</regionID><lastModifyTime>20161019164834</lastModifyTime><orgCode></orgCode><hrBranchID></hrBranchID><OUGUID>36c174b4ffe93620e5d01031e1499341</OUGUID><hrRegionID></hrRegionID><OUFullName>凤新东厅</OUFullName><hrSalepointID></hrSalepointID><orgState>1</orgState><hrUserGroupID></hrUserGroupID><OULevel>5</OULevel><branchID></branchID><orgLevel></orgLevel><userGroupID>36c174b4ffe93620e5d01031e1499341</userGroupID><salepoint></salepoint><hrHallID></hrHallID><operationType>ADD</operationType><orgID>24400191932</orgID><salepointID></salepointID><hrLogID></hrLogID><OUName>凤新东厅</OUName><orgName>凤新东厅</orgName><logID>1340506</logID></EOSORG_V_ORGANIZATION_LOG></list><continueFlag>true</continueFlag></data><return><code>1</code></return></root>";
            return result;
        }
        /// <summary>
        /// 获取用户异动信息xml
        /// </summary>
        /// <returns></returns>
        private SystemResult GetUserTranXml()
        {
            LogRecord.WriteLog(string.Format("{0}：开始获取用户异动信息", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}：开始获取用户异动信息", strCurStrategyInfo), true);

            SystemResult result = new SystemResult();
            try
            {
                decimal intStart = ConfigData.UIMinfo_lastUserLogId + 1;
                decimal intEnd = ConfigData.UIMinfo_lastUserLogId + logSpan;
                string strLogInfo = string.Format("{0}尝试获取用户异动信息,logid:{1}-{2}"
                    , strCurStrategyInfo
                    , intStart
                    , intEnd);
                LogRecord.WriteLog(strLogInfo, LogResult.normal);
                AddStrategyLog(strLogInfo, true);


                string objectXML = "<?xml version=\"1.0\" encoding=\"GBK\"?>" +
                                    "<root><data>" +
                                        "<APPLICATION>" +
                                            "<APPID>{0}</APPID>" +
                                            "<WEBSERVICEPWD>{1}</WEBSERVICEPWD>" +
                                        "</APPLICATION>" +
                                        "<EOSORG_T_EMPLOYEE_LOG>" +
                                            "<logID criteria = \"between\">{2}:{3}</logID>" +
                                        "</EOSORG_T_EMPLOYEE_LOG>" +
                                    "</data></root>";

                objectXML = string.Format(objectXML
                    , ConfigData.UIMinfo_appid
                    , ConfigData.UIMinfo_webservicepwd
                    , intStart
                    , intEnd);
                string[] args = new string[5];
                args[0] = packageName;//packageName 
                args[1] = unitID;//unitID
                args[2] = automata_UserHis;//automata
                args[3] = password;//password
                args[4] = objectXML;//inputXML 
                result.Message = WebServicesHelper.InvokeWebService(ConfigData.UIMinfo_url, ConfigData.UIMinfo_classname, ConfigData.UIMinfo_methodname, args).ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "获取用户异动信息失败:" + ex.ToString();
            }

            if (result.IsSuccess)
            {//更新数据
                LogRecord.WriteLog(string.Format("{0}：获取用户异动信息成功", strCurStrategyInfo), LogResult.normal);
                AddStrategyLog(string.Format("{0}：获取用户异动信息成功", strCurStrategyInfo), true);
            }
            else
            {
                LogRecord.WriteLog(string.Format("{0}：获取用户异动信息失败", strCurStrategyInfo), LogResult.fail);
                AddStrategyLog(string.Format("{0}：获取用户异动信息失败", strCurStrategyInfo), false);
            }

            //result.IsSuccess = true;
            //result.Message = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><data><APPLICATION><APPID>2c818836ff1c4003a8ca50db67088280</APPID><WEBSERVICEPWD>abcd1234</WEBSERVICEPWD><ERRMESSAGE><ERRCODE>88</ERRCODE><ERRDESC>验证通过</ERRDESC></ERRMESSAGE></APPLICATION><list type=\"EMPLOYEE_LOG\" rowNum=\"1\"><EOSORG_V_EMPLOYEE_LOG><userDN></userDN><departmentID>7e54f1558db1562bb37b8c2922033542</departmentID><currentLevel></currentLevel><userJoinInDate>20100628000000</userJoinInDate><region>gmcc</region><userGroup>饶平分公司县城服营厅</userGroup><oldUserID></oldUserID><userNation>01</userNation><sex>2</sex><CMCCAccount>yuchunjia@gd.cmcc</CMCCAccount><userReligion>03</userReligion><hall></hall><createTime>20100702161919</createTime><orderID>10008</orderID><PHONE3G></PHONE3G><regionID>85D176CCBC8942F3B54DE050BE748A58</regionID><lastModifyTime>20161026164410</lastModifyTime><hrBranchID></hrBranchID><workOrgID></workOrgID><userPosiLevel>03</userPosiLevel><employeeID></employeeID><userBirthday>19880611000000</userBirthday><hrSalepointID></hrSalepointID><jobType>000001</jobType><telePhone>13827366667</telePhone><fullName>余淳佳</fullName><operationType>UPT</operationType><orgID>34920440020030702</orgID><companyID>63A107F1CC13452D9349CA6F45A49DCD</companyID><userGrade></userGrade><userID>yuchunjia</userID><title>高级营销代表助理(营业员)</title><changeTime></changeTime><address>饶平县黄冈镇汕汾中路388号</address><shortMobile></shortMobile><hrCompanyID></hrCompanyID><department>城区区域营销中心</department><hallID></hallID><company>潮州分公司</company><userQuitDate></userQuitDate><dutyDesc></dutyDesc><teams></teams><hrDepartmentID></hrDepartmentID><branch>饶平分公司</branch><OUID>220114163413</OUID><workPhone></workPhone><userType>Employee</userType><OUGUID>3fe2f8f8e0dde854104bb550bea4fc39</OUGUID><hrRegionID></hrRegionID><hrUserGroupID></hrUserGroupID><branchID>585C54AFA8BF4C5596A7A02DE0C3834D</branchID><userGroupID>3fe2f8f8e0dde854104bb550bea4fc39</userGroupID><salepoint></salepoint><hrHallID></hrHallID><employee>2021940678</employee><salepointID></salepointID><email>13827366667@139.com</email><workOUGUID>3fe2f8f8e0dde854104bb550bea4fc39</workOUGUID><employeeClass>80</employeeClass><logID>3098582</logID></EOSORG_V_EMPLOYEE_LOG></list><continueFlag>true</continueFlag></data><return><code>1</code></return></root>";
            return result;
        }

        /// <summary>
        /// 同步组织异动信息
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        private SystemResult UpdataOrgByLogData(List<Uum_Organizationinfo_Log> listData)
        {
            SystemResult result = new SystemResult();
            LogRecord.WriteLog(string.Format("{0}:共{1}条组织异动信息，进行同步...", strCurStrategyInfo, listData.Count), LogResult.normal);
            AddStrategyLog(string.Format("{0}:共{1}条组织异动信息，进行同步...", strCurStrategyInfo, listData.Count), true);

            listData = listData.OrderBy(u => u.logID).ToList();
            foreach (var item in listData)
            {
                var a1 = _uumOrgLogService.FindById(item.OUGUID);
                if (a1 == null)
                    _uumOrgLogService.Insert(item);//插入log记录
                var uumInfo = _uumOrgInfoService.FindById(item.OUGUID);
                var sysInfo = _sysDeptService.FindByFeldName(u => u.DpId == item.OUGUID);

                if (item.operationType == "DEL")
                {
                    #region 删除数据
                    if (uumInfo != null)
                        _uumOrgInfoService.Delete(uumInfo);
                    if (sysInfo != null)
                        _sysDeptService.Delete(sysInfo);
                    #endregion
                }
                else if (item.operationType == "ADD" || item.operationType == "UPT")
                {
                    #region 添加或更新数据
                    if (uumInfo == null)
                    {
                        uumInfo = new Uum_Organizationinfo();
                        CommonFun.AutoMapping<Uum_Organizationinfo_Log, Uum_Organizationinfo>(item, uumInfo);
                        _uumOrgInfoService.Insert(uumInfo);
                    }
                    else
                    {
                        CommonFun.AutoMapping<Uum_Organizationinfo_Log, Uum_Organizationinfo>(item, uumInfo);
                        _uumOrgInfoService.Update(uumInfo);
                    }

                    if (sysInfo == null)
                    {
                        sysInfo = new Depts();
                        sysInfo.DpId = item.OUGUID;
                        sysInfo.ParentDpId = item.parentOUGUID;
                        sysInfo.DpName = item.OUName;
                        sysInfo.DpLevel = (int)item.OULevel;
                        int intTemp = 0;
                        sysInfo.DeptOrderNo = int.TryParse(item.OUOrder, out intTemp) ? intTemp : 0;
                        sysInfo.CreatedTime = item.createTime;
                        sysInfo.LastModTime = item.lastModifyTime;
                        sysInfo.IsTmpDp = false;
                        sysInfo.Type = (byte)(item.orgState == "1" ? 1 : 0);
                        sysInfo.DpFullName = "";

                        _sysDeptService.Insert(sysInfo);
                    }
                    else
                    {
                        sysInfo.DpId = item.OUGUID;
                        sysInfo.ParentDpId = item.parentOUGUID;
                        sysInfo.DpName = item.OUName;
                        sysInfo.DpLevel = (int)item.OULevel;
                        int intTemp = 0;
                        sysInfo.DeptOrderNo = int.TryParse(item.OUOrder, out intTemp) ? intTemp : 0;
                        sysInfo.CreatedTime = item.createTime;
                        sysInfo.LastModTime = item.lastModifyTime;
                        sysInfo.IsTmpDp = false;
                        sysInfo.Type = (byte)(item.orgState == "1" ? 1 : 0);
                        sysInfo.DpFullName = "";

                        _sysDeptService.Update(sysInfo);
                    }
                    #endregion
                }

            }

            return result;
        }
        /// <summary>
        /// 同步用户异动信息
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        private SystemResult UpdataUserByLogData(List<Uum_Userinfo_Log> listData)
        {
            SystemResult result = new SystemResult();
            LogRecord.WriteLog(string.Format("{0}:共{1}条用户异动信息，进行同步...", strCurStrategyInfo, listData.Count), LogResult.normal);
            AddStrategyLog(string.Format("{0}:共{1}条用户异动信息，进行同步...", strCurStrategyInfo, listData.Count), true);

            listData = listData.OrderBy(u => u.logID).ToList();
            foreach (var item in listData)
            {
                var a1 = _uumUserLogService.FindById(item.employee);
                if (a1 == null)
                    _uumUserLogService.Insert(item);//插入log记录
                var uumInfo = _uumUserInfoService.FindById(item.employee);//uum的用户信息
                var sysInfo = _sysUserService.FindByFeldName(u => u.EmployeeId == item.employee);//新系统的用户信息
                //var leaders = _uumLeadersService.FindByFeldName(u => u.EmployeeID == item.employee);//领导职位信息表
                //var posts = new CzUumPosts();//用户职位信息表
                //if (!string.IsNullOrEmpty(item.userID))
                //    posts = _uumPostsService.FindByFeldName(u => u.UserName == item.userID && !string.IsNullOrEmpty(u.UserName));
                //else
                //    posts = null;

                if (item.operationType == "DEL")
                {
                    #region 删除数据
                    if (uumInfo != null)
                        _uumUserInfoService.Delete(uumInfo);
                    if (sysInfo != null)
                    {
                        //sysInfo.Status = -1;
                        //_sysUserService.Update(sysInfo);
                        _sysUserService.Delete(sysInfo);
                    }
                    //领导职位关系表不修改
                    //if (leaders != null)
                    //    _uumLeadersService.Delete(leaders);
                    //if (posts != null)
                    //    _uumPostsService.Delete(posts);
                    #endregion
                }
                else if (item.operationType == "ADD" || item.operationType == "UPT")
                {//item.userID为空或''时，表示该用户已经离职
                    #region uumInfo 新增或修改数据
                    if (uumInfo == null)
                    {
                        uumInfo = new Uum_Userinfo();
                        CommonFun.AutoMapping<Uum_Userinfo_Log, Uum_Userinfo>(item, uumInfo);
                        _uumUserInfoService.Insert(uumInfo);
                    }
                    else
                    {
                        CommonFun.AutoMapping<Uum_Userinfo_Log, Uum_Userinfo>(item, uumInfo);
                        _uumUserInfoService.Update(uumInfo);
                    }
                    #endregion
                    #region sysInfo 新增或修改数据
                    if (sysInfo == null)
                    {
                        if (!string.IsNullOrEmpty(item.userID))
                        {
                            sysInfo = new Users();
                            sysInfo.EmployeeId = item.employee;
                            sysInfo.UserName = item.userID;
                            sysInfo.RealName = item.fullName;
                            sysInfo.DpId = item.OUGUID;
                            sysInfo.Tel = item.workPhone;
                            sysInfo.Mobile = item.telePhone;
                            sysInfo.Email = item.email;
                            sysInfo.CreatedTime = item.createTime;
                            sysInfo.LastModTime = item.lastModifyTime;
                            sysInfo.JoinTime = item.userJoinInDate;
                            if (item.jobType == "000001")
                                sysInfo.UserType = 1;
                            else
                                sysInfo.UserType = null;

                            sysInfo.UserId = Guid.NewGuid();
                            sysInfo.Password = "BjGwHxyM6ZUeR8X0MZ0PcA==";//password
                            if (item.userType.ToLower() == "employee")
                                sysInfo.Type = 0;
                            else
                                sysInfo.Type = 1;
                            //sysInfo.Status = string.IsNullOrEmpty(item.userID) ? -1 : 0;
                            _sysUserService.Insert(sysInfo);
                        }

                    }
                    else
                    {
                        string[] jobTypes = { "000001", "000004", "000009", "000010", "000088", "000099" };
                        if (jobTypes.Contains(item.jobType) && !string.IsNullOrEmpty(item.userID))
                        {
                            sysInfo.UserType = int.Parse(item.jobType);
                            sysInfo.EmployeeId = item.employee;
                            sysInfo.UserName = item.userID;
                            sysInfo.RealName = item.fullName;
                            sysInfo.DpId = item.OUGUID;
                            sysInfo.Tel = item.workPhone;
                            sysInfo.Mobile = item.telePhone;
                            sysInfo.Email = item.email;
                            sysInfo.CreatedTime = item.createTime;
                            sysInfo.LastModTime = item.lastModifyTime;
                            //sysInfo.Status = string.IsNullOrEmpty(item.userID) ? -1 : 0;
                            sysInfo.LastModTime = item.lastModifyTime;
                            sysInfo.JoinTime = item.userJoinInDate;
                            if (item.userType.ToLower() == "employee")
                                sysInfo.Type = 0;
                            else
                                sysInfo.Type = 1;
                            _sysUserService.Update(sysInfo);
                        }
                        else
                            _sysUserService.Delete(sysInfo);

                    }

                    #endregion

                    //#region leaders 新增或修改数据
                    //if (leaders == null)
                    //{
                    //    if (!string.IsNullOrEmpty(item.employeeClass)
                    //        && item.employeeClass != "0" && item.employeeClass != "80")
                    //    {
                    //        leaders = new CzUumLeaders();
                    //        leaders.DepartmentID = item.OUGUID;
                    //        leaders.EmployeeID = item.employee;
                    //        leaders.UserName = sysInfo.UserName;
                    //        leaders.EmployeeClass = item.employeeClass;
                    //        leaders.JobType = item.jobType;
                    //        leaders.UserType = item.userType;
                    //        _uumLeadersService.Insert(leaders);
                    //    }

                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(item.employeeClass)
                    //        && item.employeeClass != "0" && item.employeeClass != "80")
                    //    {
                    //        _uumLeadersService.Delete(leaders);
                    //    }
                    //    else
                    //    {
                    //        leaders.DepartmentID = item.OUGUID;
                    //        leaders.EmployeeID = item.employee;
                    //        leaders.UserName = item.userID;
                    //        leaders.EmployeeClass = item.employeeClass;
                    //        leaders.JobType = item.jobType;
                    //        leaders.UserType = item.userType;
                    //        _uumLeadersService.Update(leaders);
                    //    }

                    //}

                    //#endregion
                    //#region posts 
                    //if (posts == null)
                    //{
                    //    posts = new CzUumPosts();
                    //    posts.UserName = sysInfo.UserName;
                    //    posts.OUID = item.OUGUID;
                    //    posts.EmployeeClass = item.employeeClass;
                    //    posts.EmployeeLevel = item.employeeClass;
                    //    posts.IsSync = true;
                    //    _uumPostsService.Insert(posts);
                    //}
                    //else
                    //{
                    //    posts.UserName = sysInfo.UserName;
                    //    posts.OUID = item.OUGUID;
                    //    posts.EmployeeClass = item.employeeClass;
                    //    posts.EmployeeLevel = item.employeeClass;
                    //    posts.IsSync = true;
                    //    _uumPostsService.Update(posts);
                    //}

                    //#endregion
                }


            }

            return result;
        }


        /// <summary>
        /// 执行存储过程，更新组织表Depts的DpFullName
        /// </summary>
        private SystemResult ExecuteProcedure_Update_Depts_DpFullName()
        {
            SystemResult result = new SystemResult();
            try
            {
                var mm = new EfRepository<string>().Execute<string>("exec updateDataAfterGetUUM");
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "执行存储过程updateDataAfterGetUUM失败";
                return result;
            }
            result.IsSuccess = true;
            return result;
        }
    }

}
