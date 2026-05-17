using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 名字起的不算好
/// </summary>
[Serializable]
public class SkillEffectInfo : ISkillEffectInfo
{
    [SerializeReference] IEffectInfo effectInfo;
    public IEffectInfo EffectInfo() => effectInfo;
    public IntentionUIData IntentionsData()
    {
        return IntentionUIData.Create(effectInfo.EffectType(), effectInfo.GetValue());
    }
    internal List<IUnit> GetTargetList(IUnit self)
    {
        throw new NotImplementedException();
    }
}
