namespace Goop
{
    using System;
    using System.Collections.Generic;

    public static class DictionaryEx
    {
        public static bool TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Action<TValue> found)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                found(value);
                return true;
            }

            return false;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.TryGetValue(key, out TValue found))
            {
                return found;
            }

            dictionary.Add(key, value);

            return value;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            if (dictionary.TryGetValue(key, out TValue value) == false)
            {
                value = factory(key);
                dictionary.Add(key, value);
            }

            return value;
        }
    }
}
