using System;
using UnityEngine;
[Serializable]
public class UnitIntention
{
    public ISkillInfo GetSKill(IntentionData intentionData,UnitIntentionInfo Info)
    {
        try
        {
            return Info.StatusInfos()[intentionData.skillIndexs[0]].Deserialize(intentionData.skillIndexs[1]);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return NullSkillInfo.Get();
        }
    }
    public int[] GetSkillIndexs(UnitIntentionInfo info)
    {
        var indexs = new int[2];
        try
        {
            GameNode node = info;
            var i = 0;
            while (!node.IsEnd())
            {
                var childIndex = node.GetChildIndex();
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
