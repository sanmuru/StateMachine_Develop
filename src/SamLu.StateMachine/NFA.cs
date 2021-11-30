using SamLu.StateMachine.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示不确定的有限状态机。
    /// </summary>
    public abstract class NFA : FSM, INFA
    {
        protected abstract IState CreateUnionState(params IState[] states);
        protected abstract ITransition CreateUnionTransition(params ITransition[] transitions);
        protected abstract IDFA CreateDFA(params object[] args);

        /// <inheritdoc/>
        public virtual void Optimize() => this.EpsilonClosure();

        public virtual IDFA Determine(IInputSymbols inputSet)
        {
            throw new NotImplementedException();
        }
    }

    /// <inheritdoc cref="NFA"/>
    public abstract class NFA<TInput, TState, TTransition> : FSM<TInput, TState, TTransition>, INFA<TInput, TState, TTransition>
        where TState : IState<TTransition>
        where TTransition : ITransition<TInput, TState>
    {
        protected abstract TState CreateUnionState(params TState[] states);
        protected abstract TTransition CreateUnionTransition(params TTransition[] transitions);
        protected abstract IDFA<TInput, TState, TTransition> CreateDFA(params object[] args);

        /// <inheritdoc/>
        public virtual void Optimize() => this.EpsilonClosure();

        void INFA.Optimize() => this.Optimize();

        public IDFA<TInput, TState, TTransition> Determine(IInputSymbols<TInput> inputSet)
        {
            throw new NotImplementedException();
        }

        IDFA<TInput, TState, TTransition> INFA<TInput, TState, TTransition>.Determine(IInputSymbols<TInput> inputSet)
        {
            throw new NotImplementedException();
        }

        IDFA INFA.Determine(IInputSymbols inputSet)
        {
            throw new NotImplementedException();
        }
    }
}
