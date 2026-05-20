# AnimProcessSystem — 动画播放队列

管理卡牌特效/技能动画的播放顺序，支持串行入队和并行追加。

## 用法

```csharp
var process = AnimProcess.Create("攻击");
process.Append("火花", sparkSeq);  // 并行追加
process.AddInterval(0.5f);         // 间隔
process.Join(flashSeq);            // 同步播放

AnimProcessManager.Enqueue(process);  // 入队自动播放
```

## 设计意图

- 单播（`Join`/`Insert`）和并行（`Append`）分离，方便设计复合特效
- 队列机制保证多段动画不重叠
- 回调钩子（`onComplete`）串接后续逻辑

## 依赖

- `DG.Tweening`
