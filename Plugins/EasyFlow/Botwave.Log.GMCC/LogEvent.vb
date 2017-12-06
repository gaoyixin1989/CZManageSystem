Public Class LogEvent

    Public opStartTime As DateTime

    Public portalID As String

    Public clientIP As String

    Public clientComputerName As String

    Public serverIP As String

    Public opEndTime As DateTime

    Public operationID As String

    Public exceptionID As String

    Public description As String

    Public Sub New()
        'InitializeClientAndServer()
    End Sub

    Public Sub New(ByVal opStartTime As DateTime, _
                    ByVal portalID As String, _
                    ByVal opEndTime As DateTime, _
                    ByVal operationID As String, _
                    ByVal exceptionID As String, _
                    ByVal description As String)
        InitializeClientAndServer()
        Me.opStartTime = opStartTime
        Me.portalID = portalID
        Me.opEndTime = opEndTime
        Me.operationID = operationID
        Me.exceptionID = exceptionID
        Me.description = description
    End Sub

    Private Sub InitializeClientAndServer()
        clientIP = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        clientComputerName = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_HOST")
        serverIP = System.Web.HttpContext.Current.Request.ServerVariables("LOCAL_ADDR")
    End Sub
End Class
