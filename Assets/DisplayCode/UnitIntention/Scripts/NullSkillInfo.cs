using System;
using System.Collections.Generic;
using UnityEngine;
public interface IAssetLoad
{
    T Get<T>(string name);
    T Get<T>(string name, bool v);
    T GetSo<T>(string v1, bool v2);
    Sprite GetSpriteSync(string v);
}

[Serializable]
[CreateAssetMenu(fileName = nameof(NullSkillInfo))]
public class NullSkillInfo : ScriptableObject, ISkillInfo
{
    static IAssetLoad assetLoad;
    public static NullSkillInfo Get() => assetLoad.Get<NullSkillInfo>(typeof(NullSkillInfo).Name);
    [SerializeField] SelectorBox selectorBox;
    public SelectorBox SelectMode() => selectorBox;
    [SerializeReference] List<SkillEffectInfo> effectInfos = new();
    public List<SkillEffectInfo> EffectInfos() => effectInfos;
    public List<IntentionUIData> GetUIData()
    {
        return new List<IntentionUIData>() { IntentionUIData.Create(BattleEffectType.unknow, null) };
    }
}
