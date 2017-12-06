using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Botwave.Commons
{
    /// <summary>
    /// δ�ҵ��û����쳣��.
    /// </summary>
    [Serializable]
    public class UserNotFoundException : ApplicationException
    {
        /// <summary>
        /// ���췽��.
        /// </summary>
        public UserNotFoundException() : base() { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public UserNotFoundException(string message) : base(message) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public UserNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// δ�ҵ���Ӧ��¼���쳣��.
    /// </summary>
    [Serializable]
    public class RecordNotFoundException : ApplicationException
    {
        /// <summary>
        /// ���췽��.
        /// </summary>
        public RecordNotFoundException() : base() { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public RecordNotFoundException(string message) : base(message) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public RecordNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public RecordNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Ϊ�����������쳣��.
    /// </summary>
    [Serializable]
    public class ClassNotFoundException : ApplicationException
    {
        /// <summary>
        /// ���췽��.
        /// </summary>
        public ClassNotFoundException() : base() { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public ClassNotFoundException(string message) : base(message) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public ClassNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public ClassNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Ӧ�ó���Ĵ����쳣��.
    /// </summary>
    [Serializable]
    public class AppException : ApplicationException
    {
        /// <summary>
        /// ���췽��.
        /// </summary>
        public AppException() : base() { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public AppException(string message) : base(message) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public AppException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public AppException(string message, Exception innerException) : base(message, innerException) { }
    }
}
