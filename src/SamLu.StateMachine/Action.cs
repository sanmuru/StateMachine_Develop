using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示有限状态机的状态的动作。
    /// </summary>
    public abstract class Action : IAction
    {
        /// <summary>
        /// <see cref="Action"/> 的调用方法。
        /// </summary>
        /// <param name="sender">调用方法施加的对象。</param>
        /// <param name="args">调用方法的参数。</param>
        public abstract void Invoke(object sender, params object[] args);
    }
}
