using UnityEngine;

public abstract class IntentionUI
{
    public abstract void Refresh(IntentionUIData data);

    public Sprite GetSprite(IntentionUIData data)
    {
        switch (data.type)
        {
            case BattleEffectType.atk:
                if (!data.value.HasValue)
                {
                    Debug.Log("BattleEffectType.atk value is null");
                    return null;
                }
                if (BattleIconInfo.Get().atkIcons.Count <= (data.value.Value / 10).GreaterThanOrEqual())
                {
                    return BattleIconInfo.Get().atkIcons[^1];
                }
                return BattleIconInfo.Get().atkIcons[data.value.Value / 10];
            default:
                return AssetLoad.GetSpriteSync("Unknown");
        }
    }
}
