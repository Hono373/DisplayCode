using System;
using UnityEngine;

namespace GameModule.CardSandBox.UnitIntention
{
    [Serializable]
    public class GameConditionExample : IGameCondition
    {
        [SerializeField] int index;
        public void Condition(out int? result)
        {
            result = index;
        }
    }
}
