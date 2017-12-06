using CZManageSystem.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CZManageServer
{
    public partial class MyService : ServiceBase
    {
        public MyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogRecord.WriteLog("启动服务", LogResult.success);
            new ExecuteServices();
        }

        protected override void OnStop()
        {
            LogRecord.WriteLog("关闭服务", LogResult.success);
        }

    }
}
