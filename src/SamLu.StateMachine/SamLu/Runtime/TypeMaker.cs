using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Runtime
{
    public class TypeMaker
    {
        private TypeMaker modelType;
        private object[] typeSegments;

        public TypeMaker Model => this.modelType;

        public object[] Segments
        {
            get
            {
                object[] segments = new object[this.typeSegments.Length];
                Array.Copy(this.typeSegments, segments, segments.Length);
                return segments;
            }
        }

        public bool IsConstructed
        {
            get
            {
                if (!this.isMaked) this.Make();

                if (this.typeSegments.Length == 0)
                    return this.modelType != null && this.modelType.IsConstructed;
                else
                {
                    object first = this.typeSegments.First();
                    if (first is Type type)
                    {
                        if (type.IsGenericParameter) return false;
                        else if (type.IsGenericType && !type.IsConstructedGenericType) return false;
                        else return true;
                    }
                    else if (first is TypeMaker maker)
                        return maker.IsConstructed;
                    else return false;
                }
            }
        }

        public TypeMaker(Type modelType, params object[] typeSegments) :
            this(
                (TypeMaker)(modelType ?? throw new ArgumentNullException(nameof(modelType))),
                typeSegments
            )
        { }
        
        public TypeMaker(TypeMaker modelType, params object[] typeSegments)
        {
            if (modelType == null) throw new ArgumentNullException(nameof(modelType));
            if (typeSegments == null) throw new ArgumentNullException(nameof(typeSegments));

            this.modelType = modelType;
            this.typeSegments = new object[typeSegments.Length];

            object[] genericTypeArguments = modelType.GenericTypeArguments;
            for (int i = 0; i < typeSegments.Length; i++)
            {
                object segment = typeSegments[i];
                if (segment == null) throw new TypeMakeSegmentNullException();

                if (segment is Type type)
                {
                    if (type.IsGenericParameter) throw new TypeMakeInvalidSegmentException(type);

                    this.typeSegments[i] = type;
                }
                else if (segment is TypeMaker maker)
                    this.typeSegments[i] = maker;
                else if (segment is TypeParameterFillin fillin)
                {
                    if ((int)fillin < 0) throw new TypeMakeInvalidSegmentException(fillin);

                    if (fillin == TypeParameterFillin.ModifiedType) this.typeSegments[i] = modelType;
                    else this.typeSegments[i] = genericTypeArguments[(int)fillin - 1];
                }
                else throw new TypeMakeInvalidSegmentException(segment);
            }
        }

        public TypeMaker(object[] typeSegments)
        {
            if (typeSegments == null) throw new ArgumentNullException(nameof(typeSegments));

            this.modelType = null;
            this.typeSegments = new object[typeSegments.Length];
            
            for (int i = 0; i < typeSegments.Length; i++)
            {
                object segment = typeSegments[i];
                if (segment == null) throw new TypeMakeSegmentNullException();

                if (segment is Type type)
                {
                    if (type.IsGenericParameter) throw new TypeMakeInvalidSegmentException(type);

                    this.typeSegments[i] = type;
                }
                else if (segment is TypeMaker maker)
                    this.typeSegments[i] = maker;
                else if (segment is TypeParameterFillin fillin)
                {
                    if ((int)fillin < 0) throw new TypeMakeInvalidSegmentException(fillin);

                    this.typeSegments[i] = fillin;
                }
                else throw new TypeMakeInvalidSegmentException(segment);
            }
        }

        public bool ContainsGenericParameters =>
            this.typeSegments.Any(segment => segment is TypeParameterFillin);
        
        private object[] GenericTypeArguments
        {
            get
            {
                if (!this.isMaked) this.Make();

                if (this.typeSegments.Length == 0)
                    return this.modelType.GenericTypeArguments;
                else
                    return this.typeSegments.OfType<TypeParameterFillin>().Distinct()
                        .Cast<object>().ToArray();
            }
        }

        protected internal IEnumerable<object> ReadFactors(IEnumerator enumerator, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext()) throw new TypeMakeException();
                object segment = enumerator.Current;

                if (segment is Type type)
                {
                    if (type.ContainsGenericParameters)
                    {
                        var factors = this.ReadFactors(enumerator, type.GetGenericArguments().Length).ToArray();
                        if (factors.All(factor => factor is Type))
                        {
                            object result;
                            try
                            {
                                result = type.MakeGenericType(factors.OfType<Type>().ToArray());
                            }
                            catch (Exception)
                            {
                                result = null;
                            }

                            if (result != null) yield return result;
                        }

                        yield return new TypeMaker(factors)
#if true
                            .Make();
#else
                            ;
#endif
                    }
                }
                else if (segment is TypeMaker maker)
                {
                    if (maker.ContainsGenericParameters)
                    {
                        var factors = this.ReadFactors(enumerator, maker.GenericTypeArguments.Length).ToArray();

                        yield return new TypeMaker(factors)
#if true
                            .Make();
#else
                            ;
#endif
                    }
                }

                yield return segment;
            }
        }

        private bool isMaked = false;
        public Type Make()
        {
            object[] factors = this.ReadFactors(this.typeSegments.GetEnumerator()).ToArray();
            if (factors[0] is Type type)
            {
                this.isMaked = true;
                this.typeSegments = factors;

                return type;
            }
            else
                throw new TypeMakeException();
        }

        public Type Make(Type modelType)
        {
            if (modelType == null) throw new ArgumentNullException(nameof(modelType));

            return this.Make(modelType.GenericTypeArguments);
        }

        public Type Make(TypeMaker modelType)
        {
            if (modelType == null) throw new ArgumentNullException(nameof(modelType));

            return this.Make(modelType.GenericTypeArguments);
        }

        protected internal Type Make(object[] genericTypeArguments)
        {
            for (int i = 0; i < this.typeSegments.Length; i++)
            {
                object segment = this.typeSegments[i];
                if (segment is TypeParameterFillin fillin)
                {
                    if (fillin == TypeParameterFillin.ModifiedType) this.typeSegments[i] = this.modelType;
                    else this.typeSegments[i] = genericTypeArguments[(int)fillin - 1];
                }
            }

            return this.Make();
        }

        public static implicit operator TypeMaker(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return new TypeMaker(new object[] { type });
        }
    }
}
