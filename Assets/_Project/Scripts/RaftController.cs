using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace gmtk_gamejam
{
    public class RaftController : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private SharkController sharkPrefab;

        [Header("Movement")]
        [SerializeField] private float moveSpeed;

        [Header("Shark variables")]
        [SerializeField] private int startSharkCount;
        [SerializeField] private float sharkSpawnRange;

        [SerializeField] private float sharkMoveRange;
        public float SharkMoveRange => sharkMoveRange;

        [SerializeField] private float sarkAttackRange;

        public Vector2 Position => transform.position;

        private Rigidbody2D _rb;
        private List<SharkController> _sharks;
        public int CurrentSharkCount => _sharks.Count;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sharks = new List<SharkController>();
            CreateAddShark();
            Move(Direction.East);
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    _rb.velocity = Vector2.up * moveSpeed;
                    break;
                case Direction.South:
                    _rb.velocity = Vector2.down * moveSpeed;
                    break;
                case Direction.West:
                    _rb.velocity = Vector2.left * moveSpeed;
                    break;
                case Direction.East:
                    _rb.velocity = Vector2.right * moveSpeed;
                    break;
                case Direction.None:
                    _rb.velocity = Vector2.zero;
                    break;
            }
            transform.up = _rb.velocity;
        }

        [ContextMenu("Create and Add Shark")]
        public void CreateAddShark()
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sharkMoveRange);
        }
    }
}
