using System;
using System.Collections.Generic;
using UnityEngine;

namespace gmtk_gamejam.PropSystem
{
    public class PropManager : MonoBehaviour
    {
        [Serializable]
        private struct PropData
        {
            public BaseProp prop;
            public int count;
        }

        [Header("Ui")]
        [SerializeField] private Transform uiPropList;
        [SerializeField] private PropUi propUiPrefab;

        [SerializeField] private PropData[] data;

        private Dictionary<string, BaseProp> _props;
        private Dictionary<string, int> _propsCount;
        private Dictionary<string, PropUi> _uiProps;

        private Camera _cam;
        private BaseProp _currentProp;
        private bool _holdingProp;

        private void Start()
        {
            _cam = Camera.main;
            Setup();
        }
        private void Setup()
        {
            //Listen For Events
            PlayerController.OnPlayerStateChanged += HandlePlayerStateChange;
            PropUi.OnClickPropUi += HandlePropUiClick;

            _props = new Dictionary<string, BaseProp>();
            _uiProps = new Dictionary<string, PropUi>();
            _propsCount = new Dictionary<string, int>();
            foreach (var propData in data)
            {
                BaseProp prop = propData.prop;
                _propsCount.Add(prop.name, propData.count);
                _props.Add(prop.name, propData.prop);
            }
            foreach (var keyValue in _props)
            {
                PropUi uiProp = Instantiate(propUiPrefab, uiPropList);
                BaseProp prop = _props[keyValue.Key];
                uiProp.Init(prop.name, prop.propImage, _propsCount[prop.name]);
                _uiProps.Add(keyValue.Key, uiProp);
            }
            uiPropList.gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            //Remove Events
            PlayerController.OnPlayerStateChanged -= HandlePlayerStateChange;
            PropUi.OnClickPropUi -= HandlePropUiClick;
        }

        private void Update()
        {
            if (_currentProp == null) return;
            Vector2 targetPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _currentProp.transform.position = new Vector2(Mathf.FloorToInt(targetPos.x), Mathf.FloorToInt(targetPos.y)) + Vector2.one * .5f;
            //Placing the prop - Left Click
            if (Input.GetMouseButtonDown(0) && _holdingProp)
            {
                _currentProp.transform.SetParent(transform);
                _propsCount[_currentProp.name] -= 1;
                if (_propsCount[_currentProp.name] == 0)
                {
                    // TODO : remove repected UiProp.
                }
                _uiProps[_currentProp.name].UpdateCount(_propsCount[_currentProp.name]);
                _currentProp = null;
                _holdingProp = false;
            }
            //Removing temporary prop - Right Click
            if (Input.GetMouseButtonDown(1) && _holdingProp)
            {
                Destroy(_currentProp.gameObject);
                _currentProp = null;
                _holdingProp = false;
            }
        }

        #region Handle Events
        private void HandlePlayerStateChange(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Simulation:
                    uiPropList.gameObject.SetActive(false);
                    break;
                case PlayerState.PropSetup:
                    uiPropList.gameObject.SetActive(true);
                    break;
            }
        }
        private void HandlePropUiClick(string propName)
        {
            BaseProp prop = _props[propName];
            if (_propsCount[propName] <= 0)
            {
                return;
            }
            Vector2 pos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _currentProp = Instantiate(prop, pos, Quaternion.identity);
            _holdingProp = true;
        }
        #endregion

        #region Utilities
#if UNITY_EDITOR
        private void PrettyPrintProps()
        {
            foreach (var keyValue in _props)
            {
                Debug.Log($"{keyValue.Key} : {_propsCount[keyValue.Key]} -- {keyValue.Value.propImage.name}");
            }
        }
#endif
        #endregion
    }
}
