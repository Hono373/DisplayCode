/// <summary>
/// 对话节点：显示对话内容
/// 
/// 【问题分析】
/// 1. 使用 Init 方法而不是构造函数，与其他节点一致
/// 2. 有输入和输出端口（正确）
/// 3. Expand 方法会展开显示 DialogueInfo 的字段
/// 
/// 【推荐实现】
/// - 可以在节点标题中添加图标
/// - 可以在节点中预览对话内容
/// </summary>
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
