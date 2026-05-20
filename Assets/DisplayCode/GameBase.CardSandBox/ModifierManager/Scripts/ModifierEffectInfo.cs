using System;

public abstract class ModifierEffectInfo<T> : IModifierEffectInfo where T : IContext
{
    public string desc;
    public Type EffectType => typeof(T);
    public abstract string GetDescription(Modifier buff);
    public abstract void Invoke(Modifier modifier, T snapshot);
    public virtual void IsActive(Modifier modifier, ModifierCreateInfo buffCreate)
    {
        modifier.data.layer += buffCreate.layer;
        modifier.data.forever = buffCreate.forever;
    }
}
