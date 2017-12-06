using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
    public class CarFeeYear
    {
        /// <summary>
        /// 主键
        /// <summary>
        [Required(ErrorMessage = "主键不能为空")]
        public Guid CarFeeYearId { get; set; }
        /// <summary>
        /// 编辑人ID
        /// <summary>
        [Required(ErrorMessage = "编辑人ID不能为空")]
        public Nullable<Guid> EditorId { get; set; }
        /// <summary>
        /// 编辑日期
        /// <summary>
        [Required(ErrorMessage = "编辑日期不能为空")]
        public Nullable<DateTime> EditTime { get; set; }
        /// <summary>
        /// 所属单位
        /// <summary>
        [Required(ErrorMessage = "找不到相应的所属单位ID")]
        public Nullable<int> CorpId { get; set; }
        /// <summary>
        /// 使用单位
        /// <summary>
        [Required(ErrorMessage = "使用单位不能为空")]
        public string CorpName { get; set; }
        /// <summary>
        /// 车辆ID
        /// <summary>
        [MinLength (10,ErrorMessage = "找不到相应的车辆ID")]
        public Nullable<Guid> CarId { get; set; }
        /// <summary>
        /// 缴费日期
        /// <summary>
        public Nullable<DateTime> PayTime { get; set; }
        /// <summary>
        /// 费用小计
        /// <summary>
        public Nullable<decimal> TotalFee { get; set; }
        /// <summary>
        /// 经手人
        /// <summary>
        [Required(ErrorMessage = "经手人不能为空")]
        public string Person { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }
        public virtual CarInfo CarInfo { get; set; }

    }
}
