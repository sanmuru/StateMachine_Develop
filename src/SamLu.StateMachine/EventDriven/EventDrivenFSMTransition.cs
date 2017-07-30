#define OutputAssembly
#undef OutputAssembly

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.EventDriven
{
    /// <summary>
    /// 表示事件驱动的有限状态机的转换。
    /// </summary>
    public class EventDrivenFSMTransition : FSMTransition<EventDrivenFSMState>, IDisposable
    {
        private static int no = 0;
        private AppDomain appDomain;
        protected AppDomain AppDomain
        {
            get
            {
                if (this.appDomain == null)
                {
#if true
                    this.appDomain = AppDomain.CurrentDomain;
#else
                    this.appDomain = AppDomain.CreateDomain("CallingMethodAppDomain" + ++EventDrivenFSMTransition.no);
#endif
                }
                return this.appDomain;
            }
        }

        private ModuleBuilder moduleBuilder;
        protected ModuleBuilder ModuleBuilder
        {
            get
            {
                if (this.moduleBuilder == null)
                {
                    this.moduleBuilder = this.AppDomain.DefineDynamicAssembly(
                        new AssemblyName(string.Format("CallingMethodAssembly.dll")),
#if OutputAssembly
                        AssemblyBuilderAccess.RunAndSave
#else
                        AssemblyBuilderAccess.RunAndCollect
#endif
                    )
                    .DefineDynamicModule(string.Format("CallingMethodModule.dll")
#if OutputAssembly
            ,           string.Format("CallingMethodModule.dll")
#endif
                    );
                }

                return this.moduleBuilder;
            }
        }

        /// <summary>
        /// 初始化 <see cref="EventDrivenFSMTransition"/> 类的新实例。
        /// </summary>
        protected EventDrivenFSMTransition() { }

        /// <summary>
        /// 用指定的处理委托类型、添加方法和移除方法初始化 <see cref="EventDrivenFSMTransition"/> 类的新实例。
        /// </summary>
        /// <param name="handlerType">指定的处理委托类型。</param>
        /// <param name="addMethod">指定的添加方法。</param>
        /// <param name="removeMethod">指定的移除方法。</param>
        public EventDrivenFSMTransition(Type handlerType, Action<Delegate> addMethod, Action<Delegate> removeMethod) : this()
        {
            Delegate dCallingMethod = this.GetDelegateForCallingMethod(handlerType, (d => removeMethod(d)));
            addMethod?.Invoke(dCallingMethod);
            this.removeDelegateForCallingMethodMethods.Add(new Tuple<Delegate, Action<Delegate>>(dCallingMethod, removeMethod));
        }

        /// <summary>
        /// 用指定的添加方法和移除方法初始化 <see cref="EventDrivenFSMTransition"/> 类的新实例。
        /// </summary>
        /// <param name="addMethod">指定的添加方法。</param>
        /// <param name="removeMethod">指定的移除方法。</param>
        public EventDrivenFSMTransition(Action<EventHandler> addMethod, Action<EventHandler> removeMethod) :
            this(
                typeof(EventHandler),
                (addMethod == null ? null : new Action<Delegate>(d => addMethod(d as EventHandler))),
                (removeMethod == null ? null : new Action<Delegate>(d => removeMethod(d as EventHandler)))
            )
        { }

        /// <summary>
        /// 用指定的事件源和事件元数据初始化 <see cref="EventDrivenFSMTransition"/> 类的新实例。
        /// </summary>
        /// <param name="target">指定的事件源。</param>
        /// <param name="eventInfo">指定的事件元数据。</param>
        public EventDrivenFSMTransition(object target, EventInfo eventInfo) :
            this(
                eventInfo.EventHandlerType,
                ((Delegate d) => eventInfo.AddEventHandler(target, d)),
                ((Delegate d) => eventInfo.RemoveEventHandler(target, d))
            )
        { }

        //public EventDrivenFSMTransition(object target, Type declaredType, string eventName) : this(target, declaredType.GetEvent(eventName)) { }
        //public EventDrivenFSMTransition(object target, Type declaredType, string eventName, BindingFlags flags) : this(target, declaredType.GetEvent(eventName, flags)) { }

        /// <summary>
        /// 使用指定的处理委托初始化 <see cref="EventDrivenFSMTransition"/> 类的新实例。
        /// </summary>
        /// <typeparam name="THandler">指定的处理委托的类型。</typeparam>
        /// <param name="handler">指定的处理委托。</param>
        /// <returns><see cref="EventDrivenFSMTransition"/> 类的新实例，该实例使用指定的处理委托和默认的添加、移除方法。</returns>
        public static EventDrivenFSMTransition CreateEventDrivenFSMTransition<THandler>(THandler handler)
            where THandler : class
        {
            return EventDrivenFSMTransition.CreateEventDrivenFSMTransition(
                handler,
                ((THandler h) => Delegate.Combine(handler as Delegate, h as Delegate)),
                ((THandler h) => Delegate.Remove(handler as Delegate, h as Delegate))
            );
        }

        /// <summary>
        /// 使用指定的处理委托、添加方法和移除方法初始化 <see cref="EventDrivenFSMTransition"/> 类的新实例。
        /// </summary>
        /// <typeparam name="THandler">指定的处理委托的类型。</typeparam>
        /// <param name="handler">指定的处理委托。</param>
        /// <param name="addMethod">指定的添加方法。</param>
        /// <param name="removeMethod">指定的移除方法。</param>
        /// <returns><see cref="EventDrivenFSMTransition"/> 类的新实例，该实例使用指定的处理委托、添加方法和移除方法。</returns>
        public static EventDrivenFSMTransition CreateEventDrivenFSMTransition<THandler>(THandler handler, Action<THandler> addMethod, Action<THandler> removeMethod)
            where THandler : class
        {
            if (!typeof(Delegate).IsAssignableFrom(typeof(THandler)))
                throw new ArgumentOutOfRangeException(
                    nameof(handler),
                    new InvalidCastException(
                        string.Format("{0} 不能从 {1} 转换。", typeof(Delegate).FullName, typeof(THandler).FullName)
                    )
                );

            return new EventDrivenFSMTransition
                (
                    typeof(THandler),
                    (addMethod == null ? null : new Action<Delegate>((Delegate d) => addMethod(d as THandler))),
                    (removeMethod == null ? null : new Action<Delegate>((Delegate d) => removeMethod(d as THandler)))
                );
        }

        /// <summary>
        /// 将转换的目标设为指定状态。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <returns>一个值，指示操作是否成功。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> 的值为 null 。</exception>
        public override bool SetTarget(EventDrivenFSMState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            return base.SetTarget(state);
        }

        #region 核心
        /// <summary>
        /// 驱动 <see cref="EventDrivenFSMTransition"/> 转换的调用方法。
        /// </summary>
        /// <param name="args">调用方法的参数。</param>
        /// <seealso cref="CallingMethodInternal(object[], out bool)"/>
        /// <seealso cref="OnPreviewEventInvoke(EventInvokeEventArgs)"/>
        /// <seealso cref="OnEventInvoke(EventInvokeEventArgs)"/>
        public void CallingMethod(params object[] args)
        {
            this.CallingMethodInternal(args, out bool handled);

            if (!handled)
            {
                this.OnPreviewEventInvoke(new EventInvokeEventArgs(args));
                this.OnEventInvoke(new EventInvokeEventArgs(args));
            }
        }

        /// <summary>
        /// 由 <see cref="CallingMethod(object[])"/> 调用的内部方法。由派生类自定义处理方法体。
        /// </summary>
        /// <param name="args">调用方法的参数。</param>
        /// <param name="handled">一个值，指示是否继续触发 <see cref="EventInvoke"/> 事件。默认值为 false 。</param>
        /// <seealso cref="CallingMethod(object[])"/>
        protected virtual void CallingMethodInternal(object[] args, out bool handled) => handled = false;

        /// <summary>
        /// 内部缓存调用方法的字典。
        /// </summary>
        private readonly Dictionary<Type[], Tuple<object, MethodInfo>> callingMethodDic = new Dictionary<Type[], Tuple<object, MethodInfo>>();
        /// <summary>
        /// 内部缓存调用方法委托对象的字典。
        /// </summary>
        private readonly Dictionary<Type, Delegate> delegateForCallingMethodDic = new Dictionary<Type, Delegate>();
        /// <summary>
        /// 内部注册移除调用方法的列表。
        /// </summary>
        private readonly List<Tuple<MethodInfo, Action<MethodInfo>>> removeCallingMethodMethods = new List<Tuple<MethodInfo, Action<MethodInfo>>>();
        /// <summary>
        /// 内部注册移除调用方法委托对象的列表。
        /// </summary>
        private readonly List<Tuple<Delegate, Action<Delegate>>> removeDelegateForCallingMethodMethods = new List<Tuple<Delegate, Action<Delegate>>>();

        /// <summary>
        /// 获取 <see cref="CallingMethod(object[])"/> 经过指定委托类型适配的表示的方法。
        /// </summary>
        /// <param name="handlerType">指定的委托类型。</param>
        /// <param name="removeMethod">用于 <see cref="EventDrivenFSMTransition"/> 构析时调用。</param>
        /// <param name="target">用于调用调用方法的对象。</param>
        /// <returns><see cref="CallingMethod(object[])"/> 经过指定委托类型适配的表示的方法。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="handlerType"/> 为 null 。</exception>
        protected internal MethodInfo GetCallingMethod(Type handlerType, Action<MethodInfo> removeMethod, out object target)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            // 反射获取 Invoke 方法。
            var Invoke_Method = handlerType.GetMethod("Invoke");
            return this.GetCallingMethod(Invoke_Method.ReturnType, Invoke_Method.GetParameters().Select(pi => pi.ParameterType).ToArray(), removeMethod, out target);
        }

        /// <summary>
        /// 获取 <see cref="CallingMethod(object[])"/> 经过指定方法签名适配的表示的方法。
        /// </summary>
        /// <param name="method">指定的方法签名。</param>
        /// <param name="removeMethod">用于 <see cref="EventDrivenFSMTransition"/> 构析时调用。</param>
        /// <param name="target">用于调用调用方法的对象。</param>
        /// <returns><see cref="CallingMethod(object[])"/> 经过指定方法签名适配的表示的方法。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="method"/> 为 null 。</exception>
        protected internal MethodInfo GetCallingMethod(MethodInfo method, Action<MethodInfo> removeMethod, out object target)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            return this.GetCallingMethod(method.ReturnType, method.GetParameters().Select(pi => pi.ParameterType).ToArray(), removeMethod, out target);
        }

        /// <summary>
        /// 获取 <see cref="CallingMethod(object[])"/> 的具有指定返回类型和参数列表的表示的方法。
        /// </summary>
        /// <param name="returnType">指定的返回类型。</param>
        /// <param name="parameterTypes">指定的参数列表。</param>
        /// <param name="removeMethod">用于 <see cref="EventDrivenFSMTransition"/> 构析时调用。</param>
        /// <param name="target">用于调用调用方法的对象。</param>
        /// <returns><see cref="CallingMethod(object[])"/> 的具有指定返回类型和参数列表的表示的方法。</returns>
        protected internal MethodInfo GetCallingMethod(Type returnType, Type[] parameterTypes, Action<MethodInfo> removeMethod, out object target)
        {
            MethodInfo callingMethod = null;
            parameterTypes = parameterTypes ?? Type.EmptyTypes; // 去除 null 。
#if true
            // 获取第一个匹配项。
            var delegatePair = this.callingMethodDic.Where(pair => pair.Key.SequenceEqual(parameterTypes)).Select(pair => pair.Value).FirstOrDefault();
#else
            foreach (var pair in this.callingMethodDic)
            {
                if (pair.Key.Length != parameterTypes.Length) continue;

                bool f = true;
                for (int i = 0; i < parameterTypes.Length; i++)
                    if (pair.Key[i] != parameterTypes[i])
                    {
                        f = false;
                        break;
                    }

                if (f)
                {
                    System.Diagnostics.Trace.Assert(pair.Value != null, "调用方法缓存字典当前项的值为空。");
                    callingMethod = pair.Value;
                    break;
                }
            }
#endif

            if (delegatePair == null)
            {
                var typeBuilder = this.ModuleBuilder.DefineType("CallingMethodType" + (this.callingMethodDic.Count + 1));
                typeBuilder.SetCustomAttribute(
                    new CustomAttributeBuilder(
                        typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes),
                        new object[0]
                    )
                );
                #region transitionFileBuilder
                FieldBuilder transitionFieldBuilder = typeBuilder.DefineField(
                    "transition",
                    typeof(EventDrivenFSMTransition),
                    FieldAttributes.Private
                );
                #endregion
                #region constructorBuilder
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
                    MethodAttributes.Public,
                    CallingConventions.Standard,
                    new Type[] { typeof(EventDrivenFSMTransition) }
                );
                ParameterBuilder transitionParameterBuilder = constructorBuilder.DefineParameter(1, ParameterAttributes.None, "transition");
                ILGenerator ILGen = constructorBuilder.GetILGenerator();
                ILGen.Emit(OpCodes.Ldarg_0); // ldarg.0
                ILGen.Emit(OpCodes.Ldarg, transitionParameterBuilder.Position); // ldarg.n
                ILGen.Emit(OpCodes.Stfld, transitionFieldBuilder); // stfld class [#ThisAssemblyName#]#ThisAssemblyName#.StateMachine.EventDrivenFSMTransition CallingMethodType#no#::transition
                ILGen.Emit(OpCodes.Ret);
                #endregion
                #region callingMethodMethodBuilder
                MethodBuilder callingMethodMethodBuilder = typeBuilder.DefineMethod("CallingMethod", MethodAttributes.Public, returnType, parameterTypes);
                ILGen = callingMethodMethodBuilder.GetILGenerator();
                ILGen.Emit(OpCodes.Nop); // nop
                ILGen.Emit(OpCodes.Ldarg_0); // ldarg.0
                ILGen.Emit(OpCodes.Ldfld, transitionFieldBuilder); // ldfld class [#ThisAssemblyName#]#ThisAssemblyName#.StateMachine.EventDrivenFSMTransition CallingMethodType#no#::transition
                ILGen.Emit(OpCodes.Ldc_I4, parameterTypes.Length); // ldc.i4.#length#
                ILGen.Emit(OpCodes.Newarr, typeof(object)); // newarr[mscorlib]System.Object
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    ILGen.Emit(OpCodes.Dup); // dup
                    ILGen.Emit(OpCodes.Ldc_I4, i); // ldc.i4.#n#
                    ILGen.Emit(OpCodes.Ldarg, i + 1); // ldarg.#n+1#
                    if (parameterTypes[i].IsValueType)
                        ILGen.Emit(OpCodes.Box, parameterTypes[i]); // box #TParameter#
                    ILGen.Emit(OpCodes.Stelem_Ref);
                }
                ILGen.Emit(OpCodes.Call, this.GetType().GetMethod(nameof(this.CallingMethod))); // call instance void #ThisAssemblyName#.StateMachine.EventDrivenFSMTransition::CallingMethod(object[])
                ILGen.Emit(OpCodes.Nop); // nop
                if (returnType != typeof(void))
                {
                    LocalBuilder localReturnValue = ILGen.DeclareLocal(returnType);
                    ILGen.Emit(OpCodes.Ldloca, localReturnValue.LocalIndex); // ldloca.s 0
                    ILGen.Emit(OpCodes.Initobj, returnType); // initobj #TReturn#
                    ILGen.Emit(OpCodes.Ldloc, localReturnValue.LocalIndex); // ldloc.0 
                }
                ILGen.Emit(OpCodes.Ret); // ret
                #endregion

                Type type = typeBuilder.CreateType();
