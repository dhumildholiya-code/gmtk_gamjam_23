using gmtk_gamejam.PropSystem;
using System.Collections.Generic;
using UnityEngine;

namespace gmtk_gamejam
{
    public class RaftController : MonoBehaviour
    {
        //Statice Events
        public static event System.Action<int, int> OnTresureChange;

        [Header("Reference")]
        [SerializeField] private SharkController sharkPrefab;

        [Header("Movement")]
        [SerializeField] private float moveSpeed;

        [Header("Tresure")]
        [SerializeField] private int maxTresure;

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

        private int _currentTresure;
        private Direction _oldDirection;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sharks = new List<SharkController>();
            _currentTresure = maxTresure;
            CreateAddShark();
            Move(Direction.East);
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    _oldDirection = direction;
                    _rb.velocity = Vector2.up * moveSpeed;
                    transform.up = _rb.velocity;
                    break;
                case Direction.South:
                    _oldDirection = direction;
                    _rb.velocity = Vector2.down * moveSpeed;
                    transform.up = _rb.velocity;
                    break;
                case Direction.West:
                    _oldDirection = direction;
                    _rb.velocity = Vector2.left * moveSpeed;
                    transform.up = _rb.velocity;
                    break;
                case Direction.East:
                    _oldDirection = direction;
                    _rb.velocity = Vector2.right * moveSpeed;
                    transform.up = _rb.velocity;
                    break;
                case Direction.None:
                    _rb.velocity = Vector2.zero;
                    break;
            }
        }
        public void MoveContinue()
        {
            Move(_oldDirection);
        }

        public void RemoveTresure(int amount)
        {
            if (_currentTresure - amount <= 0)
            {
                // TODO : die / game over.
            }
            _currentTresure -= amount;
            OnTresureChange?.Invoke(_currentTresure, maxTresure);
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<DirectionalProp>(out var directionalProp))
            {
                transform.position = directionalProp.transform.position;
                Move(directionalProp.direction);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sharkMoveRange);
        }
    }
}
