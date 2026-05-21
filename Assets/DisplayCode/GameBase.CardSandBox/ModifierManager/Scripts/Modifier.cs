using DG.Tweening;
using Newtonsoft.Json;
using System;
using UnityEngine;
public interface IModifierOwnerAffected
{

}
[Serializable]
public partial class Modifier
{
    [JsonProperty] public ModifierData data;
    [JsonIgnore] IModifierUnitObj obj;
    Modifier() { }
    public static Modifier Create(string id, IModifierOwnerAffected owner, IModifierOwnerAffected target, ModifierCreateInfo createInfo)
    {
        var modifier = new Modifier();
        modifier.data = new ModifierData()
        {
            id = id,
            owner = owner,
            target = target,
            key = createInfo.key,
            only = createInfo.info.only,
            layer = createInfo.layer,
            forever = createInfo.forever
        };
        return modifier;
    }
    public IModifierUnitObj SetObj(IBattleUnit target)
    {
        if (obj != null)
        {
            Debug.Log("ModifierObj SetObj() obj != null");
            return obj;
        }
        return obj;
    }
    public IModifierUnitObj GetObj()
    {
        if (obj != null) return obj;
        Debug.Log("Modifier Obj is null");
        return obj;
    }
    public ModifierInfo GetInfo()=>ModifierInfo.Get(data.key);
    public void IsActive(ModifierCreateInfo buffCreate)
    {
        GetInfo().IsActive(this, buffCreate);
    }
}
