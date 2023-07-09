using gmtk_gamejam.AbillitySystem;
using UnityEngine;

namespace gmtk_gamejam
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        [Header("Dependency")]
        public Transform uiPropList;
        public GameObject levelUpPanel;
        public AbilityCard[] cards;

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
