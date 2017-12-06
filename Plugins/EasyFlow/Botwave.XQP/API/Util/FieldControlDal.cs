using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Allocator;
using Botwave.Workflow.Extension.Service;

namespace Botwave.XQP.API.Util
{
    /// <summary>
    /// 按业务/字段任务分配算符.
    /// </summary>
    public class FieldControlDal
    {
        #region ITaskAllocator Members

        /// <summary>
        /// 获取目标用户.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public static IList<string> GetTargetUsers(IDictionary<string, object> fieldDict,string workflowName,string activityName)
        {
            string sqlTemplate = @"select FieldName,FieldValue,ExtCondition,TargetUsers from xqp_WorkflowFieldControl 
            where ActivityName = '{0}' and WorkflowName ='{1}'";
            string sql = String.Format(sqlTemplate, activityName, workflowName);
            bool hasFieldControl = false;

            //获取表单数据及业务字段控制数据            
            //IDictionary<FieldName, IDictionary<FieldValue, ICollection<KeyValuePair<ExtCondition, TargetUsers>>>>
            IDictionary<string, IDictionary<string, IList<KeyValuePair<string, string>>>> fieldSettings = new Dictionary<string, IDictionary<string, IList<KeyValuePair<string, string>>>>();
            IDictionary<string, object> data = new Dictionary<string, object>();
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(System.Data.CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    hasFieldControl = true;
                    string fieldName = reader.GetString(0);
                    string fieldValue = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);
                    string extCondition = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);
                    string targetUsers = reader.IsDBNull(3) ? String.Empty : reader.GetString(3);
                    if (fieldValue.Length == 0 || targetUsers.Length == 0)
                    {
                        continue;
                    }

                    if (fieldSettings.ContainsKey(fieldName))
                    {
                        if (!fieldSettings[fieldName].ContainsKey(fieldValue))
                        {
                            fieldSettings[fieldName][fieldValue] = new List<KeyValuePair<string, string>>();
                        }
                    }
                    else
                    {
                        IDictionary<string, IList<KeyValuePair<string, string>>> dict = new Dictionary<string, IList<KeyValuePair<string, string>>>();
                        dict.Add(fieldValue, new List<KeyValuePair<string, string>>());
                        fieldSettings.Add(fieldName, dict);
                    }
                    fieldSettings[fieldName][fieldValue].Add(new KeyValuePair<string, string>(extCondition, targetUsers));
                }

                if (hasFieldControl)
                {
                    data = fieldDict;
                }
                reader.Close();
            }

            //如果没有业务字段控制
            if (!hasFieldControl)
            {
                return null;
            }

            foreach (string fieldName in fieldSettings.Keys)
            {
                //如果表单数据中包含控制字段
                if (data.ContainsKey(fieldName))
                {
                    string fieldValue = DbUtils.ToString(data[fieldName]).Trim();
                    if (fieldSettings[fieldName].ContainsKey(fieldValue))//如果表单项的值也在控制字段定义中
                    {
                        if (fieldSettings[fieldName][fieldValue].Count == 1)
                        {
                            return SplitUserStr(fieldSettings[fieldName][fieldValue][0].Value);
                        }
                        else//可能会由多个字段共同控制
                        {
                            foreach (KeyValuePair<string, string> pair in fieldSettings[fieldName][fieldValue])
                            {
                                //extCondition格式：字段名称1:字段值,字段名称2:字段值
                                string extCondition = pair.Key;
                                string targetUsers = pair.Value;                                
                                if (!String.IsNullOrEmpty(extCondition))
                                {
                                    bool isValid = true;
                                    string[] expressions = extCondition.Split(',');
                                    foreach (string expression in expressions)
                                    {
                                        string[] ss = expression.Split(':');
                                        if (ss.Length == 2)
                                        {
                                            string k = ss[0].Trim();
                                            string v = ss[1].Trim();
                                            if (!data.ContainsKey(k) || DbUtils.ToString(data[k]) != v)//只要有一项不满足
                                            {
                                                isValid = false;
                                                break;
                                            }
                                        }
                                    }

                                    if (isValid)
                                    {
                                        return SplitUserStr(targetUsers);
                                    }
                                }
                            }
                        }        
                    }
                }
            }
            return new List<string>();
        }

        #endregion

        static IList<string> SplitUserStr(string users)
        {
            if (String.IsNullOrEmpty(users))
            {
                return null;
            }

            IList<string> list = new List<string>();
            string[] ss = users.Split(',');
            foreach (string s in ss)
            {
                list.Add(s);
            }
            return list;
        }
    }
}
