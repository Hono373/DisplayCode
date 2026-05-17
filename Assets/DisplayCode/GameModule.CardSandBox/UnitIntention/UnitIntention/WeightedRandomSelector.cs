using CardSandBoxLibrary;
using System;
using System.Collections.Generic;

[Serializable]
/// <summary>
/// 行为模块管理器，对输出数据负责
/// 封装了切换逻辑与行为栏
/// </summary>
internal class WeightedRandomSelector
{
    private IReadOnlyList<IWeight> weights;

    public WeightedRandomSelector(IReadOnlyList<IWeight> weights)
    {
        this.weights = weights;
    }

    internal int? SelectIndex()
    {
        throw new NotImplementedException();
    }
}
