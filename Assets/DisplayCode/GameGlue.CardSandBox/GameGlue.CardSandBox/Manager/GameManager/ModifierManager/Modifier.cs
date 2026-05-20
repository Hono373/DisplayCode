using DG.Tweening;
using Newtonsoft.Json;
using System;
public interface IModifierUnitObj
{
    IModifierUnitObj InstantiateObj();
}
[Serializable]
public class Modifier
{
    public ModifierData data;
    [JsonIgnore] IModifierUnitObj obj;
    public static Modifier Create(IBattleUnit target, ModifierInfo info, int layer, bool forever)
    {
        var modifier = new Modifier();
        modifier.data = ModifierData.Create(target, info.key, layer, forever);
        return modifier;
    }
    public ModifierUnitObj SetObj(BattleUnit target)
    {
        if (obj != null)
        {
            DebugManager.Log("ModifierObj SetObj() obj != null");
            return obj;
        }
        obj = ResManager.InstantiateSync<ModifierUnitObj>(target.GetObj().buffBar.transform);
        obj.Init(this);
        return obj;
    }
    public ModifierUnitObj GetObj()
    {
        if (obj != null) return obj;
        DebugManager.Log("Modifier Obj is null");
        return obj;
    }
    public void IsActive(BuffCreate buffCreate)
    {
        this.GetInfo().IsActive(this, buffCreate);
    }

    internal Sequence CreateSeq()
    {
        return SequenceExtension.Create();
    }
}
public static class ModifierQuery
{
    public static ModifierInfo GetInfo(this Modifier modifier)
    {
        return ModifierInfo.Get(modifier.data.key);
    }
}
