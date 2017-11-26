using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Runtime
{
    public class TypeDeduction<TTag> : IList<TypeDeductionLevel<TTag>>
    {
        private List<TypeDeductionLevel<TTag>> levels;

        public TypeDeductionLevel<TTag> CurrentLevel =>
            this.levels.Count == 0 ? null : this.levels[this.levels.Count - 1];

        public TypeDeductionLevel<TTag> this[int index]
        {
            get => this.levels[index];
            set => this.levels[index] = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int Count => this.levels.Count;

        public bool IsReadOnly => ((ICollection<TypeDeductionLevel<TTag>>)this.levels).IsReadOnly;

        public TypeDeduction()
        {
            this.levels = new List<TypeDeductionLevel<TTag>>();
        }

        public TypeDeduction(int capacity)
        {
            this.levels = new List<TypeDeductionLevel<TTag>>(capacity);
        }

        public TypeDeduction(IEnumerable<TypeDeductionLevel<TTag>> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            this.levels = new List<TypeDeductionLevel<TTag>>(collection.Where(item => item != null));
        }

        public IEnumerable<IEnumerable<TTag>> Merge()
        {
            var dic = new Dictionary<int, Queue<IEnumerable<TTag>>>();
            foreach (var level in this.levels)
            {
                var groups = level.GroupBy(
                    (pair => pair.Value),
                    (pair => pair.Key)
                );
                foreach (var group in groups)
                {
                    Queue<IEnumerable<TTag>> queue;
                    if (!dic.ContainsKey(group.Key))
                        dic.Add(group.Key, queue = new Queue<IEnumerable<TTag>>());
                    else
                        queue = dic[group.Key];

                    queue.Enqueue(group);
                }
            }

            foreach (var pair in dic.OrderBy(pair => pair.Key))
                foreach (var tags in pair.Value)
                    yield return tags;
        }
        
        [Obsolete("未实现。")]
        public static int GetTypeDeductionWeight(Type sourceType, Type destinationType)
        {
            return int.MaxValue;
        }

        [Obsolete("未实现。")]
        public static int GetTypeDeductionWeight(TypeMaker sourceType, Type destinationType)
        {
            return int.MaxValue;
        }

        [Obsolete("未实现。")]
        public static int GetTypeDeductionWeight(Type sourceType, TypeMaker destinationType)
        {
            return int.MaxValue;
        }

        [Obsolete("未实现。")]
        public static int GetTypeDeductionWeight(TypeMaker sourceType, TypeMaker destinationType)
        {
            return int.MaxValue;
        }

        public void Add(TypeDeductionLevel<TTag> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            this.levels.Add(item);
        }

        public void Clear()
        {
            this.levels.Clear();
        }

        public bool Contains(TypeDeductionLevel<TTag> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return this.levels.Contains(item);
        }

        public void CopyTo(TypeDeductionLevel<TTag>[] array, int arrayIndex)
        {
            this.levels.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TypeDeductionLevel<TTag>> GetEnumerator()
        {
            return this.levels.GetEnumerator();
        }

        public int IndexOf(TypeDeductionLevel<TTag> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return this.levels.IndexOf(item);
        }

        public void Insert(int index, TypeDeductionLevel<TTag> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            this.levels.Insert(index, item);
        }

        public bool Remove(TypeDeductionLevel<TTag> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return this.levels.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.levels.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.levels.GetEnumerator();
        }
    }
}
