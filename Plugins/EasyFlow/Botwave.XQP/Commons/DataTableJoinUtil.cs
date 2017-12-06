using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Botwave.XQP.Commons
{
   public  static class  DataTableJoinUtil
    {

            public enum JoinType
            {
                /// <summary>
                /// Same as regular join. Inner join produces only the set of records that match in both Table A and Table B.
                /// </summary>
                Inner = 0,
                /// <summary>
                /// Same as Left Outer join. Left outer join produces a complete set of records from Table A, with the matching records (where available) in Table B. If there is no match, the right side will contain null.
                /// </summary>
                Left = 1
            }
         

            /// <summary>
            /// 联合两个表结构数据
            /// </summary>
            /// <param name="dtblLeft">左表</param>
            /// <param name="dtblRight">右表</param>
            /// <param name="leftJoinColumn">左表字段</param>
            /// <param name="rightJoinColumn">右表字段</param>
            /// <param name="joinType">内联或左联</param>
            /// <returns></returns>
            public static DataTable JoinTwoDataTables(DataTable dtblLeft, DataTable dtblRight, String leftTableColumn,String rightTableColumn, JoinType joinType=JoinType.Inner)
            {
                
                if(! dtblLeft.Columns.Contains(leftTableColumn) || ! dtblRight.Columns.Contains(rightTableColumn)){

                    throw new ArgumentException("左或右DataTable里面没有找到相应leftTableColumn或rightTableColumn");
                }
                //Change column name to a temp name so the LINQ for getting row data will work properly.
              
                //Get columns from dtblA
                DataTable dtblResult = dtblLeft.Clone();

                //Get columns from dtblB
                var dt2Columns = dtblRight.Columns.OfType<DataColumn>().Select(dc => new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping));

                //Get columns from dtblB that are not in dtblA
                var dt2FinalColumns = from dc in dt2Columns.AsEnumerable()
                                      where !dtblResult.Columns.Contains(dc.ColumnName)
                                      select dc;

                //Add the rest of the columns to dtblResult
                dtblResult.Columns.AddRange(dt2FinalColumns.ToArray());
              

                switch (joinType)
                {

                    default:
                    case JoinType.Inner:
                        #region Inner
                        //get row data
                        //To use the DataTable.AsEnumerable() extension method you need to add a reference to the System.Data.DataSetExtension assembly in your project. 
                        var rowDataLeftInner = from rowLeft in dtblLeft.Rows.Cast<DataRow>()
                                               join rowRight in dtblRight.Rows.Cast<DataRow>() on rowLeft[leftTableColumn] equals rowRight[rightTableColumn]
                                               select rowLeft.ItemArray.Concat(rowRight.ItemArray).ToArray();


                        //Add row data to dtblResult
                        foreach (object[] values in rowDataLeftInner)
                            dtblResult.Rows.Add(values);

                        #endregion
                        break;
                    case JoinType.Left:
                        #region Left
                        var rowDataLeftOuter = from rowLeft in dtblLeft.Rows.Cast<DataRow>()
                                               join rowRight in dtblRight.Rows.Cast<DataRow>() on rowLeft[leftTableColumn].ToString() equals rowRight[rightTableColumn].ToString() into gj
                                               from subRight in gj.DefaultIfEmpty()
                                               //select rowLeft.ItemArray.Concat((gj == null) ? (dtblRight.NewRow().ItemArray) : subRight.ItemArray).ToArray();
                                               select rowLeft.ItemArray.Concat((subRight==null)? new object[]{null} : subRight.ItemArray).ToArray();

                                               //select gj;

                         //foreach(var aa in rowDataLeftOuter.ToArray()){
                         //    Console.WriteLine(aa);
                         //}
                        ////Add row data to dtblResult
                         
                         foreach (Object[] dr in rowDataLeftOuter)
                            dtblResult.Rows.Add(dr);

                        #endregion
                        break;
                }

             

                return dtblResult;
            }
       
          
            
    }
}
