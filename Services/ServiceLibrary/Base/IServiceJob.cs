using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 系统服务接口
/// </summary>
namespace ServiceLibrary
{
    public interface IServiceJob
    {
        string sServiceStrategyID { get; set; }//服务策略ID
        string sMessage { get; set; }//执行信息
        bool Execute();//执行服务
    }
}
