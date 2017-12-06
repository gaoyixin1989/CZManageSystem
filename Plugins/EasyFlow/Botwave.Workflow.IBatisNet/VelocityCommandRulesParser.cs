using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using NVelocity;
using NVelocity.App;

using Botwave.Workflow.Parser;

namespace Botwave.Workflow.IBatisNet
{
    public class VelocityCommandRulesParser : AbstractCommandRulesParser
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(VelocityCommandRulesParser));

        #region ICommandRulesParser Members

        public override IDictionary<string, ICollection<string>> Parse(string rules, string command)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, ICollection<string>> Parse(string rules, Botwave.Workflow.ActivityExecutionContext context)
        {
            if (String.IsNullOrEmpty(rules))
            {
                return null;
            }

            VelocityEngine engine = VelocityEngineFactory.GetVelocityEngine();
            StringWriter sw = new StringWriter();
            VelocityContext vc = new VelocityContext();
            vc.Put("aec", context);
            vc.Put("helper", Helper.Instance);

            try
            {
                engine.Evaluate(vc, sw, "rules tag", rules);
            }
            catch (NVelocity.Exception.MethodInvocationException ex)
            {
                log.Error(ex);
            }

            //清除空白，使得步骤名称能正确找到
            StringBuilder sb = sw.GetStringBuilder();
            sb.Replace(" ", "");

            if (sb.Length == 0)
            {
                return null;
            }

            sb.Replace("\r", "");
            sb.Replace("\n", "");
            sb.Replace("\t", "");
            sb.Replace("\v", "");
            sb.Replace("\f", "");

            return ParseExpression(sb.ToString());
        }

        #endregion

        private class Helper
        {
            private static readonly Helper instance = new Helper();

            public static Helper Instance
            {
                get { return instance; }
            }

            public bool Compare(object obj, int targetValue)
            {
                int n = Int32.Parse(obj.ToString());
                return (n > targetValue);
            }

            public bool CompareFloat(object obj, object targetValue)
            {
                float n = float.Parse(obj.ToString());
                float m = float.Parse(targetValue.ToString());
                return (n > m);
            } 
        }
    }
}
