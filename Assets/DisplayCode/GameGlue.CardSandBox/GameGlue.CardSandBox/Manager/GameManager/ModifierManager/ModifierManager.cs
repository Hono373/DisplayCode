using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
public interface IModifier
{
    string Name();

}
public static class StringExtension
{
    public static string TrimPrefix(this string source, string prefix)
    {
        if (source.StartsWith(prefix))
        {
            return source.Substring(prefix.Length);
        }
        return source;
    }
}

public class ModifierCreator
{
    public ModifierInfo info;
    public string key;
    public int layer;
    public bool forever;
}
public class ModifierManager : IManager
{
    static ModifierManager GetInstance() => GameManager.Get<ModifierManager>();
    public void Init()
    {

    }
    public Dictionary<Type, ModifierEffectDict> dict = new();
    public static void CreateModifierToUnit(IBattleUnit target, ModifierCreator buffCreate, out Sequence createSeq)
    {
        createSeq = SequenceExtension.Create();

        var dict = target.data.modifierDict;
        if (!dict.TryGetValue(buffCreate.info.key, out var result))
        {
            var modifier = result = Modifier.Create(target, buffCreate.info, buffCreate.layer, buffCreate.forever);
            Register(modifier);
            dict.Add(buffCreate.info.key, modifier);
            modifier.SetObj(target);
        }
        else
        {
            result.IsActive(buffCreate);
            result.GetObj().Refresh();
        }

        createSeq.Join(result.CreateSeq());
    }
    static void Register(Modifier modifier)
    {
        var instance = GetInstance();
        foreach (var modifierEffect in modifier.GetInfo().modifierEffects)
        {
            var t = modifierEffect.GetT();
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
            var t = modifierEffect.GetT();

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

public interface IContext
{
}