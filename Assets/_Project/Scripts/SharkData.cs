using UnityEngine;

namespace gmtk_gamejam
{
    [CreateAssetMenu(fileName = "SharkData", menuName = "SharkData")]
    public class SharkData : ScriptableObject
    {
        public int attackDamage;
        public int attackRange;
        public int attackTarget;
    }
}