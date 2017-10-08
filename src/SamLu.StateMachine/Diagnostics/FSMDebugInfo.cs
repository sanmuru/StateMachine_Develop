using SamLu.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.Diagnostics
{
    public class FSMDebugInfo : IDebugInfo
    {
        private IFSM fsm;
        private object[] args;
        
        public virtual string DebugInfo
        {
            get
            {
                var states = this.fsm.States;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("StartState: ({0}), CurrentState: ({1}), total: {2}{3}", this.fsm.StartState.GetDebugInfo(this.fsm, states), this.fsm.CurrentState.GetDebugInfo(this.fsm, states), states.Count, Environment.NewLine);
                sb.Append(string.Join(
                    Environment.NewLine,
                    (from state in states
                     from transition in state.Transitions
                     select string.Format("  ({0}) --{1}-> ({2})", state.GetDebugInfo(this.fsm, states), transition.GetDebugInfo(this.fsm), transition.Target.GetDebugInfo(this.fsm, states))
                    ).ToArray()
                ));

                return sb.ToString();
            }
        }

        public FSMDebugInfo(IFSM fsm, params object[] args)
        {
            this.fsm = fsm ?? throw new ArgumentNullException(nameof(fsm));
            this.args = args;
        }
    }
}
