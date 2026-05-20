using System;

public interface IModifierEffectInfo
{
    Type EffectType { get; }
    string GetDescription(Modifier buff);
    void IsActive(Modifier modifier, ModifierCreateInfo buffCreate);
}
