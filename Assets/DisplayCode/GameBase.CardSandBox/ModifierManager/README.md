# ModifierManager — 修饰符（Buff/效果）系统

管理单位身上的增益/减益效果，支持运行时动态注册和触发。

## 核心类

| 类 | 职责 |
|----|------|
| Modifier | 修饰符实例，包含数据和目标对象引用 |
| ModifierBar | 单位身上的 Modifier 容器，增删查 |
| ModifierManager | 全局调度器，按 EffectType 分类管理和广播 |
| ModifierEffectInfo\<T\> | 泛型效果定义，实现具体的执行逻辑 |
| ModifierEffect | 效果执行包装层 |
| ModifierInfo | ScriptableObject 配置（key、图标、效果列表） |
| ModifierCreator | 创建 Modifier 时的数据传输对象 |

## 使用流程

1. 继承 `ModifierEffectInfo<T>` 实现效果逻辑
2. 在 `ModifierInfo.asset` 中配置效果列表
3. 调用 `ModifierManager.Register()` 注册效果类型
4. 通过 `ModifierManager.Send<IContext>()` 广播触发

## 接口

| 接口 | 说明 |
|------|------|
| IContext | 泛型效果上下文标记 |
| IModifierEffectInfo | 效果信息接口（EffectType / 描述 / 激活判断） |
| IModifierUnitObj | 单位对象接口（实例化） |

## 程序集

GameBase.ModifierManager，依赖 GameBase.CardSandBox。
