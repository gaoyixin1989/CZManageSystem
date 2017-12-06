'用StringDictionary存储TB_Log_ExceptionCatalog中的exceptionID与exceptionName，exceptionName为key
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Specialized

Imports Botwave.Commons

Public Class ExceptionDictionary

    Private Shared m_ExceptionDic As StringDictionary

    Shared Sub New()
        AddException2Dictionary()
    End Sub

    Private Shared Sub AddException2Dictionary()
        Dim sql As String = "select exceptionID, exceptionName from TB_Log_ExceptionCatalog"
        Dim reader As SqlDataReader = Nothing

        m_ExceptionDic = New StringDictionary

        Try
            reader = SqlHelper.ExecuteReader(ConfigInfo.ConnectionString, CommandType.Text, sql)

            While reader.Read()
                m_ExceptionDic.Add(reader.GetString(1), reader.GetString(0))
            End While
        Catch ex As Exception
            '
        Finally
            If Not (reader Is Nothing) Then
                reader.Close()
            End If
        End Try
    End Sub

    Public Shared Function GetExceptionID(ByVal exName As String) As String
        If m_ExceptionDic.ContainsKey(exName) Then
            Return m_ExceptionDic.Item(exName)
        Else
            Return "0"  '正常
        End If
    End Function

End Class
