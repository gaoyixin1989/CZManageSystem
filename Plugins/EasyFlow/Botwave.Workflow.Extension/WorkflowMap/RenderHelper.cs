using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Botwave.StateGraph;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

namespace Botwave.Workflow.Extension.WorkflowMap
{
    /// <summary>
    /// 流程图呈现辅助类.
    /// </summary>
    public class RenderHelper
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(RenderHelper));

        private const string CacheManagerWorkflowMap = "Workflow Map Cache Manager";
        private static CacheManager _cache = CacheFactory.GetCacheManager(CacheManagerWorkflowMap);

        #region service interfaces

        private static IActivityDefinitionService activityDefinitionService;

        /// <summary>
        /// 流程活动(步骤)定义服务.
        /// </summary>
        public static IActivityDefinitionService ActivityDefinitionService
        {
            get { return activityDefinitionService; }
            set { activityDefinitionService = value; }
        }
        #endregion

        #region GraphNodes

        /// <summary>
        /// 获取指定流程编号的节点对象列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static IList<GraphNode> GetGraphNodes(Guid workflowId)
        {
            IList<ActivityDefinition> activities = activityDefinitionService.GetSortedActivitiesByWorkflowId(workflowId);
            return ToGraphNodes(activities);
        }

        /// <summary>
        /// 获取指定流程编号以及起始流程步骤编号的节点对象列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="currentActivityId"></param>
        /// <returns></returns>
        public static IList<GraphNode> GetGraphNodes(Guid workflowId, Guid currentActivityId)
        {
            IList<ActivityDefinition> activities = ActivityDefinitionService.GetPartActivities(workflowId, currentActivityId);
            return ToGraphNodes(activities);
        }

        /// <summary>
        /// 转换为  Botwave.StateGraph.GraphNode 对象集合.
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        public static IList<GraphNode> ToGraphNodes(IList<ActivityDefinition> activities)
        {
            IList<GraphNode> nodes = new List<GraphNode>();
            int count = activities.Count;
            for (int i = 0; i < count; i++)
            {
                ActivityDefinition activity = activities[i];
                GraphNode item = new GraphNode(i + 1, activity.ActivityName);
                item.PrevNodeNames = activity.PrevActivityNames;
                item.NextNodeNames = activity.NextActivityNames;
                if (activity.State == 0 || activity.State == 2)
                    item.BackColorName = "yellow";
                nodes.Add(item);
            }
            return nodes;
        }
        #endregion

        /// <summary>
        /// 获取知道流程变化流程图节点.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static IList<GraphNode> GetCacheGraphNodes(Guid workflowId)
        {
            string cacheKey = workflowId.ToString();
            if (_cache.Contains(cacheKey))
                return _cache.GetData(cacheKey) as List<GraphNode>;
            IList<GraphNode> nodes = GetGraphNodes(workflowId);
            _cache.Add(cacheKey, nodes);
            return nodes;
        }

        /// <summary>
        /// 呈现流程图.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="outputStream"></param>
        /// <param name="thumbWidth"></param>
        public static void RenderWorkflowMap(Guid workflowId, Stream outputStream, int thumbWidth)
        {
            string path = string.Format("{0}{1}.jpg",  WorkflowMapManager.ImageCachePath, workflowId.ToString());
            if (!File.Exists(path)) // 文件不存在,创建图片文件.
            {
                using (MemoryStream tempStream = new MemoryStream())
                {
                    if (!WorkflowMapManager.FillRectangle(workflowId, null, tempStream))
                    {
                        IList<GraphNode> nodes = GetCacheGraphNodes(workflowId);
                        WebPreview _preview = new WebPreview(nodes);
                        _preview.Render(tempStream, 0);
                    }

                    if (tempStream != null)
                    {
                        using (Image img = Image.FromStream(tempStream))
                        {
                            if (img != null)
                            {
                                img.Save(path, ImageFormat.Jpeg);
                                img.Dispose();
                            }
                        }
                        tempStream.Close();
                    }
                }
            }
            if (File.Exists(path))
            {
                // 流程图片已经存在时.
                using (Image img = Bitmap.FromFile(path))
                {
                    int width = img.Width;
                    if (thumbWidth == 0 || width <= thumbWidth)
                    {
                        img.Save(outputStream, ImageFormat.Jpeg);
                    }
                    else
                    {
                        // 缩小图片为指定 Width 后，再输出.
                        int thumbHeigth = img.Height * thumbWidth / width;
                        using (Image outputImage = img.GetThumbnailImage(thumbWidth, thumbHeigth, null, IntPtr.Zero))
                        {
                            outputImage.Save(outputStream, ImageFormat.Jpeg);
                        }
                    }
                }
            }
        }
    }
}
