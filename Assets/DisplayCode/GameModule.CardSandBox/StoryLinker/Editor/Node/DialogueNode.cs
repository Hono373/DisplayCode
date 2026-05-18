public class DialogueNode : InfoNode
{
    public DialogueNode Init(DialogueInfo info)
    {
        Set(info);
        InPort(info);
        OutPort(info);
        Expand(info);
        return this;
    }
}
