using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gmtk_gamejam.EnemySystem
{
    public class PirateShip : MonoBehaviour, ITakeDamage, ITarget
    {
        private enum EnemyState
        {
            Idle,
            Detected
        }

        [Header("Health")]
        [SerializeField] private int maxHealth;
        [SerializeField] private GameObject health;
        [SerializeField] private Image healthBar;
        [SerializeField] private ParticleSystem deathEffect;
        [Header("Attack")]
        [SerializeField] private LayerMask detectLayer;
        [SerializeField] private float detectRange;
        [Header("Spawn")]
        [SerializeField] private SimpleBoat boatPrefab;
        [SerializeField] private float spawnTime;

        private EnemyState _state;
        private RaftController _target;
        private List<SimpleBoat> _boats;

        private float _spawnTimer;
        private int _currentHealth;

        public ITakeDamage Damagable => this;

        private void Start()
        {
            _boats = new List<SimpleBoat>();
            _spawnTimer = spawnTime;
            _currentHealth = maxHealth;
            health.SetActive(false);
        }
        private void OnDestroy()
        {
            for (int i = _boats.Count - 1; i >= 0; i--)
            {
                if (_boats[i] != null)
                    Destroy(_boats[i].gameObject);
            }
            _boats.Clear();
        }

        private void Update()
        {
            UpdateState();
        }
        public void TakeDamage(int damage)
        {
            transform.DOShakePosition(.3f, .2f);
            _currentHealth -= damage;
            health.SetActive(true);
            healthBar.DOFillAmount(_currentHealth * 1f / maxHealth, .2f);
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        public Transform GetTransform()
        {
            return transform;
        }

        public Vector2 GetPos(float timePased)
        {
            return transform.position;
        }

        #region Statemachine Methods
        private void UpdateState()
        {
            switch (_state)
            {
                case EnemyState.Idle:
                    IdleState();
                    break;
                case EnemyState.Detected:
                    DetectedState();
                    break;
            }
        }

        private void DetectedState()
        {
            if (_spawnTimer >= spawnTime)
            {
                _spawnTimer = 0f;
                //spawn
                SimpleBoat boat = Instantiate(boatPrefab, transform.position + transform.up * 2f, Quaternion.identity);
                boat.SetTarget(_target);
                _boats.Add(boat);
            }
            _spawnTimer += Time.deltaTime;
        }

        private void IdleState()
        {
            var col = Physics2D.OverlapCircle(transform.position, detectRange, detectLayer);
            if (col != null)
            {
                _target = col.GetComponent<RaftController>();
                if (_target)
                {
                    ChangeState(EnemyState.Detected);
                }
            }
        }

        private void ChangeState(EnemyState newState)
        {
            _state = newState;
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }

    }
}
