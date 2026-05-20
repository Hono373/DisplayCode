using DG.Tweening;
using GameModule.CardSandBox.UnitIntention;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
public interface IModifierEffectInfo
{

}
public interface IModifierEffectInfo<T> : IModifierEffectInfo
{
    public string GetDescription(Modifier buff);
    public void Invoke(Modifier modifier, T snapshot);
}
public class IntentionDataModifier : IModifierEffectInfo<IntentionData>
{
    public string GetDescription(Modifier buff)
    {
        throw new NotImplementedException();
    }

    public void Invoke(Modifier modifier, IntentionData snapshot)
    {
        throw new NotImplementedException();
    }

    public void Invoke(Modifier modifier, IntentionData snapshot)
    {
        throw new NotImplementedException();
    }
}
public class ModifierEffectData
{
    public Dictionary<string, int> effects = new Dictionary<string, int>();
    public string GetDescription(Modifier buff)
    {
        throw new NotImplementedException();
    }
    public void Invoke(Modifier modifier, IntentionData snapshot)
    {
        throw new NotImplementedException();
    }
    public void Invoke(Modifier modifier, IntentionData snapshot)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class ModifierInfo : ScriptableObject
{
    public const string resourceAffix = "UI_Unit_Modifier_";
    [ShowInInspector] public string key => name.TrimPrefix($"{nameof(ModifierInfo)}_");
    public Sprite img;
    public string modifierName;
    public bool repeatable;
    [SerializeReference] public List<IModifierEffectInfo> modifierEffects = new();
    public static ModifierInfo Get(string key) => AssetLoad.GetSo<ModifierInfo>(key);

    public string GetDescription(Modifier buff)
    {
        var description = string.Empty;
        foreach (var info in modifierEffects)
        {
            description += info.GetDescription(buff);
        }
        return description;
    }
    public void IsActive(Modifier modifier, BuffCreate buffCreate)
    {
        foreach (var info in modifierEffects)
        {
            info.IsActive(modifier, buffCreate);
        }
    }
}
