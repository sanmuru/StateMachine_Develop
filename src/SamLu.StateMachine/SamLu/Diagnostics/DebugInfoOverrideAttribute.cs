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

        private Type proxyType;
        private object[] proxyTypeParameters;

        public object[] DebugInfoParameters { get => this.debugInfoParameters; set => this.debugInfoParameters = value; }
        public string DebugInfoParametersFieldName { get; set; }

        public DebugInfoOverrideAttribute(Type debugInfoType, Type proxyType, params object[] proxyTypeFragments)
        {

        }

        public DebugInfoOverrideAttribute(Type debugInfoType, object[] debugInfoTypeFragments, Type proxyType, object[] proxyTypeFragments)
        {

        }

        public TypeMaker GetDebugInfoTypeMaker()
        {
            object[] segments = new object[this.debugInfoParameters.Length + 1];
            segments[0] = this.debugInfoType;
            this.debugInfoParameters.CopyTo(segments, 1);

            return new TypeMaker(this.debugInfoType, segments);
        }

        public TypeMaker GetProxyTypeMaker()
        {
            object[] segments = new object[this.proxyTypeParameters.Length + 1];
            segments[0] = this.proxyType;
            this.proxyTypeParameters.CopyTo(segments, 1);

            return new TypeMaker(this.proxyType, segments);
        }
    }
}
