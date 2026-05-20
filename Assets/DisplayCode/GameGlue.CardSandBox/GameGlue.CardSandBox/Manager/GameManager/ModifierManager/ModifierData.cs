using System;
using System.Collections.Generic;

[Serializable]
public class ModifierData
{
    public string id;
    public string key = string.Empty;
    public int layer;
    public bool forever;
    public Dictionary<string, int> dict = new();
    ModifierData() { }
    public static ModifierData Create(IBattleUnit target, string key, int layer, bool forever)
    {
        var data = new ModifierData();

        data.key = key;
        data.layer = layer;
        data.forever = forever;
        data.id = target.GetData().modifierDict.Keys.RandomID();
        return data;
    }
    public string GetDescription(Modifier buff)
    {
        return ModifierInfo.Get(this).GetDescription(buff);
    }
}
public interface IModifierData
{

}