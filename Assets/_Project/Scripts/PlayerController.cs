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
        private int _tutorialId;
        private int _tutorialCount;

        private void Start()
        {
        }
        public void Setup(int tutorialId, int tutorialCount)
        {
            _tutorialCount = tutorialCount;
            _tutorialId = tutorialId;
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
                    if (_tutorialCount > 0 && (_tutorialId == 0 || _tutorialId == 2))
                    {
                        GameManager.Instance.ShowTutorial(_tutorialId);
                        _tutorialId++;
                        _tutorialCount--;
                        Time.timeScale = 0f;
                    }
                    break;
                case PlayerState.PropSetup:
                    Time.timeScale = 0f;
                    if (_tutorialCount > 0 && _tutorialId == 1)
                    {
                        GameManager.Instance.ShowTutorial(_tutorialId);
                        _tutorialId++;
                        _tutorialCount--;
                    }
                    break;
            }
            OnPlayerStateChanged?.Invoke(_state);
        }
        #endregion
    }
}
