using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 弹出窗口：用于输入新资产的名称
/// 
/// 【问题分析】
/// 1. 使用静态字段 creator，可能导致内存泄漏
/// 2. 窗口大小固定（300x72），不能根据内容调整
/// 3. StringCheck 方法在每次输入时都调用，性能较差
/// 4. 使用正则表达式检查中文字符，但只给警告，不阻止创建
/// 
/// 【推荐实现】
/// - 使用 Modal 窗口或对话框
/// - 将创建逻辑放到 StoryLinker 中
/// - 使用 Unity 的 EditorUtility.SaveFilePanelInProject
/// </summary>
public class PopupWnd : EditorWindow
{
    string input;
    Label tip;
    Button confirm;

    public static void ShowWindow()
    {
        var wnd = GetWindow<PopupWnd>("新建资产");
        wnd.maxSize = wnd.minSize = new Vector2(300, 72);
    }

    /// <summary>
    /// 创建 UI
    /// 
    /// 【问题】
    /// 1. 使用 "Customized" 作为图标，可能不存在
    /// 2. 没有设置窗口为 Modal（模态）
    /// 3. 确认按钮先 Close() 再调用 CreateAsset()，可能导致 creator 为 null
    /// 
    /// 【推荐实现】
    /// public void CreateGUI()
    /// {
    ///     titleContent = new GUIContent("新建资产", EditorGUIUtility.IconContent("SaveAs").image as Texture2D);
    ///     
    ///     // 使用 VisualElement 布局
    ///     var root = rootVisualElement;
    ///     root.style.paddingLeft = root.style.paddingRight = 10;
    ///     
    ///     var textField = new TextField("资产名:");
    ///     textField.RegisterValueChangedCallback(evt => StringCheck(evt.newValue, confirm));
    ///     root.Add(textField);
    ///     
    ///     // 按钮区域
    ///     var buttonBox = new VisualElement() { style = { flexDirection = FlexDirection.Row } };
    ///     buttonBox.style.marginTop = 10;
    ///     
    ///     var confirmBtn = new Button(() => CreateAsset()) { text = "确认" };
    ///     // ...
    /// }
    /// </summary>
    public void CreateGUI()
    {
        titleContent = new GUIContent("新建资产", EditorGUIUtility.IconContent("Customized").image as Texture2D);

        var top = new VisualElement();
        top.style.flexDirection = FlexDirection.Row;
        top.style.marginTop = 8;

        rootVisualElement.Add(top);

        var label = new Label("新资产名:");
        label.style.marginTop = 2;
        label.style.marginLeft = 4;
        top.Add(label);

        var textField = new TextField();
        textField.RegisterValueChangedCallback(evt =>
        {
            input = evt.newValue;
            StringCheck(input, confirm);
        });
        textField.style.flexGrow = 1;
        top.Add(textField);

        var buttonBox = new VisualElement();
        buttonBox.style.marginTop = 4;
        buttonBox.style.flexDirection = FlexDirection.Row;
        rootVisualElement.Add(buttonBox);

        confirm = new Button(() =>
        {
            Close();
            GetWindow<StoryLinker>().CreateAsset(input);
        })
        { text = "确认" };
        confirm.SetEnabled(false);
        confirm.style.flexGrow = 1;
        buttonBox.Add(confirm);

        var cancel = new Button(() =>
        {
            Close();  // 关闭当前窗口
        })
        { text = "取消" };
        cancel.style.flexGrow = 1;
        buttonBox.Add(cancel);

        tip = new Label();
        rootVisualElement.Add(tip);
    }

    /// <summary>
    /// 【建议】缓存正则表达式避免每次输入都重新编译
    ///   private static readonly Regex Pattern = new Regex("[<>:\"/\\\\|?*]", RegexOptions.Compiled);
    /// </summary>
    void StringCheck(string input, Button confirm)
    {
        Regex specialCharPattern = new("[<>:\"/\\\\|?*]");
        Regex chineseCharPattern = new("[\u4e00-\u9fa5\\p{P}]");

        if (input == "")
        {
            confirm.SetEnabled(false);
            tip.text = "Tip:资产名不能为空";
        }
        else if (input.StartsWith(" "))
        {
            confirm.SetEnabled(false);
            tip.text = "Tip:资产名首个字符不能为空格";
        }
        else if (specialCharPattern.IsMatch(input))
        {
            confirm.SetEnabled(false);
            tip.text = "Tip:资产名不能包含以下特殊字符: < > : \" / \\ | ? *";
        }
        else if (input.Length > 100)
        {
            confirm.SetEnabled(false);
            tip.text = "Tip:资产名的长度不能超过100个字符";
        }
        else if (AssetDatabase.FindAssets($"t:StoryInfo {input}").Length > 0)
        {
            confirm.SetEnabled(false);
            tip.text = "Tip:同名资产已存在";
        }
        else if (chineseCharPattern.IsMatch(input))
        {
            confirm.SetEnabled(true);
            tip.text = "Tip:资产名不推荐包含中文字符";
        }
        else
        {
            confirm.SetEnabled(true);
            tip.text = "Tip:这个资产名可以使用";
        }
    }
}
