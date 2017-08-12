using SamLu.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple =false, Inherited = true)]
    public class DebugInfoProxyAttribute : Attribute
    {
        private Type proxyType;
        private object[] typeFragments;
        
        public DebugInfoProxyAttribute(Type proxyType, TypeParameterFillin[] fillins) : this(proxyType, fillins.Cast<object>().ToArray()) { }

        public DebugInfoProxyAttribute(Type proxyType, params object[] typeFragments)
        {
            if (proxyType == null) throw new ArgumentNullException(nameof(proxyType));
            if (typeFragments == null) throw new ArgumentNullException(nameof(typeFragments));

            if (!typeof(IDebugInfo).IsAssignableFrom(proxyType))
            {
                var debuginfo_Property = proxyType.GetProperty("DebugInfo");
                if (debuginfo_Property?.PropertyType != typeof(string)) throw new NotSupportedException($"{proxyType} 不支持获取调试信息。");
            }

            this.proxyType = proxyType;
            this.typeFragments = typeFragments;
        }

        public Type MakeProxyType(Type modelType)
        {
            object[] typeFragments = new object[this.typeFragments.Length + 1];
            typeFragments[0] = this.proxyType;
            this.typeFragments.CopyTo(typeFragments, 1);

            return new TypeMaker(modelType, typeFragments).Make();
        }
    }
}
