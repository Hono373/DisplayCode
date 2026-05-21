using System;
using System.Collections.Generic;

public partial class Modifier
{
    [Serializable]
    public class ModifierData
    {
        public IModifierOwnerAffected owner;
        public IModifierOwnerAffected target;
        public bool only;
        public string id = string.Empty;
        public string key = string.Empty;
        public int layer;
        public bool forever;
        public Dictionary<string, int> dict = new();
    }
}
