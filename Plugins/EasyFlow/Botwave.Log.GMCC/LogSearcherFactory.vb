Public Class LogSearcherFactory
    Private Shared m_LogSearcher As ILogSearcher

    Shared Sub New()
        m_LogSearcher = New SQLLogSearcher(ConfigInfo.ConnectionString)
    End Sub

    Public Shared Function GetLogSearcher() As ILogSearcher
        Return m_LogSearcher
    End Function
End Class
