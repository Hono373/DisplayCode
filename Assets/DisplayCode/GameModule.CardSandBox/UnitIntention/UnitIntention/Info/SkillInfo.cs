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
        [SerializeField] SelectorBox selectorBox = new();
        [SerializeReference] List<SkillEffectInfo> skillEffectInfo = new();
        public List<SkillEffectInfo> EffectInfos() => skillEffectInfo;
        public override IReadOnlyList<IWeight> Weights() => null;
        public override IReadOnlyList<GameNode> Childs() => null;
        public override ConditionBox ConditionBox() => null;
        public int Weight() => 1;
        public string Desc()
        {
            var result = new List<string>();
            foreach (var effect in skillEffectInfo)
            {
                result.Add(effect.Desc());
            }
            return string.Join("\n", result);
        }
        public List<IntentionUIData> GetUIData()
        {
            var result = new List<IntentionUIData>();
            foreach (var effect in skillEffectInfo)
            {
                result.Add(effect.IntentionsData());
            }
            return result;
        }
        public SelectorBox SelectMode() => selectorBox;
    }

