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

}
[Serializable]
public class Unit
{
    public bool temporary;
    public string id;
    public string key;
    public int maxHP;
    public int hp;

    public static Unit Create(string key, string id, bool temporary)
    {
        var unit = new Unit();



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
    public static BattleUnit Create(IUnit unitData, BattleUnitEnum team)
    {
        var battleUnit = new BattleUnit();
        return battleUnit;
    }


    public IBattleUnitInfo GetInfo()
    {
        throw new System.NotImplementedException();
    }

    public UnitIntention GetIntention()
    {
        throw new System.NotImplementedException();
    }


    public IBattleUnitObj InstantiateObj()
    {
        throw new System.NotImplementedException();
    }


    public IBattleUnitData GetData()
    {
        throw new NotImplementedException();
    }

    public IBattleUnitObj GetObj()
    {
        throw new NotImplementedException();
    }
}
public interface IBattleUnitData
{

}

public interface IBattleUnitInfo
{

}
public interface IBattleUnitObj
{

}