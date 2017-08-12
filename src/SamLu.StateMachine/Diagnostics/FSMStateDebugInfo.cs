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
                if (this.states == null) return this.state.ToString();
                else return string.Format(
                    this.state.IsTerminal ? "({0})" : "{0}",
                    this.states.IndexOf(this.state).ToString()
                );
            }
        }

        public FSMStateDebugInfo(IState state, params object[] args)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            this.state = state;
            if (args != null && args.Length >= 1 && args[0] is IEnumerable<IState> states)
                this.states = new List<IState>(states);
            else
                this.states = null;
        }
    }

    [Obsolete("User none-generic-type FSMStateDebugInfo instead.", true)]
    public class FSMStateDebugInfo<TState>
        where TState : IState
    {
        private TState state;
        private IList<TState> states;

        public string DebugInfo
        {
            get
            {
                if (this.states == null) return this.state.ToString();
                else return string.Format(
                    this.state.IsTerminal ? "({0})" : "{0}",
                    this.states.IndexOf(this.state).ToString()
                );
            }
        }

        public FSMStateDebugInfo(TState state, params object[] args)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            this.state = state;
            if (args != null && args.Length >= 1 && args[0] is IList<TState> states)
                this.states = states;
            else this.states = null;
        }
    }
}
