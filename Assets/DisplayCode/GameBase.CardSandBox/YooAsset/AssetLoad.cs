#if UNITY_EDITOR
# endif
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

public class AssetLoad
{
    static ResourcePackage Package;
    public static IEnumerator Create()
    {
        var packetName = "DefaultPackage";
        YooAssets.Initialize();
        Package = YooAssets.CreatePackage(packetName);
        YooAssets.SetDefaultPackage(Package);
#if UNITY_EDITOR
        var initParameters = new EditorSimulateModeParameters();
        var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(
            EDefaultBuildPipeline.BuiltinBuildPipeline, packetName);
        initParameters.SimulateManifestFilePath = simulateManifestFilePath;
#else
        var initParameters = new OfflinePlayModeParameters();
#endif
        var initOperation = Package.InitializeAsync(initParameters);
        yield return initOperation;
        if (initOperation.Status == EOperationStatus.Succeed)
        {
            Debug.Log("资源包初始化成功！");
        }
        else
        {
            Debug.LogError($"资源包初始化失败：{initOperation.Error}");
        }
    }
    public static async Task<T> InstantiateAsync<T>(Transform parent) where T : class
    {
        string name = typeof(T).Name;
        AssetHandle handle = Package.LoadAssetAsync<GameObject>(name);
        await handle.Task;
        GameObject go = handle.InstantiateSync(parent);
        return go.GetComponent<T>();
    }
    public static T InstantiateSync<T>(Transform parent) where T : class
    {
        string name = typeof(T).Name;
        AssetHandle handle = Package.LoadAssetSync<GameObject>(name);
        GameObject go = handle.InstantiateSync(parent);
        return go.GetComponent<T>();
    }
    public static T InstantiateSync<T>(string name, Transform parent) where T : class
    {
        AssetHandle handle = Package.LoadAssetSync<GameObject>(name);
        GameObject go = handle.InstantiateSync(parent);
        return go.GetComponent<T>();
    }
    public static async Task<T> GetObjectAsync<T>(string name) where T : Object
    {
        AssetHandle handle = Package.LoadAssetAsync<Object>(name);
        await handle.Task;
        return handle.AssetObject as T;
    }
    public static T GetObjectSync<T>(string name) where T : Object
    {
        AssetHandle handle = Package.LoadAssetSync<Object>(name);
        if (handle.AssetObject == null)
            return null;
        return handle.AssetObject as T;
    }
    public static Sprite GetSpriteSync(string name)
    {
        AssetHandle handle = Package.LoadAssetSync<Object>(name);
        var texture = handle.AssetObject as Texture2D;
        if (texture == null)
        {
            return null;
        }
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    /// <summary>
    /// 通过资源 Key 和泛型类型从 YooAsset 同步获取 ScriptableObject 配置
    /// 编辑器环境下：若资源不存在，自动在指定路径创建并保存新的配置文件
    /// 非编辑器环境下：资源不存在会直接报错
    /// </summary>
    /// <typeparam name="T">要获取的 ScriptableObject 类型</typeparam>
    /// <param name="key">资源唯一标识 Key</param>
    /// <param name="noFullName">true= fullName = key;false=fullName = $"{typeName}_{key}";</param>
    /// <returns>获取或自动创建成功的 ScriptableObject 实例</returns>
    public static T GetSo<T>(string key, bool noFullName = false) where T : ScriptableObject
    {
        var typeName = typeof(T).Name;
        var path = $"Assets/GameRes/Info/{typeName}";
        var fullName = string.Empty;
        if (noFullName)
        {
            fullName = key;
        }
        else
        {
            fullName = $"{typeName}_{key}";
        }

        var info = GetObjectSync<T>(fullName);
        if (info == null)
        {
            info = ScriptableObject.CreateInstance<T>();
            // EditorInRuntime.CreateAsset(info, path, fullName);
            Debug.Log($"{fullName} not found，but is created（Runtime only）");
        }
        return info;
    }
}
