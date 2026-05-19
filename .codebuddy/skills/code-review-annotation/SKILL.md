---
name: code-review-annotation
description: Apply structured 【问题】+【建议】+ code annotations during file-by-file code review of Unity/UIElements projects.
---

# 代码审查注释方法论

## 适用场景

对代码模块进行 **逐文件审查**，在每个关键方法上添加结构化注释指出问题和改进方向时使用。

典型触发场景：
- 重构前的摸底审查
- 对新接手的模块做质量评估
- 对不熟悉的代码库做 first pass review

## 落地流程的教训（来自 StoryLinker 审查实战）

| 阶段 | 做了什么 | 踩了什么坑 |
|------|----------|-----------|
| 初版 | 凭对 Unity API 的记忆写注释 | 部分说法不准确，如说 `Port.Create<Edge>()` 不正确（实际是官方标准用法） |
| 查阅 API | 访问 Unity Scripting API 文档核实 | 修正了多处错误，也确认了一些批评是正确的 |
| 用户反馈 | 用户指出注释只提了概念名（"Undo 系统"）没解释 | 补了具体代码示例和用法说明 |
| 用户反馈 2 | 用户要求去掉"辩论式"内容 | 删掉了自我修正记录和 ASCII 图表等 |
| 压缩 | 用户要求只保留建议 + 代码 | 最终格式定型为【问题】+【建议】+ 关键代码 |
| 过头 | 压缩过度，注释只剩 1-2 行，缺乏上下文 | 明白"清晰＞简短"，该写的话要写够 |

## 核心规则

### 规则 1：分析流程——对照函数体与 API

不是凭空想"这个函数有什么问题"，而是读函数体内的调用，逐一对照 API。

```
函数体代码                      API 文档
───────                         ───────
style.left = x;        →   GraphElement.SetPosition(Rect)
GetWindow<T>()         →   GetWindow<T>(title, focus, desiredDockNextTo)
Port.Create<Edge>(...) →   确认官方标准用法
Info.Save()            →   SetDirty / SaveAssetIfDirty / Refresh 三层开销
```

具体步骤：
1. **读函数体**：列出手动调用、手动管理、硬编码写法
2. **查 API**：这类操作在基类或库中是否有封装好的方法？
3. **对比**：当前写法 vs API 推荐写法，差异就是【问题】
4. **写注释**：用【问题】+【建议】+ 代码演示呈现对比结果

### 规则 2：注释结构 —— 【问题】+【建议】+ 代码演示

建议必须附带简单的演示示例代码（1-5 行），不能只有概念描述。

```csharp
// ✅ 正确的：问题 + 建议 + 代码演示
/// <summary>
/// 【问题】每次操作都调 Save()，无法撤销且频繁写磁盘
/// 【建议】改为 EditorUtility.SetDirty 标记脏，关闭时再 SaveAssetIfDirty
/// </summary>

// ❌ 错误的：只有概念没有代码，读者不知道怎么改
/// 【建议】不要用 layout，用 GetPosition()

// ✅ 正确的：带了代码演示
/// 【建议】使用 GetPosition() 替代 layout
///   public Vector2 Pos => GetPosition().position;
```

不要混入辩论式内容或长篇原理：

```csharp
// ❌ 坏的：自我修正、ASCII 图表、教程级解释
/// <summary>
/// 但 Dialog 也有它的用途...（不要自我修正）
/// ┌──────────┐（不要 ASCII 图表）
/// Undo.RecordObject 的作用是...（不要教程）
/// </summary>
```

### 规则 3：不对空方法写注释

```csharp
// ✅ 正确的
void OnFocus() { }  // 留空，不写注释
void OnDestroy() { } // 留空，不写注释

// ❌ 错误的
/// <summary>
/// OnFocus 在窗口获得焦点时调用，当前没用到
/// 可以在这里加一些刷新...但暂时不需要...
/// </summary>
void OnFocus() { }
```

### 规则 4：不对已经正确的写法做伪批评

```csharp
// ✅ 正确的：指出是官方标准用法，只提可改进点
/// <summary>
/// Port.Create<Edge>() 是官方标准用法
/// 【建议】portType 可以设具体类型以限制连接
/// </summary>

// ❌ 错误的：说官方 API 用法不正确
/// <summary>
/// 【问题】Port.Create<Edge>() 使用 Edge 作为类型参数不正确
/// </summary>
```

## 详细程度要求

注释的目的是让读者理解问题和改进方向，不是为了追求行数少。

