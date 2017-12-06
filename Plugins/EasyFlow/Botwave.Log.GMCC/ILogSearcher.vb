Imports System.Collections
Imports System.Data

Public Interface ILogSearcher

    'pageCurrent：当前页
    'pageSize：页尺寸
    'recordCount：总记录数
    'where：查询条件
    Function GetLogListByPage(ByVal pageCurrent As Integer, ByVal pageSize As Integer, ByRef recordCount As Integer, ByVal where As String) As DataTable

    Function GetExceptionList() As DataTable

    Function GetOperationList() As DataTable

End Interface
