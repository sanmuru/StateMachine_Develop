using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.Adapter
{
    [Obsolete("Not implemented.", true)]
    public interface ITransitionAdapter<TState, TAdaptState> : ITransition<TState>
        where TAdaptState : IState
        where TState : TAdaptState
    {
    }
}
