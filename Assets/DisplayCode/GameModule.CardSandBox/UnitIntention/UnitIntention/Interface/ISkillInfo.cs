using System.Collections.Generic;
namespace GameModule.CardSandBox.UnitIntention
{
    public interface ISkillInfo
    {
        SelectorBox SelectMode();
        List<SkillEffectInfo> EffectInfos();
        List<IntentionUIData> GetUIData();
    }
}

