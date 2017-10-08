using SamLu.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.Diagnostics
{
    public class FSMStateDebugInfo : IDebugInfo
    {
        private IState state;
        private IList<IState> states;

        public string DebugInfo
        {
            get
            {
                return string.Format(
                    this.state.IsTerminal ? "({0})" : "{0}",
                    this.states.IndexOf(this.state).ToString()
                );
            }
        }

        public FSMStateDebugInfo(IState state, params object[] args)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            this.state = state;
            this.states = new List<IState>((IEnumerable<IState>)args[1]);
        }
    }

    [Obsolete("User non-generic-type FSMStateDebugInfo instead.", true)]
    public class FSMStateDebugInfo<TState>
        where TState : IState
    {
        private TState state;
        private IList<TState> states;

        public string DebugInfo
        {
            get
            {
                return string.Format(
                    this.state.IsTerminal ? "({0})" : "{0}",
                    this.states.IndexOf(this.state).ToString()
                );
            }
        }

        public FSMStateDebugInfo(TState state, params object[] args)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            this.state = state;
            this.states = new List<TState>((IEnumerable<TState>)args[1]);
        }
    }
}
