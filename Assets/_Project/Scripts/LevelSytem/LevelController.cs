using gmtk_gamejam.AbillitySystem;
using gmtk_gamejam.CameraSystem;
using gmtk_gamejam.PropSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace gmtk_gamejam.LevelSystem
{
    public class LevelController : MonoBehaviour
    {
        [Header("Level Reference")]
        [SerializeField] private Tilemap spawnable;
        [SerializeField] private PropManager propManager;
        [SerializeField] private AbilityManager abilityManager;
        [SerializeField] private Transform obstacleParent;
        [Header("Prefabs")]
        [SerializeField] private RaftController raftPrefab;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private GameObject levelCompletePrefab;

        private List<GameObject> _obstacles;
        private GameManager _gameManager;
        private RaftController _raft;
        private CameraFollow _cameraFollow;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _obstacles = new List<GameObject>();
            _cameraFollow = Camera.main.gameObject.GetComponent<CameraFollow>();
            propManager.Setup(_gameManager.uiPropList);
            abilityManager.Setup(_gameManager.levelUpPanel, _gameManager.cards);
            SetupLevel();
        }

        private void SetupLevel()
        {
            BoundsInt bounds = spawnable.cellBounds;
            TileBase[] tiles = spawnable.GetTilesBlock(bounds);
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = tiles[x + y * bounds.size.x];
                    if (tile != null)
                    {
                        Vector3 pos = spawnable.GetCellCenterWorld(new Vector3Int(x + bounds.position.x, y + bounds.position.y, 0));
                        switch (tile.name)
                        {
                            case "obstacle_tile":
                                GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity, obstacleParent);
                                _obstacles.Add(obstacle);
                                break;
                            case "water_tile":
                                RaftController raft = Instantiate(raftPrefab);
                                raft.transform.position = pos;
                                _raft = raft;
                                _cameraFollow.SetTarget(_raft.transform);
                                break;
                            case "levelcomplete_tile":
                                Instantiate(levelCompletePrefab, pos, Quaternion.identity);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
