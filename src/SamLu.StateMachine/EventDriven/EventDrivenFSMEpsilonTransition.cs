using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.EventDriven
{
    /// <summary>
    /// 表示事件驱动的有限状态机的 ε 转换。
    /// </summary>
    public class EventDrivenFSMEpsilonTransition : EventDrivenFSMTransition, IEpsilonTransition<EventDrivenFSMState>
    {
        /// <summary>
        /// 初始化 <see cref="EventDrivenFSMEpsilonTransition"/> 类的新实例。
        /// </summary>
        public EventDrivenFSMEpsilonTransition() : base() { }
    }
}
