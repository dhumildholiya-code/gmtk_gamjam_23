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
        public void SetBoundry(float x)
        {
            xBoundry = x;
        }

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

            Vector3 targetPos = new Vector3(_target.position.x, yOffset, transform.position.z) - _offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothFactor);

            if(transform.position.x < 0f)
            {
                transform.position = new Vector3(0f, yOffset, transform.position.z);
            }
            if (transform.position.x > xBoundry)
            {
                transform.position = new Vector3(xBoundry, yOffset, transform.position.z);
            } 
        }
    }
}
