using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 定义了有限状态机的状态的动作应遵循的基本约束。动作是在给定时刻要进行的活动的描述。
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// <see cref="IAction"/> 的调用方法。
        /// </summary>
        /// <param name="sender">调用方法施加的对象。</param>
        /// <param name="args">调用方法的参数。</param>
        void Invoke(object sender, params object[] args);
    }
}
