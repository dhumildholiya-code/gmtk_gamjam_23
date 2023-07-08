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

        private void Start()
        {
            ChangeState(startState);
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
                    break;
                case PlayerState.PropSetup:
                    Time.timeScale = 0f;
                    break;
            }
            OnPlayerStateChanged?.Invoke(_state);
        }
        #endregion
    }
}
