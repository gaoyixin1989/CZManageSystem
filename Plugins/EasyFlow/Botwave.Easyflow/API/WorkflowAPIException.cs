namespace Botwave.Easyflow.API
{
    using System;
    using System.Collections.Generic;

    public class WorkflowAPIException : ApplicationException
    {
        private string _msg;
        private Dictionary<int, string> dict;
        private int index;

        public WorkflowAPIException() : this(0)
        {
        }

        public WorkflowAPIException(int mIndex)
        {
            this.dict = new Dictionary<int, string>();
            this._msg = string.Empty;
            this.index = 0;
            this.dict.Add(1, "portal 登录帐号不能为空");
            this.dict.Add(2, "唯一key不能为空");
            this.dict.Add(3, "输入XML格式错误");
            this.dict.Add(4, "命令错误");
            this.dict.Add(5, "构建输出参数内容出错");
            this.dict.Add(6, "方法处理中报错");
            this.dict.Add(7, "参数 {0} 有误");
            this.dict.Add(8, "需求单工单处理属性 数据有误");
            this.dict.Add(9, "流程名称不存在");
            this.dict.Add(10, "您没有该流程的发单权限");
            this.dict.Add(11, "对不起，您没有权限处理该步骤");
            this.dict.Add(12, "转交工单失败，被转交人已经存在于待办列表中");
            this.dict.Add(13, "参数 {0} 为空或数据格式有误");
            this.dict.Add(14, "参数 {0} 必须填写一个");
            this.dict.Add(15, "参数command只支持一下四种命令(approve：通过审核。reject：退回工单。cancel：取消工单。save：保存工单)");
            this.dict.Add(0x10, "转交工单失败，不存在被转交人portal 帐号");
            this.dict.Add(0x11, "不存在该portal 登录帐号");
            this.dict.Add(0x12, "该用户唯一key为空");
            this.dict.Add(0x13, "该用户唯一key错误");
            this.index = mIndex;
        }

        public WorkflowAPIException(int mIndex, string msg)
        {
            this.dict = new Dictionary<int, string>();
            this._msg = string.Empty;
            this.index = 0;
            this.dict.Add(1, "portal 登录帐号不能为空");
            this.dict.Add(2, "唯一key不能为空");
            this.dict.Add(3, "输入XML格式错误");
            this.dict.Add(4, "命令错误");
            this.dict.Add(5, "构建输出参数内容出错");
            this.dict.Add(6, "方法处理中报错");
            this.dict.Add(7, "参数 {0} 有误");
            this.dict.Add(8, "需求单工单处理属性 数据有误");
            this.dict.Add(10, "您没有该流程的发单权限");
            this.dict.Add(11, "对不起，您没有权限处理该步骤");
            this.dict.Add(12, "转交工单失败，被转交人已经存在于待办列表中");
            this.dict.Add(13, "参数 {0} 为空或数据格式有误");
            this.dict.Add(14, "参数 {0} 必须填写一个");
            this.dict.Add(15, "参数command只支持一下四种命令(approve：通过审核。reject：退回工单。cancel：取消工单。save：保存工单)");
            this.dict.Add(0x10, "转交工单失败，不存在被转交人portal 帐号");
            this.dict.Add(0x11, "不存在该portal 登录帐号");
            this.dict.Add(0x12, "该用户唯一key为空");
            this.dict.Add(0x13, "该用户唯一key错误");
            this.index = mIndex;
            this._msg = msg;
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        public override string Message
        {
            get
            {
                if (this.index == 0)
                {
                    return base.Message;
                }
                if (!this.dict.ContainsKey(this.index))
                {
                    return string.Empty;
                }
                return string.Format(this.dict[this.index], this._msg);
            }
        }

        public override string StackTrace
        {
            get
            {
                return ("-" + this.index.ToString());
            }
        }
    }
}

