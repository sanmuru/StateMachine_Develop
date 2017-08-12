using SamLu.StateMachine.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    public static class DebugInfo
    {
        public static string GetDebugInfo<T>(this T t, params object[] args)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));

            Type tType = t.GetType();
            var attributes =
                tType.GetCustomAttributes(typeof(DebugInfoProxyAttribute), true)
                    .OfType<DebugInfoProxyAttribute>()
                    .Concat(
                        tType.GetInterfaces()
                            .OrderBy(
                                (tInterface => tInterface),
                                Comparer<Type>.Create(
                                    (ti1, ti2) =>
                                    {
                                        if (ti1 == ti2) return 0;
                                        else if (ti1.GetInterfaces().Contains(ti2)) return -1;
                                        else if (ti2.GetInterfaces().Contains(ti1)) return 1;
                                        else return 0;
                                    }
                                )
                            )
                            .SelectMany(tInterface =>
                                tInterface.GetCustomAttributes(typeof(DebugInfoProxyAttribute), true)
                                    .OfType<DebugInfoProxyAttribute>()
                            )
                    )
                    .ToArray();
            if (attributes == null | attributes.Length == 0) return t.ToString();
            else
            { // 支持 DebugInfoProxy
                var proxyType = attributes[0].MakeProxyType(tType);
                var constructor =
                    proxyType.GetConstructors()
                        .FirstOrDefault(ci =>
                        {
                            var parameterTypes = ci.GetParameters().Select(pi => pi.ParameterType).ToArray();
                            return
                                parameterTypes.Length == 2 &&
                                    (
                                        parameterTypes[0].IsAssignableFrom(tType) &&
                                        parameterTypes[1] == typeof(object[])
                                    );
                        });
                if (constructor == null) throw new NotSupportedException($"{proxyType} 不支持获取 {typeof(T)} 调试信息。");

                var proxy = Activator.CreateInstance(proxyType, new object[] { t, args });

                if (typeof(IDebugInfo).IsAssignableFrom(proxyType))
                {
                    IDebugInfo debugInfo = (IDebugInfo)proxy;
                    return debugInfo.DebugInfo;
                }
                else
                {
                    var property = proxyType.GetProperty("DebugInfo");
                    return property.GetValue(proxy) as string;
                }
            }
        }
    }
}
