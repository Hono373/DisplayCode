using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


public class IntentionUIData
{
    IAssetLoad assetLoad;
    public BattleEffectType type;
    public int? value;
    IntentionUIData() { }
    public static IntentionUIData Create(BattleEffectType sprite, int? value)
    {
        var data = new IntentionUIData();
        data.type = sprite;
        data.value = value;
        return data;
    }
    public Sprite GetSprite()
    {
        switch (type)
        {
            case BattleEffectType.atk:
                if (!value.HasValue)
                {
                    Debug.Log("BattleEffectType.atk value is null");
                    return null;
                }
                if (BattleIcon.Get().atkIcons.Count <= (value.Value / 10).GreaterThanOrEqual())
                {
                    return BattleIcon.Get().atkIcons[^1];
                }
                return BattleIcon.Get().atkIcons[value.Value / 10];
            default:
                return assetLoad.GetSpriteSync("Unknown");
        }
    }
}
[CreateAssetMenu(fileName = nameof(BattleIcon))]
public class BattleIcon : ScriptableObject
{
    static IAssetLoad assetLoad;
    public List<Sprite> atkIcons;
    public List<Sprite> shieldIcons;
    public List<Sprite> healIcons;
    public Sprite sleepIcons;
    public Sprite debuffIcons;
    public Sprite buffIcons;
    public Sprite unknowIcons;

    internal static BattleIcon Get()
    {
        return assetLoad.GetSo<BattleIcon>(nameof(BattleIcon), true);
    }
}
public class BattleEffectTypeClass
{
    public BattleEffectType sprite;

}
public interface ISkillInfo
{
    SelectorBox SelectMode();
    List<SkillEffectInfo> EffectInfos();
    List<IntentionUIData> GetUIData();
}
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
    public List<SkillEffectInfo> EffectInfos() => effectGroupInfo;
    public SelectorBox SelectMode() => selectorBox;
}
public enum SwitchMode
{
    Single,
    Multiple,
    All,
}
[Serializable]
public class SelectorBox
{
    [SerializeReference] public List<IUnitSelector> selector = new();
    public void Trim(List<IBattleUnit> targetList)
    {
        foreach (var item in selector)
        {
            item.Trim(targetList);
        }
    }
}
public interface IUnitSelector
{
    void Trim(List<IBattleUnit> targetList);
}
public class SingleSelector_Random : IUnitSelector
{
    public void Trim(List<IBattleUnit> targetList)
    {
        var select = targetList[targetList.Count.GetRandom()];
        targetList.Clear();
        targetList.Add(select);
    }
}
public class SingleSelector_Weight : IUnitSelector
{
    public void Trim(List<IBattleUnit> targetList)
    {
        var select = targetList[targetList.Count.GetRandom()];
        targetList.Clear();
        targetList.Add(select);
    }
}
public class MultipleSelector_Random : IUnitSelector
{
    [SerializeField] int count;
    public void Trim(List<IBattleUnit> targetList)
    {
        if (targetList == null || targetList.Count <= count)
            return;

        // 随机打乱前 count 个位置
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, targetList.Count);
            // 交换元素
            IBattleUnit temp = targetList[i];
            targetList[i] = targetList[randomIndex];
            targetList[randomIndex] = temp;
        }

        // 移除剩余元素
        targetList.RemoveRange(count, targetList.Count - count);
    }
}
public class MultipleSelector_Weight : IUnitSelector
{
    [SerializeField] int count;
    public void Trim(List<IBattleUnit> targetList)
    {
        if (targetList == null || targetList.Count <= count)
            return;

        // 随机打乱前 count 个位置
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, targetList.Count);
            // 交换元素
            IBattleUnit temp = targetList[i];
            targetList[i] = targetList[randomIndex];
            targetList[randomIndex] = temp;
        }

        // 移除剩余元素
        targetList.RemoveRange(count, targetList.Count - count);
    }
}

