using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示有限状态机的状态。
    /// </summary>
    /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
    public class FSMState<TTransition> : IState<TTransition> where TTransition : ITransition
    {
        private List<TTransition> transitions = new List<TTransition>();

        /// <summary>
        /// 获取或设置一个值，指示该有限自动机的状态是否为结束状态。
        /// </summary>
        public bool IsTerminal { get; set; }

        /// <summary>
        /// 获取 <see cref="FSMState{TTransition}"/> 的转换集。
        /// </summary>
        public virtual ICollection<TTransition> Transitions => new ReadOnlyCollection<TTransition>(this.transitions);

        /// <summary>
        /// 获取表示 <see cref="FSMState{TTransition}"/> 的进入动作。在进入状态时进行
        /// </summary>
        public IAction EntryAction { get; set; }

        /// <summary>
        /// 获取表示 <see cref="FSMState{TTransition}"/> 的退出动作。在退出状态时进行。
        /// </summary>
        public IAction ExitAction { get; set; }

        /// <summary>
        /// 获取表示 <see cref="FSMState{TTransition}"/> 的输入动作。依赖于当前状态和输入条件进行。
        /// </summary>
        public IAction InputAction { get; set; }

        /// <summary>
        /// 获取表示 <see cref="FSMState{TTransition}"/> 的转换动作。在进行特定转换时进行。
        /// </summary>
        public IAction TransitAction { get; set; }

        /// <summary>
        /// 初始化 <see cref="FSMState{TTransition}"/> 类的新实例。
        /// </summary>
        public FSMState() : this(false) { }

        /// <summary>
        /// 初始化 <see cref="FSMState{TTransition}"/> 类的新实例，该实例接受一个指定是否为结束状态的值。
        /// </summary>
        /// <param name="isTerminal">一个值，指示该实例是否为结束状态。</param>
        public FSMState(bool isTerminal) => this.IsTerminal = isTerminal;

        /// <summary>
        /// 添加指定的转换。
        /// </summary>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool AttachTransition(TTransition transition)
        {
            if (this.transitions.Contains(transition))
                return false;
            else
            {
                this.transitions.Add(transition);
                return true;
            }
        }

        /// <summary>
        /// 移除指定的转换。
        /// </summary>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool RemoveTransition(TTransition transition)
        {
            if (this.transitions.Contains(transition))
            {
                return this.transitions.Remove(transition);
            }
            else return false;
        }

        #region IState Implementation
        ICollection<ITransition> IState.Transitions => new ReadOnlyCollection<ITransition>(new List<ITransition>((IEnumerable<ITransition>)this.transitions));

        bool IState.AttachTransition(ITransition transition) => this.AttachTransition((TTransition)transition);

        bool IState.RemoveTransition(ITransition transition) => this.RemoveTransition((TTransition)transition);
        #endregion
    }
}
