using System;
using System.Collections.Generic;
using System.Text;
using NVelocity;
using NVelocity.App;

namespace Botwave.DynamicForm
{
    /// <summary>
    /// Velocity 引擎工厂类.
    /// </summary>
    public static class VelocityEngineFactory
    {
        private static VelocityEngine velocityEngine;

        /// <summary>
        /// 获取 VelocityEngine.
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
