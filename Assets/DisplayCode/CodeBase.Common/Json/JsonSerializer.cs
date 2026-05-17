using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public static class JsonSerializer
{
    static void Serialize(object obj, string fullName)
    {
        string path = Path.Combine(Application.persistentDataPath, fullName);
        string jsonData = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        });

        File.WriteAllText(path, jsonData);
        Debug.Log($"{fullName} Save Succeed");
    }
    static T LoadInPersistantDataPath<T>(string fullName) where T : class
    {
        string path = Path.Combine(Application.persistentDataPath, fullName);
        return LoadWithFullPath<T>(path);
    }
    static T LoadWithFullPath<T>(string fullPath) where T : class
    {
        try
        {
            string jsonData = File.ReadAllText(fullPath);
            var data = JsonConvert.DeserializeObject<T>(jsonData, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            Debug.Log($"{fullPath} Load Succeed");
            return data;
        }
        catch (Exception ex)
        {
            Debug.Log($"{ex.Message}");
            Debug.Log($"{fullPath} Load Failed or Missing");
            return null;
        }
    }
}
