Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient

Imports Botwave.Commons

Public Class SQLLogSearcher
    Implements ILogSearcher

    Private m_ConnectionString As String
    Private Const SQL_GetExceptionList As String = "select exceptionID, parentID, exceptionName, [description] from TB_Log_ExceptionCatalog"
    Private Const SQL_GetOperationList As String = "select operationID, parentID, operationName, [description] from TB_Log_OperationCatalog where available = 1"

    Public Sub New(ByVal connStr As String)
        m_ConnectionString = connStr
        If String.IsNullOrEmpty(m_ConnectionString) Then
            m_ConnectionString = SqlHelper.ConnectionString
        End If
    End Sub

    Public Function GetLogListByPage(ByVal pageCurrent As Integer, ByVal pageSize As Integer, ByRef recordCount As Integer, ByVal where As String) As DataTable Implements ILogSearcher.GetLogListByPage
        Dim params(8) As SqlParameter

        params(0) = New SqlParameter("@TableName", SqlDbType.NVarChar, 255)
        params(1) = New SqlParameter("@FieldKey", SqlDbType.NVarChar, 255)
        params(2) = New SqlParameter("@PageCurrent", SqlDbType.Int, 4)
        params(3) = New SqlParameter("@PageSize", SqlDbType.Int, 4)
        params(4) = New SqlParameter("@FieldShow", SqlDbType.NVarChar, 1000)
        params(5) = New SqlParameter("@FieldOrder", SqlDbType.NVarChar, 1000)
        params(6) = New SqlParameter("@Where", SqlDbType.NVarChar, 1000)
        params(7) = New SqlParameter("@RecordCount", SqlDbType.Int, 4)

        params(7).Direction = ParameterDirection.InputOutput

        params(0).Value = "vw_Log_OperateLog"
        params(1).Value = "uid"
        params(2).Value = pageCurrent
        params(3).Value = pageSize
        params(4).Value = ""
        params(5).Value = "uid desc"
        params(6).Value = where
        params(7).Value = recordCount

        Dim retVal As DataTable = SqlHelper.ExecuteDataset(m_ConnectionString, CommandType.StoredProcedure, "spPageViewByStr", params).Tables(0)

        recordCount = params(7).Value

        Return retVal
    End Function

    Public Function GetExceptionList() As DataTable Implements ILogSearcher.GetExceptionList
        Return SqlHelper.ExecuteDataset(m_ConnectionString, CommandType.Text, SQL_GetExceptionList).Tables(0)
    End Function

    Public Function GetOperationList() As DataTable Implements ILogSearcher.GetOperationList
        Return SqlHelper.ExecuteDataset(m_ConnectionString, CommandType.Text, SQL_GetOperationList).Tables(0)
    End Function
End Class
