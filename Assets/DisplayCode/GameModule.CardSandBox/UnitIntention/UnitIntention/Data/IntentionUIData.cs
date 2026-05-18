public class IntentionUIData
{
    public BattleEffectType type;
    public int? value;
    IntentionUIData() { }
    public static IntentionUIData Create(BattleEffectType sprite, int? value)
    {
        var data = new IntentionUIData();
        data.type = sprite;
        data.value = value;
        return data;
    }

}
