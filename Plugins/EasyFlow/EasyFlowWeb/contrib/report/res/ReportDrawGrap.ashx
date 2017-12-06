<%@ WebHandler Language="C#" Class="URLReportDrawGrap" %>

using System;
using System.Data;
using System.Web;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;

using Botwave.Report.DrawGrapBase;

/********************************************************************
 * 统计图输出处理程序
 * 用法：
 * <img src='DrawGrap.ashx?data=销售:(1,23)(2,25.3)|收入:(3,23.7)' alt='统计图' />
 * 参数：
 * 1、[type]
 * 描述统计图类型，可选，有line\bar\pie三种，默认是line
 * 2、data
 * 统计数据，line\bar类型图的格式是：
 * 每组数据以“|”号隔开，同一组数据的x,y轴数据用小括号包括起来，如：(1,10)(2,25.3)|(1,23.7)(2,24.6)
 * 3、[title]
 * 统计图标题，可选
 * 4、[xtitle]
 * x轴标题，可选
 * 5、[ytitle]
 * y轴标题，可选
 * 6、[width]
 * 图片宽度，可选
 * 7、[height]
 * 图片高度，可选
 * 8、[color]
 * 各组数据的颜色，可选，数据是ARGB值，颜色之间以“|”隔开
 * 获取颜色ARGB值的方法：
 * System.Drawing.Color.DarkGreen.ToArgb()
 * 9、[name]
 * 各组数据的说明，可选，名称之间以“|”号隔开
 ********************************************************************/
public class URLReportDrawGrap : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        URLDrawGrap drawGraph = new URLDrawGrap();
        try
        {
            HandleQueryString(drawGraph, context.Request.QueryString);
            drawGraph.Draw();
        }
        catch (Exception ex)
        {
            context.Response.Clear();
            ChartHelper.DrawMessage(ex.Message, drawGraph.Width, drawGraph.Height);
        }
    }

    private void HandleQueryString(URLDrawGrap draw, NameValueCollection querystr)
    {

        // 获取图像类型和数据
        if (!string.IsNullOrEmpty(querystr["type"]))
        {
            switch (querystr["type"].ToLower())
            {
                case "line":
                    draw.Type = ChartType.Line;
                    draw.SetBarLineData(draw, querystr["data"]);
                    break;
                case "bar":
                    draw.Type = ChartType.Bar;
                    draw.SetBarLineData(draw, querystr["data"]);
                    break;
                case "pie":
                    draw.Type = ChartType.Pie;
                    draw.SetPieData(draw, querystr["data"]);
                    break;
            }
        }
        else
        {
            draw.Type = ChartType.Bar;
            draw.SetBarLineData(draw, querystr["data"]);
        }

        // 获取标题
        if (!string.IsNullOrEmpty(querystr["title"]))
            draw.Title = querystr["title"];

        // 获取x轴说明
        if (!string.IsNullOrEmpty(querystr["xtitle"]))
            draw.XAxisTitle = querystr["xtitle"];

        // 获取y轴说明
        if (!string.IsNullOrEmpty(querystr["ytitle"]))
            draw.YAxisTitle = querystr["ytitle"];

        // 获取图像宽度
        if (!string.IsNullOrEmpty(querystr["width"]))
            draw.Width = int.Parse(querystr["width"]);

        // 获取图像高度
        if (!string.IsNullOrEmpty(querystr["height"]))
            draw.Height = int.Parse(querystr["height"]);

        // 获取颜色
        // if (!string.IsNullOrEmpty(querystr["color"]))
        //    GetColor(draw, querystr["color"]);      
    }

    #region IHttpHandler Members

    public bool IsReusable
    {
        get { return true; }
    }

    #endregion
}
