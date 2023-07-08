using gmtk_gamejam.AbillitySystem;
using gmtk_gamejam.EnemySystem;
using gmtk_gamejam.PropSystem;
using System.Collections.Generic;
using UnityEngine;

namespace gmtk_gamejam
{
    public class RaftController : MonoBehaviour
    {
        //Statice Events
        public static event System.Action<int, int> OnTresureChange;
        public static event System.Action<int, int> OnExpChange;
        public static event System.Action OnLevelUp;

        [Header("Reference")]
        [SerializeField] private SharkController sharkPrefab;

        [Header("Movement")]
        [SerializeField] private float moveSpeed;

        [Header("Tresure")]
        [SerializeField] private int maxTresure;
        [Header("Experience")]
        [SerializeField] private int maxExp;

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
        private int _currentExp;
        private Direction _oldDirection;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sharks = new List<SharkController>();
            SimpleBoat.OnDeathCollectExp += AddExp;
            AddSharkAbillity.OnAddShark += CreateAddShark;
            _currentTresure = maxTresure;
            _currentExp = 0;
            CreateAddShark();
            Move(Direction.East);
        }
        private void OnDestroy()
        {
            SimpleBoat.OnDeathCollectExp -= AddExp;
            AddSharkAbillity.OnAddShark -= CreateAddShark;
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
        private void CreateAddShark()
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
        private void AddExp(int exp)
        {
            if(_currentExp + exp >= maxExp)
            {
                //Leve Up.
                OnLevelUp?.Invoke();
                _currentExp = 0;
                OnExpChange?.Invoke(_currentExp, maxExp);
            }
            else
            {
                _currentExp += exp;
                OnExpChange?.Invoke(_currentExp, maxExp);
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
