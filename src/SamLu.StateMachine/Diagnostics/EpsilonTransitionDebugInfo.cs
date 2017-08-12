using SamLu.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.Diagnostics
{
    public class EpsilonTransitionDebugInfo : IDebugInfo
    {
        private IEpsilonTransition epsilonTransition;

        public virtual string DebugInfo => string.Empty;

        public EpsilonTransitionDebugInfo(IEpsilonTransition epsilonTransition, params object[] args) =>
            this.epsilonTransition = epsilonTransition ?? throw new ArgumentNullException(nameof(epsilonTransition));
    }
}
