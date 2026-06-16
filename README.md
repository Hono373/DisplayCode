# DisplayCode

回合制卡牌游戏的项目模块。

## 系统模块

| 模块 | 说明 |
|------|------|
| **UnitIntention** | 敌人意图决策，加权随机 + 条件分支 |
| **AnimProcessManager** | 动画播放队列，串/并行管理 |
| **ModifierManager** | Buff/修饰符生命周期管理 |
| **StoryLinker** | 剧情节点编辑器，GraphView 可视化 |

## 架构

```
GameBase.CardSandBox   底层基础库（接口、扩展、资源加载）
  ├─ AnimProcessManager  动画队列
  └─ ModifierManager     Buff 系统
    ↓
GameGlue.CardSandBox   胶水层（Manager 入口、运行时整合）
    ↓
GameModule.CardSandBox 业务模块
  ├─ StoryLinker         剧情编辑器
  └─ UnitIntention       敌人意图
```

## 依赖

- DOTween Pro — 动画引擎
- Odin Inspector — 编辑器增强
- YooAsset — 资源热更新
