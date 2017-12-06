using CZManageSystem.Core;
using CZManageSystem.Service.SysManger;
using ServiceLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CZManageServer;

namespace winCeshi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //new ExecuteServices();
            //HrSyncManager mm = new HrSyncManager();
            //mm.sServiceStrategyID = "14";
            //bool aa = mm.Execute();
            HrSyncManager mm = new HrSyncManager();
            mm.HrDataAction();
        }

    }
}
