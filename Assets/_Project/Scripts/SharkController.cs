using TMPro.EditorUtilities;
using UnityEngine;

namespace gmtk_gamejam
{
    public class SharkController : MonoBehaviour
    {
        private enum SharkState
        {
            Idle,
            Input,
            Launch,
            Attack,
            Return,
        }
        [Header("Movement")]
        [SerializeField][Range(.05f, .1f)] private float roamSpeed;
        [SerializeField] private float attackMoveSpeed;
        [SerializeField] private float turnModifier;
        [SerializeField] private Vector2 wiggleModifier;
        [SerializeField] private Vector2 wiggleAmplitude;

        [Header("Attack")]
        [SerializeField] private LayerMask attackLayer;
        [SerializeField] private float attackSpeedMultiplier;
        [SerializeField] private LineRenderer line;
        [SerializeField] private SharkData sharkData;

        private Rigidbody2D _rb;
        private RaftController _raft;
        private SharkState _state;
        private Camera _cam;

        private QuadraticBezierCurve _attackCurve;
        private QuadraticBezierCurve _returnCurve;

        private ITarget _target;
        private Vector2 _prevPos;
        private Vector2 _lastPosBeforeAttack;
        private int _targetCounter;
        private float _time;
        private float _curveTime;
        private float _phase;
        private float _moveRadius;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _cam = Camera.main;
            ChangeState(SharkState.Idle);
            _moveRadius = Random.Range(.9f, _raft.SharkMoveRange);
            roamSpeed = Random.Range(.09f, .13f);
            _phase = Random.Range(0, 2 * Mathf.PI);
            _prevPos = _rb.position;
        }
        public void Init(RaftController raft)
        {
            _raft = raft;
        }

        private void Update()
        {
            if (_raft == null) return;
            UpdateState();
        }
        private void FixedUpdate()
        {
            if (_raft == null) return;

            //_rb.MovePosition(_pos);
        }

        #region StateMachine Methods
        private void UpdateState()
        {
            switch (_state)
            {
                case SharkState.Idle:
                    IdleState();
                    break;
                case SharkState.Input:
                    InputState();
                    break;
                case SharkState.Launch:
                    LaunchState();
                    break;
                case SharkState.Attack:
                    AttackSate();
                    break;
                case SharkState.Return:
                    ReturnState();
                    break;
                default:
                    break;
            }
        }

        private void ReturnState()
        {
            _curveTime += Time.deltaTime * attackSpeedMultiplier;
            Vector2 targetPos = _returnCurve.GetPos(_curveTime);
            transform.position = targetPos;

            transform.up = _returnCurve.GetTangent(_curveTime);

            //Transition Condition
            if (_curveTime >= 1f)
            {
                _curveTime = 0f;
                ChangeState(SharkState.Idle);
            }
        }

