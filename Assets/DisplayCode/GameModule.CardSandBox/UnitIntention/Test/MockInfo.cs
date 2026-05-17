using UnityEngine;
[CreateAssetMenu(fileName = nameof(UnitMockInfo))]
public class UnitMockInfo : ScriptableObject
{
    public UnitIntentionInfo IntentionInfo() => intentionInfo;
    [SerializeField] UnitIntentionInfo intentionInfo;
}
