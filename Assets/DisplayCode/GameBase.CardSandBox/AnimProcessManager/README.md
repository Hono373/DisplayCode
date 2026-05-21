# AnimProcessManager — 动画播放队列

管理动画的播放顺序，支持串行排队和并行追加。

## 用法

```csharp
var process = AnimProcess.Create("攻击动画");
process.Append("火花", sparkSeq);  // 和上一个并行播放
process.AddInterval(0.5f);          // 等待 0.5 秒
process.Join(flashSeq);             // 跟上个动画同步播放

AnimProcessManager.Enqueue(process);  // 入队后自动串联播放
```

## API

| 方法 | 行为 |
|------|------|
| `Append(label, sequence)` | 并行执行，不等待上一个结束 |
| `Join(label, sequence)` | 和上一个 Append/Join 同步执行 |
| `Insert(time, label, sequence)` | 在指定时间点插入 |
| `AddInterval(seconds)` | 插入一段空等待 |

## 依赖

- DG.Tweening (DOTween)
- GameBase.CardSandBox
