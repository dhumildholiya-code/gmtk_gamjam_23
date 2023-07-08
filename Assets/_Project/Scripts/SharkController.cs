using UnityEngine;

namespace gmtk_gamejam
{
    public class SharkController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField][Range(.05f, .1f)] private float roamSpeed;
        [SerializeField] private float turnModifier;
        [SerializeField] private Vector2 wiggleModifier;
        [SerializeField] private Vector2 wiggleAmplitude;


        private Rigidbody2D _rb;
        private RaftController _raft;

        private Vector2 _prevPos;
        private Vector2 _pos;
        private float _time;
        private float _phase;
        private float _moveRadius;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _moveRadius = Random.Range(.5f, _raft.SharkMoveRange);
            roamSpeed = Random.Range(.05f, .1f);
            _phase = Random.Range(0, 2*Mathf.PI);
            _prevPos = _rb.position;
        }
        public void Init(RaftController raft)
        {
            _raft = raft;
        }

        private void Update()
        {
            if (_raft == null) return;

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
        }
        private void FixedUpdate()
        {
            if (_raft == null) return;

            _rb.MovePosition(_pos);
        }

        private void OnDrawGizmosSelected()
        {
            if (_raft == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_raft.Position, _moveRadius);
        }
    }
}
