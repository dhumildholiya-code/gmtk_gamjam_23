using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gmtk_gamejam.Ui
{
    public class MenuButton : MonoBehaviour
    {
        public GameState newState;

        private Button button;
        private TextMeshProUGUI text;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ChangeGameState);
            text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = name.Split('_')[0];
        }

        private void ChangeGameState()
        {
            GameManager.Instance.ChangeState(newState);
        }
    }
}
