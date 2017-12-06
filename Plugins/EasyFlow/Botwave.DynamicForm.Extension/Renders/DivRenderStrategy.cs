using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.DynamicForm.Domain;
using System.Web.UI.HtmlControls;
using Botwave.DynamicForm.Renders;

namespace Botwave.DynamicForm.Extension.Renders
{
    /// <summary>
    /// 表单表格呈现策略类.
    /// </summary>
    public class DivRenderStrategy
    {
        #region 字段

        /// <summary>
        /// 最大行数.
        /// </summary>
        private const int MAX_ROWS = 500;	//最大行数
        /// <summary>
        /// 指定列数.
        /// </summary>
        private const int COLS_NUM = 3;		//指定列数

        /// <summary>
        /// textarea上否应当占据一行.
        /// </summary>
        private const bool SHOULD_TEXTAREA_HOLD_ONEROW = true;

        /// <summary>
        /// 存储单元格位置的队列.
        /// </summary>
        private static Queue m_CellsQueue = Queue.Synchronized(new Queue());

        /// <summary>
        /// 存储未被占用单元格的哈希表.
        /// </summary>
        private static Hashtable m_UnPossessiveCells = Hashtable.Synchronized(new Hashtable());

        #endregion

        /// <summary>
        /// 初始化队列.
        /// </summary>
        public static void Init()
        {
            //先清空队列及哈希表中的元素
            m_CellsQueue.Clear();
            m_UnPossessiveCells.Clear();
        }

