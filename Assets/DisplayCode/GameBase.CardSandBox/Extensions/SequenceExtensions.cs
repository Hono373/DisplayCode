using DG.Tweening;
public class SequenceExtensions
{
    public struct Info
    {
        public string desc;
        public override string ToString() => desc;
    }
    public static Sequence Create(string desc = "")
    {
        var seq = DOTween.Sequence();
        var info = new Info();
        info.desc = desc;
        seq.SetId(info);
        return seq;
    }
}
