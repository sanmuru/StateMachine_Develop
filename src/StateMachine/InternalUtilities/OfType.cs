namespace System.Linq;

internal static partial class EnumerableExtension
{
    extension<T>(IEnumerable<T> source)
    {
        public bool AllOfType<TOf>() where TOf : T
        {
            foreach (var item in source)
            {
                if (item is not TOf)
                    return false;
            }
            return true;
        }

        public bool AnyOfType<TOf>() where TOf : T
        {
            foreach (var item in source)
            {
                if (item is TOf)
                    return true;
            }
            return false;
        }

        public IEnumerable<T> NotOfType<TNotOf>() where TNotOf : T
        {
            foreach (var item in source)
            {
                if (item is not TNotOf)
                    yield return item;
            }
        }

        public bool AllNotOfType<TNotOf>() where TNotOf : T
        {
            foreach (var item in source)
            {
                if (item is TNotOf)
                    return false;
            }
            return true;
        }

        public bool AnyNotOfType<TNotOf>() where TNotOf : T
        {
            foreach (var item in source)
            {
                if (item is not TNotOf)
                    return true;
            }
            return false;
        }
    }
}
