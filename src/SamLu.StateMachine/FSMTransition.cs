using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示有限状态机的转换。
    /// </summary>
    public class FSMTransition : ITransition
    {
        private IState target;
        /// <summary>
        /// 获取 <see cref="FSMTransition"/> 指向的状态。
        /// </summary>
        public virtual IState Target { get => this.target; }

        /// <summary>
        /// 获取或设置表示 <see cref="FSMTransition"/> 的转换动作。在转换转换时进行。
        /// </summary>
        public IAction TransitAction { get; set; }

        /// <summary>
        /// 将转换的目标设为指定状态。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        public virtual bool SetTarget(IState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (this.target == state)
                return false;
            else
            {
                this.target = state;
                return true;
            }
        }
    }

    /// <summary>
    /// 表示有限状态机的转换。
    /// </summary>
    /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
    public class FSMTransition<TState> : ITransition<TState> where TState : IState
    {
        private TState target;
        /// <summary>
        /// 获取 <see cref="FSMTransition{TState}"/> 指向的状态。
        /// </summary>
        public virtual TState Target { get => this.target; }

        /// <summary>
        /// 获取或设置表示 <see cref="FSMTransition{TState}"/> 的转换动作。在转换转换时进行。
        /// </summary>
        public IAction TransitAction { get; set; }

        /// <summary>
        /// 将转换的目标设为指定状态。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool SetTarget(TState state)
        {
            if (object.Equals(this.target, state))
                return false;
            else
            {
                this.target = state;
                return true;
            }
        }

        #region ITransition Implementation
        IState ITransition.Target { get => this.Target; }

        bool ITransition.SetTarget(IState state) => this.SetTarget((TState)state);
        #endregion
    }
}
