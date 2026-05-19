using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// 【问题】手动管理 RectangleSelector 的添加/移除，用委托代理保存按钮，
///       用 Flex 伪元素替代 ToolbarSpacer
/// 【建议】RectangleSelector 默认由 GraphView 管理；用 ToolbarSpacer 分隔；
///       保存功能直接绑定到按钮
/// </summary>
public class CreatorTool : Toolbar
{
    public TextField assetName = null;
    ToolbarButton FastTest = null;
    Action Test = null;
    RectangleSelector selector = null;

    bool selectorBool;
    ToolbarButton selectorManager;

    /// <summary>
    /// 【问题】RectangleSelector 手动 toggle；按钮颜色硬编码；缺少 ToolbarSpacer
    /// 【建议】用 GraphView 默认操纵器；颜色用 USS 类名；按钮间隔用 ToolbarSpacer
    ///   Add(new ToolbarSpacer());
    /// </summary>
    public CreatorTool(StoryLinker creatorWnd)
    {
        var creator = EditorWindow.GetWindow<StoryLinker>();
        AddButtonNamedCreateAsset();

        CreateFlex();

        AddAssetNameField();

        CreateFlex();

        Add(FastTest = new ToolbarButton(() => Test?.Invoke()) { text = "Save" });
        FastTest.style.backgroundColor = Color.red * 0.5f;

        selector = new RectangleSelector();

        selectorManager = new ToolbarButton(() =>
        {
            selectorBool = !selectorBool;
            if (selectorBool)
            {
                selectorManager.text = "关闭框选";

                creator.nodeView.AddManipulator(selector);
            }
            else
            {
                selectorManager.text = "开启框选";
                creator.nodeView.RemoveManipulator(selector);
            }
        })
        { text = "开启框选" };
        selectorBool = false;

        Add(selectorManager);

        Add(new ToolbarButton(() => AssetWnd.ShowWindow(creator)) { text = "资源列表" });

        Add(new ToolbarButton(() => creator.nodeView.viewTransform.position = new(0, 0)) { text = "复位视图" });

        Test += Save;
    }

    private void Save()
    {
        EditorWindow.GetWindow<StoryLinker>().nodeView.Info.Save();
    }

    /// <summary>
    /// 【建议】按钮颜色用 USS 类名而非硬编码
    /// </summary>
    private void AddButtonNamedCreateAsset()
    {
        Add(new ToolbarButton(() => CreateNewAsset()) { text = "新建视图", style = { backgroundColor = Color.green * 0.5f } });
    }

    /// <summary>
    /// 【建议】弹窗提示文本过长，可简化为3行以内
    /// </summary>
    public void CreateNewAsset()
    {
        if (EditorWindow.GetWindow<StoryLinker>().nodeView.Info == null)
        {
            PopupWnd.ShowWindow();
            return;
        }
        bool result = EditorUtility.DisplayDialog("警告", $"您确定要新建视图吗？这会丢掉当前的资产视图\n\nTip:创建后的资产会自动保存修改内容\nTip:未保存的资产会丢失，请及时保存资产", "确认", "取消");
        if (result)
        {
            PopupWnd.ShowWindow();
        }
    }

    /// <summary>
    /// 【建议】硬编码 margin 值改用 USS 类名
    ///   assetName.AddToClassList("creator-tool__asset-name");
    /// </summary>
    private void AddAssetNameField()
    {
        assetName = new TextField();
        assetName.label = "资产名:";
        assetName.labelElement.style.marginTop = -1;
        assetName.style.width = 240;
        assetName.labelElement.style.marginRight = -100;
        assetName.SetEnabled(false);
        Add(assetName);
    }

    /// <summary>
    /// 【建议】用 ToolbarSpacer 替代手动创建的 Flex
    ///   Add(new ToolbarSpacer());
    /// </summary>
    private void CreateFlex()
    {
        var flex = new VisualElement();
        flex.style.flexGrow = 1;
        Add(flex);
    }
}
