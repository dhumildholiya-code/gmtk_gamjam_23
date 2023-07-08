using System.ComponentModel;
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
        }
        [Header("Movement")]
        [SerializeField][Range(.05f, .1f)] private float roamSpeed;
        [SerializeField] private float attackMoveSpeed;
        [SerializeField] private float turnModifier;
        [SerializeField] private Vector2 wiggleModifier;
        [SerializeField] private Vector2 wiggleAmplitude;

        [Header("Attack")]
        [SerializeField] private LayerMask attackLayer;
        [SerializeField] private LineRenderer line;
        [SerializeField] private int attackDamage;
        [SerializeField] private float attackRange;

        private Rigidbody2D _rb;
        private RaftController _raft;
        private SharkState _state;
        private Camera _cam;

        private Transform _target;
        private Vector2 _prevPos;
        private Vector2 _pos;
        private float _time;
        private float _phase;
        private float _moveRadius;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _cam = Camera.main;
            ChangeState(SharkState.Idle);
            _moveRadius = Random.Range(.5f, _raft.SharkMoveRange);
            roamSpeed = Random.Range(.05f, .1f);
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

            _rb.MovePosition(_pos);
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
                    break;
                default:
                    break;
            }
        }

        private void LaunchState()
        {
            Vector2 dir = _target.position - transform.position;
            Vector2 targetPos = Vector2.MoveTowards(_rb.position, _target.position, Time.deltaTime * attackMoveSpeed);
            _pos = targetPos;

            //Transition Condition
        }

        private void InputState()
        {
            Vector2 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 aimDir = mouseWorldPos - (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDir, attackRange, attackLayer);
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
                line.SetPosition(1, (Vector2)transform.position + attackRange * aimDir.normalized);
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
                    _target = hit.collider.transform;
                    ChangeState(SharkState.Launch);
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
            _pos = targetPos;

            //Rotation
            Vector2 dir = (_pos - _prevPos).normalized;
            transform.up = Vector2.Lerp(transform.up, dir, Time.deltaTime * turnModifier);

            _prevPos = _pos;

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
                    line.gameObject.SetActive(false);
                    break;
                case SharkState.Attack:
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
            if (_raft == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_raft.Position, _moveRadius);
        }
    }
}
