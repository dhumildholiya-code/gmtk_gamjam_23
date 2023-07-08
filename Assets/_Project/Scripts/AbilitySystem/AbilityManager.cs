using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gmtk_gamejam.AbillitySystem
{
    public class AbilityManager : MonoBehaviour
    {
        [SerializeField] private Ability[] abilities;
        [Header("Abillity Card")]
        [SerializeField] private GameObject levelUpPanel;
        [SerializeField] private AbilityCard[] cards;

        private void Start()
        {
            levelUpPanel.SetActive(false);
            RaftController.OnLevelUp += CreateCards;
        }
        private void OnDestroy()
        {
            RaftController.OnLevelUp -= CreateCards;
        }

        private void CreateCards()
        {
            var twoAbillity = abilities.OrderBy(x => Random.value).Take(cards.Length).ToArray();
            for (int i = 0; i < twoAbillity.Count(); i++)
            {
                cards[i].Init(twoAbillity[i], () =>
                {
                    levelUpPanel.SetActive(false);
                    Time.timeScale = 1.0f;
                });
            }
            levelUpPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
