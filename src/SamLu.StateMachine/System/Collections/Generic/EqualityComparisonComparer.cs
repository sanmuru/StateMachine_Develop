using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    [Serializable]
    public class EqualityComparisonComparer<T> : EqualityComparer<T>
    {
        private readonly EqualityComparison<T> _equalityComparison;

        public EqualityComparisonComparer(EqualityComparison<T> equalityComparison)=>this._equalityComparison=equalityComparison;

        public override bool Equals(T x, T y) => this._equalityComparison(x, y);

        public override int GetHashCode(T obj) => obj.GetHashCode();
    }
}
