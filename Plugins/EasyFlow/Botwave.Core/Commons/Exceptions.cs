using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Botwave.Commons
{
    /// <summary>
    /// 未找到用户的异常类.
    /// </summary>
    [Serializable]
    public class UserNotFoundException : ApplicationException
    {
        /// <summary>
        /// 构造方法.
        /// </summary>
        public UserNotFoundException() : base() { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public UserNotFoundException(string message) : base(message) { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public UserNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// 未找到相应记录的异常类.
    /// </summary>
    [Serializable]
    public class RecordNotFoundException : ApplicationException
    {
        /// <summary>
        /// 构造方法.
        /// </summary>
        public RecordNotFoundException() : base() { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public RecordNotFoundException(string message) : base(message) { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public RecordNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public RecordNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// 为发现类对象的异常类.
    /// </summary>
    [Serializable]
    public class ClassNotFoundException : ApplicationException
    {
        /// <summary>
        /// 构造方法.
        /// </summary>
        public ClassNotFoundException() : base() { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ClassNotFoundException(string message) : base(message) { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ClassNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ClassNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// 应用程序的错误异常类.
    /// </summary>
    [Serializable]
    public class AppException : ApplicationException
    {
        /// <summary>
        /// 构造方法.
        /// </summary>
        public AppException() : base() { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public AppException(string message) : base(message) { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public AppException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public AppException(string message, Exception innerException) : base(message, innerException) { }
    }
}
