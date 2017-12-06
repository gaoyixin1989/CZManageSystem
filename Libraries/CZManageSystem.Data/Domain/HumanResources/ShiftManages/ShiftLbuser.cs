using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.ShiftManages
{
    /// <summary>
    /// 轮班用户
    /// </summary>
	public class ShiftLbuser
    {
        /// <summary>
        /// 编号ID
        /// <summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 轮班信息表Id
        /// <summary>
        public Guid LunbanId { get; set; }
        /// <summary>
        /// 轮班用户id
        /// <summary>
        public Guid UserId { get; set; }


        public virtual Users UserObj { get; set; }

    }
}
