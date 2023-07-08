using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace gmtk_gamejam.LevelSystem
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Tilemap spawnable;
        [SerializeField] private Transform obstacleParent;
        [Header("Prefabs")]
        [SerializeField] private RaftController raftPrefab;
        [SerializeField] private GameObject obstaclePrefab;

        private List<GameObject> _obstacles;
        private RaftController _raft;

        private void Start()
        {
            _obstacles = new List<GameObject>();
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
