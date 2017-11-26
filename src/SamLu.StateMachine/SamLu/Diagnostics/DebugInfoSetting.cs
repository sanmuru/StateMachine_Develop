using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    public class DebugInfoSetting
    {
        public static readonly DebugInfoSetting CharDebugInfoSetting = new DebugInfoSetting(typeof(char), typeof(CharDebugInfo));
        public static readonly DebugInfoSetting StringDebugInfoSetting = new DebugInfoSetting(typeof(string), typeof(StringDebugInfo));

        private Type debugInfoType;
        private object[] debugInfoTypeFragments;

        private Type proxyType;
        private object[] proxyTypeFragments;

        private object[] debugInfoParameters;

        public Type DebugInfoType => this.debugInfoType;
        public object[] DebugInfoTypeFragments => this.debugInfoTypeFragments;

        public Type ProxyType => this.proxyType;
        public object[] ProxyTypeFragments => this.proxyTypeFragments;

        public object[] DebugInfoParameters => this.debugInfoParameters;

        /// <summary>
        /// 初始化 <see cref="DebugInfoSetting"/> 类的新实例。
        /// </summary>
        /// <param name="debugInfoType">要获取调试信息的对象的类型。</param>
        /// <param name="proxyType">为 <paramref name="debugInfoType"/> 类型的对象提供调试信息的代理的类型。</param>
        public DebugInfoSetting(Type debugInfoType, Type proxyType) : this(debugInfoType, proxyType, null) { }

        /// <summary>
        /// 初始化 <see cref="DebugInfoSetting"/> 类的新实例。
        /// </summary>
        /// <param name="debugInfoType">要获取调试信息的对象的类型。</param>
        /// <param name="proxyType">为 <paramref name="debugInfoType"/> 类型的对象提供调试信息的代理的类型。</param>
        /// <param name="debugInfoParameters">获取调试信息时的可选参数。</param>
        public DebugInfoSetting(
            Type debugInfoType,
            Type proxyType,
            object[] debugInfoParameters
        ) : this(debugInfoType, null, proxyType, null, debugInfoParameters) { }

        /// <summary>
        /// 初始化 <see cref="DebugInfoSetting"/> 类的新实例。
        /// </summary>
        /// <param name="debugInfoType">要获取调试信息的对象的类型。</param>
        /// <param name="debugInfoTypeFragments">要获取调试信息的对象的类型的组件序列。</param>
        /// <param name="proxyType">为 <paramref name="debugInfoType"/> 类型的对象提供调试信息的代理的类型。</param>
        /// <param name="proxyTypeFragments">为 <paramref name="debugInfoType"/> 类型的对象提供调试信息的代理的类型的组件序列。</param>
        /// <param name="debugInfoParameters">获取调试信息时的可选参数。</param>
        /// <exception cref="ArgumentNullException"><paramref name="debugInfoType"/> 的值为 null 。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="proxyType"/> 的值为 null 。</exception>
        public DebugInfoSetting(
            Type debugInfoType, object[] debugInfoTypeFragments,
            Type proxyType, object[] proxyTypeFragments,
            object[] debugInfoParameters
        )
        {
            if (debugInfoType == null) throw new ArgumentNullException(nameof(debugInfoType));
            if (proxyType == null) throw new ArgumentNullException(nameof(proxyType));

            this.debugInfoType = debugInfoType;
            this.debugInfoTypeFragments = debugInfoTypeFragments;

            this.proxyType = proxyType;
            this.proxyTypeFragments = proxyTypeFragments;

            this.debugInfoParameters = debugInfoParameters;
        }
    }
}
