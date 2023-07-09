using gmtk_gamejam.AbillitySystem;
using gmtk_gamejam.PropSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace gmtk_gamejam.LevelSystem
{
    public class LevelController : MonoBehaviour
    {
        [Header("Level Data")]
        [SerializeField] private int sharkCount;
        [SerializeField] private int maxTresure;
        [SerializeField] private Direction direction;
        [Header("Level Reference")]
        [SerializeField] private Tilemap spawnable;
        [SerializeField] private PropManager propManager;
        [SerializeField] private AbilityManager abilityManager;
        [SerializeField] private Transform obstacleParent;
        [Header("Prefabs")]
        [SerializeField] private GameObject boulder1Prefab;
        [SerializeField] private GameObject boulder2Prefab;
        [SerializeField] private GameObject levelCompletePrefab;

        private List<GameObject> _spawnObjects;
        private GameManager _gameManager;
        private RaftController _raft;

        public void Setup(GameManager gameManager, RaftController raft)
        {
            _gameManager = gameManager;
            _raft = raft;
            _spawnObjects = new List<GameObject>();
            propManager.Setup(_gameManager.uiPropList);
            abilityManager.Setup(_gameManager.levelUpPanel, _gameManager.cards);
            SetupLevel();
        }
        public void CleanUp()
        {
            propManager.CleanUp();
            _raft.CleanUp();
            for (int i = _spawnObjects.Count - 1; i >= 0; i--)
            {
                Destroy(_spawnObjects[i]);
            }
            _spawnObjects.Clear();
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
                            case "boulder1_tile":
                                GameObject obstacle1 = Instantiate(boulder1Prefab, pos, Quaternion.identity, obstacleParent);
                                _spawnObjects.Add(obstacle1);
                                break;
                            case "boulder2_tile":
                                GameObject obstacle2 = Instantiate(boulder2Prefab, pos, Quaternion.identity, obstacleParent);
                                _spawnObjects.Add(obstacle2);
                                break;
                            case "water_tile":
                                _raft.transform.position = pos;
                                _raft.Setup(maxTresure, sharkCount, direction);
                                break;
                            case "levelcomplete_tile":
                                GameObject levelComplete = Instantiate(levelCompletePrefab, pos, Quaternion.identity);
                                _spawnObjects.Add(levelComplete);
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
