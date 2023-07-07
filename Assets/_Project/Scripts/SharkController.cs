using UnityEngine;

namespace gmtk_gamejam
{
    public class SharkController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField][Range(-.15f, .15f)] private float roamSpeed;

        private Rigidbody2D _rb;
        private RaftController _raft;

        private Vector2 _prevPos;
        private Vector2 _pos;
        private float _time;
        private float _moveRadius;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _moveRadius = Random.Range(.5f, _raft.SharkMoveRange);
            roamSpeed = Random.Range(-.15f,.15f);
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


            // Position
            float moveRadius = Mathf.Cos(2*Mathf.PI*_time*10)*.1f + _moveRadius;
            Vector2 targetPos = new Vector2(Mathf.Cos(2*Mathf.PI*_time), Mathf.Sin(2*Mathf.PI*_time))*moveRadius;
            targetPos += _raft.Position;
            _pos = targetPos;

            //Rotaion
            Vector2 dir = (_pos - _prevPos).normalized;
            transform.up = Vector2.Lerp(transform.up, dir, Time.deltaTime);

            _prevPos = _pos;
        }
        private void FixedUpdate()
        {
            if (_raft == null) return;

            _rb.MovePosition(_pos);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_raft.Position, _moveRadius);
        }
    }
}
