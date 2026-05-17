using UnityEngine;

public static class ValueExtension
{
    /// <summary>
    /// 获取一个随机数，范围是0-weight
    /// </summary>
    public static int GetRandom(this int weight) => Random.Range(0, weight);
}