        private void AttackSate()
        {
            _target.Damagable.TakeDamage(sharkData.attackDamage);
            _targetCounter--;
            // Check for other nearby enemies if attack target is > 1.
            if (_targetCounter > 0)
            {
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, sharkData.bounceAttackRange, attackLayer);
                if (targets.Length == 0)
                {
                    ChangeState(SharkState.Return);
                    return;
                }
                //select target who is not equal to old target.
                int index = 0;
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i].transform == _target.GetTransform()) continue;
                    index = i;
                    break;
                }
                Collider2D target = targets[index];
                _target = target.GetComponent<ITarget>();
                if (_target != null)
                {
                    ChangeState(SharkState.Launch);
                    return;
                }
            }
            ChangeState(SharkState.Return);
        }

        private void LaunchState()
        {
            _curveTime += Time.deltaTime * attackSpeedMultiplier;
            Vector2 targetPos = _attackCurve.GetPos(_curveTime);
            transform.position = targetPos;

            transform.up = _attackCurve.GetTangent(_curveTime);

            //Transition Condition
            if (_curveTime >= 1f)
            {
                _curveTime = 0f;
                ChangeState(SharkState.Attack);
            }
        }

        private void InputState()
        {
            Vector2 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 aimDir = mouseWorldPos - (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDir, sharkData.attackRange, attackLayer);
            line.SetPosition(0, transform.position);
            if (hit.collider != null)
            {
                line.SetPosition(1, hit.point);
                line.startColor = Color.green;
                line.endColor = Color.green;
            }
            else
            {
                line.startColor = Color.white;
                line.endColor = Color.white;
                line.SetPosition(1, (Vector2)transform.position + sharkData.attackRange * aimDir.normalized);
            }

            //Transition Condition
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider == null)
                {
                    ChangeState(SharkState.Idle);
                }
                else
                {
                    _target = hit.collider.GetComponent<ITarget>();
                    if (_target != null)
                    {
                        _lastPosBeforeAttack = transform.position;
                        _targetCounter = sharkData.attackTarget;
                        ChangeState(SharkState.Launch);
                    }
                    else
                        ChangeState(SharkState.Idle);
                }
            }
        }

        private void IdleState()
        {
            //Time
            _time += Time.deltaTime * roamSpeed;

            // Wiggle Motion on Circular path
            float moveRadius01 = Mathf.InverseLerp(.5f, _raft.SharkMoveRange, _moveRadius);
            float wiggleMultiplier = Mathf.Lerp(wiggleModifier.x, wiggleModifier.y, moveRadius01);
            float amplitudeMultiplier = Mathf.Lerp(wiggleAmplitude.x, wiggleAmplitude.y, moveRadius01);
            float moveRadius = Mathf.Cos(2 * Mathf.PI * _time * wiggleMultiplier + _phase) * amplitudeMultiplier + _moveRadius;

            // Position
            Vector2 targetPos = new Vector2(Mathf.Cos(2 * Mathf.PI * _time + _phase), Mathf.Sin(2 * Mathf.PI * _time + _phase)) * moveRadius;
            targetPos += _raft.Position;
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * 5f);

            //Rotation
            Vector2 dir = (targetPos - _prevPos).normalized;
            transform.up = Vector2.Lerp(transform.up, dir, Time.deltaTime * turnModifier);

            _prevPos = targetPos;

            //transition Condition.
            //using OnMousedown
        }

        private void ChangeState(SharkState newState)
        {
            _state = newState;
            switch (_state)
            {
                case SharkState.Idle:
                    _raft.MoveContinue();
                    line.gameObject.SetActive(false);
                    break;
                case SharkState.Input:
                    _raft.Move(Direction.None);
                    line.gameObject.SetActive(true);
                    break;
                case SharkState.Launch:
                    _curveTime = 0f;
                    _attackCurve = new QuadraticBezierCurve(transform.position, _target.GetPos(.8f));
                    line.gameObject.SetActive(false);
                    break;
                case SharkState.Attack:
                    _curveTime = 0f;
                    line.gameObject.SetActive(false);
                    break;
                case SharkState.Return:
                    _curveTime = 0f;
                    _returnCurve = new QuadraticBezierCurve(transform.position, _lastPosBeforeAttack);
                    line.gameObject.SetActive(false);
                    break;
            }
        }
        #endregion

        private void OnMouseDown()
        {
            ChangeState(SharkState.Input);
        }

        private void OnDrawGizmosSelected()
        {
            if (_raft != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_raft.Position, _moveRadius);
            }
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, sharkData.bounceAttackRange);

            Gizmos.color = Color.magenta;
            if (_attackCurve != null)
            {
                Gizmos.DrawWireSphere(_attackCurve.P0, .2f);
                Gizmos.DrawWireSphere(_attackCurve.P1, .2f);
                Gizmos.DrawWireSphere(_attackCurve.P2, .2f);
            }
            if (_returnCurve != null)
            {
                Gizmos.DrawWireSphere(_returnCurve.P0, .2f);
                Gizmos.DrawWireSphere(_returnCurve.P1, .2f);
                Gizmos.DrawWireSphere(_returnCurve.P2, .2f);
            }
        }
    }
}
