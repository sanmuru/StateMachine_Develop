using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.Adapter
{
    [Obsolete("Not implemented.", true)]
    public interface IStateAdapter<TTransition, TAdaptTransition> : IState<TTransition>
        where TAdaptTransition : ITransition
        where TTransition : TAdaptTransition
    {
    }
}