        /// <summary>
        /// 附加表单项单元格.
        /// </summary>
        /// <param name="item">表单项定义.</param>
        /// <param name="tbForm">表单临时表.</param>
        /// <param name="tr"></param>
        /// <param name="tcItem">表单项单元格.</param>
        public static void AppendItemCell(FormItemDefinition item, HtmlGenericControl tbForm, out HtmlGenericControl tr, out HtmlGenericControl tcItem)
        {
            //hPositio小于等于1时，认为没有指定布局
            //vPosition小于等于0时，也认为没有指定布局
            //hPosition大于6(指定列数)时，同样认为没有指定布局
            //这时，选择未被占用的最靠前的位置填充

            TableCell tcItemName;
            HtmlGenericControl tcCell;
            if (DbUtils.ToInt32(item.Left) <= 1 || DbUtils.ToInt32(item.Top) <= 0 || DbUtils.ToInt32(item.Left) > COLS_NUM)
            {
                if (m_UnPossessiveCells.Count > 0 && m_CellsQueue.Count > 0 && item.ItemType != FormItemDefinition.FormItemType.TextArea && !item.RowExclusive)//如果有未被占用的位置，并且不独占一行，同时输入表单类型不为textarea(为textarea时强制填充在新行)
                {
                    Position pos = m_CellsQueue.Dequeue() as Position;
                    while (!m_UnPossessiveCells.ContainsKey(pos) && m_CellsQueue.Count > 0)//如果队列中的位置已经被占用，则弹出后继续寻找
                    {
                        pos = m_CellsQueue.Dequeue() as Position;
                    }

                    tr = tbForm.Controls[pos.Y - 1] as HtmlGenericControl;
                    tcCell = tr.Controls[pos.X-1] as HtmlGenericControl;
                    tcItemName = ((Table)tcCell.Controls[0]).Rows[0].Cells[0];
                    tcItem = tcCell;

                    tcItemName.Text = (item.Name + ":");

                    //位置已经被占用，从哈希表中移除
                    m_UnPossessiveCells.Remove(pos);
                }
                else	//如果没有，则创建
                {
                    int y = tbForm.Controls.Count + 1;
                    CreateRow(y, ref tbForm);

                    tr = tbForm.Controls[y - 1] as HtmlGenericControl;

                    if (item.ItemType == FormItemDefinition.FormItemType.TextArea || item.RowExclusive)
                    {
                        tcItem = DivControlCreator.CreateTitleCell();
                        tcItemName = ((Table)tcItem.Controls[0]).Rows[0].Cells[0];
                        tcItemName.Text += (item.Name + ":");
                        ((Table)tcItem.Controls[0]).Rows[0].Cells.Add(DivControlCreator.CreateInputCell());

                        MergeCell(tr, 0, 3, tcItem);

                        //位置已经被占用，从哈希表中移除
                        m_UnPossessiveCells.Remove(new Position(1, y));
                        m_UnPossessiveCells.Remove(new Position(2, y));
                        m_UnPossessiveCells.Remove(new Position(3, y));
                    }
                    else
                    {
                        tcItem = tr.Controls[0] as HtmlGenericControl;
                        tcItemName = ((Table)tcItem.Controls[0]).Rows[0].Cells[0];
                        
                        tcItemName.Text = (item.Name + ":");
                        //tcItem = tcCell;
                        //位置已经被占用，从哈希表中移除
                        m_UnPossessiveCells.Remove(new Position(1, y));
                    }
                }
            }
            else	//指定布局时
            {
                //如果所要求的行列(实际上是行，只有6列)在表中已存在
                if (tbForm.Controls.Count > DbUtils.ToInt32(item.Top)
                    && tbForm.Controls[DbUtils.ToInt32(item.Top) - 1].Controls.Count >= DbUtils.ToInt32(item.Top))
                {
                    tr = tbForm.Controls[DbUtils.ToInt32(item.Top) - 1] as HtmlGenericControl;

                    if (item.ItemType == FormItemDefinition.FormItemType.TextArea || item.RowExclusive)
                    {
                        //tcItemName = tr.Cells[0];
                        //tcItem = DivControlCreator.CreateInputCell();

                        //tcItemName.Text += (item.Name + ":");
                        tcItem = DivControlCreator.CreateTitleCell();
                        tcItemName = ((Table)tcItem.Controls[0]).Rows[0].Cells[0];
                        tcItemName.Text += (item.Name + ":");
                        ((Table)tcItem.Controls[0]).Rows[0].Cells.Add(DivControlCreator.CreateInputCell());
                        MergeCell(tr, 0, 3, tcItem);

                        //位置已经被占用，从哈希表中移除
                        m_UnPossessiveCells.Remove(new Position(1, DbUtils.ToInt32(item.Top)));
                        m_UnPossessiveCells.Remove(new Position(2, DbUtils.ToInt32(item.Top)));
                        m_UnPossessiveCells.Remove(new Position(3, DbUtils.ToInt32(item.Top)));
                    }
                    else
                    {
                        
                        tcItem = tr.Controls[DbUtils.ToInt32(item.Left) - 1] as HtmlGenericControl;
                        tcItemName = ((Table)tcItem.Controls[0]).Rows[0].Cells[0];
                        tcItemName.Text += (item.Name + ":");

                        //位置已经被占用，从哈希表中移除
                        m_UnPossessiveCells.Remove(new Position(DbUtils.ToInt32(item.Left), DbUtils.ToInt32(item.Top)));
                    }
                }
                else	//否则，创建行列，直到已生成(不超过最大行数)
                {
                    int i = tbForm.Controls.Count + 1;
                    while (i <= MAX_ROWS && i <= DbUtils.ToInt32(item.Top))
                    {
                        CreateRow(i, ref tbForm);
                        i++;
                    }

                    if (i > MAX_ROWS)
                    {
                        throw new ArgumentOutOfRangeException(String.Format("指定位置{0}已超过最大行数", MAX_ROWS));
                    }
                    else
                    {
                        tr = tbForm.Controls[DbUtils.ToInt32(item.Top) - 1] as HtmlGenericControl;

                        if (item.ItemType == FormItemDefinition.FormItemType.TextArea || item.RowExclusive)
                        {
                            //tcItemName = tr.Cells[0];
                            //tcItem = ControlCreator.CreateInputCell();

                            //tcItemName.Text += (item.Name + ":");
                            tcItem = DivControlCreator.CreateTitleCell();
                            tcItemName = ((Table)tcItem.Controls[0]).Rows[0].Cells[0];
                            tcItemName.Text += (item.Name + ":");
                            ((Table)tcItem.Controls[0]).Rows[0].Cells.Add(DivControlCreator.CreateInputCell());
                            MergeCell(tr, 0, 3, tcItem);

                            //位置已经被占用，从哈希表中移除
                            m_UnPossessiveCells.Remove(new Position(1, DbUtils.ToInt32(item.Top)));
                            m_UnPossessiveCells.Remove(new Position(2, DbUtils.ToInt32(item.Top)));
                            m_UnPossessiveCells.Remove(new Position(3, DbUtils.ToInt32(item.Top)));
                        }
                        else
                        {
                            if (tr.Controls.Count < item.Left)
                                item.Left = tr.Controls.Count;
                            tcItem = tr.Controls[DbUtils.ToInt32(item.Left) - 1] as HtmlGenericControl;
                            tcItemName = ((Table)tcItem.Controls[0]).Rows[0].Cells[0];
                            //tcItemName.Text += (item.Name + ":");

                            //位置已经被占用，从哈希表中移除
                            m_UnPossessiveCells.Remove(new Position(DbUtils.ToInt32(item.Left), DbUtils.ToInt32(item.Top)));

                            tcItemName.Text += (item.Name + ":");
                        }
                    }
                }
            }
        }

