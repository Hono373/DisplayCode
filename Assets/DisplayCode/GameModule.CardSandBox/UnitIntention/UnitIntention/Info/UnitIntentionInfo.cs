using CardSandBoxLibrary;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace GameModule.CardSandBox.UnitIntention
{
    [Serializable]
    public class UnitIntentionInfo : GameNode
    {
        [SerializeField] ConditionBox conditionBox = new();
        [SerializeReference] List<StatusInfo> statusInfos = new();
        public IReadOnlyList<StatusInfo> StatusInfos() => statusInfos;
        public override IReadOnlyList<IWeight> Weights() => statusInfos;
        public override IReadOnlyList<GameNode> Childs() => statusInfos;
        public override ConditionBox ConditionBox() => conditionBox;
        public ISkillInfo Deserialize(IntentionData skillData)
        {
            try
            {
                return statusInfos[skillData.skillIndexs[0]].Deserialize(skillData.skillIndexs[1]);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return NullSkillInfo.Get();
            }
        }
        public int[] GetSkillIndexs()
        {
            var indexs = new int[2];
            try
            {
                GameNode node = this;
                var i = 0;
                while (!node.IsEnd())
                {
                    var childIndex = GetChildIndex();
                    indexs[i] = childIndex;
                    node = node.Childs()[childIndex];
                    i++;
                }
                return indexs;
            }
            catch (Exception e)
            {
                Debug.Log($"[{nameof(UnitIntentionInfo)}]{e.Message}");
                return indexs;
            }
        }
    }
}
