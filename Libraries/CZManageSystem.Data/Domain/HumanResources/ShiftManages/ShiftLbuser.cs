using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.ShiftManages
{
    /// <summary>
    /// �ְ��û�
    /// </summary>
	public class ShiftLbuser
    {
        /// <summary>
        /// ���ID
        /// <summary>
        public Guid Id { get; set; }
        /// <summary>
        /// �ְ���Ϣ��Id
        /// <summary>
        public Guid LunbanId { get; set; }
        /// <summary>
        /// �ְ��û�id
        /// <summary>
        public Guid UserId { get; set; }


        public virtual Users UserObj { get; set; }

    }
}
