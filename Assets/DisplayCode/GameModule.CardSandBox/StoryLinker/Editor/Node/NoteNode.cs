/// <summary>
/// 笔记节点：用于添加备注信息，不影响故事流程
/// 
/// 【问题分析】
/// 1. 没有输入和输出端口（正确，笔记节点不参与流程）
/// 2. 但 Expand 方法会展开显示 NoteInfo 的字段
/// 3. 应该设置为不可连接
/// 
/// 【推荐实现】
/// public class NoteNode : InfoNode
/// {
///     public NoteNode Init(NoteInfo info)
///     {
///         capabilities &= ~Capabilities.Connectable;  // 设置为不可连接
///         Set(info);
///         Expand(info);
///         return this;
///     }
/// }
/// </summary>
public class NoteNode : InfoNode
{
    public NoteNode Init(NoteInfo info)
    {
        Set(info);
        Expand(info);
        return this;
    }
}