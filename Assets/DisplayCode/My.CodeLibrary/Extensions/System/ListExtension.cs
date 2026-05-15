using System.Collections.Generic;

public static class ListExtension
{
    public static bool Random<T>(this List<T> list, out T randomEle)
    {
        if (list == null || list.Count == 0)
        {
            randomEle = default;
            return false;
        }
        randomEle = list[UnityEngine.Random.Range(0, list.Count)];
        return true;
    }
}
