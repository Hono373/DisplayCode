using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.Port;
public class InfoNode : Node
{
    public BaseInfo info = null;
    public Vector2 Pos { get => new((int)layout.x, (int)layout.y); }
    public InfoNode()
    {
        style.maxWidth = style.minWidth = 240;
        extensionContainer.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
    }
    public void Set(BaseInfo info)
    {
        this.info = info;

        style.left = info.pos.x;
        style.top = info.pos.y;

        title = info.GetType().Name;
    }
    public virtual void Expand(BaseInfo info)
    {
        extensionContainer.Clear();
        FieldGenerate.Start(info, extensionContainer, ExtendSwitch, "parent");
        RefreshExpandedState();
    }
    void ExtendSwitch(object data, VisualElement parent)
    {
        switch (data)
        {
            case TextInfo value:
                Label label = FieldGenerate.StringLabel(data.GetType().Name);
                parent.Add(label);

                if (!StoryLinker.Config.ShowFullTextInfo)
                {
                    FieldGenerate.Start(value, parent, ExtendSwitch, "en", "jp");
                }
                else
                {
                    FieldGenerate.Start(value, parent, ExtendSwitch);
                }

                break;
        }
    }
    public Port InPort(BaseInfo info)
    {
        var port = Create<Edge>(Orientation.Horizontal, Direction.Input, Capacity.Multi, null);
        port.userData = info;
        port.viewDataKey = info.inPort.guid;
        port.portName = "InPort";
        inputContainer.Add(port);
        return port;
    }
    public Port OutPort(BaseInfo info)
    {
        var port = Create<Edge>(Orientation.Horizontal, Direction.Output, Capacity.Multi, null);
        port.userData = info;
        port.viewDataKey = info.outPort.guid;
        port.portName = "OutPort";
        outputContainer.Add(port);
        return port;
    }
    #region 回调
    public override void OnSelected()
    {
        base.OnSelected();
    }
    public override void OnUnselected()
    {
        base.OnUnselected();
    }
    protected override void OnPortRemoved(Port port)
    {
        Debug.Log("OnPortRemoved(Port port)");
        base.OnPortRemoved(port);
    }
    protected override void ToggleCollapse()
    {
        Debug.Log("ToggleCollapse");
        base.ToggleCollapse();
    }
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //Debug.Log(evt.currentTarget);
        base.BuildContextualMenu(evt);
    }

    #endregion
}