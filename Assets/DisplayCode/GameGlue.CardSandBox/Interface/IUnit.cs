using System;
using System.Collections.Generic;
using GameModule.CardSandBox.UnitIntention;
using Newtonsoft.Json;

public interface IBattleUnit
{
    IBattleUnitData GetData();
    IBattleUnitInfo GetInfo();
    IBattleUnitObj GetObj();
    IBattleUnitObj InstantiateObj();
    UnitIntention GetIntention();
    void SetModifier(Modifier modifier);
}
[Serializable]
public class Unit
{
    public bool temporary;
    public string id;
    public string key;
    public int maxHP;
    public int hp;
    public Dictionary<string, Modifier> modifierDict = new();
    public static Unit Create(string key, string id, bool temporary)
    {
        var unit = new Unit();

        unit.temporary = temporary;
        unit.id = id;
        unit.key = key;

        var info = UnitInfo.Get(key);

        unit.data = UnitData.Create(info);

        return unit;
    }
}

public class BattleUnitObj : IBattleUnitObj
{

}
public enum BattleUnitEnum { None, All, Player, Mate, Enemy, Neutral }
[Serializable]
public class BattleUnit : IBattleUnit
{
    [JsonIgnore] IBattleUnitObj obj = new BattleUnitObj();

    public BattleUnitEnum team;

    BattleUnit() { }
    public static BattleUnit Create(IUnit unitData, BattleUnit.BattleUnitEnum team)
    {
        var battleUnit = new BattleUnit();
        battleUnit.team = team;
        battleUnit.data = BattleUnitData.Create(unitData, team);
        return battleUnit;
    }
    public BattleUnitObj GetObj()
    {
        if (obj != null) return obj;
        var topObj = DeckBuildBattle.GetObj();
        obj = ResManager.InstantiateSync<BattleUnitObj>(topObj.unitContainer.transform);
        obj.Init(this);
        return obj;
    }

    public IBattleUnitInfo GetInfo()
    {
        throw new System.NotImplementedException();
    }

    public UnitIntention GetIntention()
    {
        throw new System.NotImplementedException();
    }

    public IBattleUnitObj GetObj()
    {
        throw new System.NotImplementedException();
    }

    public IBattleUnitObj InstantiateObj()
    {
        throw new System.NotImplementedException();
    }

    public void SetModifier(Modifier modifier)
    {
        throw new System.NotImplementedException();
    }

    public IBattleUnitData GetData()
    {
        throw new NotImplementedException();
    }
}
public interface IBattleUnitData
{

}
public class BattleUnitData
{
    public string id;
    public string key;
    public int maxHP;
    public int hp;
    public Dictionary<string, Modifier> modifierDict = new();
    public List<int> lastActionIndexList = new();
    public UnitIntention intention;
    public static BattleUnitData Create(IBattleUnitInfo info, BattleUnit.BattleUnitEnum team)
    {
        var data = new BattleUnitData();
        data.id = info.id;
        data.key = info.key;
        data.maxHP = info.data.maxHP;
        data.hp = info.data.hp;
        return data;
    }
}

public interface IBattleUnitInfo
{

}
public interface IBattleUnitObj
{

}