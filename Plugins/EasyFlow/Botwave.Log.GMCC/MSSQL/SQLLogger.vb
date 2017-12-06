Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient

Imports Botwave.Commons

Public Class SQLLogger
    Implements ILogger

    Private Shared m_arrEvents As New ArrayList

    Private Const m_CmdText As String = "INSERT INTO TB_Log_OperateLog (opStartTime, portalID, clientIP, clientComputerName, serverIP,opEndTime,operationID,exceptionID,[description]) VALUES (@opStartTime, @portalID, @clientIP, @clientComputerName, @serverIP,@opEndTime,@operationID,@exceptionID,@description)"

    Private m_ConnectionString As String

    Public Sub New(ByVal connStr As String)
        m_ConnectionString = connStr
        If String.IsNullOrEmpty(m_ConnectionString) Then
            m_ConnectionString = SqlHelper.ConnectionString
        End If
    End Sub

    '并没有每次都写数据库，而是累积到一定数量时才批量提交
    Public Function Info(ByVal theEvent As LogEvent) As Integer Implements ILogger.Info
        m_arrEvents.Add(theEvent)
        If m_arrEvents.Count >= ConfigInfo.MaxCacheSize Then
            SendBuffer()
        End If
    End Function

    Private Sub SendBuffer()
        Dim conn As New SqlConnection(Me.m_ConnectionString)
        Dim trans As SqlTransaction = Nothing

        Try
            conn.Open()
            trans = conn.BeginTransaction()

            For i As Integer = 0 To m_arrEvents.Count - 1
                Dim log As LogEvent = CType(m_arrEvents.Item(i), LogEvent)

                Dim params As SqlParameter() = Me.GetParams(log)

                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, m_CmdText, params)
            Next

            trans.Commit()
            m_arrEvents.Clear()
        Catch ex As SqlException
            If (Not trans Is Nothing) Then
                trans.Rollback()
            End If

            If (ConfigInfo.ShouldThrowException) Then
                Throw ex
            End If
        Finally
            If ((Not conn Is Nothing) And (conn.State <> ConnectionState.Closed)) Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Function GetParams(ByVal log As LogEvent) As SqlParameter()
        Dim params(9) As SqlParameter

        params(0) = New SqlParameter("@opStartTime", SqlDbType.DateTime, 8)
        params(1) = New SqlParameter("@portalID", SqlDbType.VarChar, 50)
        params(2) = New SqlParameter("@clientIP", SqlDbType.VarChar, 20)
        params(3) = New SqlParameter("@clientComputerName", SqlDbType.VarChar, 50)
        params(4) = New SqlParameter("@serverIP", SqlDbType.VarChar, 20)
        params(5) = New SqlParameter("@opEndTime", SqlDbType.DateTime, 8)
        params(6) = New SqlParameter("@operationID", SqlDbType.Char, 10)
        params(7) = New SqlParameter("@exceptionID", SqlDbType.Char, 10)
        params(8) = New SqlParameter("@description", SqlDbType.VarChar, 500)

        If log.description.Length > 247 Then
            log.description = log.description.Substring(0, 247) & "..."
        End If

        params(0).Value = log.opStartTime
        params(1).Value = log.portalID
        params(2).Value = log.clientIP
        params(3).Value = log.clientComputerName
        params(4).Value = log.serverIP
        params(5).Value = log.opEndTime
        params(6).Value = log.operationID
        params(7).Value = log.exceptionID
        params(8).Value = log.description

        Return params
    End Function
End Class
