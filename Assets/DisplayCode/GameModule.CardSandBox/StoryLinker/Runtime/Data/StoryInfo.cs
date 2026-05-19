using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 【问题】guidHash 未序列化导致每次反序列化后重现构造，GUID 可能重复；
///       Save 含 Refresh() 导致全量扫描
/// 【建议】去掉 guidHash，直接 Guid.NewGuid().ToString()；Save 只调 SaveAssetIfDirty
/// </summary>
public class StoryInfo : ScriptableObject
{
    #region 字段
    public StartInfo startInfo;
    /// <summary>
    /// 【问题】使用 [SerializeReference] 可以序列化多态类型，但性能较差
    /// 【推荐】如果类型固定，使用 [SerializeField] + 具体类型更好
    /// </summary>
    [SerializeReference] public List<BaseInfo> list = new();
    #endregion
    
    /// <summary>
    /// 【建议】用配置表或常量管理初始节点位置，避免硬编码
    /// </summary>
    public void Init()
    {
        // 先初始化 startInfo，再清空 guidHash，避免 NewGuid() 时访问未初始化的 startInfo
        startInfo = new StartInfo().Init(this, new(200, 100));
        guidHash = null; // 清空 guidHash，让它重新构建
        list.Clear(); // 清空旧数据，避免重复累积
        new DialogueInfo().Init(this, new(200 + 240, 100));
        new OptionInfo().Init(this, new(200 + 240 * 2, 100));
        new EndInfo().Init(this, new(200 + 240 * 3, 100));
        new NoteInfo().Init(this, new(200, 400));
        Debug.Log($"Init 完成，list 中有 {list.Count} 个节点");
    }
    
    /// <summary>
    /// 【建议】去掉 guidHash，直接 Guid.NewGuid().ToString()
    /// </summary>
    public string NewGuid()
    {
        var guid = Guid.NewGuid().ToString();
        // 安全检查：如果 guidHash 还没初始化，直接返回
        if (guidHash == null)
        {
            guidHash = new HashSet<string>();
            guidHash.Add(guid);  // 修复：使用 guidHash 而不是 GuidHash
            return guid;
        }
        while (guidHash.Contains(guid))  // 修复：使用 guidHash 而不是 GuidHash（避免递归）
        {
            Debug.Log("Guid Error");
            guid = Guid.NewGuid().ToString();
        }
        guidHash.Add(guid);  // 修复：使用 guidHash 而不是 GuidHash
        return guid;
    }
    
    /// <summary>
    /// 【建议】用 list.ToList() 遍历避免修改集合的异常；加 Undo.RecordObject 支持撤销
    /// </summary>
    public void Remove(BaseInfo info)
    {
        list.Remove(info);
        startInfo.Cleaner(info);
        foreach (var i in list)
        {
            i.Cleaner(info);
        }
    }
    
    /// <summary>
    /// 标记脏 + 写盘。不调用 Refresh()，避免全量扫描卡顿。
    /// </summary>
    public void Save()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
#endif
    }
    /// <summary>
    /// 【建议】guidHash 未序列化，重构后应去掉。直接 Guid.NewGuid() 即可。
    /// </summary>
    HashSet<string> guidHash;
    HashSet<string> GuidHash
    {
        get
        {
            if (guidHash == null)
            {
                guidHash = new HashSet<string>();
                // 安全检查：startInfo 可能为 null（如 Reset 时）
                if (startInfo != null)
                {
                    guidHash.Add(startInfo.inPort.guid);
                    guidHash.Add(startInfo.outPort.guid);
                }
                foreach (var info in list) 
                { 
                    if (info?.inPort != null) guidHash.Add(info.inPort.guid);
                    if (info?.outPort != null) guidHash.Add(info.outPort.guid);
                }
                return guidHash;
            }
            else
            {
                return guidHash;
            }
        }
    }
}