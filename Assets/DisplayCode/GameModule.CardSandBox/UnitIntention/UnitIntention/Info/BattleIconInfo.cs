using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(BattleIconInfo))]
public class BattleIconInfo : ScriptableObject
{
    static IAssetLoad assetLoad;
    public List<Sprite> atkIcons;
    public List<Sprite> shieldIcons;
    public List<Sprite> healIcons;
    public Sprite sleepIcons;
    public Sprite debuffIcons;
    public Sprite buffIcons;
    public Sprite unknowIcons;

    internal static BattleIconInfo Get()
    {
        return assetLoad.GetSo<BattleIconInfo>(nameof(BattleIconInfo), true);
    }
}

