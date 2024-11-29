using SamLu.RegularExpression.StateMachine;

namespace SamLu.RegularExpression;

/// <summary>
/// 表示重复正则。匹配内部正则对象重复指定的次数。
/// </summary>
/// <typeparam name="T">正则接受的对象的类型。</typeparam>
public class RegexRepeat<T> : RegexObject<T>
{
    /// <summary>
    /// 重复正则内部的正则对象。
    /// </summary>
    protected RegexObject<T> innerRegex;
    /// <summary>
    /// 获取重复正则内部的正则对象。
    /// </summary>
    public virtual RegexObject<T> InnerRegex => this.innerRegex;

    /// <summary>
    /// 重复正则内部的最小重复次数。
    /// </summary>
    protected ulong? minimumCount = null;
    /// <summary>
    /// 重复正则内部的最大重复次数。
    /// </summary>
    protected ulong? maximumCount = null;
    /// <summary>
    /// 重复正则是否尽可能多得匹配。
    /// </summary>
    protected bool isGreedy = true;

    /// <summary>
    /// 获取重复正则内部的最小重复次数。
    /// </summary>
    /// <value>
    /// 重复正则内部的最小重复次数。
    /// <para>当值为 null 时，与值为 0 时意义相同，但在正则对象合并时的行为不同。在对重复正则进行连接、重复操作时，只要其中一个重复正则的 <see cref="MinimumCount"/> <see langword="null"/>， 则结果的重复正则的 <see cref="MinimumCount"/> <see langword="null"/> 。</para>
    /// </value>
    public virtual ulong? MinimumCount => this.minimumCount;
    /// <summary>
    /// 获取重复正则内部的最大重复次数。
    /// </summary>
    /// <value>
    /// 重复正则内部的最大重复次数。
    /// <para>当值为 null 时，指示重复正则的最大重复次数为无限。</para>
    /// </value>
    public virtual ulong? MaximumCount => this.maximumCount;

    /// <summary>
    /// 获取一个值，指示重复正则的最大重复次数是否为无限。
    /// </summary>
    public virtual bool IsInfinte => !this.maximumCount.HasValue;

    /// <summary>
    /// 获取一个值，指示重复正则是否尽可能多得匹配。
    /// </summary>
    public virtual bool IsGreedy => this.isGreedy;

    /// <summary>
    /// 初始化 <see cref="RegexRepeat{T}"/> 类的新实例。该实例指定内部的正则对象。
    /// </summary>
    /// <param name="provider">提供操作正则对象的方法的对象。</param>
    /// <param name="regex">指定的内部正则对象。</param>
    /// <exception cref="ArgumentNullException"><paramref name="regex"/> <see langword="null"/> 。</exception>
    protected RegexRepeat(RegexProvider<T> provider, RegexObject<T> regex) : base(provider) => this.innerRegex = regex ?? throw new ArgumentNullException(nameof(regex));

    /// <summary>
    /// 初始化 <see cref="RegexRepeat{T}"/> 类的新实例。该实例指定内部的正则对象，重复次数的最小值、最大值，是否尽可能多得匹配。
    /// </summary>
    /// <param name="provider">提供操作正则对象的方法的对象。</param>
    /// <param name="regex">指定的内部正则对象。</param>
    /// <param name="minimumCount">最小重复次数。</param>
    /// <param name="maximumCount">最大重复次数。</param>
    /// <param name="isGreedy">是否尽可能多得匹配。</param>
    /// <exception cref="ArgumentNullException"><paramref name="regex"/> <see langword="null"/> 。</exception>
    /// <exception cref="ArgumentOutOfRangeException">重复次数最小值 <paramref name="minimumCount"/> 大于重复次数最大值 <paramref name="maximumCount"/> 。</exception>
    public RegexRepeat(RegexProvider<T> provider, RegexObject<T> regex, ulong? minimumCount, ulong? maximumCount, bool isGreedy) : this(provider, regex)
    {
        if ((minimumCount.HasValue && maximumCount.HasValue) &&
            (minimumCount.Value > maximumCount.Value)
        )
            throw new ArgumentOutOfRangeException(
                $"{nameof(minimumCount)}, {nameof(maximumCount)}",
                $"{minimumCount}, {maximumCount}",
                "次数最小值不能大于最大值。"
            );

        this.minimumCount = minimumCount;
        this.maximumCount = maximumCount;
        this.isGreedy = isGreedy;
    }

    /// <summary>
    /// 初始化 <see cref="RegexRepeat{T}"/> 类的新实例。该实例指定内部的正则对象，重复次数的最小值、最大值，是否尽可能多得匹配。
    /// </summary>
    /// <param name="provider">提供操作正则对象的方法的对象。</param>
    /// <param name="regex">指定的内部正则对象。</param>
    /// <param name="minimumCount">最小重复次数。</param>
    /// <param name="maximumCount">最大重复次数。</param>
    /// <param name="isGreedy">是否尽可能多得匹配。</param>
    /// <exception cref="ArgumentNullException"><paramref name="regex"/> <see langword="null"/> 。</exception>
    /// <exception cref="ArgumentOutOfRangeException">重复次数最小值 <paramref name="minimumCount"/> 大于重复次数最大值 <paramref name="maximumCount"/> 。</exception>
    public RegexRepeat(RegexProvider<T> provider, RegexObject<T> regex, ulong minimumCount, ulong maximumCount, bool isGreedy) : this(provider, regex, (ulong?)minimumCount, (ulong?)maximumCount, isGreedy) { }

