using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Botwave.Web.HttpHandler
{
    /// <summary>
    /// 验证码处理类.
    /// </summary>
    public class CheckCodeHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 验证码存储 Key.
        /// </summary>
        public static string CheckCode_Key = "Botwave_CheckCode_Handler";
        /// <summary>
        /// 验证码长度.
        /// </summary>
        public static int CheckCode_Length = 5;

        #region IHttpHandler 成员

        /// <summary>
        /// 获取一个值，该值指示其他请求是否可以使用 System.Web.IHttpHandler 实例.
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 处理请求.
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            // 设置不缓存.
            context.Response.Cache.SetNoStore();

            // 生成验证码图片.
            string checkCode = this.GenerateCheckCode(CheckCode_Length);
            context.Session[CheckCode_Key] = checkCode;
            context.Response.ClearContent();
            context.Response.ContentType = "image/gif";
            this.DrawImage(checkCode, context.Response);
        }

        #endregion

        #region methods

        /// <summary>
        /// 生成验证码.
        /// </summary>
        /// <param name="length">验证码的长度.</param>
        /// <returns></returns>
        private string GenerateCheckCode(int length)
        {
            string checkCode = string.Empty;

           Random random = new Random();
           for (int i = 0; i < length; i++)
            {
                int number = random.Next();
                char code;
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// 绘出验证码.
        /// </summary>
        /// <param name="checkCode">验证码.</param>
        /// <param name="response">响应输出流.</param>
        private void DrawImage(string checkCode, HttpResponse response)
        {
            if (string.IsNullOrEmpty(checkCode))
                return;

            int length = checkCode.Length;
            using (Bitmap image = new Bitmap((int)Math.Ceiling((length * 12.5)), 22))
            {
                Graphics g = Graphics.FromImage(image);
                try
                {
                    //生成随机生成器
                    Random random = new Random();
                    //清空图片背景色
                    g.Clear(Color.White);
                    //画图片的背景噪音线
                    for (int i = 0; i < 25; i++)
                    {
                        int x1 = random.Next(image.Width);
                        int x2 = random.Next(image.Width);
                        int y1 = random.Next(image.Height);
                        int y2 = random.Next(image.Height);

                        g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                    }

                    Font font = new Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                    LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                    g.DrawString(checkCode, font, brush, 2, 2);

                    //画图片的前景噪音点
                    for (int i = 0; i < 100; i++)
                    {
                        int x = random.Next(image.Width);
                        int y = random.Next(image.Height);
                        image.SetPixel(x, y, Color.FromArgb(random.Next()));
                    }
                    //画图片的边框线
                    g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                    image.Save(response.OutputStream, ImageFormat.Gif);
                }
                finally
                {
                    g.Dispose();
                    image.Dispose();
                }
            }
        }

        #endregion
    }
}