#if OutputAssembly
                ((AssemblyBuilder)moduleBuilder.Assembly).Save(type.Assembly.GetName().Name + ".dll");
#endif
                target = Activator.CreateInstance(type, this);
                callingMethod = type.GetMethod(callingMethodMethodBuilder.Name);
#if false && DEBUG
                callingMethod.Invoke(target, parameterTypes.Select(pt => pt.IsValueType ? Activator.CreateInstance(pt) : null).ToArray());
#endif
            }
            else
            {
                target = delegatePair.Item1;
                callingMethod = delegatePair.Item2;
            }

            if (removeMethod != null)
            {
                Tuple<MethodInfo, Action<MethodInfo>> tuple = new Tuple<MethodInfo, Action<MethodInfo>>(callingMethod, removeMethod);
                if (!this.removeCallingMethodMethods.Any(t => object.Equals(t.Item1, tuple.Item1) && object.Equals(t.Item2, tuple.Item2)))
                {
                    this.removeCallingMethodMethods.Add(tuple);
                }
            }

            return callingMethod;
        }

        /// <summary>
        /// 获取 <see cref="CallingMethod(object[])"/> 经过指定委托类型适配的委托对象。
        /// </summary>
        /// <param name="handlerType">指定的委托类型。</param>
        /// <param name="removeMethod">用于 <see cref="EventDrivenFSMTransition"/> 构析时调用。</param>
        /// <returns><see cref="CallingMethod(object[])"/> 经过指定委托类型适配的委托对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="handlerType"/> 为 null 。</exception>
        public Delegate GetDelegateForCallingMethod(Type handlerType, Action<Delegate> removeMethod)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            Delegate dCallingMethod;
            if (this.delegateForCallingMethodDic.ContainsKey(handlerType))
                dCallingMethod = this.delegateForCallingMethodDic[handlerType];
            else
            {
                MethodInfo method = this.GetCallingMethod(handlerType, null, out object target);
                dCallingMethod = Delegate.CreateDelegate(handlerType, target, method);
                this.delegateForCallingMethodDic.Add(handlerType, dCallingMethod);
                return dCallingMethod;
            }

            if (dCallingMethod != null)
            {
                Tuple<Delegate, Action<Delegate>> tuple = new Tuple<Delegate, Action<Delegate>>(dCallingMethod, removeMethod);
                if (!this.removeDelegateForCallingMethodMethods.Any(t => object.Equals(t.Item1, tuple.Item1) && object.Equals(t.Item2, tuple.Item2)))
                {
                    this.removeDelegateForCallingMethodMethods.Add(tuple);
                }
            }

            return dCallingMethod;
        }

        /// <summary>
        /// 获取 <see cref="CallingMethod(object[])"/> 经过指定的拥有指定委托类型的移除方法适配的委托对象。
        /// </summary>
        /// <typeparam name="THandler">指定的委托类型。</typeparam>
        /// <param name="removeMethod">指定的拥有指定委托类型的移除方法。</param>
        /// <returns><see cref="CallingMethod(object[])"/> 经过指定的拥有指定委托类型的移除方法适配的委托对象。</returns>
        public THandler GetDelegateForCallingMethod<THandler>(Action<THandler> removeMethod)
            where THandler : class
        {
            if (!typeof(Delegate).IsAssignableFrom(typeof(THandler)))
                throw new ArgumentOutOfRangeException(
                    nameof(removeMethod),
                    new InvalidCastException(
                        string.Format("{0} 不能从 {1} 转换。", typeof(Delegate).FullName, typeof(THandler).FullName)
                    )
                );

            return this.GetDelegateForCallingMethod(
                typeof(THandler),
                ((Delegate d) => removeMethod(d as THandler))
            ) as THandler;
        }
        #endregion

        #region PreviewEventInvoke
        /// <summary>
        /// <see cref="EventDrivenFSMTransition"/> 的注册事件引发的事件。在注册事件引发前发生。
        /// </summary>
        public event EventInvokeEventHandler PreviewEventInvoke;

        /// <summary>
        /// 引发 <see cref="PreviewEventInvoke"/> 事件。
        /// </summary>
        /// <param name="e"></param>
        protected internal void OnPreviewEventInvoke(EventInvokeEventArgs e)
        {
            this.PreviewEventInvoke?.Invoke(this, e);
        }
        #endregion

        #region EventInvoke
        /// <summary>
        /// <see cref="EventDrivenFSMTransition"/> 的注册事件引发的事件。在注册事件引发后发生。
        /// </summary>
        public event EventInvokeEventHandler EventInvoke;

        /// <summary>
        /// 引发 <see cref="EventInvoke"/> 事件。
        /// </summary>
        /// <param name="e"></param>
        protected internal void OnEventInvoke(EventInvokeEventArgs e)
        {
            this.EventInvoke?.Invoke(this, e);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

#pragma warning disable 1591
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                #region 调用移除方法
                this.callingMethodDic.Clear();
                this.delegateForCallingMethodDic.Clear();
                foreach (var tuple in this.removeCallingMethodMethods)
                    tuple.Item2(tuple.Item1);
                this.removeCallingMethodMethods.Clear();
                foreach (var tuple in this.removeDelegateForCallingMethodMethods)
                    tuple.Item2(tuple.Item1);
                this.removeDelegateForCallingMethodMethods.Clear();
                #endregion

#if false
                AppDomain.Unload(this.AppDomain); // 卸载应用程序域。
#endif

                disposedValue = true;
            }
        }
#pragma warning restore 1591

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        /// <summary>
        /// <see cref="EventDrivenFSMTransition"/> 终结器。
        /// </summary>
        ~EventDrivenFSMTransition()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
