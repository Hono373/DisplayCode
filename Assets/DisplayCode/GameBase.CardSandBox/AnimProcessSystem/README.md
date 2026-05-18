# AnimProcessSystem

> 动画流程队列系统 - 基于 DG.Tweening

## 是什么

管理动画队列播放，支持串行入队、并行追加、回调钩子。

## 结构

```
AnimProcessManager (队列管理)
    └── 队列 → AnimProcess (单个动画)
            ├── 主 Sequence
            ├── appendDict (并行追加序列)
            └── 回调 (onPrepend/onComplete)
```

## 目录

```
AnimProcessSystem/
├── Scripts/
│   ├── AnimProcess.cs          # 动画进程
│   ├── AnimProcessManager.cs    # 队列管理
│   └── SequenceExtension.cs     # Sequence 工厂
└── AnimProcessSystem.asmdef
```

## 核心类

| 类 | 作用 |
|---|------|
| `AnimProcess` | 封装单个动画，含主序列 + 并行序列 |
| `AnimProcessManager` | 队列管理（入队/播放/清空） |
| `SequenceExtension` | Sequence 工厂（带描述ID） |

## 使用

```csharp
// 创建动画
var process = AnimProcess.Create("攻击");
process.Append("特效", effectSeq);
process.AddInterval(0.5f);
process.Join(otherSeq);

// 入队
AnimProcessManager.Enqueue(process);
AnimProcessManager.TryStart();

// 清空
AnimProcessManager.Clear();
```

## AnimProcess API

| 方法 | 说明 |
|-----|------|
| `Create(info)` | 创建进程 |
| `Append(name, seq)` | 追加并行序列 |
| `Join(seq)` | 主序列并行 |
| `Insert(time, seq)` | 插入指定时间 |
| `AddInterval(t)` | 添加间隔 |
| `AddPrepend(cb)` | 播放前回调 |
| `AddComplete(cb)` | 完成后回调 |

## AnimProcessManager API

| 方法 | 说明 |
|-----|------|
| `Enqueue(p)` | 入队 |
| `TryStart()` | 启动下一个 |
| `Clear()` | 清空队列 |

## 依赖

- `DG.Tweening`
