using SamLu.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    [AttributeUsage(
            AttributeTargets.Class |
            AttributeTargets.Struct |
            AttributeTargets.Enum |
            AttributeTargets.Interface |
            AttributeTargets.Delegate,
        AllowMultiple = false, Inherited = true)]
    public class DebugInfoProxyAttribute : Attribute
    {
        private Type proxyType;
        private object[] typeSegments;

        public DebugInfoProxyAttribute(Type proxyType, TypeParameterFillin[] fillins) : this(proxyType, fillins.Cast<object>().ToArray()) { }

        public DebugInfoProxyAttribute(Type proxyType, params object[] typeSegments)
        {
            if (proxyType == null) throw new ArgumentNullException(nameof(proxyType));
            if (typeSegments == null) throw new ArgumentNullException(nameof(typeSegments));

            if (proxyType.IsAbstract)
                throw new NotSupportedException($"{proxyType} 不支持获取调试信息。");
            else if (!typeof(IDebugInfo).IsAssignableFrom(proxyType))
            {
                var debuginfo_Property = proxyType.GetProperty("DebugInfo");
                if (debuginfo_Property?.PropertyType != typeof(string))
                    throw new NotSupportedException($"{proxyType} 不支持获取调试信息。");
            }

            this.proxyType = proxyType;
            this.typeSegments = typeSegments;
        }

        public Type MakeProxyType(Type modelType)
        {
            return this.GetProxyTypeMaker().Make(modelType);
        }

        public TypeMaker GetProxyTypeMaker()
        {
            object[] typeSegments = new object[this.typeSegments.Length + 1];
            typeSegments[0] = this.proxyType;
            this.typeSegments.CopyTo(typeSegments, 1);

            return new TypeMaker(typeSegments);
        }
    }
}
