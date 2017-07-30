using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.EventDriven
{
    /// <summary>
    /// 为与事件驱动的有限状态机的状态的事件引发相关的事件（如 <see cref="EventDrivenFSMTransition.PreviewEventInvoke"/> 、 <see cref="EventDrivenFSMTransition.EventInvoke"/>）提供数据。
    /// </summary>
    /// <seealso cref="EventDrivenFSMTransition.PreviewEventInvoke"/>
    /// <seealso cref="EventDrivenFSMTransition.EventInvoke"/>
    public class EventInvokeEventArgs : EventArgs
    {
        private Predicate<object[]> predicate;

        /// <summary>
        /// 获取引发条件检测方法。
        /// </summary>
        public Predicate<object[]> InvokePredicate { get => this.predicate; }

        private object[] args;
        /// <summary>
        /// 获取引发事件的参数数组。
        /// </summary>
        public object[] InvokeArgs { get => this.args; }

        /// <summary>
        /// 初始化 <see cref="EventInvokeEventArgs"/> 类的新实例。
        /// </summary>
        public EventInvokeEventArgs() : this(null, null) { }

        /// <summary>
        /// 初始化 <see cref="EventInvokeEventArgs"/> 类的新实例。该实例使用指定的引发条件检测方法。
        /// </summary>
        /// <param name="predicate">指定的引发条件检测方法。</param>
        public EventInvokeEventArgs(Predicate<object[]> predicate) : this(predicate, null) { }

        /// <summary>
        /// 初始化 <see cref="EventInvokeEventArgs"/> 类的新实例。该实例使用指定的引发事件的参数数组。
        /// </summary>
        /// <param name="args">指定的引发事件的参数数组。</param>
        public EventInvokeEventArgs(object[] args) : this(null, args) { }

        /// <summary>
        /// 初始化 <see cref="EventInvokeEventArgs"/> 类的新实例。该实例使用指定的引发条件检测方法和引发事件的参数数组。
        /// </summary>
        /// <param name="predicate">指定的引发条件检测方法。</param>
        /// <param name="args">指定的引发事件的参数数组。</param>
        public EventInvokeEventArgs(Predicate<object[]> predicate, object[] args) : base()
        {
            this.predicate = predicate;
            this.args = args;
        }
    }
}
