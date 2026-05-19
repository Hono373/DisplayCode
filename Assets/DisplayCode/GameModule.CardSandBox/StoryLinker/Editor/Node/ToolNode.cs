using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// 工具节点：用于创建新的故事节点
/// 
/// 【问题分析】
/// 1. 直接继承 Node，但 ToolNode 不是真正的数据节点
/// 2. 在按钮回调中使用 parent.Add()，但 parent 可能不是 GraphView
/// 3. 节点位置硬编码（left=5, top=200）
/// 4. 每次创建节点都创建新的按钮，没有复用
/// 
/// 【推荐实现】
/// - 使用 VisualElement 而不是 Node
/// - 将创建逻辑放到 StoryLinker 或 NodeView 中
/// - 使用工厂模式创建不同类型的节点
/// </summary>
public class ToolNode : Node
{
    StoryInfo info = null;
    
    /// <summary>
    /// 构造函数
    /// 
    /// 【问题】
    /// 1. 硬编码位置和大小
    /// 2. 使用 extensionContainer 来存放按钮，但 Node 的 extensionContainer 是用于展开内容的
    /// 
    /// 【推荐实现】
    /// public ToolNode(StoryInfo info)
    /// {
    ///     this.info = info;
    ///     title = "创建节点";
    ///     
    ///     // 使用 contentContainer 而不是 extensionContainer
    ///     contentContainer.Add(CreateButton("对白节点", typeof(DialogueInfo)));
    ///     contentContainer.Add(CreateButton("分支节点", typeof(OptionInfo)));
    ///     // ...
    ///     
    ///     capabilities &= ~Capabilities.Deletable;
    ///     capabilities &= ~Capabilities.Movable;
    /// }
    /// </summary>
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
    
    /// <summary>
    /// 创建"对白节点"按钮
    /// 
    /// 【严重问题】
    /// 1. parent.Add(newNode) 中的 parent 是 VisualElement，不是 GraphView
    ///    应该使用 GraphView.AddElement()
    /// 2. 新节点位置基于 layout，但 layout 可能尚未计算
    /// 3. 没有使用 Undo 系统
    /// 
    /// 【推荐实现】
    /// public Button CreateDialogue()
    /// {
    ///     var btn = new Button(() =>
    ///     {
    ///         var graphView = GetFirstAncestorOfType<GraphView>();
    ///         if (graphView == null) return;
    ///         
    ///         var pos = new Vector2(layout.center.x + 100, layout.center.y);
    ///         var newInfo = new DialogueInfo().Init(info, pos);
    ///         var newNode = new DialogueNode().Init(newInfo);
    ///         
    ///         // 使用 Undo 系统
    ///         Undo.RegisterCreatedObjectUndo(newInfo, "Create Dialogue Node");
    ///         graphView.AddElement(newNode);
    ///     });
    ///     btn.text = "对白节点";
    ///     return btn;
    /// }
    /// </summary>
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
    
    /// <summary>
    /// 创建"分支节点"按钮
    /// 
    /// 【问题】同 CreateDialogue
    /// </summary>
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
    
    /// <summary>
    /// 创建"结束节点"按钮
    /// 
    /// 【问题】同 CreateDialogue
    /// </summary>
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
    
    /// <summary>
    /// 创建"Note节点"按钮
    /// 
    /// 【问题】同 CreateDialogue
    /// </summary>
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