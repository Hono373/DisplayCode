using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
public class AnimProcessManager
{
    int log;
    private static readonly object _lock = new();
    private static AnimProcessManager _instance;
    public static AnimProcessManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new AnimProcessManager();
                }
            }
            return _instance;
        }
    }
    private AnimProcessManager() { }
    Queue<AnimProcess> queue = new();
    AnimProcess current;
    public void Init() { }
    public static void Enqueue(AnimProcess pa)
    {
        var instance = Instance;
        pa.SetString(instance.log++.ToString());
        instance.queue.Enqueue(pa);
        TryStart();
    }
    public static void TryStart()
    {
        var ins = Instance;
        if (ins.current == null)
        {
            if (ins.queue.Count > 0)
            {
                var pa = ins.queue.Dequeue();
                ins.current = pa;
                Debug.Log($"[动画 {pa.GetString()}]{pa.Info()} 播放开始");
                ins.current.Play();
            }
        }
    }
    public static void Clear()
    {
        var ins = Instance;

        var destroy = ins.current;
        DOVirtual.DelayedCall(0.1f, () => destroy.Kill());

        ins.current = null;
        TryStart();
    }
}

