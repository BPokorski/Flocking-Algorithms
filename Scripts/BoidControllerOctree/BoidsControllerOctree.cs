using UnityEngine;

namespace BoidController
{
    public class BoidsControllerOctree : BaseBoidController
    {
        [Tooltip("Size of smallest node in Tree")]
        [SerializeField] private int minNodeSize = 10;
    
        private Octree.Octree _octree;
    
        void Start()
        {
            _octree = new Octree.Octree(_areaExtent, minNodeSize);
        }

        // Update is called once per frame
        void Update()
        {
            _averageFlockingPosition = Vector3.zero;
            foreach (var fish in _allBoids)
            {
                _averageFlockingPosition += fish.TransformProperty.position;
            
                var rotationDirection = fish.GetRotationDirection(_octree.octreeBounds,
                    _currentTarget.transform.position,
                    obstacleDistance);
                if (rotationDirection == Vector3.zero)
                {
                    if (_currentFrame == _nthFrameCalculation)
                    {
                        fish.neighbours = _octree.FindNodesIntersectsSphere(fish.TransformProperty.position, cohesionDistance);
                        fish.CalculateFlocking(cohesionDistance, separationDistance, _currentTarget.transform.position);
                    }
                
                }
                else
                {
                    fish.RotateTowardDirection(rotationDirection);
                }
            
                fish.transform.Translate(0.0f, 0.0f, Time.deltaTime * fish.currentSpeed);
            }
        
            ChangeFlockingTarget();

            _octree.UpdateTree();
        
            UpdateCurrentFrameRate();
        }
    
        protected override void Spawn()
        {
            SpawnBoids();
            _octree.AddObjects(_allBoids);
            StartStopWatch();
        }

    
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                _octree.rootNode.Draw();
            }
        
            DrawGizmo();
        }
    }
}
