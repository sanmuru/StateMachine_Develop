using SamLu.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    /// <summary>
    /// 用户定制的获取调试信息的基类，为 <typeparamref name="T"/> 及其派生类型提供调试信息。
    /// </summary>
    /// <typeparam name="T">要获取调试信息的对象的类型。</typeparam>
    public class CustomizedDebugInfo<T> : IDebugInfo
    {
        /// <summary>
        /// 要获取调试信息的对象。
        /// </summary>
        protected T t;
        /// <summary>
        /// 获取 <see cref="t"/> 的调试信息的方法。
        /// </summary>
        protected Delegate getDebugInfoFunc;
        /// <summary>
        /// 获取调试信息时的参数。
        /// </summary>
        protected object[] args;

        /// <summary>
        /// 获取调试信息。
        /// </summary>
        public virtual string DebugInfo => this.getDebugInfoFunc.DynamicInvoke(this.args) as string;

        /// <summary>
        /// 此为支持获取调试信息的类型的必要约定。初始化 <see cref="CustomizedDebugInfo{T}"/> 的新实例。
        /// </summary>
        /// <param name="t">要获取调试信息的范围对象。</param>
        /// <param name="args">获取调试信息时的可选参数。</param>
        /// <exception cref="ArgumentNullException"><paramref name="args"/> 为 null 。</exception>
        /// <exception cref="NotSupportedException"><paramref name="args"/> 为空，或首位不为 <see cref="Delegate"/> 。</exception>
        /// <exception cref="NotSupportedException"><paramref name="args"/> 的首位 <see cref="Delegate"/> 的签名不为 <see cref="string"/>(<see cref="object"/>[]) 。</exception>
        public CustomizedDebugInfo(T t, params object[] args)
        {
            this.t = t;

            if (args == null) throw new ArgumentNullException(nameof(args));
            else if (args.Length > 0 && args[0] is Delegate func)
            {
                // 如果委托的方法签名为 string (object[]) 。
                if (func.Method.ReturnType == typeof(string) &&
                    func.Method.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(new[] { typeof(object[]) }))
                {
                    this.getDebugInfoFunc = func;
                    this.args = args.Skip(1).ToArray();
                }
                else
                    throw new NotSupportedException($"不支持的方法签名：{func.Method}");
            }
            else throw new NotSupportedException($"缺失必要参数，无法获取调试信息。");
        }
    }
}
