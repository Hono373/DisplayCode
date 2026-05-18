using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 名字起的不算好
/// </summary>
[Serializable]
public class SkillEffectInfo : ISkillEffectInfo
{
    [SerializeField] string desc;
    [SerializeReference] IEffectInfo effectInfo;
    public IEffectInfo EffectInfo() => effectInfo;
    public IntentionUIData IntentionsData()
    {
        return IntentionUIData.Create(effectInfo.EffectType(), effectInfo.GetValue());
    }
    public string Desc()
    {
        var result = effectInfo.GetValue();
        if (result.HasValue)
        {
            return string.Format(desc, result.Value);
        }
        return desc;
    }
}
