Imports System.Collections
Imports System.Data

Public Interface ILogSearcher

    'pageCurrent����ǰҳ
    'pageSize��ҳ�ߴ�
    'recordCount���ܼ�¼��
    'where����ѯ����
    Function GetLogListByPage(ByVal pageCurrent As Integer, ByVal pageSize As Integer, ByRef recordCount As Integer, ByVal where As String) As DataTable

    Function GetExceptionList() As DataTable

    Function GetOperationList() As DataTable

End Interface
