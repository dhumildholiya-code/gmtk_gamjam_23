using UnityEngine;

namespace gmtk_gamejam.AbillitySystem
{
    [CreateAssetMenu(fileName = "AddTargetAbillity", menuName = "Abillity / AddTargetAbillity")]
    public class AddTargetAbillity : Ability
    {
        [SerializeField] private SharkData sharkData;
        public override void Execute()
        {
            sharkData.attackTarget += amount;
        }
    }
}
