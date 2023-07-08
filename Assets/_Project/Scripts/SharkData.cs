﻿using UnityEngine;

namespace gmtk_gamejam
{
    [CreateAssetMenu(fileName = "SharkData", menuName = "SharkData")]
    public class SharkData : ScriptableObject
    {
        public int attackDamage;
        public float attackRange;
        public int attackTarget;

#if UNITY_EDITOR
        [ContextMenu("Data Reset")]
        public void DataReset()
        {
            attackDamage = 1;
            attackRange = 6f;
            attackTarget = 1;
        }
#endif
    }
}