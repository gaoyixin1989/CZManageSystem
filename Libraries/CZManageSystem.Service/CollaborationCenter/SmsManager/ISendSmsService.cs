using CZManageSystem.Data.Domain.CollaborationCenter.SmsManager;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

/// <summary>
/// 短信发送表
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.SmsManager
{
    public interface ISendSmsService : IBaseService<SendSms>
    {
        IList<SendSms> GetForPaging(out int count, SendSmsQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
