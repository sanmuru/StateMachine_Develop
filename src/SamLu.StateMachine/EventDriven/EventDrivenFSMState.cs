using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.EventDriven
{
    /// <summary>
    /// 表示事件驱动的有限状态机的状态。
    /// </summary>
    public class EventDrivenFSMState : FSMState<EventDrivenFSMTransition>
    {
        /// <summary>
        /// 获取 <see cref="EventDrivenFSMState"/> 的转换集。
        /// </summary>
        public sealed override ICollection<EventDrivenFSMTransition> Transitions =>
            base.Transitions.IsReadOnly?
                base.Transitions :
                new ReadOnlyCollection<EventDrivenFSMTransition>(base.Transitions.ToList());

        /// <summary>
        /// 添加指定的转换。
        /// </summary>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public override bool AttachTransition(EventDrivenFSMTransition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            return base.AttachTransition(transition);
        }

        /// <summary>
        /// 移除指定的转换。
        /// </summary>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public override bool RemoveTransition(EventDrivenFSMTransition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            return base.RemoveTransition(transition);
        }
    }
}
