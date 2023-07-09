using DG.Tweening;
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
        [SerializeField] private int startExp;
        [SerializeField] private int diffExp;

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
        private int _levelId;
        private int _maxExp;
        private Direction _oldDirection;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sharks = new List<SharkController>();
            SimpleBoat.OnDeathCollectExp += AddExp;
            AddSharkAbillity.OnAddShark += CreateAddShark;
        }
        private void OnDestroy()
        {
            SimpleBoat.OnDeathCollectExp -= AddExp;
            AddSharkAbillity.OnAddShark -= CreateAddShark;
        }
        public void Setup(int maxTresure, int sharkCount, Direction direction)
        {
            _levelId = 0;
            _maxExp = startExp + diffExp * _levelId;
            this.maxTresure = maxTresure;
            this.startSharkCount = sharkCount;
            _currentTresure = maxTresure;
            _currentExp = 0;
            for (int i = 0; i < startSharkCount; i++)
            {
                CreateAddShark();
            }
            Move(direction);
        }
        public void CleanUp()
        {
            transform.position = new Vector2(-6.5f, .5f);
            for (int i = _sharks.Count - 1; i >= 0; i--)
            {
                _sharks[i].CleanUp();
                Destroy(_sharks[i].gameObject);
            }
            _sharks.Clear();
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
                GameManager.Instance.ChangeStateDelay(GameState.LevelFailed, 1f);
            }
            _currentTresure -= amount;
            OnTresureChange?.Invoke(_currentTresure, maxTresure);
        }

        [ContextMenu("Create and Add Shark")]
        private void CreateAddShark()
        {
            Vector2 pos = Random.insideUnitCircle * sharkSpawnRange + (Vector2)transform.position;
            SharkController shark = Instantiate(sharkPrefab, pos, Quaternion.identity);
            shark.Setup(this);
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
            _currentExp += exp;
            OnExpChange?.Invoke(_currentExp, _maxExp);
            if (_currentExp >= _maxExp)
            {
                _levelId++;
                _currentExp = 0;
                _maxExp = startExp + diffExp * _levelId;
                OnExpChange?.Invoke(_currentExp, _maxExp);
                OnLevelUp?.Invoke();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("obstacle"))
            {
                GameManager.Instance.ChangeStateDelay(GameState.LevelFailed, 1f);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<DirectionalProp>(out var directionalProp))
            {
                transform.DOMove(directionalProp.transform.position, 1f).OnComplete(() =>
                {
                    Move(directionalProp.direction);
                    Destroy(other.gameObject);
                });
            }
            else if (other.CompareTag("levelComplete"))
            {
                Debug.Log("win");
                GameManager.Instance.ChangeStateDelay(GameState.LevelComplete, 1f);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sharkMoveRange);
        }
    }
}
