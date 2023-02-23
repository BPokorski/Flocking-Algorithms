using System.Collections.Generic;
using UnityEngine;

namespace BoidController
{
    public class BoidsController : BaseBoidController
    {
        private Bounds _extentBound = new Bounds();
        private void Start()
        {
            _extentBound.SetMinMax(Vector3.zero, new Vector3(_areaExtent, _areaExtent, _areaExtent));
            
        }
    
        void Update()
        {
            _averageFlockingPosition = Vector3.zero;
            foreach (var fish in _allBoids)
            {
                fish.neighbours = new HashSet<Boid>(_allBoids);
                _averageFlockingPosition += fish.TransformProperty.position;
                var rotationDirection = fish.GetRotationDirection(_extentBound,
                    _currentTarget.transform.position,
                    obstacleDistance);
                if (rotationDirection == Vector3.zero)
                {
                    if (_currentFrame == _nthFrameCalculation)
                    {
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
            
            UpdateCurrentFrameRate();
        }

        protected override void Spawn()
        {
            SpawnBoids();
            StartStopWatch();
        }
        private void OnDrawGizmos()
        {
        
            DrawGizmo();
        }
    }
}
