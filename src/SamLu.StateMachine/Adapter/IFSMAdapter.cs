using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.Adapter
{
    [Obsolete("Not implemented.", true)]
    public interface IFSMAdapter<TState, TTransition, TAdaptState, TAdaptTransition> : IFSM<TState, TTransition>
        where TAdaptState : IState<TTransition>
        where TState : TAdaptState
        where TAdaptTransition : ITransition<TState>
        where TTransition : TAdaptTransition
    {
    }
}
