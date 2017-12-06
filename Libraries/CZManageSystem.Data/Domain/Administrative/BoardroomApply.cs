using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative
{
    public class BoardroomApplyQueryBuilder
    {
        public int[] Room { get; set; }//会议室
        public int[] State { get; set; }//状态
        public int[] State_without { get; set; }//不包括的状态
        public string[] AppPerson { get; set; }//申请人
        public int[] JudgeState { get; set; }//评价状态

        public DateTime? AppTime_start { get; set; }//申请时间
        public DateTime? AppTime_end { get; set; }
        public DateTime? MeetingDate_start { get; set; }//查询会议日期
        public DateTime? MeetingDate_end { get; set; }
        public DateTime? MeetingTime_start { get; set; }//查询会议时间
        public DateTime? MeetingTime_end { get; set; }

        public DateTime? EndDate_Real_start { get; set; }//会议结束时间范围
        public DateTime? EndDate_Real_end { get; set; }

    }

    public class BoardroomApply
    {
        /// <summary>
        /// 编号
        /// <summary>
        public int ID { get; set; }
        /// <summary>
        /// 申请ID
        /// <summary>
        public string WorkflowInstanceId { get; set; }
        /// <summary>
        /// 主题
        /// <summary>
        public string ApplyTitle { get; set; }
        /// <summary>
        /// 所属单位
        /// <summary>
        public Nullable<int> CorpID { get; set; }
        /// <summary>
        /// 会议结束日期
        /// <summary>
        public Nullable<DateTime> EndDate { get; set; }
        /// <summary>
        /// 会议的真实结束时间(默认为申请时的结束时间)
        /// <summary>
        public Nullable<DateTime> EndDate_Real { get; set; }
        /// <summary>
        /// 流程单号
        /// <summary>
        public string Code { get; set; }
        /// <summary>
        /// 状态：0待提交、1已经提交、-1撤销
        /// <summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// 状态备注，撤销状态时必须填写撤销原因
        /// <summary>
        public string StateRemark { get; set; }
        /// <summary>
        /// 申请部门
        /// <summary>
        public string AppDept { get; set; }
        /// <summary>
        /// 申请人
        /// <summary>
        public string AppPerson { get; set; }
        /// <summary>
        /// 申请时间
        /// <summary>
        public Nullable<DateTime> AppTime { get; set; }
        /// <summary>
        /// 联系电话
        /// <summary>
        public string ContactMobile { get; set; }
        /// <summary>
        /// 会议室ID
        /// <summary>
        public Nullable<int> Room { get; set; }
        /// <summary>
        /// 与会人数
        /// <summary>
        public string JoinNum { get; set; }
        /// <summary>
        /// 与会人员
        /// <summary>
        public string JoinPeople { get; set; }
        /// <summary>
        /// 会议开始日期
        /// <summary>
        public Nullable<DateTime> MeetingDate { get; set; }
        /// <summary>
        /// 开始时间
        /// <summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// <summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 会议提醒时间
        /// <summary>
        public Nullable<DateTime> AwokeTime { get; set; }
        /// <summary>
        /// 会议时间段
        /// <summary>
        public string UseFor { get; set; }
        /// <summary>
        /// 领导参加情况
        /// <summary>
        public string Fugle { get; set; }
        /// <summary>
        /// 是否有省公司领导参加
        /// <summary>
        public Nullable<bool> Fugle_pro { get; set; }
        /// <summary>
        /// 所需设备
        /// <summary>
        public string NeedEquip { get; set; }
        /// <summary>
        /// 需要的其他设备
        /// <summary>
        public string OtherEquip { get; set; }
        /// <summary>
        /// 横幅内容
        /// <summary>
        public string BannerContent { get; set; }
        /// <summary>
        /// 横幅长度
        /// <summary>
        public string BannerLength { get; set; }
        /// <summary>
        /// 横幅宽度
        /// <summary>
        public string BannerWidth { get; set; }
        /// <summary>
        /// 横幅模式
        /// <summary>
        public string BannerMode { get; set; }
        /// <summary>
        /// 欢迎词
        /// <summary>
        public string WelcomeContent { get; set; }
        /// <summary>
        /// 欢迎词播放时间
        /// <summary>
        public Nullable<DateTime> WelcoomeTime { get; set; }
        /// <summary>
        /// 播放段
        /// <summary>
        public string WelcoomeSect { get; set; }
        /// <summary>
        /// 订会时间
        /// <summary>
        public Nullable<DateTime> BookTime { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// 自定义字段
        /// <summary>
        public string Field00 { get; set; }
        public string Field01 { get; set; }
        public string Field02 { get; set; }
        /// <summary>
        /// 编辑人
        /// <summary>
        public string Editor { get; set; }
        /// <summary>
        /// 评价：服务质量：好、较好、一般、差
        /// <summary>
        public string JudgeServiceQuality { get; set; }
        /// <summary>
        /// 评价：环境卫生：好、较好、一般、差
        /// <summary>
        public string JudgeEnvironmental { get; set; }
        /// <summary>
        /// 其他建议
        /// <summary>
        public string JudgeOtherSuggest { get; set; }
        /// <summary>
        /// 评价状态：0或空未评价，1已经评价，2自动评价
        /// <summary>
        public Nullable<int> JudgeState { get; set; }
        /// <summary>
        /// 是否电视会议
        /// <summary>
        public Nullable<bool> ISTVMeeting { get; set; }
        /// <summary>
        /// 评价时间
        /// <summary>
        public Nullable<DateTime> JudgeTime { get; set; }
    }
}
