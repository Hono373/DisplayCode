using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
///具体实现由胶水层决定
/// </summary>
[Serializable]
public class SelectorBox
{
    [InfoBox("通过设置条件，剔除不符合条件的单位")]
    [SerializeReference] public List<IUnitSelector> selector = new();
    public void Trim(List<string> targetList)
    {
        foreach (var item in selector)
        {
            item.Trim(targetList);
        }
    }
}