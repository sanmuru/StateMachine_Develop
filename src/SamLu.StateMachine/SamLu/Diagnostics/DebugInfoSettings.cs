using SamLu.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    public class DebugInfoSettings : Dictionary<TypeMaker, DebugInfoSetting>
    {
        internal sealed class TypeMakerEqualityComparer : EqualityComparer<TypeMaker>
        {
            public override bool Equals(TypeMaker x, TypeMaker y)
            {
                if (object.ReferenceEquals(x, y)) return true;
                else if (x == null | y == null) return false;
                else
                    return (x.Segments ?? new object[0]).SequenceEqual(y.Segments ?? new object[0]);
            }

            public override int GetHashCode(TypeMaker obj)
            {
                if (obj == null || obj.Segments == null) return 0;
                else
                    return obj.Segments
                        .Select(fragment => fragment == null ? 0 : fragment.GetHashCode())
                        .Aggregate(0, ((hc1, hc2) => hc1 ^ hc2));
            }
        }

        public DebugInfoSettings() : base(new TypeMakerEqualityComparer()) { }

        public DebugInfoSettings(int capacity) : base(capacity, new TypeMakerEqualityComparer()) { }

        public DebugInfoSettings(IDictionary<TypeMaker, DebugInfoSetting> dictionary) : base(dictionary, new TypeMakerEqualityComparer()) { }

        protected DebugInfoSettings(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public DebugInfoSetting this[Type key]
        {
            get => base[new TypeMaker(key)];
            set => base[new TypeMaker(key)] = value;
        }

        public void Add(Type key, DebugInfoSetting setting)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            
            base.Add(new TypeMaker(new object[] { key }), setting);
        }

        public bool ContainsKey(Type key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return base.ContainsKey(new TypeMaker(key));
        }

        public bool Remove(Type key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return base.Remove(new TypeMaker(key));
        }

        public bool TryGetValue(Type key, out DebugInfoSetting value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return base.TryGetValue(new TypeMaker(key), out value);
        }
    }
}
