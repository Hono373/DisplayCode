using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
public class ModifierManager
{
    private static readonly object _lock = new();
    private static ModifierManager _instance;
    public static ModifierManager GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                _instance ??= new ModifierManager();
            }
        }
        return _instance;
    }
    public Dictionary<Type, ModifierEffectDict> dict = new();
    public static void Register(Modifier modifier)
    {
        var instance = GetInstance();
        foreach (var modifierEffect in modifier.GetInfo().modifierEffects)
        {
            var t = modifierEffect.EffectType;
            if (!instance.dict.ContainsKey(t))
            {
                instance.dict[t] = ModifierEffectDict.Create();
            }
            var effect = ModifierEffect.Create(modifier, modifierEffect);
            instance.dict[t].Add(effect);
        }
    }
    public static void Unregister(Modifier modifier)
    {
        var instance = GetInstance();
        foreach (var modifierEffect in modifier.GetInfo().modifierEffects)
        {
            var t = modifierEffect.EffectType;

            if (instance.dict.TryGetValue(t, out var effectDict))
            {
                effectDict.Remove(modifier.data.id);
            }
            else
            {
                Debug.Log($"未找到类型为{t.Name}的修饰符字典，无需执行注销操作");
            }
        }
    }
    public static T Send<T>(T snapshot) where T : IContext
    {
        var instance = GetInstance();
        if (!instance.dict.TryGetValue(typeof(T), out var effectDict))
        {
            effectDict = ModifierEffectDict.Create();
            instance.dict[typeof(T)] = effectDict;
        }
        foreach (var modifierEffect in effectDict.Dict.Values)
        {
            modifierEffect.Invoke(snapshot);
        }
        return snapshot;
    }
}
