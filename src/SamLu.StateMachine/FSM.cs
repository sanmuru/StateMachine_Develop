using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    /// <summary>
    /// 表示有限状态机。
    /// </summary>
    public class FSM : IFSM
    {
        private IState currentState;
        /// <summary>
        /// 获取 <see cref="FSM"/> 的当前状态。
        /// </summary>
        public virtual IState CurrentState
        {
            get => this.currentState;
            protected set => this.currentState = value;
        }

        #region StartState
        private IState startState;
        /// <summary>
        /// 获取 <see cref="FSM"/> 的起始状态。
        /// </summary>
        public virtual IState StartState
        {
            get => this.startState;
            set
            {
                if (this.startState != value)
                {
                    var oldValue = this.startState;
                    this.startState = value;
                    var e = new ValueChangingEventArgs<IState>(oldValue, value);
                    this.OnStartStateChanging(e);
                    if (!e.Handled)
                    {
                        this.OnStartStateChanged(new ValueChangedEventArgs<IState>(oldValue, value));
                    }
                }
            }
        }

        #region StartStateChanged
        /// <summary>
        /// <see cref="FSM"/> 的起始状态改变事件。在改变后发生。
        /// </summary>
        /// <seealso cref="StartState"/>
        public event EventHandler<ValueChangedEventArgs<IState>> StartStateChanged;

        /// <summary>
        /// 引发 <see cref="StartStateChanged"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="ValueChangedEventArgs{T}"/> 。</param>
        protected internal void OnStartStateChanged(ValueChangedEventArgs<IState> e)
        {
            this.StartStateChanged?.Invoke(this, e);
        }

#pragma warning disable 1591
        protected virtual void this_StartStateChanged(object sender, ValueChangedEventArgs<IState> e)
        {
            this.Transit(e.NewValue, null);
        }
#pragma warning restore 1591
        #endregion

        #region StartStateChanging
        /// <summary>
        /// <see cref="FSM"/> 的起始状态改变事件。在改变前发生。
        /// </summary>
        /// <seealso cref="StartState"/>
        public event EventHandler<ValueChangingEventArgs<IState>> StartStateChanging;

        /// <summary>
        /// 引发 <see cref="StartStateChanging"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="ValueChangingEventArgs{T}"/> 。</param>
        protected internal void OnStartStateChanging(ValueChangingEventArgs<IState> e)
        {
            this.StartStateChanging?.Invoke(this, e);
        }

#pragma warning disable 1591
        protected virtual void this_StartStateChanging(object sender, ValueChangingEventArgs<IState> e) { }
#pragma warning restore 1591
        #endregion
        #endregion

        /// <summary>
        /// 获取 <see cref="FSM"/> 的状态集。
        /// </summary>
        public virtual ICollection<IState> States =>
            new ReadOnlyCollection<IState>(
                this.StartState == null ?
                    new List<IState>() :
                    this.StartState.RecurGetStates().ToList()
            );

        /// <summary>
        /// 获取 <see cref="FSM"/> 的用户数据字典。
        /// </summary>
        public virtual IDictionary<object, object> UserData { get; } = new Dictionary<object, object>();

        /// <summary>
        /// 初始化 <see cref="FSM"/> 类的新实例。
        /// </summary>
        public FSM()
        {
            this.StartStateChanged += this.this_StartStateChanged;
            this.StartStateChanging += this.this_StartStateChanging;
        }

        /// <summary>
        /// 将所有状态和转换从有限状态机模型中移除。
        /// </summary>
        public virtual void Clear()
        {
            this.StartState = null;
        }

        /// <summary>
        /// 为 <see cref="FSM"/> 的一个指定状态添加指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        public virtual bool AttachTransition(IState state, ITransition transition)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (this.States.Contains(state))
                return state.AttachTransition(transition);
            else return false;
        }

        /// <summary>
        /// 从 <see cref="FSM"/> 的一个指定状态移除指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        public virtual bool RemoveTransition(IState state, ITransition transition)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (this.States.Contains(state))
                return state.RemoveTransition(transition);
            else return false;
        }

        /// <summary>
        /// 将 <see cref="FSM"/> 的一个指定转换的目标设为指定状态。
        /// </summary>
        /// <param name="transition">指定的目标。</param>
        /// <param name="state">指定的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="transition"/> 的值为 null 。</exception>
        public virtual bool SetTarget(ITransition transition, IState state)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            if (transition.RecurGetTransitions().Contains(transition))
                return transition.SetTarget(state);
            else return false;
        }

        /// <summary>
        /// 重置 <see cref="FSM"/> 的方法。
        /// </summary>
        /// <exception cref="NotSupportedException">不存在起始状态，即 <see cref="StartState"/> 的值为 null 。</exception>
        public virtual void Reset()
        {
            if (this.StartState == null)
                throw new NotSupportedException(
                    "不存在起始状态。",
                    new ArgumentNullException(nameof(this.StartState))
                );

            this.Transit(this.StartState, null);
        }

        /// <summary>
        /// <see cref="FSM"/> 的转换操作。此操作沿指定的转换进行。（默认的参数为此状态机本身）。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        protected bool Transit(ITransition transition) => this.Transit(transition, this);

        /// <summary>
        /// <see cref="FSM"/> 的转换操作。此操作沿指定的转换进行，接受指定的参数。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <param name="args">指定的参数。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool Transit(ITransition transition, params object[] args)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));
            if (transition.Target == null) throw new InvalidOperationException("转换的目标状态为空。", new ArgumentNullException(nameof(transition.Target)));

            if (this.currentState.RecurGetReachableTransitions().Contains(transition) && this.States.Contains(transition.Target))
            {
                return this.Transit(transition.Target, transition, args);
            }
            else return false;
        }

        /// <summary>
        /// <see cref="FSM"/> 的转换操作。此操作将使有限状态机模型的 <see cref="CurrentState"/> 转换为指定的状态。（默认的参数为此状态机本身）。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">指定的转换。若转换操作非正常逻辑转换，应设为 null 。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        protected bool Transit(IState state, ITransition transition) => this.Transit(state, transition, this);

        /// <summary>
        /// <see cref="FSM"/> 的转换操作。此操作将使有限状态机模型的 <see cref="CurrentState"/> 转换为指定的状态，接受指定的参数。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">指定的转换。若转换操作非正常逻辑转换，应设为 null 。</param>
        /// <param name="args">指定的参数。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        protected virtual bool Transit(IState state, ITransition transition, params object[] args)
        {
            // 引发退出动作。
            this.CurrentState?.ExitAction?.Invoke(this.CurrentState);

            // 切换当前状态。
            this.CurrentState = state;

            // 引发转换动作。
            transition?.TransitAction?.Invoke(transition, args);

            // 引发进入动作。
            state?.EntryAction?.Invoke(state);

            return true;
        }

        /// <summary>
        /// 接受一个指定输入并进行转换。返回一个值，指示操作是否成功。
        /// </summary>
        /// <typeparam name="TInput">输入的类型。</typeparam>
        /// <param name="input">指定的输入。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentOutOfRangeException">接受的输入超出 <see cref="FSM"/> 能接受的范围。</exception>
        public virtual bool Transit<TInput>(TInput input)
        {
            if (input is ITransition transition)
                return this.Transit(transition);
            else if (input is IState state)
                return this.Transit(state, null);
            else
                throw new ArgumentOutOfRangeException(
                    nameof(input),
                    input,
                    string.Format("不支持的输入类型：{0}", typeof(TInput))
                );
        }

        #region IFSM Implementation
        ICollection<IState> IFSM.States => this.States;
        #endregion
    }

    /// <summary>
    /// 表示有限状态机。
    /// </summary>
    /// <typeparam name="TState">有限状态机的状态的类型。</typeparam>
    /// <typeparam name="TTransition">有限状态机的转换的类型。</typeparam>
    public class FSM<TState, TTransition> : IFSM<TState, TTransition>
        where TState : IState<TTransition>
        where TTransition : ITransition<TState>
    {
        private TState currentState;
        /// <summary>
        /// 获取 <see cref="FSM{TState, TTransition}"/> 的当前状态。
        /// </summary>
        public virtual TState CurrentState
        {
            get => this.currentState;
            protected set => this.currentState = value;
        }

        #region StartState
        private TState startState;
        /// <summary>
        /// 获取 <see cref="FSM{TState, TTransition}"/> 的起始状态。
        /// </summary>
        public virtual TState StartState
        {
            get => this.startState;
            set
            {
                if (!object.Equals(this.startState, value))
                {
                    var oldValue = this.startState;
                    this.startState = value;
                    var e = new ValueChangingEventArgs<TState>(oldValue, value);
                    this.OnStartStateChanging(e);
                    if (!e.Handled)
                    {
                        this.OnStartStateChanged(new ValueChangedEventArgs<TState>(oldValue, value));
                    }
                }
            }
        }

        #region StartStateChanged
        /// <summary>
        /// <see cref="FSM{TState, TTransition}"/> 的起始状态改变事件。在改变后发生。
        /// </summary>
        /// <seealso cref="StartState"/>
        public event EventHandler<ValueChangedEventArgs<TState>> StartStateChanged;

        /// <summary>
        /// 引发 <see cref="StartStateChanged"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="ValueChangedEventArgs{T}"/> 。</param>
        protected internal void OnStartStateChanged(ValueChangedEventArgs<TState> e)
        {
            this.StartStateChanged?.Invoke(this, e);
        }

#pragma warning disable 1591
        protected virtual void this_StartStateChanged(object sender, ValueChangedEventArgs<TState> e)
        {
            this.Transit(e.NewValue, default(TTransition));
        }
#pragma warning restore 1591
        #endregion

        #region StartStateChanging
        /// <summary>
        /// <see cref="FSM{TState, TTransition}"/> 的起始状态改变事件。在改变前发生。
        /// </summary>
        /// <seealso cref="StartState"/>
        public event EventHandler<ValueChangingEventArgs<TState>> StartStateChanging;

        /// <summary>
        /// 引发 <see cref="StartStateChanging"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="ValueChangingEventArgs{T}"/> 。</param>
        protected internal void OnStartStateChanging(ValueChangingEventArgs<TState> e)
        {
            this.StartStateChanging?.Invoke(this, e);
        }

#pragma warning disable 1591
        protected virtual void this_StartStateChanging(object sender, ValueChangingEventArgs<TState> e) { }
#pragma warning restore 1591
        #endregion
        #endregion

        /// <summary>
        /// 获取 <see cref="FSM{TState, TTransition}"/> 的状态集。
        /// </summary>
        public virtual ICollection<TState> States =>
            new ReadOnlyCollection<TState>(
                object.Equals(this.StartState, default(TState)) ?
                    new List<TState>() :
                    this.StartState.RecurGetStates<TState, TTransition>().ToList()
            );

        /// <summary>
        /// 获取 <see cref="FSM{TState, TTransition}"/> 的用户数据字典。
        /// </summary>
        public IDictionary<object, object> UserData { get; } = new Dictionary<object, object>();

        /// <summary>
        /// 初始化 <see cref="FSM{TState, TTransition}"/> 类的新实例。
        /// </summary>
        public FSM()
        {
            this.StartStateChanged += this.this_StartStateChanged;
            this.StartStateChanging += this.this_StartStateChanging;
        }

        /// <summary>
        /// 将所有状态和转换从有限状态机模型中移除。
        /// </summary>
        public virtual void Clear()
        {
            this.StartState = default(TState);
        }

        /// <summary>
        /// 为 <see cref="FSM{TState, TTransition}"/> 的一个指定状态添加指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool AttachTransition(TState state, TTransition transition)
        {
            if (this.States.Contains(state))
                return state.AttachTransition(transition);
            else return false;
        }

        /// <summary>
        /// 从 <see cref="FSM{TState, TTransition}"/> 的一个指定状态移除指定转换。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">要添加的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool RemoveTransition(TState state, TTransition transition)
        {
            if (this.States.Contains(state))
                return state.RemoveTransition(transition);
            else return false;
        }

        /// <summary>
        /// 将 <see cref="FSM{TState, TTransition}"/> 的一个指定转换的目标设为指定状态。
        /// </summary>
        /// <param name="transition">指定的目标。</param>
        /// <param name="state">指定的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool SetTarget(TTransition transition, TState state)
        {
            if (transition.RecurGetTransitions<TState, TTransition>().Contains(transition))
                return transition.SetTarget(state);
            else return false;
        }

        /// <summary>
        /// 重置 <see cref="FSM{TState, TTransition}"/> 的方法。
        /// </summary>
        /// <exception cref="NotSupportedException">不存在起始状态，即 <see cref="StartState"/> 的值为 null 。</exception>
        public virtual void Reset()
        {
            if (this.StartState == null)
                throw new NotSupportedException(
                    "不存在起始状态。",
                    new ArgumentNullException(nameof(this.StartState))
                );

            this.Transit(this.StartState, default(TTransition));
        }

        /// <summary>
        /// <see cref="FSM{TState, TTransition}"/> 的转换操作。此操作沿指定的转换进行。（默认的参数为此状态机本身）。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        protected bool Transit(TTransition transition) => this.Transit(transition, this);

        /// <summary>
        /// <see cref="FSM{TState, TTransition}"/> 的转换操作。此操作沿指定的转换进行，接受指定的参数。
        /// </summary>
        /// <param name="transition">指定的转换。</param>
        /// <param name="args">指定的参数。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        public virtual bool Transit(TTransition transition, params object[] args)
        {
            if (this.CurrentState.RecurGetReachableTransitions<TState, TTransition>().Contains(transition) && this.States.Contains(transition.Target))
            {
                return this.Transit(transition.Target, transition, args);
            }
            else return false;
        }

        /// <summary>
        /// <see cref="FSM{TState, TTransition}"/> 的转换操作。此操作将使有限状态机模型的 <see cref="CurrentState"/> 转换为指定的状态。（默认的参数为此状态机本身）。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">指定的转换。若转换操作非正常逻辑转换，应设为 null 。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        protected bool Transit(TState state, TTransition transition) => this.Transit(state, transition, this);

        /// <summary>
        /// <see cref="FSM{TState, TTransition}"/> 的转换操作。此操作将使有限状态机模型的 <see cref="CurrentState"/> 转换为指定的状态，接受指定的参数。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="transition">指定的转换。若转换操作非正常逻辑转换，应设为 null 。</param>
        /// <param name="args">指定的参数。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        protected virtual bool Transit(TState state, TTransition transition, params object[] args)
        {
            // 引发退出动作。
            this.CurrentState?.ExitAction?.Invoke(this.CurrentState);

            // 切换当前状态。
            this.CurrentState = state;

            // 引发转换动作。
            transition?.TransitAction?.Invoke(transition, args);

            // 引发进入动作。
            state?.EntryAction?.Invoke(state);

            return true;
        }

        /// <summary>
        /// 接受一个指定输入并进行转换。返回一个值，指示操作是否成功。
        /// </summary>
        /// <typeparam name="TInput">输入的类型。</typeparam>
        /// <param name="input">指定的输入。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentOutOfRangeException">接受的输入超出 <see cref="FSM"/> 能接受的范围。</exception>
        public virtual bool Transit<TInput>(TInput input)
        {
            if ((object)input is TTransition transition)
                return this.Transit(transition);
            else if ((object)input is TState state)
                return this.Transit(state, default(TTransition));
            else
                throw new ArgumentOutOfRangeException(
                    nameof(input),
                    input,
                    string.Format("不支持的输入类型：{0}", typeof(TInput))
                );
        }

        #region IFSM, IFSM{TState, TTransition} Implementation
        IState IFSM.CurrentState => this.CurrentState;

        IState IFSM.StartState { get => this.StartState; set => this.StartState = (TState)value; }

        ICollection<IState> IFSM.States => new ReadOnlyCollection<IState>(this.States.Cast<IState>().ToList());

        bool IFSM.AttachTransition(IState state, ITransition transition)
        {
            return this.AttachTransition((TState)state, (TTransition)transition);
        }

        bool IFSM.RemoveTransition(IState state, ITransition transition)
        {
            return this.RemoveTransition((TState)state, (TTransition)transition);
        }

        bool IFSM.SetTarget(ITransition transition, IState state)
        {
            return this.SetTarget((TTransition)transition, (TState)state);
        }
        #endregion
    }
}
