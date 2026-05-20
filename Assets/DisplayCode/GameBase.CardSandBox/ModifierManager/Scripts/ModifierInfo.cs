using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ModifierInfo))]
public class ModifierInfo : ScriptableObject
{
    public const string resourceAffix = "UI_Unit_Modifier_";
    [ShowInInspector] public string key => name.TrimPrefix($"{nameof(ModifierInfo)}_");
    public Sprite img;
    public string modifierName;

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
    public void IsActive(Modifier modifier, ModifierCreateInfo buffCreate)
    {
        foreach (var info in modifierEffects)
        {
            info.IsActive(modifier, buffCreate);
        }
    }
}
