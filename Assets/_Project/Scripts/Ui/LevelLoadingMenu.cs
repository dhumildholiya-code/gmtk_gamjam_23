using TMPro;
using UnityEngine;

namespace gmtk_gamejam.Ui
{
    public class LevelLoadingMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelName;
        private void OnEnable()
        {
            levelName.text = $"Level : {GameManager.Instance.CurrentLevelId}";
        }
    }
}