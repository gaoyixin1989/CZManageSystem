using CZManageSystem.Data.Domain.Composite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Admin.Models
{
    public partial class BasicInfoViewModel
    {

        // ApplyID  ApplyTitle  ApplySn  Creator  CreatorID   CreateTime   MobilePhone   ThemeType   ThemeName   StartTime   EndTime   IsNiming  Remark   MemberName   TempdeptID   TempdeptName   TempuserID  TempuserName  IsProc
        /// <summary>
        /// 申请单号
        /// </summary>
        public int ApplyID { get; set; }
        /// <summary>
        /// 申请主题
        /// </summary>
        [Required]
        public string ApplyTitle { get; set; }
        /// <summary>
        /// 流程单号
        /// </summary>
        [Required]
        public string ApplySn { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [Required]
        public string Creator { get; set; }
        /// <summary>
        /// 申请人ID
        /// </summary>
        public Nullable<System.Guid> CreatorID { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Phone]
        public string MobilePhone { get; set; }

        /// <summary>
        /// 主题类型
        /// </summary>
        [Required]
        public string ThemeType { get; set; }
        /// <summary>
        /// 主题名称
        /// </summary>
        [Required]
        public string ThemeName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public Nullable<System.DateTime> StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public Nullable<System.DateTime> EndTime { get; set; }
        /// <summary>
        /// 是否匿名
        /// </summary>
        public string IsNiming { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 成员名
        /// </summary>
        public string MemberName { get; set; }
        public string MemberIDs { get; set; }
        public Nullable<int> MemberType { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string TempdeptID { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string TempdeptName { get; set; }
        /// <summary>
        /// 所在部门
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string TempuserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string TempuserName { get; set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        public Nullable<int> IsProc { get; set; }
    }

    public partial class QuestionInfoViewModel
    {
        public QuestionInfoViewModel()
        {
            this.VoteAnsers = new List<AnserInfoViewModel>();
        }
        public Nullable<System.Guid> CreatorID { get; set; }
        public int QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionType { get; set; }
        public Nullable<int> AnswerNum { get; set; }
        public Nullable<int> MaxValue { get; set; }
        public Nullable<int> MinValue { get; set; }
        public virtual ICollection<AnserInfoViewModel> VoteAnsers { get; set; }
    }
    public partial class AnserInfoViewModel
    { 
        public   int AnserID { get; set; }
        public Nullable<int> QuesionID { get; set; }
        public string AnserContent { get; set; }
        public Nullable<decimal> AnserScore { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> MaxValue { get; set; }
        public Nullable<int> MinValue { get; set; }
    }
    public partial class SelectedAnserViewModel
    {
        public int SelectedAnserID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<int> ThemeID { get; set; }
        public Nullable<int> QuestionID { get; set; }
        public Nullable<int> AnserID { get; set; }
        public string OtherContent { get; set; }
        public virtual VoteAnser VoteAnser { get; set; }
        public virtual VoteQuestion VoteQuestion { get; set; }
    }

}
