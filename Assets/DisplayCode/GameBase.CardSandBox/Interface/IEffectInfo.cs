using System.Collections.Generic;
using DG.Tweening;
public interface IEffectInfo
{
    BattleEffectType EffectType();
    List<IUnit> GetTargetList(IUnit self);
    void Apply(IEffectContext context, List<IUnit> targetList, out Sequence seq);
    string Desc(IEffectContext context);
    string ChoiceDesc(IEffectContext context, IUnit target);
    int? GetValue();
}
