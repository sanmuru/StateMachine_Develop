using SamLu.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    [AttributeUsage(
            AttributeTargets.Assembly |
            AttributeTargets.Module |
            AttributeTargets.Class |
            AttributeTargets.Struct |
            AttributeTargets.Constructor |
            AttributeTargets.Method |
            AttributeTargets.Interface,
        AllowMultiple = false, Inherited = true)]
    public class DebugInfoOverrideAttribute : Attribute
    {
        private Type debugInfoType;
        private object[] debugInfoParameters = null;

        public object[] DebugInfoParameters { get => this.debugInfoParameters; set => this.debugInfoParameters = value; }
        public string DebugInfoParametersFieldName { get; set; }

        public DebugInfoOverrideAttribute(Type debugInfoType, Type proxyType, params object[] proxyTypeFragments)
        {

        }

        public DebugInfoOverrideAttribute(Type debugInfoType, object[] debugInfoTypeFragments, Type proxyType, object[] proxyTypeFragments)
        {

        }

        public TypeMaker GetDebugInfoTypeMaker() { }

        public TypeMaker GetProxyTypeMaker() { }
    }
}
