namespace Diwen.Tiff
{
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static TValue ValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        => dictionary.ContainsKey(key)
            ? dictionary[key]
            : defaultValue;

        public static TValue ValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        => ValueOrDefault(dictionary, key, default(TValue));
    }
}