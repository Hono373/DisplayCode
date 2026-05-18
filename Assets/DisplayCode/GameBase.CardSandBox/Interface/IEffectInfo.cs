using System.Collections.Generic;
using DG.Tweening;
/// <summary>
/// 通过胶水层实现
/// </summary>
public interface IEffectInfo
{
    BattleEffectType EffectType();
    List<IUnit> GetTargetList(IUnit self);
    void Apply(IEffectContext context, List<IUnit> targetList, out Sequence seq);
    int? GetValue();
}
