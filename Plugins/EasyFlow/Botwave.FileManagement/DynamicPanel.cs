using System;
using System.Collections;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Botwave.FileManagement
{
    /// <summary>
    /// 动态 Panel 控件。可动态保存内部添加的子控件（子控件必须有 ID 标识）。

    /// </summary>
    [ToolboxData("<{0}:DynamicPanel runat=server></{0}:DynamicPanel>")]
    public class DynamicPanel : Anthem.Panel
    {
        public DynamicPanel()
        {

        }

        /// <summary>
        /// 控件执行方式。

        /// </summary>
        [DefaultValue(DynamicPanel.HandleDynamicControls.DontPersist)]
        public HandleDynamicControls ControlsWithoutIDs
        {
            get
            {
                if (ViewState["ControlsWithoutIDs"] == null)
                    return HandleDynamicControls.DontPersist;
                else
                    return (HandleDynamicControls)ViewState["ControlsWithoutIDs"];
            }
            set { ViewState["ControlsWithoutIDs"] = value; }
        }

        #region 视图状态管理


        protected override void LoadViewState(object savedState)
        {
            object[] viewState = (object[])savedState;

            Pair persistInfo = (Pair)viewState[0];
            foreach (Pair pair in (ArrayList)persistInfo.Second)
            {
                RestoreChildStructure(pair, this);
            }

            base.LoadViewState(viewState[1]);
        }

        protected override object SaveViewState()
        {

            if (HttpContext.Current == null)
                return null;

            object[] viewState = new object[2];
            viewState[0] = PersistChildStructure(this, "C");
            viewState[1] = base.SaveViewState();
            return viewState;
        }

        private void RestoreChildStructure(Pair persistInfo, Control parent)
        {
            Control control;

            string[] persistedString = persistInfo.First.ToString().Split(';');

            string[] typeName = persistedString[1].Split(':');
            switch (typeName[0])
            {

                case "C":
                    Type type = Type.GetType(typeName[1], true, true);
                    try
                    {
                        control = (Control)Activator.CreateInstance(type);
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException(String.Format("类型 '{0}' 不能恢复状态", type.

                  ToString()), e);
                    }
                    break;
                default:
                    throw new ArgumentException("无法识别的类型.不能恢复状态.");
            }

            control.ID = persistedString[2];

            switch (persistedString[0])
            {
                case "C":
                    parent.Controls.Add(control);
                    break;
            }

            foreach (Pair pair in (ArrayList)persistInfo.Second)
            {
                RestoreChildStructure(pair, control);
            }
        }

        private Pair PersistChildStructure(Control control, string controlCollectionName)
        {
            string typeName;
            ArrayList childPersistInfo = new ArrayList();

            if (control.ID == null)
            {
                if (ControlsWithoutIDs == HandleDynamicControls.ThrowException)
                    throw new NotSupportedException("你必须设置你的ID");
                else if (ControlsWithoutIDs == HandleDynamicControls.DontPersist)
                    return null;
            }

            typeName = "C:" + control.GetType().AssemblyQualifiedName;

            string persistedString = controlCollectionName + ";" + typeName + ";" + control.ID;

            if (!(control is UserControl) && !(control is CheckBoxList))
            {
                for (int counter = 0; counter < control.Controls.Count; counter++)
                {
                    Control child = control.Controls[counter];
                    Pair pair = PersistChildStructure(child, "C");
                    if (pair != null)
                        childPersistInfo.Add(pair);
                }
            }

            return new Pair(persistedString, childPersistInfo);
        }

        #endregion 视图状态管理


        public enum HandleDynamicControls
        {
            DontPersist,
            Persist,
            ThrowException
        }
    }
}
