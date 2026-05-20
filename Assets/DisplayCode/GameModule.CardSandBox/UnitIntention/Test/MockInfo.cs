using UnityEngine;

namespace GameModule.CardSandBox.UnitIntention
{
    [CreateAssetMenu(fileName = nameof(UnitMockInfo))]
    public class UnitMockInfo : ScriptableObject
    {
        public UnitIntentionInfo IntentionInfo() => intentionInfo;
        [SerializeField] UnitIntentionInfo intentionInfo;
    }
}
