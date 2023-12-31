using System.Linq;
using UnityEngine;

namespace gmtk_gamejam.AbillitySystem
{
    public class AbilityManager : MonoBehaviour
    {
        [SerializeField] private Ability[] abilities;

        private GameObject _levelUpPanel;
        private AbilityCard[] _cards;

        public void Setup(GameObject levelUpPanel, AbilityCard[] cards)
        {
            _levelUpPanel = levelUpPanel;
            _cards = cards;
            _levelUpPanel.SetActive(false);
        }
        private void Start()
        {
            RaftController.OnLevelUp += CreateCards;
        }
        private void OnDestroy()
        {
            RaftController.OnLevelUp -= CreateCards;
        }

        private void CreateCards()
        {
            if (abilities.Length <= 2)
            {
                return;
            }
            var twoAbillity = abilities.OrderBy(x => Random.value).Take(_cards.Length).ToArray();
            for (int i = 0; i < twoAbillity.Count(); i++)
            {
                _cards[i].Init(twoAbillity[i], () =>
                {
                    _levelUpPanel.SetActive(false);
                    Time.timeScale = 1.0f;
                });
            }
            _levelUpPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
