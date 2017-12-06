using System;
using System.Collections.Generic;
using System.Text;

using NVelocity;
using NVelocity.App;

namespace Botwave.Workflow.IBatisNet
{
    public static class VelocityEngineFactory
    {
        private static VelocityEngine velocityEngine;

        /// <summary>
        /// 获取VelocityEngine
        /// </summary>
        /// <returns></returns>
        public static VelocityEngine GetVelocityEngine()
        {
            if (null == velocityEngine)
            {
                lock (typeof(VelocityEngineFactory))
                {
                    if (null == velocityEngine)
                    {
                        velocityEngine = new VelocityEngine();
                        velocityEngine.Init();
                    }
                }
            }
            return velocityEngine;
        }
    }
}
