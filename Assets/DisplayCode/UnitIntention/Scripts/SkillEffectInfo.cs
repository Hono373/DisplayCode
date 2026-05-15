using System;
using System.Collections.Generic;
using UnityEngine;
public interface IEffectInfo
{
    BattleEffectType EffectType();
    List<IBattleUnit> GetTargetList(IBattleUnit self);
    void Apply(IEffectContext context, List<IBattleUnit> targetList, out Sequence seq);
    string Desc(IEffectContext context);
    string ChoiceDesc(IEffectContext context, IBattleUnit target);
    int? GetValue();
}

public class Sequence
{
}

public interface IEffectContext
{
}

public interface ISkillEffectInfo
{
    public IEffectInfo EffectInfo();
    public IntentionUIData IntentionsData();
}
/// <summary>
/// 技能表现
/// 封装了技能效果，技能图标与技能索敌模式
/// </summary>
public enum BattleEffectType
{
    atk, shield, heal, sleep, debuff, buff, unknow
}
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
    internal List<IBattleUnit> GetTargetList(IBattleUnit self)
    {
        throw new NotImplementedException();
    }
}
public class IBattleUnit
{

}