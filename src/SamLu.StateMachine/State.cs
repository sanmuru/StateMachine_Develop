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
    public abstract class State : IState
    {
        /// <summary>
        /// 储存状态的转换集。
        /// </summary>
        protected HashSet<ITransition> transitions = new();

        /// <inheritdoc/>
        public virtual bool IsTerminal { get; set; }

        /// <inheritdoc/>
        public virtual ICollection<ITransition> Transitions => this.transitions.ToList().AsReadOnly();

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
        public virtual bool AttachTransition(ITransition transition)
        {
            ArgumentNullException.ThrowIfNull(transition);

            return this.transitions.Add(transition);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
        public bool RemoveTransition(ITransition transition)
        {
            ArgumentNullException.ThrowIfNull(transition);

            return this.transitions.Remove(transition);
        }
    }

    /// <inheritdoc cref="State"/>
    /// <typeparam name="TTransition">转换的类型。</typeparam>
    public abstract class State<TTransition> : IState<TTransition> where TTransition : ITransition
    {
        /// <summary>
        /// 储存状态的转换集。
        /// </summary>
        protected HashSet<TTransition> transitions = new();

        /// <inheritdoc/>
        public virtual bool IsTerminal { get; set; }

        /// <inheritdoc/>
        public virtual ICollection<TTransition> Transitions => this.transitions.ToList().AsReadOnly();

        ICollection<ITransition> IState.Transitions => this.transitions.Cast<ITransition>().ToList().AsReadOnly();

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
        public virtual bool AttachTransition(TTransition transition)
        {
            ArgumentNullException.ThrowIfNull(transition);

            return this.transitions.Add(transition);
        }

        bool IState.AttachTransition(ITransition transition) => this.AttachTransition((TTransition)transition);

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 <see langword="null"/> 。</exception>
        public virtual bool RemoveTransition(TTransition transition)
        {
            ArgumentNullException.ThrowIfNull(transition);

            return this.transitions.Remove(transition);
        }

        bool IState.RemoveTransition(ITransition transition) => this.RemoveTransition((TTransition)transition);
    }
}
