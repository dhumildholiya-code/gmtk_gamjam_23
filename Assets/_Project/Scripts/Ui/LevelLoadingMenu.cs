using TMPro;
using UnityEngine;

namespace gmtk_gamejam.Ui
{
    public class LevelLoadingMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelName;

        private void Update()
        {
            levelName.text = $"Level : {GameManager.Instance.CurrentLevelId}";
        }
    }
}