using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

using GameModule.CardSandBox.UnitIntention;

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
        var skillInfo = intention.GetSKill(unitMockInfo.IntentionInfo());
        Debug.Log("skillInfo:" + skillInfo.ToString());
    }
}

