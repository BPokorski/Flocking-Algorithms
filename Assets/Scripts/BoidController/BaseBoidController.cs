using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BoidController
{
    public abstract class BaseBoidController : MonoBehaviour
    {
        [Header("User Interface")]
        [SerializeField] protected Button _spawnButton;
        [SerializeField] protected TMP_InputField _spawnNumberInput;
        [SerializeField] protected TextMeshProUGUI _watchText;
        [SerializeField] protected Button _restartButton;
        
        [Header("Boid Algorithm parameters")]
        [Range(1f, 15f)] [SerializeField] protected float cohesionDistance = 10f;
        [Range(0f, 5f)] [SerializeField] protected float separationDistance = 1f;
        [Range(0f, 20f)] [SerializeField] protected float obstacleDistance = 0f;
        
        [Header("Scene parameters")] 
        [SerializeField] protected float _areaExtent = 200f;
        
        [Tooltip("Number of frames between calculation")]
        [SerializeField] protected int _nthFrameCalculation = 1;
        
        [Tooltip("Target to which boids will go")]
        [SerializeField] protected GameObject _currentTarget;

        [Tooltip("If boids group will be closer to target than this distance, target will change")]
        [SerializeField] protected float _flockingTargetDistance = 16;
        
        protected List<Vector3> _targetPositions = new List<Vector3>();
        protected Vector3 _spawnLimits = new Vector3(10f, 10f, 10f);
        protected List<Boid> _allBoids = new List<Boid>();
        protected StopWatch _watch;
        protected int _currentFrame;
        protected int _currentTargetIndex;
        protected Vector3 _averageFlockingPosition = Vector3.zero;
        
        private void Awake()
        {
            _spawnButton.onClick.AddListener(Spawn);
            _restartButton.onClick.AddListener(Restart);
            _watch = _watchText.GetComponent<StopWatch>();
            _targetPositions.Add(new Vector3(10f, 10f, 78.4f));
            _targetPositions.Add(new Vector3(140f, 70f, 90f));
            _targetPositions.Add(new Vector3(190f, 10f, 180f));
            _targetPositions.Add(new Vector3(130f, 195f, 67f));
            _targetPositions.Add(new Vector3(63.2f, 104f, 107.4f));
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        protected abstract void Spawn();
        
        protected void SpawnBoids()
        {
            int numberOfSpawnObject;
            bool canSpawn = int.TryParse(_spawnNumberInput.text, out numberOfSpawnObject);

            if (!canSpawn)
            {
                return;
            }
            for (int i = 0; i < numberOfSpawnObject; ++i) {

                Vector3 pos = new Vector3(Random.Range(0, _spawnLimits.x),
                    Random.Range(0, _spawnLimits.y),
                    Random.Range(0, _spawnLimits.z));
                GameObject gameObjectBoid = Resources.Load("Boid") as GameObject;
                GameObject instantiatedBoid = Instantiate(gameObjectBoid, pos, Quaternion.identity);
            
                Boid boid = instantiatedBoid.GetComponent<Boid>();
                boid.TransformProperty = boid.transform;
                instantiatedBoid.gameObject.name = $"Boid {boid.GetInstanceID()}";
            
                _allBoids.Add(boid);

            }
        }
        protected void StartStopWatch()
        {
            if (!_watch.isTimerActive)
            {
                _watch.StartStopTiming();
            }
        }

        protected void ChangeFlockingTarget()
        {
            if (_allBoids.Count > 0)
            {
                _averageFlockingPosition /= _allBoids.Count;
            }
            var flockingTargetDistance = (_averageFlockingPosition - _currentTarget.transform.position).sqrMagnitude;
            if (flockingTargetDistance < 16f)
            {
                if (_currentTargetIndex < (_targetPositions.Count - 1))
                {
                    _currentTargetIndex++;
                    _currentTarget.transform.position = _targetPositions[_currentTargetIndex];
                }
                else
                {
                    StartStopWatch();
                }
                _averageFlockingPosition = Vector3.zero;
            }
        }

        protected void UpdateCurrentFrameRate()
        {
            if (_currentFrame == _nthFrameCalculation)
            {
                _currentFrame = 1;
            }
            else
            {
                _currentFrame++;
            }
        }

        protected void DrawGizmo()
        {
            foreach (var fish in _allBoids)
            {
                fish.Draw(cohesionDistance);
            }
        }
    }
}