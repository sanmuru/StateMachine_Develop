﻿namespace SamLu.StateMachine;

/// <summary>
/// 表示实例对象未初始化时产生的错误。
/// </summary>
public class UninitializedException : Exception
{
    /// <summary>
    /// 初始化实例。
    /// </summary>
    public UninitializedException() : this("实例对象未初始化。") { }

    /// <summary>
    /// 使用指定错误信息初始化实例。
    /// </summary>
    public UninitializedException(string? message) : base(message) { }

    /// <summary>
    /// 使用指定错误信息和内部错误对象初始化实例。
    /// </summary>
    public UninitializedException(string? message, Exception? innerException) : base(message, innerException) { }
}
