using System;

namespace GameModule.CardSandBox.UnitIntention
{
    [Serializable]
    public class IntentionData
    {
        public int[] skillIndexs = new int[2];
        IntentionData() { }
        internal static IntentionData Create(int[] skillIndexs) => new() { skillIndexs = skillIndexs };

        public override string ToString() => $"IntentionData {{ skillIndexs = [{string.Join(", ", skillIndexs)}] }}";
    }
}
