using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace gmtk_gamejam.EnemySystem
{
    public class SimpleBoat : MonoBehaviour, ITakeDamage, ITarget
    {
        //Events
        public static event System.Action<int> OnDeathCollectExp;

        private enum EnemyState
        {
            Detect,
            Chase,
            Attack
        }

        [Header("Detect")]
        [SerializeField] private LayerMask detectLayer;
        [SerializeField] private float detectRadius;
        [Header("Health")]
        [SerializeField] private int maxHealth;
        [SerializeField] private GameObject health;
        [SerializeField] private Image healthBar;
        [Header("Movement")]
        [SerializeField] private float chaseSpeed;
        [Header("Attack")]
        [SerializeField] private float attackRange;
        [SerializeField] private float attackTime;
        [SerializeField] private int lootDamage;
        [SerializeField] private GameObject tresure;
        [Header("Exp")]
        [SerializeField] private int expPoints;

        private RaftController _target;
        private Animator _animator;

        private float _speed;
        private int _currentHealth;
        private bool _inAttackRange;
        private float _attackTimer;
        private EnemyState _state;

        public ITakeDamage Damagable => this;

        private void Start()
        {
            _currentHealth = maxHealth;
            health.SetActive(false);
            _animator = GetComponent<Animator>();
            ChangeState(EnemyState.Detect);
        }

        private void Update()
        {
            UpdateState();
        }
        public void TakeDamage(int damage)
        {
            if (_currentHealth - damage <= 0)
            {
                OnDeathCollectExp?.Invoke(expPoints);
                Destroy(gameObject);
            }
            transform.DOShakePosition(.3f, .2f);
            _currentHealth -= damage;
            if (_currentHealth < maxHealth)
            {
                health.SetActive(true);
            }
            healthBar.DOFillAmount(_currentHealth * 1f/ maxHealth, .2f);
        }
        public Transform GetTransform()
        {
            return transform;
        }
        public Vector2 GetPos(float timePased)
        {
            if(_speed <= 0f)
            {
                return transform.position;
            }
            Vector2 pos = transform.position + transform.up *_speed * timePased; 
            return pos;
        }

        #region StateMachine Methods
        private void UpdateState()
        {
            switch (_state)
            {
                case EnemyState.Detect:
                    DetectState();
                    break;
                case EnemyState.Attack:
                    AttackState();
                    break;
                case EnemyState.Chase:
                    ChaseState();
                    break;
            }
        }

        private void ChaseState()
        {
            Vector2 dir = _target.Position - (Vector2)transform.position;
            Vector2 targetPos = Vector2.MoveTowards(transform.position, _target.Position, _speed * Time.deltaTime);
            transform.position = targetPos;
            transform.up = dir;

            float targetDistance = dir.magnitude;
            _inAttackRange = targetDistance <= attackRange - .2f;

            //Transition Condition
            if (_inAttackRange)
            {
                ChangeState(EnemyState.Attack);
            }
        }

        private void AttackState()
        {
            if (_attackTimer <= 0f)
            {
                _attackTimer = attackTime;
                tresure.SetActive(true);
                _target.RemoveTresure(lootDamage);
            }
            _attackTimer -= Time.deltaTime;

            Vector2 dir = _target.Position - (Vector2)transform.position;
            float targetDistance = dir.magnitude;
            _inAttackRange = targetDistance <= attackRange;
            transform.up = dir;

            if (!_inAttackRange)
            {
                _attackTimer = attackTime;
                ChangeState(EnemyState.Chase);
            }
        }

        private void DetectState()
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, detectRadius, detectLayer);
            if (col != null)
            {
                _target = col.gameObject.GetComponent<RaftController>();
            }

            //Transition Condition
            if (_target != null)
            {
                ChangeState(EnemyState.Chase);
            }
        }

        private void ChangeState(EnemyState newState)
        {
            _state = newState;
            switch (_state)
            {
                case EnemyState.Detect:
                    _speed = 0f;
                    _animator.SetBool("run", false);
                    break;
                case EnemyState.Chase:
                    _speed = chaseSpeed;
                    _animator.SetBool("run", true);
                    break;
                case EnemyState.Attack:
                    _speed = 0f;
                    _animator.SetBool("run", false);
                    break;
            }
        }
        #endregion

        public void SetTarget(RaftController target)
        {
            _target = target;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

    }
}
