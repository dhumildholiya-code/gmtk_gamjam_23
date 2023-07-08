using UnityEngine;

namespace gmtk_gamejam.AbillitySystem
{
    public abstract class Ability : ScriptableObject
    {
        public string name;
        [TextArea]
        public string description;
        public int amount;

        public abstract void Execute();
    }
}
