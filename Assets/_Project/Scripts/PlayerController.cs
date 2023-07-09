using gmtk_gamejam.PropSystem;
using System;
using UnityEngine;

namespace gmtk_gamejam
{
    public enum PlayerState
    {
        Simulation,
        PropSetup
    }
    public class PlayerController : MonoBehaviour
    {
        public static event Action<PlayerState> OnPlayerStateChanged;

        [SerializeField] private PlayerState startState;

        private PlayerState _state;
        private bool _isTutorial;
        private int _tutorialCount;

        private void Start()
        {
        }
        public void Setup(bool isTutorial)
        {
            _tutorialCount = 2;
            _isTutorial = isTutorial;
            ChangeState(startState);
        }
        public void CleanUp()
        {
        }

        private void Update()
        {
            UpdateState();
        }

        #region State Methods
        private void UpdateState()
        {
            switch (_state)
            {
                case PlayerState.Simulation:
                    SimulationState();
                    break;
                case PlayerState.PropSetup:
                    PropSetupState();
                    break;
            }
        }

        private void PropSetupState()
        {
            //state Transition logic
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(PlayerState.Simulation);
            }
        }

        private void SimulationState()
        {
            //state Transition logic
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(PlayerState.PropSetup);
            }
        }

        private void ChangeState(PlayerState newState)
        {
            _state = newState;
            switch (_state)
            {
                case PlayerState.Simulation:
                    Time.timeScale = 1.0f;
                    if(_tutorialCount > 0 && _isTutorial)
                    {
                        GameManager.Instance.ShowPropTutorial(false);
                        _tutorialCount--;
                        GameManager.Instance.ShowMoveTutorial(true);
                        Time.timeScale = 0f;
                    }
                    break;
                case PlayerState.PropSetup:
                    Time.timeScale = 0f;
                    if(_tutorialCount > 0 && _isTutorial)
                    {
                        GameManager.Instance.ShowMoveTutorial(false);
                        _tutorialCount--;
                        GameManager.Instance.ShowPropTutorial(true);
                    }
                    break;
            }
            OnPlayerStateChanged?.Invoke(_state);
        }
        #endregion
    }
}
