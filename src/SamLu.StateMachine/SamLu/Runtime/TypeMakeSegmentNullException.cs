using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Runtime
{
    public class TypeMakeSegmentNullException : TypeMakeException
    {
        public TypeMakeSegmentNullException() : this("生成类型时遇到为空的段。") { }
        public TypeMakeSegmentNullException(string message) : base(message) { }
        public TypeMakeSegmentNullException(string message, Exception innerException) : base(message, innerException) { }
        protected TypeMakeSegmentNullException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
