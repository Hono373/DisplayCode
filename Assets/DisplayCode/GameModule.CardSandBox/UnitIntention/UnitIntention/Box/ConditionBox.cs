using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ConditionBox
{
    [InfoBox("通过设置条件，获取子节点索引")]
    [SerializeReference] List<IGameCondition> rules = new();
    public int? Result()
    {
        foreach (var rule in rules)
        {
            rule.Condition(out var result);
            if (result.HasValue)
                return result.Value;
        }
        return null;
    }
}
