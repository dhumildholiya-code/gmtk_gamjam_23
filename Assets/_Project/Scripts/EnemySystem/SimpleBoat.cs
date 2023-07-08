using System;
using UnityEngine;
using UnityEngine.UI;

namespace gmtk_gamejam.EnemySystem
{
    public class SimpleBoat : MonoBehaviour, ITakeDamage
    {
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

        private RaftController _target;
        private Rigidbody2D _rb;

        private float _speed;
        private int _currentHealth;
        private bool _inAttackRange;
        private float _attackTimer;
        private EnemyState _state;

        private void Start()
        {
            _currentHealth = maxHealth;
            health.SetActive(false);
            _rb = GetComponent<Rigidbody2D>();
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
                Destroy(gameObject);
            }
            _currentHealth -= damage;
            if (_currentHealth < maxHealth)
            {
                health.SetActive(true);
            }
            healthBar.fillAmount = _currentHealth * 1f / maxHealth;
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
            _speed = chaseSpeed;
            Vector2 dir = _target.Position - (Vector2)transform.position;
            Vector2 targetPos = Vector2.MoveTowards(transform.position, _target.Position, _speed * Time.deltaTime);
            transform.position = targetPos;
            transform.up = dir;

            float targetDistance = dir.magnitude;
            _inAttackRange = targetDistance < attackRange;

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
                _target.RemoveTresure(lootDamage);
            }
            _attackTimer -= Time.deltaTime;

            Vector2 dir = _target.Position - (Vector2)transform.position;
            float targetDistance = dir.magnitude;
            _inAttackRange = targetDistance < attackRange;
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
