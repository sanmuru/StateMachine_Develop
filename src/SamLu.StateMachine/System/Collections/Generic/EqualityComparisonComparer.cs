using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// 使用 <see cref="EqualityComparison{T}"/> 相等比较方法的相等比较器。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public class EqualityComparisonComparer<T> : EqualityComparer<T>
    {
        private readonly EqualityComparison<T> _equalityComparison;

        /// <summary>
        /// 初始化 <see cref="EqualityComparisonComparer{T}"/> 类的新实例，该实例使用指定的相等比较方法。
        /// </summary>
        /// <param name="equalityComparison">指定的相等比较方法。</param>
        public EqualityComparisonComparer(EqualityComparison<T> equalityComparison) => this._equalityComparison = equalityComparison;

        /// <summary>
        /// 确定两个对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>如果指定的对象相等，则为 true；否则为 false 。</returns>
        public override bool Equals(T x, T y) => this._equalityComparison(x, y);

        /// <summary>
        /// 指定对象的哈希函数。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns>指定对象的哈希代码。</returns>
        public override int GetHashCode(T obj) => obj.GetHashCode();
    }
}