| 程度 | 特征 | 判断 |
|------|------|------|
| 适当 | 问题说清了问题，建议有代码演示，上下文自明 | 保留 |
| 过多 | 长篇原理、ASCII 图表、辩论式、自我修正 | 压缩 |
| 过少 | 只有概念没有代码，读者不知道怎么改 | 补充 |

判断标准：脱离讨论上下文后，只看注释能否理解问题和怎么做。

## 最终注释格式

```csharp
/// <summary>
/// 【问题】简要说明问题是什么
/// 【建议】怎么改 + 关键代码演示（1-5行）
/// </summary>
```

两个部分都必要。【问题】让读者知道有什么坑，【建议】告诉读者怎么改。代码演示让读者可以直接参考写法。

### 注释风格分级

| 类型 | 格式 | 使用场景 |
|------|------|----------|
| XML 文档注释 | `/// <summary>...</summary>` | 方法/字段的完整说明，包含问题和建议 |
| 行内说明 | `// 一句话` | 配合 XML 注释补充关键逻辑，解释"为什么这样做"而非"做了什么" |
| 流程注释 | `// Step N: ...` | 在多步骤方法中标注每个步骤的意图 |

### 行内注释示例

仅 XML 注释不足以解释核心逻辑时，在实现代码旁补充行内注释：

```csharp
// 节点从面板移除时释放 Odin 属性树，避免回调持有强引用导致无法 GC
RegisterCallback<DetachFromPanelEvent>(_ => DisposePropertyTree());

// IMGUIContainer 嵌入 Odin IMGUI 绘制，复用 Odin 的抽屉系统
extensionContainer.Add(new IMGUIContainer(() =>
{
    _propertyTree?.UpdateTree();
    _propertyTree?.Draw();
}));
```

行内注释不重复 XML 注释已说明的内容，只补充"实现层面的意图"。

## 审查后的流程推演

### 场景：第三方库（Odin）接管渲染后的逻辑断链检查

当审查涉及将手写 UI 生成替换为第三方库渲染时，逐链路检查输入输出是否一致：

**替换前链路：**
```
用户编辑字段 → FieldGenerate.Start() 反射 SetValue → 直接修改内存对象
```

**替换后链路：**
```
用户编辑字段 → Odin PropertyTree 内部赋值 → 同一内存对象被修改
```

**断链检查清单（逐条对比增量）：**

| 检查项 | 旧方案 | 新方案 | 是否断链 |
|--------|--------|--------|----------|
| 字段可见性 | 通过 `excludes` 数组排除指定字段（如 `parent`） | Odin 显示所有 `[Serializable]` 字段 | **⚠️ 新暴露了 `parent` 字段** |
| 条件隐藏 | `ExtendSwitch` 中按配置决定隐藏哪些字段 | 需 `[HideIf]` 属性标记 | **⚠️ 条件逻辑丢失** |
| 空值提示 | `DefaultCheck()` 用红色背景标记空字段 | 需 `[Required]` 属性 | 视觉差异，非逻辑断链 |
| 多态列表创建 | `DynamicList` 自定义类型选择弹窗 | Odin 原生多态选择器 | ✅ 功能等价 |
| 双击跳脚本 | 反射查找脚本，PingObject | Odin 不内置 | ⛔ 非核心功能，可接受 |
| Undo/Redo | 不支持 | `OnPropertyValueChanged` + `Undo.RecordObject` | ✅ 新增 |
| 数据写盘 | `Info.Save()` 含 Refresh 全量扫描 | `SetDirty` + `SaveAssetIfDirty` | ✅ 改进 |

**处理方法：**
- ⚠️ 断链 → 在数据类上补 `[HideInInspector]` 或 `[HideIf]` 属性
- 非逻辑断链 → 记录但不阻塞，由用户决定是否要保留
- 新增功能 → 标注为新能力

### 审查输出示例

对审查过的每个文件，顶部标注"替换概览"：

```csharp
/// <summary>
/// 替换概览：
/// - 原 `FieldGenerate.Start()` 手动反射 → `PropertyTree.Create()` + Odin 抽屉
/// - 新增 Undo/Redo 支持 + 自动 SetDirty
/// - 字段可见性改为由 Odin 属性标记控制（[HideInInspector], [ShowIf]）
/// </summary>
```


## 关键自查清单

写完后逐条检查：
- 每个【问题】是否能在官方文档中找到依据？
- 是否混入了"我"、"我认为"等主观描述？
- 是否混入了在用户对话中自我修正的记录？
- 建议是否给出了可运行的代码（哪怕一行）？
- 脱离对话上下文，只看注释能理解问题和改法吗？
- 注释是否包含了 ASCII 图表、长篇教程、辩论式内容？（有则删除）
