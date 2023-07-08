using UnityEngine;
using UnityEngine.UI;

namespace gmtk_gamejam.Ui
{
    public class ExpBar : MonoBehaviour
    {
        [SerializeField] private Image bar;

        private void Start()
        {
            RaftController.OnExpChange += HandleExpChange;
        }
        private void OnDestroy()
        {
            RaftController.OnExpChange -= HandleExpChange;
        }

        private void HandleExpChange(int updateValue, int maxAmount)
        {
            float percentage =  (updateValue*1f / maxAmount*1f);
            bar.fillAmount = percentage; 
        }
    }
}