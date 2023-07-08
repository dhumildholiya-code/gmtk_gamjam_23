using System;
using UnityEngine;

namespace gmtk_gamejam.AbillitySystem
{
    [CreateAssetMenu(fileName = "AddSharkAbillity", menuName = "Abillity / AddSharkAbillity")]
    public class AddSharkAbillity : Ability
    {
        public static event Action OnAddShark;
        public override void Execute()
        {
            OnAddShark?.Invoke();
        }
    }

    [CreateAssetMenu(fileName = "AddDamageAbillity", menuName = "Abillity / AddDamageAbillity")]
    public class AddDamageAbillity : Ability
    {
        [SerializeField] private SharkData sharkData;
        public override void Execute()
        {
            sharkData.attackDamage += amount;
        }
    }

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
