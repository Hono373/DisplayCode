using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.Port;
/// <summary>
/// 节点基类。关键改进点见各方法注释。
/// </summary>
public class InfoNode : Node
{
    public BaseInfo info = null;
    
    /// <summary>
    /// 【建议】使用 GetPosition() 替代 layout，避免拖拽时位置滞后
    ///   public Vector2 Pos => GetPosition().position;
    /// </summary>
    public Vector2 Pos { get => new((int)layout.x, (int)layout.y); }
    
    /// <summary>
    /// 由 Odin 创建的属性树，负责解析 data 类的字段并管理 GUI 生命周期
    /// </summary>
    private PropertyTree _propertyTree;
    /// <summary>
    /// 缓存的属性值变化回调委托，用于在释放时精确移除事件订阅
    /// </summary>
    private PropertyTree.OnPropertyValueChangedDelegate _onValueChanged;
    
    public InfoNode()
    {
        style.maxWidth = style.minWidth = 240;
        extensionContainer.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);

        // 节点从 VisualElement 面板移除时（删除/关闭），主动释放 Odin 属性树
        // 避免 _propertyTree 的回调持有 info.parent 的强引用导致无法 GC
        RegisterCallback<DetachFromPanelEvent>(_ => DisposePropertyTree());
    }
    
    /// <summary>
    /// 【建议】使用 SetPosition() 替代 style.left/top，处理坐标系变换
    ///   SetPosition(new Rect(info.pos.x, info.pos.y, 0, 0));
    /// </summary>
    public void Set(BaseInfo info)
    {
        this.info = info;

        style.left = info.pos.x;
        style.top = info.pos.y;

        title = info.GetType().Name;
    }

    /// <summary>
    /// 用 Odin PropertyTree 渲染当前 info 的所有可序列化字段。
    /// 
    /// 流程：
    /// 1. 清理旧的属性树（防止重复创建导致内存泄漏）
    /// 2. 基于 info 实例创建新的 PropertyTree（Odin 自行解析字段和 Odin 属性标记）
    /// 3. 注册值变化回调：Undo 记录 + SetDirty 标记（纯 C# 类不自带 Undo 支持）
    /// 4. 将 IMGUIContainer 挂入 extensionContainer，内嵌 Odin 的 IMGUI 绘制
    /// 
    /// 效果等价于在 Inspector 中查看 ScriptableObject ，只是渲染目标从 EditorGUI 换成了 VisualElement。
    /// </summary>
    public virtual void Expand(BaseInfo info)
    {
        extensionContainer.Clear();
        RefreshExpandedState();

        DisposePropertyTree();

        _propertyTree = PropertyTree.Create(info);

        // 纯 C# 类（BaseInfo）的属性树不自带 Undo 支持，
        // 手动监听值变化记录 Undo，变更自动同步到 info.parent（StoryInfo，ScriptableObject）
        _onValueChanged = (_, _) =>
        {
            if (info.parent != null)
            {
                Undo.RecordObject(info.parent, "Edit Field");
                EditorUtility.SetDirty(info.parent);
            }
        };
        _propertyTree.OnPropertyValueChanged += _onValueChanged;

        // IMGUIContainer 是 UIToolkit 中嵌入 IMGUI 内容的容器，
        // 这里让 Odin 的 PropertyTree 在内部以标准 IMGUI 方式绘制字段，
        // 从而复用 Odin 的所有抽屉（LabelText、ShowIf、Required 等）
        extensionContainer.Add(new IMGUIContainer(() =>
        {
            _propertyTree?.UpdateTree();
            // withUndo=false：跳过 Odin 的自动 Undo 检查（纯 C# 类不支持），用回调手动管理
            _propertyTree?.Draw(false);
        }));
        RefreshExpandedState();
    }

    /// <summary>
    /// 释放 Odin 属性树，清理事件回调，防止内存泄漏。
    /// 在以下两种场景调用：
    /// - Expand() 前清理旧树
    /// - 节点从面板移除时（DetachFromPanelEvent / NodeEdgeClear 主动调用）
    /// </summary>
    internal void DisposePropertyTree()
    {
        if (_propertyTree == null) return;
        _propertyTree.OnPropertyValueChanged -= _onValueChanged;  // 精确移除回调，切断对 info.parent 的引用链
        _onValueChanged = null;
        _propertyTree.Dispose();
        _propertyTree = null;
    }
    
/// <summary>
/// 【建议】portType 设具体类型限制连接，viewDataKey 不应与业务 GUID 混用
///   var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Capacity.Multi, typeof(BaseInfo));
///   port.viewDataKey = $"in_{info.GetHashCode()}";
/// </summary>
public Port InPort(BaseInfo info)
{
    var port = Create<Edge>(Orientation.Horizontal, Direction.Input, Capacity.Multi, null);
    port.userData = info;
    port.viewDataKey = info.inPort.guid;
    port.portName = "InPort";
    inputContainer.Add(port);
    return port;
}

/// <summary>
/// 【建议】同 InPort。Port.Create<Edge> 是官方标准用法。
/// </summary>
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
    
    /// <summary>
    /// 节点被选中时调用
    /// 
    /// 【问题】空实现，没有实际功能
    /// 【推荐】可以在这里实现节点选中时的高亮或属性面板更新
    /// </summary>
    public override void OnSelected()
    {
        base.OnSelected();
    }
    
    /// <summary>
    /// 节点取消选中时调用
    /// 
    /// 【问题】空实现
    /// 【推荐】可以在这里清理选中状态
    /// </summary>
    public override void OnUnselected()
    {
        base.OnUnselected();
    }
    
    /// <summary>
    /// 端口被移除时调用
    /// 
    /// 【问题】只有 Debug.Log，没有实际处理逻辑
    /// 
    /// 【推荐实现】
    /// protected override void OnPortRemoved(Port port)
    /// {
    ///     // 清理与该端口相关的连接数据
    ///     if (port.userData is BaseInfo info)
    ///     {
    ///         // 清理连接数据
    ///     }
    ///     base.OnPortRemoved(port);
    /// }
    /// </summary>
    protected override void OnPortRemoved(Port port)
    {
        Debug.Log("OnPortRemoved(Port port)");
        base.OnPortRemoved(port);
    }
    
    /// <summary>
    /// 切换节点折叠状态时调用
    /// 
    /// 【问题】只有 Debug.Log
    /// </summary>
    protected override void ToggleCollapse()
    {
        Debug.Log("ToggleCollapse");
        base.ToggleCollapse();
    }
    
    /// <summary>
    /// 构建右键菜单
    /// 
    /// 【推荐】可以在这里添加自定义菜单项
    /// public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    /// {
    ///     evt.menu.AppendAction("自定义操作", _ => { /* 处理逻辑 */ });
    ///     base.BuildContextualMenu(evt);
    /// }
    /// </summary>
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //Debug.Log(evt.currentTarget);
        base.BuildContextualMenu(evt);
    }

    #endregion
}