using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.ShiftManages
{

    public class ZhibanQueryBuilder
    {
        public Guid? Editor { get; set; }//编辑者
        public string Year { get; set; }//年份
        public string Month { get; set; }//月份
        public string State { get; set; }//状态
        public List<string> DeptId { get; set; }//部门ID
    }

    /// <summary>
    /// 排班信息表
    /// </summary>
    public class ShiftZhiban
    {

        public ShiftZhiban()
        {
            this.ShiftBancis = new List<ShiftBanci>();
            this.ShiftLunbans = new List<ShiftLunban>();
        }
        /// <summary>
        /// 排班ID
        /// <summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// <summary>
        public string Title { get; set; }
        /// <summary>
        /// 编辑人
        /// <summary>
        public Nullable<Guid> Editor { get; set; }
        /// <summary>
        /// 编辑时间
        /// <summary>
        public Nullable<DateTime> EditTime { get; set; }
        /// <summary>
        /// 部门ID
        /// <summary>
        public string DeptId { get; set; }
        /// <summary>
        /// 年
        /// <summary>
        public string Year { get; set; }
        /// <summary>
        /// 月
        /// <summary>
        public string Month { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态：0-未提交，1-提交
        /// <summary>
        public string State { get; set; }


        public virtual Users EditorObj { get; set; }
        public virtual Depts DeptObj { get; set; }
        public virtual ICollection<ShiftBanci> ShiftBancis { get; set; }
        public virtual ICollection<ShiftLunban> ShiftLunbans { get; set; }
    }
}
