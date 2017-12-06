using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Xml;
using System.Web;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Extension.Configuration;
using Botwave.Workflow.Extension.Util;

namespace Botwave.Workflow.Extension.WorkflowMap
{
    /// <summary>
    /// 流程图管理类.
    /// </summary>
    public static class WorkflowMapManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowMapManager));

        #region Fields

        private static HttpContext context = HttpContext.Current;
        private static readonly Color SelectedColor;

        /// <summary>
        /// 输出图片格式.
        /// </summary>
        private static ImageFormat OutputImageFormat = ImageFormat.Jpeg;

        /// <summary>
        /// 颜色名称.
        /// </summary>
        public static string ColorName = "yellow";

        /// <summary>
        /// 颜色透明度.
        /// </summary>
        public static int ColorAlpha = 30;

        /// <summary>
        /// 图片保存(缓存)路径.
        /// </summary>
        public static string ImageCachePath = string.Empty;
        /// <summary>
        /// 流程图字典(Key 忽略大小写).
        /// </summary>
        public static IDictionary<string, WorkflowItem> Maps = new Dictionary<string, WorkflowItem>(StringComparer.CurrentCultureIgnoreCase);

        #endregion

        /// <summary>
        /// 静态构造方法.
        /// </summary>
        static WorkflowMapManager()
        {
            WorkflowMapSectionHandler.Initialize();
            // 默认路径
            if (string.IsNullOrEmpty(ImageCachePath))
                ImageCachePath = "~/App_Data/Cache/";
            if (!string.IsNullOrEmpty(ImageCachePath))
            {
                string path = HttpContext.Current.Server.MapPath(ImageCachePath);
                if (!path.EndsWith("\\"))
                    path = path + "\\";
                ImageCachePath = path;
                // 目录不存在则创建.
                if (!Directory.Exists(ImageCachePath))
                    Directory.CreateDirectory(ImageCachePath);
            }

            SelectedColor = Color.FromArgb(ColorAlpha, Color.FromName(ColorName));
        }

        #region 画图

        /// <summary>
        /// 查询是否存在知道流程图.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static bool ExistsMap(string workflowName)
        {
            return Maps.ContainsKey(workflowName);
        }

        /// <summary>
        /// 填充长方形区域.
        /// </summary>
        /// <param name="workflowId">流程编号.</param>
        /// <param name="activityName">流程步骤名称.</param>
        /// <param name="outputStream">输出流.</param>
        /// <returns></returns>
        public static bool FillRectangle(Guid workflowId, string activityName, Stream outputStream)
        {
            return FillRectangle(workflowId, activityName, outputStream, 0);
        }

        /// <summary>
        /// 填充长方形区域.
        /// </summary>
        /// <param name="workflowId">流程编号.</param>
        /// <param name="activityName">流程步骤名称.</param>
        /// <param name="outputStream">输出流.</param>
        /// <param name="thumbWidth">缩小图的宽度.</param>
        /// <returns></returns>
        public static bool FillRectangle(Guid workflowId, string activityName, Stream outputStream, int thumbWidth)
        {
            string workflowName = WorkflowUtility.GetWorkflowName(workflowId);
            if (string.IsNullOrEmpty(workflowName))
            {
                return false;
            }
            return FillRectangle(workflowName.Trim(), activityName, outputStream, thumbWidth);
        }

        /// <summary>
        /// 填充长方形区域.
        /// </summary>
        /// <param name="workflowName">流程名称.</param>
        /// <param name="activityName">流程步骤名称.</param>
        /// <param name="outputStream">输出流.</param>
        /// <returns></returns>
        public static bool FillRectangle(string workflowName, string activityName, Stream outputStream)
        {
            return FillRectangle(workflowName, activityName, outputStream, 0);
        }

        /// <summary>
        /// 填充长方形区域.
        /// </summary>
        /// <param name="workflowName">流程名称.</param>
        /// <param name="activityName">流程步骤名称.</param>
        /// <param name="outputStream">输出流.</param>
        /// <param name="thumbWidth">缩小图的宽度.</param>
        /// <returns></returns>
        public static bool FillRectangle(string workflowName, string activityName, Stream outputStream, int thumbWidth)
        {
            if (string.IsNullOrEmpty(workflowName))
                throw new Exception("argument [workflowName] is null or empty.");

            workflowName = workflowName.ToLower();
            if (!Maps.ContainsKey(workflowName))
            {
                log.DebugFormat("流程图缓存数：{0}.未找到流程图：{1}", Maps.Count,  workflowName);
                return false;
            }

            WorkflowItem item = Maps[workflowName];
            ActivityItem activity = null;

            if (!string.IsNullOrEmpty(activityName))
            {
                activityName = activityName.ToLower();
                if (item.Activities.ContainsKey(activityName))
                    activity = item.Activities[activityName];
            }

            return FillRectangle(item.Path, activity, outputStream, thumbWidth);
        }

        /// <summary>
        /// 填充长方形区域.
        /// </summary>
        /// <param name="path">流程图片路径.</param>
        /// <param name="activity"></param>
        /// <param name="outputStream"></param>
        /// <param name="thumbWidth">缩小图的宽度.</param>
        /// <returns></returns>
        private static bool FillRectangle(string path, ActivityItem activity, Stream outputStream, int thumbWidth)
        {
            if (!File.Exists(path)) // 文件不存在.
                return false;

            using (Image img = Image.FromFile(path))
            {
                if (img == null)
                    return false;

                if (activity != null)
                {
                    FillRectangle(img, activity.X, activity.Y, activity.Width, activity.Height);
                }

                int imgWidth = img.Width;
                if (thumbWidth <= 0 || imgWidth <= thumbWidth)
                {
                    img.Save(outputStream, OutputImageFormat);
                }
                else
                {
                    int thumbHeigth = img.Height * thumbWidth / imgWidth;
                    using (MemoryStream tempStream = new MemoryStream())
                    {
                        img.Save(tempStream, ImageFormat.Jpeg);
                        using (Image streamImage = Image.FromStream(tempStream))
                        {
                            Image outputImage = streamImage.GetThumbnailImage(thumbWidth, thumbHeigth, null, IntPtr.Zero);
                            outputImage.Save(outputStream, OutputImageFormat);
                            outputImage.Dispose();
                        }
                        tempStream.Close();
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 填充长方形区域.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void FillRectangle(Image source, int x, int y, int width, int height)
        {
            using (Graphics g = Graphics.FromImage(source))
            {
                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.HighQuality;
                using (Brush b = new SolidBrush(SelectedColor))
                {
                    g.FillRectangle(b, x, y, width, height);
                }
            }
        }

        #endregion   

        #region Workflow

        /// <summary>
        /// 流程.
        /// </summary>
        public class WorkflowItem
        {
            /// <summary>
            /// 流程名称.
            /// </summary>
            public string Name;

            /// <summary>
            /// 流程图片文件路径.
            /// </summary>
            public string Path;

            /// <summary>
            /// 流程步骤列表.
            /// </summary>
            public IDictionary<string, ActivityItem> Activities;

            /// <summary>
            /// 构造方法.
            /// </summary>
            public WorkflowItem()
            {
                this.Activities = new Dictionary<string, ActivityItem>();
            }
        }
        #endregion

        #region Activity

        /// <summary>
        /// 流程步骤.
        /// </summary>
        public class ActivityItem
        {
            /// <summary>
            /// 索引.
            /// </summary>
            public int Index;

            /// <summary>
            /// 流程步骤名称.
            /// </summary>
            public string Name;

            /// <summary>
            /// 起始位置 X 坐标.
            /// </summary>
            public int X;

            /// <summary>
            /// 起始位置 Y 坐标.
            /// </summary>
            public int Y;

            /// <summary>
            /// 流程步骤边框宽度.
            /// </summary>
            public int Width;

            /// <summary>
            /// 流程步骤边框高度.
            /// </summary>
            public int Height;
        }
        
        #endregion
    }
}
