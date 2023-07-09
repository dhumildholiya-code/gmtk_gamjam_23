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
}
