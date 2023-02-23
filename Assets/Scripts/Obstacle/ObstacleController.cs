using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Obstacle
{
    public class ObstacleController : MonoBehaviour
    {
        [Header("GUI")]
        [SerializeField] private Button _spawnObstacleButtonClick;
        [SerializeField] private TMP_InputField _spawnObstacleNumber;
    
        [Header("Extent")]
        [SerializeField] private float _areaExtent;
        [Header("Obstacle Parameters")]
        [Range(1f, 5f)][SerializeField] private float _speed = 2f;
        private List<global::Obstacle.Obstacle> _obstacles = new List<global::Obstacle.Obstacle>();
        void Start()
        {
            _spawnObstacleButtonClick.onClick.AddListener(SpawnObstacles);
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var obstacle in _obstacles)
            {
                obstacle.AreaExtent = _areaExtent;
                obstacle.Speed = _speed;
                obstacle.Move();
            }
        
        }
    
        private void SpawnObstacles()
        {
            int numberOfSpawnObject;
            bool canSpawn = int.TryParse(_spawnObstacleNumber.text, out numberOfSpawnObject);

            if (!canSpawn)
            {
                return;
            }
            for (int i = 0; i < numberOfSpawnObject; i++)
            {
                GameObject obstacle = Resources.Load("Obstacle") as GameObject;
                Vector3 position = new Vector3(Random.Range(0, _areaExtent),
                    Random.Range(0, _areaExtent),
                    Random.Range(0, _areaExtent));
                GameObject instantiatedObstacle = Instantiate(obstacle, position, Quaternion.identity);
                _obstacles.Add(instantiatedObstacle.GetComponent<global::Obstacle.Obstacle>());
            }
        }
    }
}
