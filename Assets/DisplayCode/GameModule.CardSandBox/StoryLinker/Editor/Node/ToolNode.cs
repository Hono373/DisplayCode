using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
public class ToolNode : Node
{
    StoryInfo info = null;
    public ToolNode(StoryInfo info)
    {
        this.info = info;

        title = "[+] 创建节点";
        style.width = 120;
        style.left = 5;
        style.top = 200;
        capabilities &= ~Capabilities.Deletable;
        extensionContainer.Add(CreateDialogue());
        extensionContainer.Add(CreateOption());
        extensionContainer.Add(CreateEnd());

        extensionContainer.Add(CreateNote());
        RefreshExpandedState();
    }
    public Button CreateDialogue()
    {
        var btn = new Button(() =>
        {
            var pos = new Vector2(layout.xMax + 20, layout.y + 20);
            var newInfo = new DialogueInfo().Init(info, pos);
            var newNode = new DialogueNode().Init(newInfo);
            parent.Add(newNode);
        });
        btn.text = "对白节点";
        return btn;
    }
    private Button CreateOption()
    {
        var btn = new Button(() =>
        {
            var pos = new Vector2(layout.xMax + 20, layout.y + 20);
            var newInfo = new OptionInfo().Init(info, pos);
            var newNode = new OptionNode().Init(newInfo);
            parent.Add(newNode);
        });
        btn.text = "分支节点";

        return btn;
    }
    private Button CreateEnd()
    {
        var btn = new Button(() =>
        {
            var pos = new Vector2(layout.xMax + 20, layout.y + 20);
            var newInfo = new EndInfo().Init(info, pos);
            var newNode = new EndNode().Init(newInfo);
            parent.Add(newNode);
        });
        btn.text = "结束节点";

        return btn;
    }
    private Button CreateNote()
    {
        var btn = new Button(() =>
        {
            var pos = new Vector2(layout.xMax + 20, layout.y + 20);
            var newInfo = new NoteInfo().Init(info, pos);
            var newNode = new NoteNode().Init(newInfo);
            parent.Add(newNode);
        });
        btn.text = "Note节点";
        return btn;
    }
}