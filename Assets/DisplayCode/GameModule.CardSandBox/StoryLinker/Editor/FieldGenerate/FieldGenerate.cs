using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
public static class FieldGenerate
{
        public static void Start(object data, VisualElement parent, Action<object, VisualElement> extendSwitch, params string[] exculdes)
        {
            if (data == null) return;
            if (exculdes == null) exculdes = Array.Empty<string>();

            MonoTitle(data, parent);

            foreach (FieldInfo field in data.GetType().GetFields())
            {
                var fieldType = field.FieldType;
                var fieldName = field.Name;
                var fieldValue = field.GetValue(data);

                foreach (var t in exculdes)
                {
                    if (fieldName == t) goto NextIteration;
                }

                if (typeof(Object).IsAssignableFrom(fieldType))
                {
                    var objectField = new ObjectField(fieldName) { value = (Object)fieldValue, objectType = fieldType };
                    DefaultCheck(objectField, objectField.value == null);
                    objectField.RegisterValueChangedCallback(evt =>
                    {
                        field.SetValue(data, evt.newValue);
                        DefaultCheck(objectField, objectField.value == null);
                    });
                    parent.Add(objectField);
                    continue;
                }

                if (fieldType == typeof(string))
                {
                    if (fieldValue == null)
                    {
                        fieldValue = string.Empty;
                    }
                }

                switch (fieldValue)
                {
                    case int value:
                        var intField = new IntegerField(fieldName) { value = value };
                        intField.RegisterValueChangedCallback(evt => field.SetValue(data, evt.newValue));
                        parent.Add(intField);
                        break;

                    case float value:
                        var floatField = new FloatField(fieldName) { value = value };
                        floatField.RegisterValueChangedCallback(evt => field.SetValue(data, evt.newValue));
                        parent.Add(floatField);
                        break;

                    case string value:

                        var stringField = new TextField(fieldName);
                        if (value != null)
                        {
                            stringField.value = value;
                        }
                        else
                        {
                            stringField.value = string.Empty;
                        }
                        DefaultCheck(stringField, stringField.value == string.Empty);
                        stringField.RegisterValueChangedCallback(evt =>
                        {
                            field.SetValue(data, evt.newValue);
                            DefaultCheck(stringField, stringField.value == string.Empty);
                        });
                        stringField.multiline = true;
                        stringField.labelElement.style.marginRight = -60;
                        stringField.style.textOverflow = TextOverflow.Ellipsis;
                        parent.Add(stringField);
                        break;

                    case bool value:
                        var toggleField = new Toggle(fieldName) { value = value };
                        toggleField.RegisterValueChangedCallback(evt => field.SetValue(data, evt.newValue));
                        parent.Add(toggleField);
                        break;

                    case Object value:
                        var objectField = new ObjectField(fieldName) { value = value, objectType = value.GetType() };
                        DefaultCheck(objectField, objectField.value == null);
                        objectField.RegisterValueChangedCallback(evt =>
                        {
                            field.SetValue(data, evt.newValue);
                            DefaultCheck(objectField, objectField.value == null);
                        });
                        parent.Add(objectField);
                        break;

                    case Color value:
                        var colorField = new ColorField(fieldName) { value = value };
                        colorField.RegisterValueChangedCallback(evt => field.SetValue(data, evt.newValue));
                        parent.Add(colorField);
                        break;

                    case Enum value:
                        var enumField = new EnumField(fieldName, value) { value = value };
                        DefaultCheck(enumField, (int)fieldValue == 0);
                        enumField.RegisterValueChangedCallback(evt =>
                        {
                            field.SetValue(data, evt.newValue);
                            DefaultCheck(enumField, Convert.ToInt32(enumField.value) == 0);
                        });
                        enumField.RegisterCallback<MouseDownEvent>(evt =>
                        {
                            if (evt.button == 0 && evt.clickCount == 2)
                            {
                                HighlightScript(data);
                            }
                        });
                        parent.Add(enumField);
                        break;

                    case IList value:
                        parent.Add(StringLabel(fieldName));
                        parent.Add(new DynamicList(value));
                        break;

                    default:
                        extendSwitch?.Invoke(fieldValue, parent);
                        break;
                }
            NextIteration:;
            }
        }
        private static void MonoTitle(object data, VisualElement parent)
        {
            var monoScript = MonoScript.FromScriptableObject(data as ScriptableObject);
            if (monoScript == null)
            {
                monoScript = MonoScript.FromMonoBehaviour(data as MonoBehaviour);
            }
            if (monoScript)
            {
                var monoField = new ObjectField("Script")
                {
                    objectType = data.GetType(),
                };
                monoField.ElementAt(1).SetEnabled(false);
                monoField.value = monoScript;
                monoField.RegisterCallback<ClickEvent>(evt =>
                {
                    if (evt.clickCount == 1)
                    {
                        EditorGUIUtility.PingObject(monoScript);
                    }
                    if (evt.clickCount != 2) return;
                    if (monoField.value != null)
                    {
                        AssetDatabase.OpenAsset(monoField.value);
                    }
                });

                parent.Add(monoField);
            }
        }

        public static Label StringLabel(string fieldName)
        {
            return new Label($"{fieldName}") { style = { marginLeft = 2, marginTop = 4, marginBottom = 4, unityFontStyleAndWeight = FontStyle.Bold } };
        }

        public static void DefaultCheck<T>(BaseField<T> valueField, bool isNull)
        {
            if (isNull)
            {
                valueField.style.backgroundColor = Color.red * 0.3f;
            }
            else
            {
                valueField.style.backgroundColor = Color.clear;
            }
        }
        public static void HighlightScript(object obj)
        {
            System.Type type = obj.GetType();
            MonoScript script = FindScript(type);
            if (script != null)
            {
                EditorGUIUtility.PingObject(script);
            }
            else
            {
                Debug.LogError($"未找到 {type.Name} 相关的脚本！");
            }
        }
        public static MonoScript FindScript(Type type)
        {
            //var script = MonoScript.FindObjectOfType(type);
            //return (MonoScript)script;
            string[] guids = AssetDatabase.FindAssets("t:MonoScript");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (script != null && script.GetClass() == type)
                {
                    return script;
                }
            }
            return null;
        }
    }
