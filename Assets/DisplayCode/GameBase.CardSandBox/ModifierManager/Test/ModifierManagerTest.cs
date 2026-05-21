using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class ModifierManagerTest : MonoBehaviour
{

    IEnumerator Start()
    {
        yield return AssetLoad.Create();
    }

    [Button]
    public void Test()
    {
        var caster = new UnitMock();
        var target = new UnitMock();
        var modifierModule = target.GetModifierModule();
        ModifierCreateInfo createInfo = new ModifierCreateInfo();
        modifierModule.AddModifier(caster,target,createInfo);
    }
}

public class UnitMock : IModifierOwnerAffected
{
    [SerializeField] ModifierModule modifierModule = new();
    public ModifierModule GetModifierModule() => modifierModule;
}
