using System.Collections;

namespace SamLu.StateMachine.ObjectModel;

/// <summary>
/// 提供输入符号相关的比较、相等判定、相邻关系及符号集合生成/分割能力的接口。
/// </summary>
public interface IInputSymbolProvider : IComparer, IEqualityComparer
{
    /// <summary>
    /// 指示指定值是否存在前驱符号（在其有序域中是否存在前驱）。
    /// </summary>
    /// <param name="value">要检查的值，可能为 <see langword="null"/>。</param>
    /// <returns>若存在前驱则返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    bool HasPrevious(object? value);

    /// <summary>
    /// 指示指定值是否存在后继符号（在其有序域中是否存在后继）。
    /// </summary>
    /// <param name="value">要检查的值，可能为 <see langword="null"/>。</param>
    /// <returns>若存在后继则返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    bool HasNext(object? value);

    /// <summary>
    /// 返回指定值的前驱符号（如果存在）。
    /// </summary>
    /// <param name="value">要查询的值，可能为 <see langword="null"/>。</param>
    /// <returns>前驱符号，若不存在可按实现约定返回 <see langword="null"/>。</returns>
    object? Previous(object? value);

    /// <summary>
    /// 返回指定值的后继符号（如果存在）。
    /// </summary>
    /// <param name="value">要查询的值，可能为 <see langword="null"/>。</param>
    /// <returns>后继符号，若不存在可按实现约定返回 <see langword="null"/>。</returns>
    object? Next(object? value);

    /// <summary>
    /// 判断两个值是否为直接相邻（即 <paramref name="y"/> 是否为 <paramref name="x"/> 的直接后继）。
    /// </summary>
    /// <param name="x">第一个值，可能为 <see langword="null"/>。</param>
    /// <param name="y">第二个值，可能为 <see langword="null"/>。</param>
    /// <returns>若二者相邻则返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    bool NextTo(object? x, object? y);

    /// <summary>
    /// 判断给定的输入条目集合是否包含指定的值（按提供者的比较/ 范围逻辑）。
    /// </summary>
    /// <param name="inputEntries">输入条目集合，条目类型为 <see cref="InputEntry"/>。</param>
    /// <param name="value">要检查的值，可能为 <see langword="null"/>。</param>
    /// <returns>若集合包含该值则返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    bool Contains(IEnumerable<InputEntry> inputEntries, object? value);

    /// <summary>
    /// 判断给定的输入条目集合是否包含指定的输入条目（按范围包含关系或等价关系）。
    /// </summary>
    /// <param name="inputEntries">输入条目集合，条目类型为 <see cref="InputEntry"/>。</param>
    /// <param name="value">要检查的输入条目。</param>
    /// <returns>若集合包含该条目则返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    bool Contains(IEnumerable<InputEntry> inputEntries, InputEntry value);

    /// <summary>
    /// 创建一个包含指定值（包含端点）的输入符号集合（用于匹配与划分符号）。
    /// </summary>
    /// <param name="value">要包含的值，可能为 <see langword="null"/>。</param>
    /// <returns>由若干 <see cref="InputEntry"/> 组成的集合，表示包含该值的符号集合。</returns>
    IEnumerable<InputEntry> CreateSymbolsInclude(object? value);

    /// <summary>
    /// 创建一个包含指定范围（包含端点）的输入符号集合（用于匹配与划分符号）。
    /// </summary>
    /// <param name="from">范围起点，可能为 <see langword="null"/>。</param>
    /// <param name="to">范围终点，可能为 <see langword="null"/>。</param>
    /// <returns>由若干 <see cref="InputEntry"/> 组成的集合，表示包含该范围的符号集合。</returns>
    IEnumerable<InputEntry> CreateSymbolsInclude(object? from, object? to);

    /// <summary>
    /// 创建一个不包含指定值的输入符号集合（用于从全集中排除该值）。
    /// </summary>
    /// <param name="value">要排除的值，可能为 <see langword="null"/>。</param>
    /// <returns>由若干 <see cref="InputEntry"/> 组成的集合，表示排除该值后的符号集合。</returns>
    IEnumerable<InputEntry> CreateSymbolsExclude(object? value);

    /// <summary>
    /// 创建一个不包含指定范围的输入符号集合（用于从全集中排除该范围）。
    /// </summary>
    /// <param name="from">排除范围起点，可能为 <see langword="null"/>。</param>
    /// <param name="to">排除范围终点，可能为 <see langword="null"/>。</param>
    /// <returns>由若干 <see cref="InputEntry"/> 组成的集合，表示排除该范围后的符号集合。</returns>
    IEnumerable<InputEntry> CreateSymbolsExclude(object? from, object? to);

    /// <summary>
    /// 将可能重叠或聚合的输入条目集合分割为一组基础且不重叠的 <see cref="InputEntry"/>（以便后续处理）。
    /// </summary>
    /// <param name="entries">要分割的条目集合。</param>
    /// <returns>分割后的不重叠条目集合。</returns>
    IEnumerable<InputEntry> SplitEntries(IEnumerable<InputEntry> entries);
}

/// <summary>
/// 泛型版本的 <see cref="IInputSymbolProvider"/>，为特定输入类型提供对应的比较、相等与符号生成能力。
/// </summary>
/// <typeparam name="T">输入符号的类型。</typeparam>
public interface IInputSymbolProvider<T> : IInputSymbolProvider, IComparer<T?>, IEqualityComparer<T?>
{
    /// <inheritdoc cref="IInputSymbolProvider.HasPrevious"/>
    bool HasPrevious(T? value);

    /// <inheritdoc cref="IInputSymbolProvider.HasNext"/>
    bool HasNext(T? value);

    /// <inheritdoc cref="IInputSymbolProvider.Previous"/>
    T? Previous(T? value);

    /// <inheritdoc cref="IInputSymbolProvider.Next"/>
    T? Next(T? value);

    /// <inheritdoc cref="IInputSymbolProvider.NextTo"/>
    bool NextTo(T? x, T? y);

    /// <inheritdoc cref="IInputSymbolProvider.Contains(IEnumerable{InputEntry}, object?)"/>
    bool Contains(IEnumerable<InputEntry<T>> entries, T? value);

    /// <inheritdoc cref="IInputSymbolProvider.Contains(IEnumerable{InputEntry}, InputEntry)"/>
    bool Contains(IEnumerable<InputEntry<T>> entries, InputEntry<T> value);

    /// <inheritdoc cref="IInputSymbolProvider.CreateSymbolsInclude(object?)"/>
    IEnumerable<InputEntry<T>> CreateSymbolsInclude(T? value);

    /// <inheritdoc cref="IInputSymbolProvider.CreateSymbolsInclude(object?, object?)"/>
    IEnumerable<InputEntry<T>> CreateSymbolsInclude(T? from, T? to);

    /// <inheritdoc cref="IInputSymbolProvider.CreateSymbolsExclude(object?)"/>
    IEnumerable<InputEntry<T>> CreateSymbolsExclude(T? value);

    /// <inheritdoc cref="IInputSymbolProvider.CreateSymbolsExclude(object?, object?)"/>
    IEnumerable<InputEntry<T>> CreateSymbolsExclude(T? from, T? to);

    /// <inheritdoc cref="IInputSymbolProvider.SplitEntries"/>
    IEnumerable<InputEntry<T>> SplitEntries(IEnumerable<InputEntry<T>> entries);
}
