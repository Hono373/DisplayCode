# UnitIntention — 敌人意图决策系统

决定敌方单位下回合行为的系统，意图提前展示给玩家。

## 决策流程

条件判断 → 加权随机 → 选中行为 → 选中技能 → 展示意图

## 配置链

| 配置项 | 类型 | 作用 |
|--------|------|------|
| UnitIntentionInfo | ScriptableObject | 行为树根，列出一组行为条 |
| StatusInfo | ScriptableObject | 单个行为栏（如"猛攻"），含权重和技能列表 |
| SkillInfo | ScriptableObject | 具体技能，含权重和目标选择规则 |
| BattleIconInfo | ScriptableObject | 意图图标映射表，技能 → 图标 |
| NullSkillInfo / NullEffectInfo | ScriptableObject | 无技能/无效果时的占位引用 |

## 核心组件

| 组件 | 说明 |
|------|------|
| GameNode | 行为树节点基类，统一条件判断和加权随机的入口 |
| ConditionBox | 条件盒，检查战斗状态（血量、Buff等），返回子节点索引 |
| SelectorBox | 选择器盒，组合单位筛选条件 |
| WeightedRandomSelector | 加权随机选择器，前缀和 + 二分查找，O(log n) |
| UnitIntention (Controller) | 意图控制器，遍历行为树生成当前回合的意图数据 |

## 程序集

| 程序集 | 说明 |
|--------|------|
| GameModule.CardSandBox.UnitIntention | 核心逻辑 |
| GameModule.CardSandBox.UnitIntention.Test | 测试脚本 |
