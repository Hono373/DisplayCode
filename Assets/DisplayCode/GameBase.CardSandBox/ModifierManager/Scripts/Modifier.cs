using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
public interface IModifierOwner
{

}
[Serializable]
public class Modifier
{
    [Serializable]
    public class Data
    {
        public IModifierOwner owner;
        public bool only;
        public string key = string.Empty;
        public int layer;
        public bool forever;
        public Dictionary<string, int> dict = new();
    }
    [JsonProperty] public Data data;
    [JsonIgnore] IModifierUnitObj obj;
    Modifier() { }
    public static Modifier Create(ModifierCreateInfo createInfo)
    {
        var modifier = new Modifier();
        modifier.data = new Data();


        modifier.data.key = createInfo.key;

        modifier.data.layer = createInfo.layer;

        modifier.data.forever = createInfo.forever;


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
    public ModifierInfo GetInfo()
    {
        return ModifierInfo.Get(data.key);
    }
    public void IsActive(ModifierCreateInfo buffCreate)
    {
        GetInfo().IsActive(this, buffCreate);
    }
}
