using System.Collections.Generic;
using UnityEngine;

namespace gmtk_gamejam
{
    public class RaftController : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private SharkController sharkPrefab;

        [Header("Shark variables")]
        [SerializeField] private float sharkSpawnRange;

        [SerializeField] private float sharkMoveRange;
        public float SharkMoveRange => sharkMoveRange;

        [SerializeField] private float sarkAttackRange;

        private List<SharkController> _sharks;

        public Vector2 Position => transform.position;

        private void Start()
        {
            _sharks = new List<SharkController>();
        }

        [ContextMenu("Add Shark")]
        public void AddShark()
        {
            Vector2 pos = Random.insideUnitCircle * sharkSpawnRange + (Vector2)transform.position;
            SharkController shark = Instantiate(sharkPrefab, pos, Quaternion.identity);
            shark.Init(this);
            _sharks.Add(shark);
        }
        public void RemoveShark(SharkController shark)
        {
            if (_sharks.Contains(shark))
            {
                _sharks.Remove(shark);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sharkMoveRange);
        }
    }
}
