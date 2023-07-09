using gmtk_gamejam.AbillitySystem;
using gmtk_gamejam.CameraSystem;
using gmtk_gamejam.LevelSystem;
using gmtk_gamejam.Ui;
using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace gmtk_gamejam
{
    public enum GameState
    {
        MainMenu,
        LevelLoading,
        Gameplay,
        LevelComplete,
        LevelFailed,
        Win,
        Quit
    }
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        [Header("GameData")]
        [SerializeField] private int levelToLoad;
        [SerializeField] private LevelController[] levels;
        [Header("Manager Dependency")]
        [SerializeField] private UiManager uiManager;
        [Header("Dependency")]
        public RaftController raft;
        public Transform uiPropList;
        public GameObject levelUpPanel;
        public AbilityCard[] cards;

        private GameState _state;
        private int _currentLevelId;
        public int CurrentLevelId => _currentLevelId + 1;
        private LevelController _currentLevel;
        private CameraFollow _camFollow;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            _currentLevelId = levelToLoad;
            _camFollow = Camera.main.gameObject.GetComponent<CameraFollow>();
            ChangeState(GameState.MainMenu);
        }
        public void ShowTutorial(int id)
        {
            uiManager.ShowTutorial(id);
        }

        #region StateMachine Methods
        private IEnumerator Co_LoadLevel()
        {
            _currentLevel = Instantiate(levels[_currentLevelId]);

            yield return new WaitForSeconds(.5f);
            _camFollow.SetTarget(raft.transform);
            ChangeState(GameState.Gameplay);
        }

        public void ChangeState(GameState newState)
        {
            _state = newState;
            uiManager.UpdateState(_state);

            switch (_state)
            {
                case GameState.MainMenu:
                    break;
                case GameState.LevelLoading:
                    StartCoroutine(Co_LoadLevel());
                    break;
                case GameState.Gameplay:
                    _currentLevel.Setup(Instance, raft, _camFollow);
                    break;
                case GameState.LevelComplete:
                    _currentLevel.CleanUp();
                    Destroy(_currentLevel.gameObject);
                    if (_currentLevelId + 1 >= levels.Length)
                    {
                        //All level are complete.
                        ChangeState(GameState.Win);
                    }
                    else
                    {
                        _currentLevelId++;
                        StartCoroutine(Co_LoadLevel());
                    }
                    break;
                case GameState.LevelFailed:
                    _currentLevel.CleanUp();
                    Destroy(_currentLevel.gameObject);
                    StartCoroutine(Co_LoadLevel());
                    break;
                case GameState.Quit:
                    _currentLevel.CleanUp();
                    Destroy(_currentLevel.gameObject);
                    Application.Quit();
                    break;
                case GameState.Win:
                    break;
            }
        }

        public void ChangeStateDelay(GameState newState, float delayTime)
        {
            StartCoroutine(Co_WaitForAction(delayTime, () =>
            {
                ChangeState(newState);
            }));
        }
        private IEnumerator Co_WaitForAction(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback.Invoke();
        }
        #endregion
    }
}
