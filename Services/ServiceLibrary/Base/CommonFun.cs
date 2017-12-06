using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace ServiceLibrary.Base
{
    public class CommonFun
    {
        /// <summary>
        /// 利用json的序列化与反序列化，将对象强制类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T TranObjToObjByJson<T>(object obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strTemp = js.Serialize(obj);
            return js.Deserialize<T>(strTemp);
        }
                
        /// <summary>
        /// 根据属性名称，将第一个对象复制到第二个对象
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public static void AutoMapping<S, T>(S s, T t)
        {
            // get source PropertyInfos
            PropertyInfo[] pps = GetPropertyInfos(s.GetType());
            // get target type
            Type target = t.GetType();

            foreach (var pp in pps)
            {
                PropertyInfo targetPP = target.GetProperty(pp.Name);
                object value = pp.GetValue(s, null);

                if (targetPP != null && value != null)
                {
                    try { targetPP.SetValue(t, value, null); }
                    catch (Exception ex) { }

                }
            }
        }
        private static PropertyInfo[] GetPropertyInfos(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// 获取推送todoid
        /// </summary>
        /// <param name="dataSource">数据类别</param>
        /// <param name="dataId">数据id</param>
        /// <returns></returns>
        public static int GetId(string dataSource, string dataId)
        {
            IDataIdToIntService _dataIdToIntService = new DataIdToIntService();
            int id = 0;
            var temp = _dataIdToIntService.FindByFeldName(u => u.DataSource == dataSource && u.DataId == dataId);
            if (temp != null && temp.ID != 0)
                id = temp.ID;
            else
            {
                DataIdToInt obj = new DataIdToInt();
                obj.DataSource = dataSource;
                obj.DataId = dataId;
                if (_dataIdToIntService.Insert(obj))
                    id = obj.ID;
            }
            return id;
        }

        /// <summary>
        /// 新增或更新推送数据记录信息
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataId"></param>
        /// <param name="sendId"></param>
        /// <param name="owner"></param>
        /// <param name="remark"></param>
        /// <param name="isSuccess"></param>
        public static void UpdatePendingData(string dataSource, string dataId, int sendId, string owner, string remark, bool isSuccess)
        {
            IPendingDataService _pendingDataService = new PendingDataService();
            var obj = _pendingDataService.FindByFeldName(u => u.DataSource == dataSource && u.DataID == dataId && u.SendID == sendId && u.Owner == owner);
            if (obj != null && obj.ID != 0)
            {
                obj.State = isSuccess ? 2 : 1;
                obj.TryTime++;
                obj.DealTime = DateTime.Now;
                obj.Remark = remark;
                _pendingDataService.Update(obj);
            }
            else
            {
                obj = new PendingData()
                {
                    DataSource = dataSource,
                    DataID = dataId,
                    SendID = sendId,
                    Owner = owner,
                    State = isSuccess ? 2 : 1,
                    TryTime = 1,
                    DealTime = DateTime.Now,
                    Remark = remark
                };
                _pendingDataService.Insert(obj);
            }

        }


        /// <summary>
        /// 发送手机短信
        /// </summary>
        /// <param name="mobileto">接收手机号码</param>
        /// <param name="content">短信内容</param>
        /// <returns></returns>
        public static bool SendSms(string mobileto, string content)
        {
            SmsDAO.init(ConfigData.SMS_Connection);//初始化短信系统数据库连接字符串
            bool result = false;
            if (!string.IsNullOrEmpty(mobileto))
            {
                int temperrcode = SmsDAO.sqlExec(string.Format("Exec {0} '{1}','{2}','{3}','{4}','{5}','','{6}'"
                    , ConfigData.SMS_ProName
                    , mobileto
                    , ConfigData.SMS_Port
                    , mobileto
                    , content
                    , ConfigData.SMS_Type
                    , ConfigData.SMS_State));
                if (temperrcode <= 0)
                    result = true;
            }

            return result;
        }


    }
}
