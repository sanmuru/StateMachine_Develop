using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Runtime
{
    public class TypeDeductionLevel<TTag> : IDictionary<TTag, int>
    {
        private SortedDictionary<TTag, int> weightDic;

        public int this[TTag key] { get => this.weightDic[key]; set => this.weightDic[key] = value; }

        public ICollection<TTag> Keys => this.weightDic.Keys;

        public ICollection<int> Values => this.weightDic.Values;

        public int Count => this.weightDic.Count;

        public bool IsReadOnly => ((ICollection<int>)this.weightDic).IsReadOnly;

        public TypeDeductionLevel()
        {
            this.weightDic = new SortedDictionary<TTag, int>();
        }

        public TypeDeductionLevel(IDictionary<TTag, int> dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            this.weightDic = new SortedDictionary<TTag, int>(dictionary);
        }

        public void Add(Type sourceType, Type destinationType, TTag tag) =>
            ((IDictionary<TTag, int>)this).Add(tag, TypeDeduction<TTag>.GetTypeDeductionWeight(sourceType, destinationType));
        
        public void Add(Type sourceType, TypeMaker destinationType, TTag tag) =>
            ((IDictionary<TTag, int>)this).Add(tag, TypeDeduction<TTag>.GetTypeDeductionWeight(sourceType, destinationType));

        public void Add(TypeMaker sourceType, Type destinationType, TTag tag) =>
            ((IDictionary<TTag, int>)this).Add(tag, TypeDeduction<TTag>.GetTypeDeductionWeight(sourceType, destinationType));

        public void Add(TypeMaker sourceType, TypeMaker destinationType, TTag tag) =>
            ((IDictionary<TTag, int>)this).Add(tag, TypeDeduction<TTag>.GetTypeDeductionWeight(sourceType, destinationType));

        public void Add(TTag key, int value)
        {
            this.weightDic.Add(key, value);
        }

        public void Clear()
        {
            this.weightDic.Clear();
        }

        public bool Contains(KeyValuePair<TTag, int> item)
        {
            return this.weightDic.Contains(item);
        }

        public bool ContainsKey(TTag key)
        {
            return this.weightDic.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TTag, int>[] array, int arrayIndex)
        {
            this.weightDic.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TTag, int>> GetEnumerator()
        {
            return this.weightDic.GetEnumerator();
        }

        public bool Remove(TTag key)
        {
            return this.weightDic.Remove(key);
        }

        public bool TryGetValue(TTag key, out int value)
        {
            return this.weightDic.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<TTag, int>>.Add(KeyValuePair<TTag, int> item)
        {
            ((ICollection<KeyValuePair<TTag, int>>)this.weightDic).Add(item);
        }

        bool ICollection<KeyValuePair<TTag, int>>.Remove(KeyValuePair<TTag, int> item)
        {
            return ((ICollection<KeyValuePair<TTag, int>>)this.weightDic).Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.weightDic.GetEnumerator();
        }
    }
}
