public class NoteNode : InfoNode
{
    public NoteNode Init(NoteInfo info)
    {
        Set(info);
        Expand(info);
        return this;
    }
}