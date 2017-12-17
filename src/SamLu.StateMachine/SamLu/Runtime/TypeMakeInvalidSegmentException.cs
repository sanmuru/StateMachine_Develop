using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Runtime
{
    public class TypeMakeInvalidSegmentException : TypeMakeException
    {
        private object segment;
        public object Segment => this.segment;

        public TypeMakeInvalidSegmentException() : this(default(object)) { }
        public TypeMakeInvalidSegmentException(object segment) : this(segment, $"生成类型时遇到不合法的段。{(segment == null ? string.Empty : $"{Environment.NewLine}段：\"{segment}\"。")}") { }
        public TypeMakeInvalidSegmentException(string message) : this(default(object), message) { }
        public TypeMakeInvalidSegmentException(object segment, string message) : base(message) => this.segment = segment;
        public TypeMakeInvalidSegmentException(string message, Exception innerException) : this(default(object), message, innerException) { }
        public TypeMakeInvalidSegmentException(object segment, string message, Exception innerException) : base(message, innerException) => this.segment = segment;
        protected TypeMakeInvalidSegmentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context
        )
        { }
    }
}
