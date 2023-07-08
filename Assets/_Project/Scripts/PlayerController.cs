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
            Time.timeScale = 0f;

            //state Transition logic
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(PlayerState.Simulation);
            }
        }

        private void SimulationState()
        {
            Time.timeScale = 1.0f;

            //state Transition logic
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(PlayerState.PropSetup);
            }
        }

        private void ChangeState(PlayerState newState)
        {
            _state = newState;
            OnPlayerStateChanged?.Invoke(_state);
        }
        #endregion
    }
}
