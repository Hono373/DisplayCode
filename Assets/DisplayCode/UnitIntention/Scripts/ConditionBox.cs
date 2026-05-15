using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 调用时需要主动try,catch
/// </summary>
[Serializable]
public class ConditionBox
{
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
