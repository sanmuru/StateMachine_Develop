using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Runtime
{
    public class InheritLevel : IEnumerable<Type>
    {
        private HashSet<Type> types;

        public bool IsEmpty => this.types.Count == 0;

        public Type[] Interfaces => this.types.Where(type => type.IsInterface).ToArray();

        public Type[] BaseTypes => this.types.Where(type => !type.IsInterface).ToArray();

        public InheritLevel(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            this.types = new HashSet<Type>() { type };
        }

        public InheritLevel(IEnumerable<Type> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            this.types = new HashSet<Type>(collection.Where(item => item != null));
        }

        /// <summary>
        /// 获取上层继承层次。
        /// </summary>
        /// <returns>比当前继承层次上一层的继承层次。</returns>
        public InheritLevel GetUpperLevel()
        {
            HashSet<Type> upperLevelTypes = new HashSet<Type>();
            foreach (var type in types)
            {
                if (type != typeof(object) && !type.IsInterface && type.BaseType != null)
                    upperLevelTypes.Add(type.BaseType);

                var interfaces = type.GetInterfaces();
                upperLevelTypes.UnionWith(
                    interfaces.Where(i =>
                        interfaces.All(_i =>
                            i != _i || !_i.GetInterfaces().Contains(i)
                        )
                    )
                );
            }
            return new InheritLevel(upperLevelTypes);
        }

        public IEnumerator<Type> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