    /// <summary>
    /// 初始化 <see cref="RegexRepeat{T}"/> 类的新实例。该实例指定内部的正则对象，重复次数的最大值是否为无限，是否尽可能多得匹配。
    /// </summary>
    /// <param name="provider">提供操作正则对象的方法的对象。</param>
    /// <param name="regex">指定的内部正则对象。</param>
    /// <param name="isInfinite">一个值，指示重复次数的最大值是否为无限。</param>
    /// <param name="isGreedy">是否尽可能多得匹配。</param>
    /// <exception cref="ArgumentNullException"><paramref name="regex"/> <see langword="null"/> 。</exception>
    public RegexRepeat(RegexProvider<T> provider, RegexObject<T> regex, bool isInfinite, bool isGreedy) : this(provider, regex, null, isInfinite, isGreedy) { }

    /// <summary>
    /// 初始化 <see cref="RegexRepeat{T}"/> 类的新实例。该实例指定内部的正则对象，重复次数的最小值以及重复次数的最大值是否为无限，是否尽可能多得匹配。
    /// </summary>
    /// <param name="provider">提供操作正则对象的方法的对象。</param>
    /// <param name="regex">指定的内部正则对象。</param>
    /// <param name="minimumCount">最小重复次数。</param>
    /// <param name="isInfinite">一个值，指示重复次数的最大值是否为无限。</param>
    /// <param name="isGreedy">是否尽可能多得匹配。</param>
    /// <exception cref="ArgumentNullException"><paramref name="regex"/> <see langword="null"/> 。</exception>
    public RegexRepeat(RegexProvider<T> provider, RegexObject<T> regex, ulong? minimumCount, bool isInfinite, bool isGreedy) : this(provider, regex, minimumCount, (isInfinite ? null : (ulong?)(minimumCount ?? ulong.MinValue)), isGreedy) { }

    /// <summary>
    /// 初始化 <see cref="RegexRepeat{T}"/> 类的新实例。该实例指定内部的正则对象，重复次数的最小值以及重复次数的最大值是否为无限，是否尽可能多得匹配。
    /// </summary>
    /// <param name="provider">提供操作正则对象的方法的对象。</param>
    /// <param name="regex">指定的内部正则对象。</param>
    /// <param name="minimumCount">最小重复次数。</param>
    /// <param name="isInfinite">一个值，指示重复次数的最大值是否为无限。</param>
    /// <param name="isGreedy">是否尽可能多得匹配。</param>
    /// <exception cref="ArgumentNullException"><paramref name="regex"/> <see langword="null"/> 。</exception>
    public RegexRepeat(RegexProvider<T> provider, RegexObject<T> regex, ulong minimumCount, bool isInfinite, bool isGreedy) : this(provider, regex, (ulong?)minimumCount, isInfinite, isGreedy) { }

    /// <summary>
    /// 将此重复正则与另一个正则对象串联。
    /// </summary>
    /// <inheritdoc/>
    protected override RegexObject<T> Concat(RegexRepeat<T> repeat) =>
        this.provider.CreateRegexObject(
            repeat,
            this.InnerRegex,
            ((this.MinimumCount.HasValue && repeat.MinimumCount.HasValue) ? this.MinimumCount + repeat.MinimumCount : null),
            ((this.MaximumCount.HasValue && repeat.MaximumCount.HasValue) ? this.MaximumCount + repeat.MaximumCount : null),
            this.IsGreedy
        );

    /// <inheritdoc/>
    protected internal override RegexObject<T> Clone() => this.provider.CreateRegexObject(this, this.innerRegex, this.minimumCount, this.maximumCount, this.isGreedy);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int minHashCode = this.MinimumCount.HasValue ? this.MinimumCount.Value.GetHashCode() : 0;
        int maxHashCode = ~(this.MaximumCount.HasValue ? this.MaximumCount.Value.GetHashCode() : ulong.MaxValue.GetHashCode());
        return this.InnerRegex.GetHashCode() ^ (minHashCode ^ maxHashCode) ^ this.IsGreedy.GetHashCode();
    }

    /// <summary>
    /// 获取重复正则对象的字符串表示。
    /// </summary>
    /// <returns>重复正则对象的字符串表示。</returns>
    public override string? ToString()
    {
        if (this.IsInfinte && (this.MinimumCount ?? ulong.MinValue) == ulong.MinValue)
            return $"{this.InnerRegex}*";
        else if (this.IsInfinte && this.MinimumCount == 1)
            return $"{this.InnerRegex}+";
        else if ((this.MinimumCount ?? ulong.MinValue) == ulong.MinValue && this.MaximumCount == 1)
            return $"({this.InnerRegex})?";
        else
            return $"{this.InnerRegex}{'{'}{this.MinimumCount ?? ulong.MinValue},{(this.IsInfinte ? string.Empty : this.MaximumCount!.Value.ToString())}{'}'}";
    }

    /// <inheritdoc/>
    public override (RegexTransition<T> start, RegexTransition<T> end) GenerateStateMachineSegment()
    {
        var state = this.provider.CreateRegexState();
        var (innerStart, innerEnd) = this.InnerRegex.GenerateStateMachineSegment();
        state.AttachTransition(innerStart.WithRepeat(this.MinimumCount, this.MaximumCount, this.IsGreedy));
        innerEnd.SetTarget(state);

        var start = this.provider.CreateRegexTransition();
        var end = this.provider.CreateRegexTransition();
        start.SetTarget(state);
        state.AttachTransition(end);
        return (start, end);
    }
}