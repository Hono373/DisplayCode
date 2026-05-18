public class EndNode : InfoNode
{
    public EndNode Init(EndInfo info)
    {
        Set(info);
        InPort(info);
        return this;
    }
}