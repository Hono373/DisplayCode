using UnityEditor.Experimental.GraphView;
/// <summary>
/// 开始节点：故事流程的起点
/// 
/// 【问题分析】
/// 1. 只在构造函数中设置，没有 Init 方法（与其他节点不一致）
/// 2. 没有输入端口，但代码中没有明确说明
/// 3. capabilities 设置正确（不可删除）
/// 
/// 【推荐实现】
/// public class StartNode : InfoNode
/// {
///     public StartNode Init(StartInfo info)
///     {
///         capabilities &= ~Capabilities.Deletable;
///         Set(info);
///         // 开始节点只有输出端口
///         OutPort(info);
///         Expand(info);
///         return this;
///     }
/// }
/// </summary>
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