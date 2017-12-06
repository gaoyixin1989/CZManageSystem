Public Class LoggerHelper

    Private Shared logger As Botwave.Log.GMCC.ILogger = Botwave.Log.GMCC.LoggerFactory.GetLogger()

    Public Shared Function WriteNomalLog(ByVal portalID As String, ByVal operation As String, ByVal description As String) As Integer
        Dim log As LogEvent = New LogEvent(Now(), portalID, Now(), OperationDictionary.GetOperationID(operation), "0", description)
        logger.Info(log)
    End Function

    Public Shared Function WriteNomalLog(ByVal startDT As DateTime, ByVal portalID As String, ByVal operation As String, ByVal description As String) As Integer
        Dim log As LogEvent = New LogEvent(startDT, portalID, Now(), OperationDictionary.GetOperationID(operation), "0", description)
        logger.Info(log)
    End Function

    Public Shared Function WriteExLog(ByVal portalID As String, ByVal operation As String, ByVal exception As String, ByVal description As String) As Integer
        Dim log As LogEvent = New LogEvent(Now(), portalID, Now(), OperationDictionary.GetOperationID(operation), ExceptionDictionary.GetExceptionID(exception), description)
        logger.Info(log)
    End Function

    Public Shared Function WriteExLog(ByVal startDT As DateTime, ByVal portalID As String, ByVal operation As String, ByVal exception As String, ByVal description As String) As Integer
        Dim log As LogEvent = New LogEvent(startDT, portalID, Now(), OperationDictionary.GetOperationID(operation), ExceptionDictionary.GetExceptionID(exception), description)
        logger.Info(log)
    End Function

End Class
