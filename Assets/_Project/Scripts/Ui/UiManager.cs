﻿using System;
using UnityEngine;

namespace gmtk_gamejam.Ui
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject gameHUD;
        [SerializeField] private LevelLoadingMenu levelLoadingScreen;
        [SerializeField] private GameObject winMenu;
        [SerializeField] private GameObject moveTutorial;
        [SerializeField] private GameObject propTutorial;

        public void UpdateState(GameState state)
        {
            mainMenu.SetActive(state == GameState.MainMenu);
            gameHUD.SetActive(state == GameState.Gameplay);
            levelLoadingScreen.gameObject.SetActive(
                state == GameState.LevelLoading
                || state == GameState.LevelComplete
                || state == GameState.LevelFailed
                );
            winMenu.SetActive(state == GameState.Win);

            switch (state)
            {
                case GameState.MainMenu:
                    break;
                case GameState.LevelLoading:
                    break;
                case GameState.Gameplay:
                    break;
                case GameState.LevelComplete:
                    break;
                case GameState.LevelFailed:
                    break;
                case GameState.Quit:
                    break;
                case GameState.Win:
                    break;
            }
        }

        public void ShowMoveTutorial(bool isActive)
        {
            moveTutorial.SetActive(isActive);
        }
        public void ShowPropTutorial(bool isActive)
        {
            propTutorial.SetActive(isActive);
        }
    }
}