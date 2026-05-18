using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupWnd : EditorWindow
{
        string input;
        static StoryLinker creator;
        Label tip;
        Button confirm;
        public static void ShowWindow(StoryLinker creator)
        {
            PopupWnd.creator = creator;
            var wnd = CreateWindow<PopupWnd>();
            wnd.maxSize = wnd.minSize = new Vector2(300, 72);
        }
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
                creator.CreateAsset(input);
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
        private void OnDestroy()
        {
            Debug.Log("PopupWnd Destroyed");
        }
    }
