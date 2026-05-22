using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = nameof(NullEffectInfo))]
public class NullEffectInfo : ScriptableObject, ISkillEffectInfo
{
    static IAssetLoad assetLoad;
    public static NullEffectInfo Get() => assetLoad.Get<NullEffectInfo>(typeof(NullEffectInfo).Name, true);
    [SerializeField] Sprite sprite;
    [SerializeReference] IEffectInfo effectInfo;
    public Sprite Sprite() => sprite;
    public IEffectInfo EffectInfo() => effectInfo;

    public IntentionUIData IntentionsData()
    {
        throw new NotImplementedException();
    }
}
