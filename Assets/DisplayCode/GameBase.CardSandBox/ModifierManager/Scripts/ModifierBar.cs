using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifierModule
{
    [JsonProperty] Dictionary<string, Modifier> modifiers = new();
    [JsonIgnore] public IReadOnlyDictionary<string, Modifier> Modifiers => modifiers;
    public event Action<Modifier> OnAdded;
    public event Action<Modifier> OnRemoved;
    public void AddModifier(IModifierOwnerAffected caster, IModifierOwnerAffected target, ModifierCreateInfo createInfo)
    {
        var id = modifiers.Keys.RandomID();
        var modifier = Modifier.Create(id, caster, target, createInfo);
        modifiers[id] = modifier;
        ModifierManager.Register(modifier);
        OnAdded?.Invoke(modifier);
    }
    public bool RemoveBuff(string id)
    {
        if (modifiers.TryGetValue(id, out var modifier))
        {
            modifiers.Remove(id);
            OnRemoved?.Invoke(modifier);
            return true;
        }
        return false;
    }
    public void Deserialize()
    {
        foreach (var modifier in modifiers.Values)
        {
            ModifierManager.Register(modifier);
        }
    }
}