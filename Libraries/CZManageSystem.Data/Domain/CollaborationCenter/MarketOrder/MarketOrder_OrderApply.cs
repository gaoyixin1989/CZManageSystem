using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// 营销订单-营销订单工单
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
    /// <summary>
    /// 历史项目暂估查询
    /// </summary>
    public class OrderApplyQueryBuilder
    {
        public string SerialNo { get; set; }//受理单编号
        public List<string> ListOrderStatus { get; set; }//受理单状态
        public DateTime? ApplyTime_start { get; set; }//发起时间、创建时间
        public DateTime? ApplyTime_end { get; set; }//发起时间、创建时间
        public List<string> ListStatus { get; set; }//流程状态
        public string CustomPhone { get; set; }//客户号码
        public string CustomName { get; set; }//客户名称
        public List<Guid?> ListEndTypeID { get; set; }//终端机型
        public List<Guid?> ListAreaID { get; set; }//所属区域
        public DateTime? DealTime_start { get; set; }//受理时间
        public DateTime? DealTime_end { get; set; }//受理时间 
        public string ProjectID { get; set; }//项目编号

        public string Title { get; set; }//标题
        public bool? isJK { get; set; }//是否家宽业务
        public Guid? Applicant { get; set; }//发起人
    }


    public class MarketOrder_OrderApply
    {
        /// <summary>
        /// 工单ID
        /// <summary>
        public Guid ApplyID { get; set; }
        /// <summary>
        /// 流程实例ID
        /// <summary>
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        /// <summary>
        /// 受理单编号
        /// <summary>
        public string SerialNo { get; set; }
        /// <summary>
        /// 发起时间
        /// <summary>
        public Nullable<DateTime> ApplyTime { get; set; }
        /// <summary>
        /// 发起人ID
        /// <summary>
        public Nullable<Guid> Applicant { get; set; }
        /// <summary>
        /// 发起人手机号码
        /// <summary>
        public string MobilePh { get; set; }
        /// <summary>
        /// 流程状态
        /// <summary>
        public string Status { get; set; }
        /// <summary>
        /// 受理单状态
        /// <summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// 标题
        /// <summary>
        public string Title { get; set; }
        /// <summary>
        /// 营销方案
        /// <summary>
        public Nullable<Guid> MarketID { get; set; }
        /// <summary>
        /// 目标客户号码
        /// <summary>
        public string CustomPhone { get; set; }
        /// <summary>
        /// 客户姓名
        /// <summary>
        public string CustomName { get; set; }
        /// <summary>
        /// 客户身份证号
        /// <summary>
        public string CustomPersonID { get; set; }
        /// <summary>
        /// 客户联系地址
        /// <summary>
        public string CustomAddr { get; set; }
        /// <summary>
        /// 客户联系电话
        /// <summary>
        public string CustomOther { get; set; }
        /// <summary>
        /// 终端机型ID
        /// <summary>
        public Nullable<Guid> EndTypeID { get; set; }
        /// <summary>
        /// 可用号码
        /// <summary>
        public string UseNumber { get; set; }
        /// <summary>
        /// SIM卡号
        /// <summary>
        public string SIMNumber { get; set; }
        /// <summary>
        /// 配送-IMEI码
        /// <summary>
        public string IMEI { get; set; }
        /// <summary>
        /// 基本套餐ID
        /// <summary>
        public Nullable<Guid> SetmealID { get; set; }
        /// <summary>
        /// 捆绑业务
        /// <summary>
        public Nullable<Guid> BusinessID { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// 所属区域ID
        /// <summary>
        public Nullable<Guid> AreaID { get; set; }
        /// <summary>
        /// 项目编号
        /// <summary>
        public string ProjectID { get; set; }
        public Nullable<DateTime> YZSubmitTime { get; set; }
        /// <summary>
        /// 配送状态（配送超时）
        /// </summary>
		public string SendStatus { get; set; }
        public string GDOrderID { get; set; }
        /// <summary>
        /// BOSS商品标识
        /// </summary>
        public string BossOfferID { get; set; }
        public string MainOrder { get; set; }
        public string SubOrder { get; set; }
        /// <summary>
        /// 邮件号码
        /// </summary>
        public string MailNo { get; set; }
        /// <summary>
        /// 发送对象账号
        /// </summary>
		public string SendTo { get; set; }

        /// <summary>
        /// 鉴权方式
        /// </summary>
        public Nullable<Guid> AuthenticationID { get; set; }


        public virtual Tracking_Workflow Tracking_Workflow { get; set; }//流程实例
        public virtual Users ApplicantObj { get; set; }//发起人
        public virtual MarketOrder_Market MarketObj { get; set; }//营销方案
        public virtual MarketOrder_EndType EndTypeObj { get; set; }//终端机型
        public virtual MarketOrder_Setmeal SetmealObj { get; set; }//基本套餐
        public virtual MarketOrder_Business BusinessObj { get; set; }//捆绑业务
        public virtual MarketOrder_Area AreaObj { get; set; }//所属区域
        public virtual MarketOrder_Authentication AuthenticationObj { get; set; }//鉴权方式

    }
}
