using UnityEngine;

namespace gmtk_gamejam.CameraSystem
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float yOffset;
        [SerializeField] private float xBoundry;
        [SerializeField] private float smoothFactor;

        private Transform _target;
        private Vector3 _offset;

        public void SetTarget(Transform target)
        {
            _target = target;
            _offset = _target.position - transform.position;
            _offset.y = 0f;
            _offset.z = 0f;
        }

        private void Update()
        {
            if (_target == null) return;
            if (transform.position.x >= xBoundry) return;

            Vector3 targetPos = new Vector3(_target.position.x, yOffset, transform.position.z) - _offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothFactor);
        }
    }
}
