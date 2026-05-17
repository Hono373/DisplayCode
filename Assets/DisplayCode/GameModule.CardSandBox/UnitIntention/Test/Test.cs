using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] UnitMockInfo unitMockInfo;
    IEnumerator Start()
    {
        yield return AssetLoad.Create();
    }
    [Button("pingAsset")]
    void Ping()
    {
        var sprite = AssetLoad.GetSpriteSync("Block");
        Debug.Log(sprite.rect);
    }
    [Button("获取技能存档与技能信息")]
    public void Invoke()
    {
        var intention = new UnitIntention();
        var index = intention.GetSkillIndexs(unitMockInfo.IntentionInfo());
        var intentionData = IntentionData.Create(index);
        var skillInfo = intention.GetSKill(intentionData, unitMockInfo.IntentionInfo());
        Debug.Log("intentionData:" + intentionData.ToString());
        Debug.Log("skillInfo:" + skillInfo.ToString());
    }
}
