using UnityEngine;

namespace Obstacle
{
    public class Obstacle : MonoBehaviour
    {
        private float _areaExtent;
    
        private Vector3 _targetPosition;

        private float _speed;
        public float AreaExtent
        {
            set => _areaExtent = value;
        }

        public float Speed
        {
            set => _speed = value;
        }
        void Start()
        {
            _targetPosition = transform.position;
        }

        public void Move()
        {
            if (_areaExtent != 0)
            {
                if (Vector3.Distance(transform.position, _targetPosition) <= 0.01f)
                {
                    _targetPosition = new Vector3(Random.Range(0f, _areaExtent),
                        Random.Range(0f, _areaExtent),
                        Random.Range(0f, _areaExtent));
                }
            }
        
            var movement = (_targetPosition - transform.position).normalized * _speed * Time.deltaTime;
            transform.Translate(movement);
        }
    }
}
