using SamLu.StateMachine;
using SamLu.StateMachine.ObjectModel;

namespace SamLu.RegularExpression.StateMachine;

public class RegexNFA<T> : NFA<T, RegexState<T>, RegexTransition<T>, RegexState<T>, RegexTransition<T>>
{
    public override RegexState<T> CreateDFAState(params IEnumerable<RegexState<T>> states)
    {
        return new() { IsTerminal = states.Any(s => s.IsTerminal) };
    }

    public override RegexTransition<T> CreateDFATransition(IEnumerable<InputEntry<T>> inputEntries, params IEnumerable<RegexTransition<T>> transitions)
    {
        return new(inputEntries);
    }

    public override bool Transit(T? input, IInputSymbolProvider<T> provider) => throw new NotSupportedException();
}

public class RegexDFA<T> : DFA<T, RegexState<T>, RegexTransition<T>>
{
    public override bool Transit(T? input, IInputSymbolProvider<T> provider)
    {
        foreach (var transition in this.CurrentState.Transitions)
        {
            if (provider.Contains(transition.InputEntries, input))
            {
                this.CurrentState = transition.Target!;
                return true;
            }
        }
        return false;
    }
}
