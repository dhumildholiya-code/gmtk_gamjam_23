using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace gmtk_gamejam.AbillitySystem
{
    public class AbilityCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Button useButton;

        private Ability _abillity;

        public void Init(Ability abillity, UnityAction closePanel)
        {
            _abillity = abillity;

            cardName.text = _abillity.name.Replace('_', ' ');
            description.text = _abillity.description.Replace("{amount}", _abillity.amount.ToString());
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(_abillity.Execute);
            useButton.onClick.AddListener(closePanel);
        }
    }
}
