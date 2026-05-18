using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;
public class StoryLinker : EditorWindow
{
    [MenuItem("我的工具/StoryLinker.Test")]
    public static void ShowExample()
    {
        GetWindow<StoryLinker>();
    }
    public CreatorTool ToolBar = null;
    public NodeView nodeView = null;

    public void CreateGUI()
    {
        titleContent = new GUIContent("故事编辑器_Test", IconContent("d__Popup@2x").image as Texture2D);

        rootVisualElement.Add(nodeView ??= new(this));
        rootVisualElement.Add(ToolBar ??= new(this));
    }
    public void CreateAsset(string received)
    {
        var info = CreateInstance<StoryInfo>();
        string assetPath = AssetCreatePath($"{received}.asset");
        AssetDatabase.CreateAsset(info, assetPath);
        info.Save();
        LoadData(info);
    }
    public void LoadData(StoryInfo info)
    {
        ToolBar.assetName.value = info.name;
        nodeView.viewTransform.position = new(0, 0);
        nodeView.Fresh(info);
    }
    public override void DiscardChanges()
    {
        Debug.Log("DiscardChanges");
        base.DiscardChanges();
    }
    public override void SaveChanges()
    {
        Debug.Log("SaveChanges");
        base.SaveChanges();
    }

    void OnFocus() { }
    private void OnLostFocus() { }
    private void OnDestroy()
    {
        // 安全保存，避免 null 引用
        if (nodeView != null && nodeView.Info != null)
        {
            nodeView.Info.Save();
        }
    }
    public string AssetCreatePath(string key) => Path.Combine(Config.GetCreatePath, key);
    public static StoryLinkerConfig Config { get => StoryLinkerConfig.GetInstance(); }
}
