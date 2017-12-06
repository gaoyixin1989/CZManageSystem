'用StringDictionary存储TB_Log_OperationCatalog中的operationID与operationName，operationName为key
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Specialized

Imports Botwave.Commons

Public Class OperationDictionary

    Private Shared m_OpDic As StringDictionary

    Shared Sub New()
        AddOperation2Dictionary()
    End Sub

    Private Shared Sub AddOperation2Dictionary()
        Dim sql As String = "select operationID, operationName from TB_Log_OperationCatalog"
        Dim reader As SqlDataReader = Nothing

        m_OpDic = New StringDictionary

        Try
            reader = SqlHelper.ExecuteReader(ConfigInfo.ConnectionString, CommandType.Text, sql)

            While reader.Read()
                m_OpDic.Add(reader.GetString(1), reader.GetString(0))
            End While
        Catch ex As Exception
            '
        Finally
            If Not (reader Is Nothing) Then
                reader.Close()
            End If
        End Try
    End Sub

    Public Shared Function GetOperationID(ByVal opName As String) As String
        If m_OpDic.ContainsKey(opName) Then
            Return m_OpDic.Item(opName)
        Else
            Return "1"  'Root
        End If
    End Function

End Class
