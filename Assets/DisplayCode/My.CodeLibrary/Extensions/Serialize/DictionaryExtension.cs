using System;
using System.Collections.Generic;

public static class DictionaryExtension
{
    public static void Add<TKey, TValue>(this IDictionary<TKey, List<TValue>> dict, TKey key, TValue tValue)
    {
        if (!dict.TryGetValue(key, out List<TValue> list) || list == null)
        {
            dict[key] = list = new();
        }
        list.Add(tValue);
    }

    public static void Add<TKey, TValue>(this IDictionary<TKey, List<TValue>> dict, Func<List<string>> getKeyList, TKey key, TValue tValue)
    {
        dict.Add(getKeyList, key, tValue);
    }
}
