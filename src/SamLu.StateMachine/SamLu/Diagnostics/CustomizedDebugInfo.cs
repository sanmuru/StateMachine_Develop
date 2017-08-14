using SamLu.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.SamLu.Diagnostics
{
    public class CustomizedDebugInfo<T> : IDebugInfo
    {
        protected T t;
        protected Delegate getDebugInfoFunc;
        protected object[] args;

        public virtual string DebugInfo => this.getDebugInfoFunc.DynamicInvoke(this.args) as string;

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
            else throw new NotSupportedException($"确实必要参数，无法获取调试信息。");
        }
    }
}
