﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了有限状态机的转换应遵循的基本约束。
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// 获取转换指向的状态。
        /// </summary>
        IState? Target { get; }

        /// <summary>
        /// 获取一个值，指示该转换是否为 ε 转换。
        /// </summary>
        bool IsEpsilon { get; }

        /// <summary>
        /// 将转换的目标设为指定状态。
        /// </summary>
        /// <param name="state">要设为目标的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        bool SetTarget(IState state);

        /// <summary>
        /// 测试指定输入是否能被接收。
        /// </summary>
        /// <param name="input">指定的输入。</param>
        /// <returns>一个值，指示测试指定输入是否能被接收。</returns>
        bool Predicate(object? input);
    }

    /// <inheritdoc cref="ITransition"/>
    /// <typeparam name="TInput">接收的输入的类型。</typeparam>
    /// <typeparam name="TState">状态的类型。</typeparam>
    public interface ITransition<TInput, TState> : ITransition where TState : IState
    {
        /// <inheritdoc cref="ITransition.Target"/>
        new TState? Target { get; }

        /// <inheritdoc cref="ITransition.SetTarget(IState)"/>
        bool SetTarget(TState state);

        /// <inheritdoc cref="ITransition.Predicate(object?)"/>
        bool Predicate(TInput? input);
    }
}
