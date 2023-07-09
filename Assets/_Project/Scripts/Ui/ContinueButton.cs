using UnityEngine;
using UnityEngine.UI;

namespace gmtk_gamejam.Ui
{
    public class ContinueButton : MonoBehaviour
    {
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => { Time.timeScale = 1f; });
        }
    }
}
