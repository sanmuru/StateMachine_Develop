using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.Adapter
{
    [Obsolete("Not implemented.", true)]
    public class FSMAdapter<TState, TTransition, TAdaptState, TAdaptTransition> : FSM<TState, TTransition>, IFSMAdapter<TState, TTransition, TAdaptState, TAdaptTransition>
        where TAdaptState : IState<TTransition>
        where TState : TAdaptState
        where TAdaptTransition : ITransition<TState>
        where TTransition : TAdaptTransition
    {
    }
}
