using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.EditorGUIUtility;
public class AssetWnd : EditorWindow
{
        static StoryLinker creator;
        ListView AssetList;
        public static void ShowWindow(StoryLinker wnd)
        {
            creator = wnd;
            var window = GetWindow<AssetWnd>();
            window.maxSize = window.minSize = new Vector2(240, 600);
        }
        public void CreateGUI()
        {
            titleContent = new GUIContent("资源列表[StoryData]", IconContent("d_Folder Icon").image as Texture2D);

            AssetList = CreateList();
            rootVisualElement.Add(AssetList);
        }
        private ListView CreateList()
        {
            var listView = new ListView();

            listView.style.flexGrow = 1;

            var allObjectGuids = AssetDatabase.FindAssets("t:StoryInfo", new[] { "Assets/GameRes" });
            // 查找所有类型为 StoryInfo 的资源 GUID，在指定路径下查找

            var allObjects = new List<StoryInfo>();  // 创建一个 StoryInfo 类型的列表，用来存储所有加载的 StoryInfo 对象
            foreach (var guid in allObjectGuids)
            {
                var asset = AssetDatabase.LoadAssetAtPath<StoryInfo>(AssetDatabase.GUIDToAssetPath(guid));
                if (asset != null)
                {
                    allObjects.Add(asset);
                }
                else
                {
                    Debug.LogWarning($"无法加载资源，GUID: {guid}");
                }
            }
            listView.Clear();

            listView.makeItem = () => new Label();  // 设置 ListView 创建一个新的 StringLabel 来表示每一项

            // 设置每项的绑定方式：将每个 StringLabel 的文本设置为对应 StoryInfo 对象的名称
            listView.bindItem = (item, index) => 
            { 
                var label = item as Label;
                if (label != null && index < allObjects.Count && allObjects[index] != null)
                {
                    label.text = allObjects[index].name;
                }
                else
                {
                    label.text = "无效资源";
                }
            };

            listView.itemsSource = allObjects;  // 设置 ListView 的数据源为所有加载的 StoryInfo 对象

            listView.selectionChanged += WhenSelectionChange;  // 注册选择变化时的事件处理方法
            return listView;  // 返回创建的 ListView
        }

        // 当选择发生变化时的事件处理方法
        private void WhenSelectionChange(IEnumerable<object> enumerable)
        {
            var storyData = enumerable?.FirstOrDefault() as StoryInfo;  // 获取选中的第一个 StoryInfo 对象
            if (storyData == null) return;
            
            bool result = EditorUtility.DisplayDialog("读取确认", $"您确定要读取{storyData.name}吗？ \n (当前的视图会被重建，但做出的修改已经被保存)", "是", "否");
            // 弹出确认对话框，询问是否继续读取选中的 StoryInfo 对象

            if (result)  // 如果用户点击了"是"
            {
                creator.LoadData(storyData);  // 使用已保存的 creator 引用，而不是获取新窗口实例
            }
        }

        // OnFocus 方法在窗口获得焦点时调用，用于重新加载列表（例如，确保数据同步）
        private void OnFocus()
        {

        }
        private void OnProjectChange()
        {
            if (AssetList != null)
            {
                rootVisualElement.Remove(AssetList);
                AssetList = CreateList();
                rootVisualElement.Add(AssetList);
            }
        }
        // OnDestroy 方法在窗口销毁时调用，这里暂时没有具体实现
        private void OnDestroy()
        {

        }
    }

