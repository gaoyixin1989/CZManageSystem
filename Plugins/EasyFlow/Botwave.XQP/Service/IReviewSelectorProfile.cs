using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Botwave.Entities;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 流程抄送选择器界面接口.
    /// </summary>
    public interface IReviewSelectorProfile
    {
        /// <summary>
        /// 生成选择器 HTML.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        string BuildSelectorHtml(HttpContext webContext, ReviewSelectorContext selecotrContext);
    }

    /// <summary>
    /// 抄送上下文.
    /// </summary>
    [Serializable]
    public class ReviewSelectorContext
    {
        /// <summary>
        /// 变量名称.
        /// </summary>
        public static readonly string VariableName = "reviewActors_values";

        private Guid _currentActivityInstanceId;
        private BasicUser _currentActor;
        private IList<ActivityProfile> _nextProfiles;

        public Guid CurrentActivityInstanceId
        {
            get { return _currentActivityInstanceId; }
            set { _currentActivityInstanceId = value; }
        }

        public BasicUser CurrentActor
        {
            get { return _currentActor; }
            set { _currentActor = value; }
        }

        public IList<ActivityProfile> NextProfiles
        {
            get { return _nextProfiles; }
            set { _nextProfiles = value; }
        }
    }
}
