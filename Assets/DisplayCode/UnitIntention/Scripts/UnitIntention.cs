using System;
using Sirenix.OdinInspector;
using UnityEngine;
public class TestCase : MonoBehaviour
{
    [SerializeField] UnitIntentionInfo info;
    [Button]
    public void Test()
    {
        var intention = new UnitIntention();
        var index = intention.GetSkillIndexs(info);
        var intentionData = IntentionData.Create(index);
        var skill = intention.GetSKill(intentionData);
    }

    [Serializable]
    public class UnitIntention
    {
        IntentionData intentionData;
        UnitIntentionInfo Info() => null;
        ILog Log() => null;
        IAssetLoad Asset() => null;
        public ISkillInfo GetSKill(IntentionData intentionData)
        {
            try
            {
                return Info().StatusInfos()[intentionData.skillIndexs[0]].Deserialize(intentionData.skillIndexs[1]);
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
                while (!info.IsEnd())
                {
                    var childIndex = info.GetChildIndex();
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