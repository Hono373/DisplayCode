using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInfo
{
    string Key();
    string Desc();
}

public static class InfoExtension
{
    [Serializable]
    public struct Info
    {
        public string key;
        public string name;
        public Info(string key, string name)
        {
            this.key = key;
            this.name = name;
        }
        public override string ToString()
        {
            return $"{key} - {name}";
        }
    }

    public static List<IInfo> GetKeyList<T>() where T : ScriptableObject, IInfo
    {
        List<IInfo> keys = new();
#if UNITY_EDITOR
        string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");

        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null && !string.IsNullOrEmpty(asset.Key()))
            {
                keys.Add(asset);
            }
        }
#endif
        return keys;
    }
    public static List<Info> GetKeyList<T>(Func<T, Info> getInfo) where T : ScriptableObject
    {
        List<Info> keys = new();
#if UNITY_EDITOR
        string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");

        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            var info = getInfo(asset);
            if (asset != null && !string.IsNullOrEmpty(info.key))
            {
                keys.Add(info);
            }
        }
#endif
        return keys;
    }
}
