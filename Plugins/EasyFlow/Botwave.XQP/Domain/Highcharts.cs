using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.XQP.Domain
{
    [Serializable]
    public class Highcharts
    {
        public Highcharts() { this.labels = new labels(); this.style = new style(); this.chart = new chart(); this.xAxis = new xAxis(); this.series = new List<series>(); yAxis = new List<yAxis>(); colors = new List<string>() {   "#058DC7",
                 "#50B432",
                 "#ED561B",
                  "#DDDF00",
                 "#24CBE5", 
                 "#64E572",
                 "#FF9655",
                 "#FFF263",
                 "#6AF9C4","#6BBC79","#FE8475","#FBF1A5"}; plotOptions = new plotOptions(); this.title = new title();
        }


        public title title { set; get; }
       public style style { set; get; }
       public chart chart { set; get; }
       public xAxis xAxis { set; get; }
       public labels labels { set; get; }
       public List<string> colors { set; get; }
       public List<series> series { set; get; }
       public plotOptions plotOptions { set; get; }
       public List<yAxis> yAxis { set; get; }
    }
    [Serializable]
    public class pie
    {
        public bool allowPointSelect { set; get; }
        public string cursor { set; get; }
          
    }
    [Serializable]
    public class chart
    {
        public string renderTo { set; get; }
    }
    [Serializable]
    public class style
    {
        public string color { set; get; }
        
    }
    [Serializable]
    public class plotOptions
    {
        public plotOptions() {
            pie = new pie() { allowPointSelect = true, cursor = "pointer" };
        }
        public pie pie { set; get; }
    }
    [Serializable]
    public class xAxis
    {
        public List<string> categories { set; get; }
    }
    [Serializable]
    public class yAxis
    {
        public string title { set; get; }
 
        public bool opposite { set; get; }
    }
    [Serializable]
    public class title
    {
        public title() { this.style = new style(); }
        public string text { set; get; }
        public style style { set; get; }
    }
    [Serializable]
    public class labels
    {
        public int x { set; get; }
        public int y { set; get; }
        public List<items> items { set; get; }
    }
    [Serializable]
    public class items
    {
        public items() { this.style = new style(); }
        public string html { set; get; }
        public style style { set; get; }
    }
    [Serializable]
    public class series
    {
        public series() { this.data = new List<data>(); }
        public string type { set; get; }
        public string name { set; get; }
        public List<data> data { set; get; }
     
        public int yAxis { set; get; }
       // public marker marker { set; get; }
    }
    [Serializable]
    public class data {
        public string color { set; get; }
        public decimal y { set; get; }
        public string name { set; get; }
    }
    [Serializable]
    public class marker
    {
        public int lineWidth { set; get; }
 
    }
}
