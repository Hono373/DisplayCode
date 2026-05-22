using DG.Tweening;
using System.Collections.Generic;

public interface IModifierModuleUI
{
    void OnValueChange(IReadOnlyDictionary<string, Modifier> modifiers, Sequence seq);
}