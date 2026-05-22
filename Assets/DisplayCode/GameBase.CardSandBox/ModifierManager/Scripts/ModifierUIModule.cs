using DG.Tweening;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class ModifierUIModule
{
    [JsonIgnore] HashSet<IModifierModuleUI> uiListeners = new();
    public void AddListener(IModifierModuleUI listener)
    {
        if (!uiListeners.Add(listener))
            Debug.Log($"{listener.GetType().Name} already exists");
    }
    public void RemoveListener(IModifierModuleUI listener)
    {
        if (!uiListeners.Remove(listener))
            Debug.Log($"{listener.GetType().Name} does not exist");
    }
    public void OnValueChange(IReadOnlyDictionary<string, Modifier> modifiers, Sequence seq)
    {
        foreach (var listener in uiListeners)
        {
            listener.OnValueChange(modifiers, seq);
        }
    }
}
