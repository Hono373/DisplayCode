using System;

[Serializable]
public class IntentionData
{
    public int[] skillIndexs = new int[2];
    IntentionData() { }
    internal static IntentionData Create(int[] skillIndexs) => new() { skillIndexs = skillIndexs };
}