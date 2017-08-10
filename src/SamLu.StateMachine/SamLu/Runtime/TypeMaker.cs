using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Runtime
{
    public class TypeMaker
    {
        private Type[] typeFragments;

        public TypeMaker(Type modelType, params object[] typeFragments)
        {
            if (modelType == null) throw new ArgumentNullException(nameof(modelType));
            if (typeFragments == null) throw new ArgumentNullException(nameof(typeFragments));

            this.typeFragments = new Type[typeFragments.Length];
            for (int i = 0; i < typeFragments.Length; i++)
            {
                if (typeFragments[i] is Type type) this.typeFragments[i] = type;
                else if (typeFragments[i] is TypeParameterFillin fillin)
                {
                    if (fillin == TypeParameterFillin.ModifiedType) this.typeFragments[i] = modelType;
                    else this.typeFragments[i] = modelType.GetGenericArguments()[(int)fillin - 1];
                }
                else throw new NotSupportedException();
            }
        }

        public TypeMaker(params Type[] typeFragments)
        {
            if (typeFragments == null) throw new ArgumentNullException(nameof(typeFragments));

            this.typeFragments = typeFragments;
        }

        public Type Make()
        {
            Stack<Type> typeStack = new Stack<Type>();
            Stack<int> countStack = new Stack<int>();

            foreach (var fragment in typeFragments)
            {
                if (fragment.ContainsGenericParameters)
                {
                    typeStack.Push(fragment);
                    int length = fragment.GetGenericArguments().Length;
                    countStack.Push(length);
                    countStack.Push(length);
                }
                else
                {
                    if (countStack.Count == 0) return fragment;
                    else typeStack.Push(fragment);

                    countStack.Push(countStack.Pop() - 1);

                    while (countStack.Count!=0&& countStack.Peek()==0)
                    {
                        countStack.Pop();
                        int length = countStack.Pop();
                        Type[] types = new Type[length];
                        for (int i = 0; i < length; i++) types[i] = typeStack.Pop();

                        Type type = typeStack.Pop().MakeGenericType(types);
                        if (typeStack.Count == 0 && !type.ContainsGenericParameters) return type;

                        typeStack.Push(type);
                        countStack.Push(countStack.Pop() - 1);
                    }
                }
            }

            return typeStack.Pop();
        }
    }
}
