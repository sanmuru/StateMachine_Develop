using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示有限状态机。
    /// </summary>
    public abstract class FSM : IFSM
    {
        /// <summary>
        /// 储存当前状态。
        /// </summary>
        protected IState? currentState;
        /// <summary>
        /// 储存起始状态。
        /// </summary>
        protected IState? startState;

        /// <inheritdoc/>
        /// <exception cref="UninitializedException">有限状态机未初始化。</exception>
        public virtual IState CurrentState => this.currentState ?? throw new UninitializedException("有限状态机未初始化。");

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 的值为 <see langword="null"/> 。</exception>
        public virtual IState StartState
        {
            get => this.startState ?? throw new UninitializedException("有限状态机未初始化。");
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                this.startState = value;
                this.Reset();
            }
        }

        /// <inheritdoc/>
        /// <exception cref="UninitializedException">有限状态机未初始化。</exception>
        public virtual void Reset()
        {
            if (this.currentState is null) throw new UninitializedException("有限状态机未初始化。");

            this.currentState = this.startState;
        }

        /// <inheritdoc/>
        public abstract bool Transit(object? input);
    }

    /// <inheritdoc cref="FSM"/>
    public abstract class FSM<TInput, TState, TTransition> : IFSM<TInput, TState, TTransition>
        where TState : IState<TTransition>
        where TTransition : ITransition<TInput, TState>
    {
        /// <inheritdoc cref="FSM.currentState"/>
        protected TState? currentState;
        /// <inheritdoc cref="FSM.startState"/>
        protected TState? startState;

        /// <inheritdoc/>
        /// <exception cref="UninitializedException">有限状态机未初始化。</exception>
        public virtual TState CurrentState => this.currentState ?? throw new UninitializedException("有限状态机未初始化。");

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 的值为 <see langword="null"/> 。</exception>
        public virtual TState StartState
        {
            get => this.startState ?? throw new UninitializedException("有限状态机未初始化。");
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                this.startState = value;
                this.Reset();
            }
        }

        IState IFSM.CurrentState => this.CurrentState;

        IState IFSM.StartState
        {
            get => this.StartState;
            set => this.StartState = (TState)value;
        }

        /// <inheritdoc/>
        /// <exception cref="UninitializedException">有限状态机未初始化。</exception>
        public virtual void Reset()
        {
            if (this.currentState is null) throw new UninitializedException("有限状态机未初始化。");

            this.currentState = this.startState;
        }

        /// <inheritdoc/>
        public abstract bool Transit(TInput? input);

        bool IFSM.Transit(object? input) => this.Transit((TInput?)input);
    }
}
