using SamLu.Runtime;
using SamLu.StateMachine.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    public static class DebugInfo
    {
        public static readonly DebugInfoSettings Settings = new DebugInfoSettings()
        {
            { typeof(char), DebugInfoSetting.CharDebugInfoSetting },
            { typeof(string), DebugInfoSetting.StringDebugInfoSetting }
        };

        public static string GetDebugInfo<T>(this T t, params object[] args)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));

            Type tType = t.GetType();
            object[] attributes = null;
            DebugInfoProxyAttribute proxyAttr = null;
            DebugInfoOverrideAttribute overrideAttr = null;
            int typeDeductionWeight = int.MaxValue;

            // 在调用堆栈的所有方法的自定义特性中查找 DebugInfoOverrideAttribute 。
            // 记为第一类型推断序列。
            TypeDeduction<DebugInfoOverrideAttribute> deduction = new TypeDeduction<DebugInfoOverrideAttribute>(
                new StackTrace() // 获取堆栈跟踪。
                    .GetFrames() // 获取所有堆栈帧。
                    .Select(frame =>
                        new TypeDeductionLevel<DebugInfoOverrideAttribute>(
                            frame.GetMethod() // 获取调用的方法。
                                .GetCustomAttributes<DebugInfoOverrideAttribute>(false)
                                .ToDictionary(
                                    (attr => attr),
                                    (attr => TypeDeduction<DebugInfoOverrideAttribute>.GetTypeDeductionWeight(attr.GetDebugInfoTypeMaker(), tType))
                                )
                        )
                    )
            );

            // 逐级向上追溯继承级，查找 DebugInfoProxyAttribute 和 DebugInfoOverrideAttribute 。
            for (
                InheritLevel level = new InheritLevel(tType);
                (proxyAttr == null || overrideAttr == null) && !level.IsEmpty;
                level = level.GetUpperLevel())
            {
                // 按类继承方式查找。
                Type baseType = level.BaseTypes.FirstOrDefault();
                if (baseType != null)
                {
                    attributes = baseType.GetCustomAttributes(false);
                    proxyAttr = proxyAttr ??
                        attributes.OfType<DebugInfoProxyAttribute>()
                            .FirstOrDefault();
                    // 记为第二类型推断序列。
                    deduction.Add(new TypeDeductionLevel<DebugInfoOverrideAttribute>(
                        attributes.OfType<DebugInfoOverrideAttribute>()
                            .ToDictionary(
                                (attr => attr),
                                (attr => TypeDeduction<DebugInfoOverrideAttribute>.GetTypeDeductionWeight(attr.GetDebugInfoTypeMaker(), tType))
                            )
                    ));
                    attributes = null;
                }

                // 按接口实现方式查找。
                attributes = level.Interfaces
                    .SelectMany(i => i.GetCustomAttributes(false))
                    .ToArray();
                proxyAttr = proxyAttr ??
                    attributes.OfType<DebugInfoProxyAttribute>()
                        .FirstOrDefault();
                // 记为第三类型推断序列。
                deduction.Add(new TypeDeductionLevel<DebugInfoOverrideAttribute>(
                    attributes.OfType<DebugInfoOverrideAttribute>()
                        .ToDictionary(
                            (attr => attr),
                            (attr => TypeDeduction<DebugInfoOverrideAttribute>.GetTypeDeductionWeight(attr.GetDebugInfoTypeMaker(), tType))
                        )
                ));
                attributes = null;
            }

            // 在类型所在的模块中查找。
            attributes = tType.Module.GetCustomAttributes(false);
            proxyAttr = proxyAttr ??
                attributes.OfType<DebugInfoProxyAttribute>()
                    .FirstOrDefault();
            // 记为第四类型推断序列。
            deduction.Add(new TypeDeductionLevel<DebugInfoOverrideAttribute>(
                attributes.OfType<DebugInfoOverrideAttribute>()
                    .ToDictionary(
                        (attr => attr),
                        (attr => TypeDeduction<DebugInfoOverrideAttribute>.GetTypeDeductionWeight(attr.GetDebugInfoTypeMaker(), tType))
                    )
            ));
            attributes = null;

            // 在类型所在的程序集中查找。
            attributes = tType.Assembly.GetCustomAttributes(false);
            proxyAttr = proxyAttr ??
                attributes.OfType<DebugInfoProxyAttribute>()
                    .FirstOrDefault();
            // 记为第四类型推断序列。
            deduction.Add(new TypeDeductionLevel<DebugInfoOverrideAttribute>(
                attributes.OfType<DebugInfoOverrideAttribute>()
                    .ToDictionary(
                        (attr => attr),
                        (attr => TypeDeduction<DebugInfoOverrideAttribute>.GetTypeDeductionWeight(attr.GetDebugInfoTypeMaker(), tType))
                    )
            ));
            attributes = null;

            // 推断最适的 DebugInfoOverrideAttribute 。
            var deductionResult = deduction.Merge();

            Type proxyType;
            DebugInfoOverrideAttribute[] overrideAttrs = deductionResult.FirstOrDefault()?.ToArray();
            if (overrideAttrs != null && overrideAttrs.Length == 1)
                overrideAttr = overrideAttrs[0];
            else
            {
                // 多个推断结果，无法明确，舍弃。
                //overrideAttr = null;
            }
            if (overrideAttr == null)
            {
                if (proxyAttr == null)
                {
                    if (DebugInfo.Settings.TryGetValue(tType, out DebugInfoSetting setting))
                    {
                        // 通过 DebugInfoSetting 获取调试信息字符串。
                        throw new NotImplementedException();
                    }
                    else
                        // 现有信息不足以获取调试信息字符串。
                        return t.ToString();
                }
                else
                    proxyType = proxyAttr.MakeProxyType(tType);
            }
            else
                // 通过 overrideAttr 生成 proxyType;
                proxyType = (proxyType = null) ?? throw new NotImplementedException();

            return DebugInfo.GetDebugInfoInternal(t, tType, proxyType);
        }

        public static string GetDebugInfo<T>(this T t, Type proxyType, params object[] args)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            if (proxyType == null) throw new ArgumentNullException(nameof(proxyType));

            return DebugInfo.GetDebugInfoInternal(t, proxyType, args);
        }

        private static string GetDebugInfoInternal<T>(T t, Type proxyType, params object[] args)
        {
            Type tType = t.GetType();
            // 获取 proxyType 的构造函数。
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

            // 创建 proxyType 的实例。
            var proxy = Activator.CreateInstance(proxyType, new object[] { t, args });

            if (typeof(IDebugInfo).IsAssignableFrom(proxyType))
            {
                // 如果 proxyType 实现 IDebugInfo 。
                IDebugInfo debugInfo = (IDebugInfo)proxy;
                return debugInfo.DebugInfo;
            }
            else
            {
                // 如果 proxyType 实现约束：含有名为 DebugInfo 的无参数属性。
                var property = proxyType.GetProperty("DebugInfo");
                if (property != null && !property.GetMethod.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(Type.EmptyTypes))
                    throw new NotSupportedException($"{proxyType} 不支持获取 {typeof(T)} 调试信息。");
                object debugInfo = property.GetValue(proxy);
                return debugInfo == null ? string.Empty : debugInfo.ToString();
            }
        }
    }
}
