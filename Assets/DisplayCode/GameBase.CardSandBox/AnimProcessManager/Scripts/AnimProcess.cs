using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimProcess
{
    Sequence sequence;
    Dictionary<string, Sequence> appendDict = new();
    Action onPrepend;
    Action onComplete;
    AnimProcess() { }
    public static AnimProcess Create(string info)
    {
        var p = new AnimProcess();
        p.sequence = SequenceExtensions.Create(info).Pause().SetAutoKill(false);
        return p;
    }
    public void AddInterval(float spd)
    {
        var interval = SequenceExtensions.Create();
        interval.AppendInterval(spd);
        sequence.Append(interval);
    }
    public void Append(string info, Sequence seq)
    {
        if (!appendDict.TryGetValue(info, out var result))
        {
            appendDict[info] = result = SequenceExtensions.Create($"{info}").Pause();
        }
        result.Append(seq);
    }
    public void Join(Sequence seq) => sequence.Join(seq);
    public void Insert(float time, Sequence seq) => sequence.Insert(time, seq);
    public void Play()
    {
        foreach (var list in appendDict.Values)
        {
            sequence.Join(list);
        }

        sequence.onComplete += () => Debug.Log($"[动画 {sequence.stringId}]{Info()} 播放完毕");
        sequence.onComplete += () => onComplete?.Invoke();
        sequence.onComplete += () => AnimProcessManager.Clear();
        onPrepend?.Invoke();
        sequence.Play();
    }
    public string Info() => sequence.id.ToString();
    public void AddComplete(Action action) => onComplete += action;
    public void AddPrepend(Action action) => onPrepend += action;
    public void SetString(string v) => sequence.stringId = v;
    public string GetString() => sequence.stringId;
    public void Kill() => sequence.Kill();
}

