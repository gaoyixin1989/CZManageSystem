Public Class LoggerFactory

    Private Shared m_Logger As ILogger

    Shared Sub New()
        m_Logger = New SQLLogger(ConfigInfo.ConnectionString)
    End Sub

    Public Shared Function GetLogger() As ILogger
        Return m_Logger
    End Function
End Class
