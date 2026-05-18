using UnityEditor.Experimental.GraphView;
public class StartNode : InfoNode
{
    public StartNode(StartInfo info)
    {
        capabilities &= ~Capabilities.Deletable;
        Set(info);
        OutPort(info);
        Expand(info);
    }
}