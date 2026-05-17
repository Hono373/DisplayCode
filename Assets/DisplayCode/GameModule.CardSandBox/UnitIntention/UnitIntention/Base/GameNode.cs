using CardSandBoxLibrary;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 条件权重混合节点
/// </summary>
public abstract class GameNode
{
    public virtual bool IsEnd() => false;
    public abstract IReadOnlyList<IWeight> Weights();
    public abstract IReadOnlyList<GameNode> Childs();
    [SerializeReference] ConditionBox conditionBox = new();
    public int GetChildIndex()
    {
        var result = conditionBox.Result();
        if (!result.HasValue)
        {
            var weights = Weights();
            if (weights?.Count == 0)
                throw new("Weights未配置");
            result = new WeightedRandomSelector(weights).SelectIndex();
        }
        if (!result.HasValue)
            throw new("WeightedRandomSelector(Weights).SelectIndex() faild");
        return result.Value;
    }
}
