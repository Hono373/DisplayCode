using System.Collections.Generic;

public interface ISkillInfo
{
    SelectorBox SelectMode();
    List<SkillEffectInfo> EffectInfos();
    List<IntentionUIData> GetUIData();
}

