using System;
using UnityEngine;

[Serializable]
public class GameConditionExample : IGameCondition
{
    [SerializeField] int index;
    public void Condition(out int? result)
    {
        result = index;
    }
}
