/// <summary>
/// 结束节点：故事流程的终点
/// 
/// 【问题分析】
/// 1. 只有输入端口，没有输出端口（正确）
/// 2. 使用 Init 方法，与其他节点保持一致
/// 3. 没有设置 capabilities，应该设置为不可删除
/// 
/// 【推荐实现】
/// public class EndNode : InfoNode
/// {
///     public EndNode Init(EndInfo info)
///     {
///         capabilities &= ~Capabilities.Deletable;  // 结束节点也不应该被删除
///         Set(info);
///         InPort(info);
///         return this;
///     }
/// }
/// </summary>
public class EndNode : InfoNode
{
    public EndNode Init(EndInfo info)
    {
        Set(info);
        InPort(info);
        return this;
    }
}