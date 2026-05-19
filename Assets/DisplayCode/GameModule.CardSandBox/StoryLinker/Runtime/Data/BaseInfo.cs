using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有故事节点的基础数据类
/// 
/// 【问题分析】
/// 1. 使用 string GUID 关联端口，而不是 Unity 的 SerializedProperty 引用
/// 2. Set() 方法中直接修改 parent.list，可能导致集合修改异常
/// 3. LinkTo/DisConnect 使用字符串 GUID，容易出错且难以调试
/// 4. 没有使用 [SerializeReference] 或 [SerializeField] 的规范管理
/// 
/// 【推荐实现】
/// - 使用 Unity 的 SerializedProperty 或直接引用进行数据关联
/// - 将 GUID 逻辑封装到 Port 类中
/// - 使用接口或抽象方法定义节点行为
/// </summary>
[Serializable]
public class BaseInfo
{
    /// <summary>
    /// 【问题】直接引用父对象，可能导致循环引用
    /// 【推荐】使用 indirect reference 或 ScriptableObject 的 GUID
    /// </summary>
    [HideInInspector] public StoryInfo parent = null;
    
    public InPort inPort = new();
    public Vector2 pos = Vector2.zero;
    public OutPort outPort = new();
    
    /// <summary>
    /// 【建议】Set 不应自动 parent.list.Add(this)，
    /// 注册到集合应由调用方显式执行，避免时序耦合。
    /// 用虚方法替代 is not StartInfo / is not EndInfo：
    ///   public virtual bool HasInPort => true;       // StartInfo: false
    ///   public virtual bool HasOutPort => true;      // EndInfo: false
    /// </summary>
    public void Set(StoryInfo info, Vector2 v)
    {
        parent = info;
        if (this is not StartInfo)
            inPort.guid = parent.NewGuid();
        if (this is not EndInfo)
            outPort.guid = parent.NewGuid();
        pos = v;
        if (this is not StartInfo)
            parent.list.Add(this);
    }
    
    /// <summary>
    /// 连接到另一个节点
    /// 
    /// 【问题】
    /// 1. 使用字符串 GUID，类型不安全
    /// 2. 没有检查 nextGuid 是否有效
    /// 
    /// 【推荐实现】
    /// public void LinkTo(BaseInfo nextNode)
    /// {
    ///     if (nextNode == null) return;
    ///     if (!outPort.nextGuids.Contains(nextNode.inPort.guid))
    ///         outPort.nextGuids.Add(nextNode.inPort.guid);
    /// }
    /// </summary>
    public void LinkTo(string nextGuid)
    {
        if (!outPort.nextGuids.Contains(nextGuid))
            outPort.nextGuids.Add(nextGuid);
    }
    
    /// <summary>
    /// 断开与另一个节点的连接
    /// 
    /// 【问题】没有检查 nextGuid 是否存在
    /// </summary>
    public void DisConnect(string nextGuid)
    {
        outPort.nextGuids.Remove(nextGuid);
    }
    
    /// <summary>
    /// 更新节点位置
    /// 
    /// 【问题】直接赋值，没有触发任何变更通知
    /// 
    /// 【推荐实现】
    /// public void UpdatePos(Vector2 pos)
    /// {
    ///     this.pos = pos;
    ///     // 触发 OnPositionChanged 事件
    /// }
    /// </summary>
    public void UpdatePos(Vector2 pos)
    {
        this.pos = pos;
    }
    
    /// <summary>
    /// 清理对已删除节点的引用
    /// 
    /// 【问题】方法名 Cleaner 不符合 C# 命名规范，应该是 Clean 或 RemoveReferences
    /// </summary>
    public void Cleaner(BaseInfo removed)
    {
        outPort.nextGuids.Remove(removed.inPort.guid);
    }
}

/// <summary>
/// 输入端口数据
/// 
/// 【问题】只存储 GUID，没有存储端口类型或其他元数据
/// 
/// 【推荐实现】
/// [Serializable]
/// public class InPort
/// {
///     public string guid = string.Empty;
///     public System.Type acceptedType;  // 接受的端口类型
/// }
/// </summary>
[Serializable]
public class InPort
{
    public string guid = string.Empty;
}

/// <summary>
/// 输出端口数据
/// 
/// 【问题】
/// 1. nextGuids 使用字符串列表，应该考虑使用直接引用
/// 2. 没有限制连接数量（虽然 Capacity.Multi 允许多个连接）
/// 
/// 【推荐实现】
/// [Serializable]
/// public class OutPort
/// {
///     public string guid = string.Empty;
///     public List<string> nextGuids = new();
///     
///     // 添加验证方法
///     public bool CanConnectTo(string targetGuid)
///     {
///         return !nextGuids.Contains(targetGuid);
///     }
/// }
/// </summary>
[Serializable]
public class OutPort
{
    public string guid = string.Empty;
    public List<string> nextGuids = new();
}