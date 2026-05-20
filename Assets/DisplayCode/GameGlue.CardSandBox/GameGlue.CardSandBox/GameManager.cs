using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager GetInstance() => instance;
    [SerializeField] Transform managerContainer;
    [SerializeReference] Dictionary<Type, IManager> managerDict = new();
    IEnumerator Start()
    {
        QualitySettings.vSyncCount = 0; // 关闭垂直同步
        Application.targetFrameRate = 30; // 锁定30帧

        instance = this;

        yield return AssetLoad.Create();
    }
    private void Update()
    {
        debug?.Invoke();
    }
    public Action debug;
    public static Transform GetContainer() => instance.managerContainer;
    public static T Get<T>() where T : class, IManager, new()
    {
        if (!instance.managerDict.TryGetValue(typeof(T), out IManager value))
        {
            T manager = new T();
            instance.managerDict[typeof(T)] = manager;
            manager.Init();
            return manager;
        }
        return value as T;
    }
    public static void RenameParentChild(GameObject parent)
    {
        foreach (var script in parent.gameObject.GetComponents<MonoBehaviour>())
        {
            var type = script.GetType();
            if (type == typeof(TextMeshProUGUI))
            {
                parent.gameObject.name = "Text";
            }
            if (type.Namespace != null) continue;
            Debug.Log("找到第一个自定义脚本: " + type.Name);
            parent.gameObject.name = type.Name;
            break;
        }
    }
}