        #region RowMethods

        /// <summary>
        /// 创建表格行.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="tbForm"></param>
        private static void CreateRow(int y, ref HtmlGenericControl tbForm)
        {
            HtmlGenericControl tmpRow;
            if (!(y % 2 == 0))
                tmpRow = DivControlCreator.CreateSingleRow();
            else tmpRow = DivControlCreator.CreateDoubleRow();

            //生成六个单元格
            HtmlGenericControl titleCell = DivControlCreator.CreateTitleCell();

            ((Table)titleCell.Controls[0]).Rows[0].Cells.Add(DivControlCreator.CreateInputCell());
            tmpRow.Controls.Add(titleCell);
            Position pos1 = new Position(1, y);
            m_CellsQueue.Enqueue(pos1);
            m_UnPossessiveCells.Add(pos1, pos1);

            titleCell = DivControlCreator.CreateTitleCell();
            ((Table)titleCell.Controls[0]).Rows[0].Cells.Add(DivControlCreator.CreateInputCell());
            tmpRow.Controls.Add(titleCell);
            Position pos2 = new Position(2, y);
            m_CellsQueue.Enqueue(pos2);
            m_UnPossessiveCells.Add(pos2, pos2);

            titleCell = DivControlCreator.CreateTitleCell();
            ((Table)titleCell.Controls[0]).Rows[0].Cells.Add(DivControlCreator.CreateInputCell());
            tmpRow.Controls.Add(titleCell);

            tbForm.Controls.Add(tmpRow);

            Position pos3 = new Position(3, y);
            m_CellsQueue.Enqueue(pos3);
            m_UnPossessiveCells.Add(pos3, pos3);
        }

        /// <summary>
        /// 合并表格单元格.
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="startAt"></param>
        /// <param name="length"></param>
        /// <param name="replacer"></param>
        private static void MergeCell(HtmlGenericControl tr, int startAt, int length, HtmlGenericControl replacer)
        {
            if (null == tr) throw new ArgumentNullException("tr", "tr不能为空");
            if (null == replacer) throw new ArgumentNullException("replacer", "replacer不能为空");
            if (startAt < 0) throw new ArgumentOutOfRangeException("startAt", startAt, "startAt不能小于0");
            if (length < 1) throw new ArgumentOutOfRangeException("length", length, "length不能小于1");

            if (tr.Controls.Count >= (startAt + length))
            {
                for (int i = 1; i <= length; i++)
                {
                    tr.Controls.RemoveAt(startAt);
                }
                //replacer.Width = Unit.Pixel(1000);
                if (length == 3)
                {
                    replacer.Attributes.Remove("class");
                    replacer.Attributes.Add("class", "col");
                }
                tr.Controls.AddAt(startAt, replacer);
            }
        }
        
        #endregion
    }
}
