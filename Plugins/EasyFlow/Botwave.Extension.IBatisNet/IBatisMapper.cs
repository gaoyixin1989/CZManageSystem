using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using IBatisNet;
using IBatisNet.Common;
using IBatisNet.DataMapper;
using common = IBatisNet.Common;
using dataMapper = IBatisNet.DataMapper;

namespace Botwave.Extension.IBatisNet
{
    /// <summary>
    /// IBatisNet Mapper 辅助类.
    /// </summary>
    public static class IBatisMapper
    {
        /// <summary>
        /// IBatisNet 的 ISqlMapper 实例对象.
        /// 修改数据库连接字符串，从 web.config 中读取相应的配置节点.
        /// </summary>
        public static readonly ISqlMapper Mapper;

        static IBatisMapper()
        {
            Mapper = dataMapper.Mapper.Instance();
            if (Mapper.DataSource == null)
                return;
            string key = Mapper.DataSource.ConnectionString;
            string connectionString = System.Configuration.ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(connectionString))
            {
                Mapper.DataSource.ConnectionString = connectionString;
            }
        }

        /// <summary>
        /// 插入指定 statementId、paramValue 的数据.
        /// </summary>
        /// <param name="statementId"></param>
        /// <param name="paramValue"></param>
        public static object Insert(string statementId, object paramValue)
        {
            return Mapper.Insert(statementId, paramValue);
        }

        /// <summary>
        /// 更新指定 statementId、paramValue 的数据.
        /// </summary>
        /// <param name="statementId"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static int Update(string statementId, object paramValue)
        {
            return Mapper.Update(statementId, paramValue);
        }

        /// <summary>
        /// 删除指定 statementId、paramValue 的数据.
        /// </summary>
        /// <param name="statementId"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static int Delete(string statementId, object paramValue)
        {
            return Mapper.Delete(statementId, paramValue);
        }

        /// <summary>
        /// 根据 statementId、paramValue 获取对象T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statementId"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static T Load<T>(string statementId, object paramValue)
        {
            IList<T> list = Mapper.QueryForList<T>(statementId, paramValue);
            if (null == list || list.Count == 0)
            {
                return default(T);
            }
            return list[0];
        }

        /// <summary>
        /// 根据 statementId 获取对象 T 列表
        /// </summary>
        /// <returns></returns>
        public static IList<T> Select<T>(string statementId)
        {
            return Mapper.QueryForList<T>(statementId, null);
        }

        /// <summary>
        /// 根据statementId、paramValue获取对象T列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statementId"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static IList<T> Select<T>(string statementId, object paramValue)
        {
            return Mapper.QueryForList<T>(statementId, paramValue);
        }
    }
}
