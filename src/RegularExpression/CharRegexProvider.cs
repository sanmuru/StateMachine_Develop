using SamLu.RegularExpression.StateMachine.ObjectModel;
using SamLu.StateMachine.ObjectModel;

namespace SamLu.RegularExpression;

public class CharRegexProvider : RegexProvider<char>
{
    public override IInputSymbolProvider<char> InputSymbolProvider { get; } = new CharInputSymbolProvider();
}
