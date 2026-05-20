using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class ModifierBar
{
    [JsonProperty] Dictionary<string, Modifier> modifiers = new();

    public void AddModifier(IModifierOwner owner, ModifierCreateInfo createInfo, IModiferBarUI ui)
    {
        var modifier = Modifier.Create(createInfo);
    }
}
public interface IModiferBarUI
{

}