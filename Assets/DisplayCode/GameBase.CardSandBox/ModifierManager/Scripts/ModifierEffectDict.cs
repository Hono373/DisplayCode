using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ModifierEffectDict
{
    [ShowInInspector] Dictionary<string, ModifierEffect> dict = new();
    public Dictionary<string, ModifierEffect> Dict => dict;
    ModifierEffectDict() { }
    public static ModifierEffectDict Create()
    {
        var query = new ModifierEffectDict();
        return query;
    }
    public ModifierEffect this[string key] => dict[key];
    public void Add(ModifierEffect effect)
    {
        var id = effect.modifier.data.id;
        if (dict.ContainsKey(id))
        {
            Debug.Log("dict.ContainsKey(id)");
            return;
        }
        dict[id] = effect;
    }
    public void Remove(string id)
    {
        dict.Remove(id);
    }
}

