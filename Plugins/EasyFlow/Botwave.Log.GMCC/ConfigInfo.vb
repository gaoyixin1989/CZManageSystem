'������config�ļ���AppSettings��д��һ����ΪLog.ConnectionString�ļ���ֵΪ��־���������ݿ�������ַ���
'���ڳ����ʼ��ʱ����ConnectionString
Imports System.Configuration

Public Class ConfigInfo

    Private Const DefaultCacheSize As Integer = 35

    Private Shared m_ConnectionString As String

    Private Shared m_MaxCacheSize As Integer

    Private Shared m_ShouldThrowException As Boolean = False

    Shared Sub New()
        m_ConnectionString = ConfigurationManager.AppSettings("Log.ConnectionString")
        '�� Log.ConnectionString ���ýڵ����Ҳ���ʱ���Ŵ� ConnectionString �������ҵ����ݿ�����.
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

    '�Ƿ��׳��쳣
    '�ڿ����׶ο���ΪTrue���Ա���٣�������������Ӧ��ΪFalse
    Public Shared Property ShouldThrowException() As Boolean
        Get
            Return m_ShouldThrowException
        End Get
        Set(ByVal Value As Boolean)
            m_ShouldThrowException = Value
        End Set
    End Property
End Class
