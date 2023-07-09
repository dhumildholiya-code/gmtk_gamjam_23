using UnityEngine;

namespace gmtk_gamejam.AbillitySystem
{
    [CreateAssetMenu(fileName = "AddDamageAbillity", menuName = "Abillity / AddDamageAbillity")]
    public class AddDamageAbillity : Ability
    {
        [SerializeField] private SharkData sharkData;
        public override void Execute()
        {
            sharkData.attackDamage += amount;
        }
    }
}
