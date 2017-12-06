'可以在config文件的AppSettings中写入一个名为Log.ConnectionString的键，值为日志表所在数据库的连接字符串
'或在程序初始化时设置ConnectionString
Imports System.Configuration

Public Class ConfigInfo

    Private Const DefaultCacheSize As Integer = 35

    Private Shared m_ConnectionString As String

    Private Shared m_MaxCacheSize As Integer

    Private Shared m_ShouldThrowException As Boolean = False

    Shared Sub New()
        m_ConnectionString = ConfigurationManager.AppSettings("Log.ConnectionString")
        '从 Log.ConnectionString 配置节点中找不到时，才从 ConnectionString 配置中找到数据库链接.
        If String.IsNullOrEmpty(m_ConnectionString) Then
            m_ConnectionString = ConfigurationManager.AppSettings("ConnectionString")
        End If

        Dim sCacheSize As String = ConfigurationManager.AppSettings("Log.MaxCacheSize")
        If String.IsNullOrEmpty(sCacheSize) = False Then
            Try
                m_MaxCacheSize = Convert.ToInt32(sCacheSize)
                If m_MaxCacheSize <= 0 Then
                    m_MaxCacheSize = DefaultCacheSize
                End If
            Catch
                m_MaxCacheSize = DefaultCacheSize
            End Try

        Else
            m_MaxCacheSize = 25
        End If

        Dim sShouldThrowException As String = ConfigurationManager.AppSettings("Log.ShouldThrowException")
        If String.IsNullOrEmpty(sShouldThrowException) Then
            Try
                m_ShouldThrowException = CType(sShouldThrowException, Boolean)
            Catch
                '
            End Try
        End If
    End Sub

    Public Shared Property ConnectionString() As String
        Get
            Return m_ConnectionString
        End Get
        Set(ByVal Value As String)
            m_ConnectionString = Value
        End Set
    End Property

    Public Shared Property MaxCacheSize() As Integer
        Get
            Return m_MaxCacheSize
        End Get
        Set(ByVal Value As Integer)
            m_MaxCacheSize = Value
        End Set
    End Property

    '是否抛出异常
    '在开发阶段可设为True，以便跟踪，在生产环境下应设为False
    Public Shared Property ShouldThrowException() As Boolean
        Get
            Return m_ShouldThrowException
        End Get
        Set(ByVal Value As Boolean)
            m_ShouldThrowException = Value
        End Set
    End Property
End Class
