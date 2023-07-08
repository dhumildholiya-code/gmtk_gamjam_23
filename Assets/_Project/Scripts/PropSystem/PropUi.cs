using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace gmtk_gamejam.PropSystem
{
    public class PropUi : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static event Action<string> OnClickPropUi;

        [SerializeField] private Image propImage;
        [SerializeField] private TextMeshProUGUI countText;

        private string _name;

        public void Init(string name, Sprite propSprite, int count)
        {
            _name = name;
            propImage.sprite = propSprite;
            countText.text = $"{count}";
        }
        public void UpdateCount(int count)
        {
            countText.text = count.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //OnClickPropUi?.Invoke(_name);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnClickPropUi?.Invoke(_name);
        }
    }
}
