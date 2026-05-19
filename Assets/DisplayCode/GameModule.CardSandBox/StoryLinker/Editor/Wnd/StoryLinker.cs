using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.EditorGUIUtility;
/// <summary>
/// StoryLinker 主窗口
///
/// 【问题】graphViewChanged 中每次修改都调 Info.Save()，导致：
///   1. 无法 Ctrl+Z 撤销
///   2. 频繁磁盘写入
///
/// 【建议】使用 Undo 系统记录修改，只在关闭窗口时保存：
///
///   Undo.RecordObject(storyInfo, "Move Node")   // 修改前记录
///   EditorUtility.SetDirty(storyInfo)            // 标记脏，不写磁盘
///   AssetDatabase.SaveAssetIfDirty(storyInfo)    // 关闭时写入
///
/// </summary>
public class StoryLinker : EditorWindow
{
    [MenuItem("我的工具/StoryLinker.Test")]
    public static void ShowExample()
    {
        // 吸附到 SceneView 标签栏（作为同区的标签页）
        GetWindow<StoryLinker>("StoryLinker", true, typeof(SceneView));
    }

    public CreatorTool ToolBar = null;
    public NodeView nodeView = null;

    public void CreateGUI()
    {
        titleContent = new GUIContent("StoryLinker.Test", IconContent("d__Popup@2x").image as Texture2D);
        minSize = new Vector2(800, 600);

        var root = rootVisualElement;
        root.Add(ToolBar ??= new(this));

        var viewContainer = new VisualElement();

        viewContainer.style.flexGrow = 1;
        viewContainer.Add(nodeView ??= new NodeView(this));
        root.Add(viewContainer);
    }

    /// <summary>
    /// 【问题】创建前未检查文件是否已存在；CreateAsset 已保存，无需再 Save()
    /// 【建议】创建前用 AssetDatabase.LoadAssetAtPath 检查，省略冗余 Save()
    /// </summary>
    public void CreateAsset(string received)
    {
        var info = CreateInstance<StoryInfo>();
        string assetPath = AssetCreatePath($"{received}.asset");
        AssetDatabase.CreateAsset(info, assetPath);
        info.Save();
        LoadData(info);
    }

    /// <summary>
    /// 【问题】未检查 info 是否为 null；viewTransform.position 可能延迟生效
    /// 【建议】加 null 检查，视图变换用 EditorApplication.delayCall 推迟一帧
    ///   if (info == null) return;
    /// </summary>
    public void LoadData(StoryInfo info)
    {
        ToolBar.assetName.value = info.name;
        nodeView.viewTransform.position = new(0, 0);
        nodeView.Fresh(info);
    }

    public override void DiscardChanges()
    {
        base.DiscardChanges();
    }

    public override void SaveChanges()
    {
        base.SaveChanges();
    }

    void OnFocus() { }
    private void OnLostFocus() { }

    /// <summary>
    /// 【建议】关闭时保存脏数据并清理引用
    ///   AssetDatabase.SaveAssetIfDirty(nodeView.Info);
    /// </summary>
    private void OnDestroy()
    {
        // 安全保存，避免 null 引用
        if (nodeView != null && nodeView.Info != null)
        {
            nodeView.Info.Save();
        }
    }

    public string AssetCreatePath(string key) => Path.Combine(GetConfig().GetCreatePath(), key);

    public static StoryLinkerConfig GetConfig() => StoryLinkerConfig.Get();
}
