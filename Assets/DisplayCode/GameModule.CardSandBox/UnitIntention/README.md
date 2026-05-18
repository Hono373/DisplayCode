# UnitIntention 模块

> 敌人意图决策系统 - 基于树形结构的加权随机行为选择器

## 是什么

卡牌游戏中决定敌方单位**下一步行为**（攻击/防御/逃跑等）的系统。

**核心设计**：树形节点 + 条件分支 + 加权随机选择

## 架构

```
UnitIntention (Controller)
    └── UnitIntentionInfo (根)
            └── StatusInfo[] (行为栏)
                    └── SkillInfo[] (技能 - 叶子节点)
                            └── SkillEffectInfo[] (效果)

View (UI层)
    └── IntentionUI + IntentionUIData
```

**决策流程**：根节点 → 递归检查 `ConditionBox` → 加权随机 → 叶子节点

## 目录结构

```
UnitIntention/
├── UnitIntention/
│   ├── Base/GameNode.cs          # 树形节点基类
│   ├── Box/
│   │   ├── ConditionBox.cs       # 条件盒 - 返回子节点索引
│   │   └── SelectorBox.cs        # 选择器盒 - 筛选目标单位
│   ├── Controller/
│   │   └── UnitIntention.cs      # 意图管理器（随机决策/存档恢复）
│   ├── Data/
│   │   ├── IntentionData.cs     # 存档数据 [行为栏索引, 技能索引]
│   │   └── IntentionUIData.cs   # UI显示数据
│   ├── Info/
│   │   ├── UnitIntentionInfo.cs  # 根节点
│   │   ├── StatusInfo.cs         # 行为栏
│   │   ├── SkillInfo.cs          # 技能（叶子）
│   │   ├── SkillEffectInfo.cs    # 技能效果
│   │   ├── BattleIconInfo.cs     # 图标资源
│   │   └── Null*.cs              # 空实现
│   ├── Interface/
│   │   ├── IGameCondition.cs      # 条件接口
│   │   ├── ISkillInfo.cs           # 技能接口
│   │   └── GameConditionExample.cs # 条件示例
│   ├── View/
│   │   └── IntentionUI.cs        # UI视图抽象类
│   └── WeightedRandomSelector.cs  # 加权随机选择器
├── GameRes/                       # 资源（asset + sprite）
├── Test/                          # 测试代码
├── TestScene.unity                # 测试场景
└── README.md
```

## 关键类

| 类 | 作用 |
|---|------|
| `GameNode` | 树形节点基类 |
| `UnitIntention` | 意图管理器（随机决策/存档恢复） |
| `UnitIntentionInfo` | 意图根节点 |
| `StatusInfo` | 行为栏（如"攻击"） |
| `SkillInfo` | 技能叶子节点 |
| `ConditionBox` | 条件盒 |
| `SelectorBox` | 选择器盒 |
| `WeightedRandomSelector` | 前缀和+二分 O(log n) 加权随机 |
| `IntentionUI` | UI视图抽象类 |

## 接口依赖

- `IGameCondition` - 条件判断（胶水层实现）
- `IUnitSelector` - 目标筛选（胶水层实现）
- `IEffectInfo` - 效果执行（胶水层实现）

## 测试

**场景测试** (`TestScene.unity`):
1. 挂载 `Test.cs`，配置 `UnitMockInfo`
2. Inspector 点击 **"获取技能存档与技能信息"**
3. 查看 Console 输出

**单元测试** (`Test/UnitIntention.cs` → 已变更为 `Controller/UnitIntention.cs`):
```csharp
var intention = new UnitIntention();
int[] indexs = intention.GetSkillIndexs(info);           // 随机决策
var skill = intention.GetSKill(IntentionData.Create(indexs), info); // 存档恢复
```

## 常见问题

| 错误 | 原因 |
|-----|------|
| "Weights未配置" | 非叶子节点没有子节点 |
| 图标显示 Unknown | `BattleIconInfo.asset` 未配置 |
