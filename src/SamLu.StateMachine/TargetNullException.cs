using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示实例对象未初始化时产生的错误。
    /// </summary>
    public class TargetNullException : Exception
    {
        /// <summary>
        /// 初始化实例。
        /// </summary>
        public TargetNullException() : this("转换指向的状态为空。") { }

        /// <summary>
        /// 使用指定错误信息初始化实例。
        /// </summary>
        public TargetNullException(string? message) : base(message) { }

        /// <summary>
        /// 使用指定错误信息和内部错误对象初始化实例。
        /// </summary>
        public TargetNullException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
