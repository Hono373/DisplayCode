

/// <summary>
/// 选项节点：提供分支选择
/// 
/// 【问题分析】
/// 1. 设置了 portType 为 typeof(OptionInfo)，但端口连接逻辑可能没有检查类型
/// 2. 一个选项节点可能有多个输出端口（每个选项一个），但当前只有一个 OutPort
/// 
/// 【推荐实现】
/// - 考虑为每个选项创建一个输出端口
/// - 使用 portType 进行端口兼容性检查
/// </summary>
public class OptionNode : InfoNode
{
    public OptionNode Init(OptionInfo info)
    {
        Set(info);
        var inPort = InPort(info);
        var outPort = OutPort(info);
        /// 【问题】设置 portType 但没有在 GetCompatiblePorts 中检查
        inPort.portType = outPort.portType = typeof(OptionInfo);
        Expand(info);
        return this;
    }
}