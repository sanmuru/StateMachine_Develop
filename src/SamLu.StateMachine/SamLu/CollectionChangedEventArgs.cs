using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu
{
    /// <summary>
    /// 集合内容改变事件参数。使用指定的集合类型和项类型。
    /// </summary>
    /// <typeparam name="TCollection">指定的集合类型。</typeparam>
    /// <typeparam name="TItem">指定的项类型。</typeparam>
    public class CollectionChangedEventArgs<TCollection, TItem> : ValueChangedEventArgs<TCollection> where TCollection : ICollection<TItem>
    {
        private ICollection<TItem> addedItems;
        private ICollection<TItem> removedItems;

        /// <summary>
        /// 获取已添加的项。
        /// </summary>
        public ICollection<TItem> AddedItems { get => this.addedItems; }
        /// <summary>
        /// 获取已移除的项。
        /// </summary>
        public ICollection<TItem> RemovedItems { get => this.removedItems; }

        /// <summary>
        /// 初始化 <see cref="CollectionChangedEventArgs{TCollection, TItem}"/> 类的新实例。实例使用指定的旧值和新值。
        /// </summary>
        /// <param name="oldValue">指定的旧值。</param>
        /// <param name="newValue">指定的新值。</param>
        public CollectionChangedEventArgs(TCollection oldValue, TCollection newValue) : base(oldValue, newValue)
        {
            if (oldValue == null)
            {
                this.removedItems = new List<TItem>();
                if (newValue == null)
                    this.addedItems = new List<TItem>();
                else
                    this.addedItems = newValue.ToList();
            }
            else if (newValue == null)
            {
                this.addedItems = new List<TItem>();
                if (oldValue == null)
                    this.removedItems = new List<TItem>();
                else
                    this.removedItems = oldValue.ToList();
            }
            else
            {
                this.addedItems = this.NewValue.Except(this.OldValue).ToList();
                this.removedItems = this.OldValue.Except(this.NewValue).ToList();
            }
        }
    }

    /// <summary>
    /// 集合内容改变事件参数。使用指定的项类型。
    /// </summary>
    /// <typeparam name="TItem">指定的项类型。</typeparam>
    public class CollectionChangedEventArgs<TItem> : CollectionChangedEventArgs<ICollection<TItem>, TItem>
    {
        /// <summary>
        /// 初始化 <see cref="CollectionChangedEventArgs{TItem}"/> 类的新实例。实例使用指定的旧值和新值。
        /// </summary>
        /// <param name="oldValue">指定的旧值。</param>
        /// <param name="newValue">指定的新值。</param>
        /// <exception cref="InvalidCastException"><paramref name="oldValue"/> 中其中一个项的类型不能转换为 <typeparamref name="TItem"/> ；或<paramref name="newValue"/> 中其中一个项的类型不能转换为 <typeparamref name="TItem"/> 。</exception>
        public CollectionChangedEventArgs(ICollection oldValue, ICollection newValue) : this((ICollection<TItem>)oldValue?.Cast<TItem>().ToList(), (ICollection<TItem>)oldValue?.Cast<TItem>().ToList()) { }

        /// <summary>
        /// 初始化 <see cref="CollectionChangedEventArgs{TItem}"/> 类的新实例。实例使用指定的旧值和新值。
        /// </summary>
        /// <param name="oldValue">指定的旧值。</param>
        /// <param name="newValue">指定的新值。</param>
        public CollectionChangedEventArgs(ICollection<TItem> oldValue, ICollection<TItem> newValue) : base(oldValue, newValue) { }
    }
}
