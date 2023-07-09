using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace gmtk_gamejam.Ui
{
    public class TresureBar : MonoBehaviour
    {
        [SerializeField] private Image bar;

        private void Start()
        {
            RaftController.OnTresureChange += HandleTresureChange;
        }
        private void OnDestroy()
        {
            RaftController.OnTresureChange -= HandleTresureChange;
        }

        private void HandleTresureChange(int updateValue, int maxAmount)
        {
            float percentage =  (updateValue*1f / maxAmount*1f);
            bar.DOFillAmount(percentage, .3f);
        }

    }
}
