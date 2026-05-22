using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
    /// <summary>
    /// 行为栏
    /// 用于存放行为 比如 巡逻 待机 攻击 等
    /// </summary>
    [Serializable]
    public class StatusInfo : GameNode, IWeight
    {
        [SerializeField] string name;
        [SerializeField][MinValue(1)] int weight = 1;
        [SerializeField] ConditionBox conditionBox = new();
        [SerializeReference] List<SkillInfo> skillInfos = new();
        public override IReadOnlyList<IWeight> Weights() => skillInfos;
        public override IReadOnlyList<GameNode> Childs() => skillInfos;
        public override ConditionBox ConditionBox() => conditionBox;
        public SkillInfo Deserialize(int index) => skillInfos[index];
        public int Weight() => weight;
    }

