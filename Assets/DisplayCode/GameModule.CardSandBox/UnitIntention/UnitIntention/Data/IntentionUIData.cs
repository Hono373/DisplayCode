using UnityEngine;

public class IntentionUIData
{
    IAssetLoad assetLoad;
    public BattleEffectType type;
    public int? value;
    IntentionUIData() { }
    public static IntentionUIData Create(BattleEffectType sprite, int? value)
    {
        var data = new IntentionUIData();
        data.type = sprite;
        data.value = value;
        return data;
    }
    public Sprite GetSprite()
    {
        switch (type)
        {
            case BattleEffectType.atk:
                if (!value.HasValue)
                {
                    Debug.Log("BattleEffectType.atk value is null");
                    return null;
                }
                if (BattleIconInfo.Get().atkIcons.Count <= (value.Value / 10).GreaterThanOrEqual())
                {
                    return BattleIconInfo.Get().atkIcons[^1];
                }
                return BattleIconInfo.Get().atkIcons[value.Value / 10];
            default:
                return assetLoad.GetSpriteSync("Unknown");
        }
    }
}

