using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Runtime
{
    public class TypeMakeException : Exception
    {
        public TypeMakeException() : this("生成类型时发生错误。") { }
        public TypeMakeException(string message) : base(message) { }
        public TypeMakeException(string message, Exception innerException) : base(message, innerException) { }
        protected TypeMakeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context
        ) : base(info, context) { }
    }
}
