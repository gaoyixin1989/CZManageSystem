using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 测试用例子
/// </summary>
namespace ServiceLibrary
{
    public class ExecuteContent : ServiceJob
    {
        public override bool Execute()
        {
            sMessage = "hello;world";
            return true;
        }
    }
}
