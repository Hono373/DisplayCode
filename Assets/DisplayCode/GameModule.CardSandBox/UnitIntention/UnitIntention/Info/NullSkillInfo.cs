using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = nameof(NullSkillInfo))]
public class NullSkillInfo : ScriptableObject, ISkillInfo
{
    public static NullSkillInfo Get() => AssetLoad.GetObjectSync<NullSkillInfo>(typeof(NullSkillInfo).Name);
    [SerializeField] SelectorBox selectorBox;
    public SelectorBox SelectMode() => selectorBox;
    [SerializeReference] List<SkillEffectInfo> effectInfos = new();
    public List<SkillEffectInfo> EffectInfos() => effectInfos;
    public List<IntentionUIData> GetUIData()
    {
        return new List<IntentionUIData>() { IntentionUIData.Create(BattleEffectType.unknow, null) };
    }
}
