using System;
using System.Collections;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
public class DynamicList : ListView
    {
        public DynamicList(IList list)
            : base(list, -1, null, null)
        {
            showAddRemoveFooter = true;
            //showFoldoutHeader = true;
            //var foldout = this.Q(foldoutHeaderUssClassName);
            //if (foldout is Foldout target)
            //{
            //    target.value = false;
            //}

            reorderable = true;
            reorderMode = ListViewReorderMode.Simple;
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

            makeItem = MakeItem;
            bindItem = BindItem;

            var element = this.Q(arraySizeFieldUssClassName);
            if (element is TextField textField)
            {
                textField.SetEnabled(false);
            }

            ExchangeAddBtn(list);
        }
        private void ExchangeAddBtn(IList list)
        {
            Type listType = list.GetType();
            if (!listType.IsGenericType) return;

            Type elementType = listType.GetGenericArguments()[0]; // 获取泛型参数类型
            //Debug.Log($"列表元素类型: {elementType}");

            var typeList = DerivedReflection.GetDerivedTypes(elementType);

            var typeName = elementType.Name;

            var element = this.Q("unity-list-view__add-button");
            if (element == null) return;
            var btnContainer = element.parent;
            if (btnContainer == null) return;
            element.RemoveFromHierarchy();

            headerTitle = typeName;
            if (typeList.Length == 0)
            {
                btnContainer.Add(new Button(() => CreateChildToList(elementType)) { text = "+" });
            }
            else
            {
                btnContainer.Add(new Button(() => GenericMenu(typeList)) { text = "+" });
            }

        }
        #region 默认实现
        VisualElement MakeItem()
        {
            var foldout = new Foldout();
            return foldout;
        }

        void BindItem(VisualElement element, int index)
        {
            element.Clear();

            var data = element.userData = itemsSource[index];

            element.RegisterCallback<MouseDownEvent>(evt =>
            {
                var node = GetFirstAncestorOfType<Node>();
                if (node != null)
                    node.capabilities &= ~Capabilities.Movable;
                if (evt.button == 0 && evt.clickCount == 2)
                {
                    FieldGenerate.HighlightScript(data);
                }
            });

            element.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                var node = GetFirstAncestorOfType<Node>();
                if (node != null)
                    node.capabilities |= Capabilities.Movable;
            });

            if (element is Foldout foldout)
            {
                foldout.text = data.GetType().Name;
                foldout.style.unityFontStyleAndWeight = FontStyle.Bold;
            }

            FieldGenerate.Start(data, element, null);
        }

        void GenericMenu(params Type[] types)
        {
            var menu = new GenericMenu();
            foreach (var type in types)
            {
                menu.AddItem(new GUIContent(type.Name), false,
                    () =>
                    {
                        CreateChildToList(type);
                    });
            }
            menu.ShowAsContext();
        }

        void CreateChildToList(Type type)
        {
            if (type == typeof(string))
            {
                itemsSource.Add(string.Empty);
            }
            else
            {
                try
                {
                    var constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor != null)
                    {
                        itemsSource.Add(Activator.CreateInstance(type));
                    }
                    else
                    {
                        Debug.LogError($"类型 {type.Name} 没有无参构造函数，无法实例化");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"创建 {type.Name} 实例时出错: {ex.Message}");
                }
            }

            Rebuild();
        }
        #endregion
    }
