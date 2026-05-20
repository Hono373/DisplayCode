using CardSandBoxLibrary;
using System;
using System.Collections.Generic;

namespace GameModule.CardSandBox.UnitIntention
{
    /// <summary>
    /// 行为模块管理器，对输出数据负责
    /// 封装了切换逻辑与行为栏
    /// </summary>
    [Serializable]
    public class WeightedRandomSelector
    {
        private readonly IReadOnlyList<IWeight> _items;
        private readonly int[] _prefixSums;   // 前缀和数组，_prefixSums[i] = 前 i+1 项权重之和
        private readonly int _totalWeight;

        /// <param name="items">元素列表，每个元素需实现 IWeight 接口提供权重值（非负整数）。</param>
        /// <exception cref="ArgumentException">列表为空、含负权重或总权重为 0。</exception>
        public void Add(IWeight item)
        {

        }
        public WeightedRandomSelector(IReadOnlyList<IWeight> items)
        {
            this._items = items;
            if (items == null || items.Count == 0)
                throw new ArgumentException("列表不能为空", nameof(items));

            _prefixSums = new int[items.Count];
            int sum = 0;
            for (int i = 0; i < items.Count; i++)
            {
                int w = items[i].Weight();
                if (w < 0)
                    throw new ArgumentException($"权重不能为负数，索引 {i} 的值为 {w}");
                sum += w;
                _prefixSums[i] = sum;
            }

            _totalWeight = sum;
            if (_totalWeight <= 0)
                throw new ArgumentException("总权重必须大于 0", nameof(items));
        }

        /// <summary>根据随机值选取索引。</summary>
        /// <param name="randomValue">范围 [0, TotalWeight) 的随机整数。</param>
        /// <returns>权重区间对应的索引。</returns>
        public int SelectIndex(int randomValue)
        {
            if (randomValue < 0 || randomValue >= _totalWeight)
                throw new ArgumentOutOfRangeException(nameof(randomValue),
                    $"随机值应在 [0, {_totalWeight}) 内");

            // 二分查找第一个前缀和 > randomValue 的索引
            int left = 0, right = _prefixSums.Length - 1;
            while (left < right)
            {
                int mid = (left + right) / 2;
                if (_prefixSums[mid] > randomValue)
                    right = mid;
                else
                    left = mid + 1;
            }
            return left;
        }

        /// <summary>自动生成随机数并选取索引。</summary>
        public int SelectIndex()
        {
            int randomValue = _totalWeight.GetRandom();
            return SelectIndex(randomValue);
        }

        /// <summary>总权重。</summary>
        public int TotalWeight => _totalWeight;
    }
}
