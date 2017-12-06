using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    [Serializable]
    public class CZWorkflowInterface : Botwave.Entities.TrackedEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public int SortOrder { get; set; }

        public int Status { get; set; }

        public CZWorkflowInterface()
        { }

        public CZWorkflowInterface(int id, string name, string url, string description, int sortOrder, int status, string creator)
        {
            this.Id = id;
            this.Name = name;
            this.Url = url;
            this.Description = description;
            this.SortOrder = sortOrder;
            this.Status = status;
            this.Creator = this.LastModifier = creator;
            this.CreatedTime = this.LastModTime = DateTime.Now;
        }

        public void Update()
        {
            if (this.Id > 0)
            {
                IBatisMapper.Update("cz_WorkflowInterfaces_Update", this);
            }
            else
            {
                IBatisMapper.Insert("cz_WorkflowInterfaces_Insert", this);
            }
        }

        public static void Delete(int id)
        {
            IBatisMapper.Delete("cz_WorkflowInterfaces_Delete", id);
        }

        public static CZWorkflowInterface Get(int id)
        {
            return IBatisMapper.Load<CZWorkflowInterface>("cz_WorkflowInterfaces_Select", id);
        }

        public static IList<CZWorkflowInterface> GetAll()
        {
            return IBatisMapper.Select<CZWorkflowInterface>("cz_WorkflowInterfaces_Select");
        }

        public static int Count()
        {
            return IBatisMapper.Mapper.QueryForObject<int>("cz_WorkflowInterfaces_Count", null);
        }

        public static string Build(string title, string id)
        {
            IList<CZWorkflowInterface> items = IBatisMapper.Select<CZWorkflowInterface>("cz_WorkflowInterfaces_Display");
            if (items == null || items.Count == 0)
                return null;

            string appPath = Botwave.Web.WebUtils.GetAppPath();
            StringBuilder result = new StringBuilder();
            result.AppendFormat("<h2 onclick=\"changeView(this.id)\" id=\"{0}\"><img src=\"{1}app_themes/gmcc/img/ico_have.gif\" id=\"ico_{0}\" />{2}</h2>", id, appPath, title);
            result.AppendLine(string.Format("<div class=\"navigation\" id=\"div_{0}\" style=\"display:none\">", id));
            result.AppendLine("<div class=\"menuItems\">");

            foreach (CZWorkflowInterface item in items)
            {
                result.AppendFormat("<a href=\"{0}\" target=\"rightFrame\" title=\"{1}\">{1}</a>", item.Url, item.Name);
            }

            result.AppendLine("</div>");
            result.AppendLine("</div>");
            return result.ToString();
        }
    }
}
