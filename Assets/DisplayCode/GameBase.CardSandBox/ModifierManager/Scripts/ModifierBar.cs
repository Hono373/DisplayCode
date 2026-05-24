using DG.Tweening;
using Newtonsoft.Json;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ModifierModule
{
    [JsonProperty] Dictionary<string, Modifier> modifiers = new();
    [SerializeField] ModifierUIModule uiModule = new();
    public void RegisterUI(IModifierModuleUI ui) => uiModule.AddListener(ui);
    public void UnregisterUI(IModifierModuleUI ui) => uiModule.RemoveListener(ui);
    public void AddModifier(IModifierAffected caster, IModifierAffected target, ModifierCreateInfo createInfo, Sequence seq)
    {
        var id = modifiers.Keys.RandomID();
        var modifier = Modifier.Create(id, caster, target, createInfo);
        modifiers[id] = modifier;
        ModifierManager.Register(modifier);

        uiModule.OnValueChange(modifiers, seq);
    }
    public void RemoveModifier(Modifier modifier, Sequence seq)
    {
        var id = modifier.data.id;
        if (modifiers.Remove(id))
        {
            ModifierManager.Unregister(modifier);
            uiModule.OnValueChange(modifiers, seq);
        }
        else
        {
            Debug.Log($"!modifiers.Contains{modifier.GetInfo().name}");
        }
    }
    public void Deserialize(Sequence seq)
    {
        foreach (var modifier in modifiers.Values)
        {
            ModifierManager.Register(modifier);
        }
        uiModule.OnValueChange(modifiers, seq);
    }
}
