using CardSandBoxLibrary;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 技能
/// 决定技能的实现
/// </summary>
[Serializable]
public class SkillInfo : GameNode, ISkillInfo, IWeight
{
    public override bool IsEnd() => true;
    [SerializeField] string name;
    [SerializeField][MinValue(1)] int weight = 1;
    [SerializeField] SelectorBox selectorBox;
    [SerializeReference] List<SkillEffectInfo> effectGroupInfo = new();
    public List<SkillEffectInfo> EffectInfos() => effectGroupInfo;
    public override IReadOnlyList<IWeight> Weights() => null;
    public override IReadOnlyList<GameNode> Childs() => null;

    public int Weight() => 1;

    public List<IntentionUIData> GetUIData()
    {
        var result = new List<IntentionUIData>();
        foreach (var effect in effectGroupInfo)
        {
            result.Add(effect.IntentionsData());
        }
        return result;
    }
    public SelectorBox SelectMode() => selectorBox;
}
