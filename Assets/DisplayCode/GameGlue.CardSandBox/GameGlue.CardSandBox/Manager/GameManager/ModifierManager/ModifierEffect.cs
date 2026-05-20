using System;

[Serializable]
public class ModifierEffect
{
    public Modifier modifier;
    public IModifierEffectInfo info;
    public static ModifierEffect Create(Modifier modifier, IModifierEffectInfo info)
    {
        var effect = new ModifierEffect();
        effect.modifier = modifier;
        effect.info = info;
        return effect;
    }
    public void Invoke<T>(T snapshot) where T : IContext
    {
        if (info is ModifierEffectInfo<T> effect)
        {
            effect.Invoke(modifier, snapshot);
        }
    }
}

