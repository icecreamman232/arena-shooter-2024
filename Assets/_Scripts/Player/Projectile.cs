using System;
using JustGame.Script.Manager;
using UnityEngine;

namespace JustGame.Script.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [Header("Range")] 
        [SerializeField] private bool m_destroyWhenOutOfRange;
        [SerializeField] private float m_maxRange;

        [Header("Speed")] 
        [SerializeField] private float m_moveSpeed;
        [SerializeField] private Vector2 m_direction;

        [Header("Collision")] 
        [SerializeField] private bool m_destroyWhenCollide;
        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private Transform m_rotationPivot;


        private DamageHandler m_damageHandler;
        private Vector2 m_startPos;
        private float m_traveledDist;

        private void Start()
        {
            m_damageHandler = GetComponent<DamageHandler>();
            m_damageHandler.OnHit += DestroyProjectile;
        }

        public virtual void Spawn(Vector2 position, Vector2 direction, Quaternion rotation)
        {
            m_startPos = position;
            m_direction = direction;

            transform.position = m_startPos;
            m_rotationPivot.rotation = rotation;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!m_destroyWhenCollide) return;
            if (!LayerManager.IsInLayerMask(other.gameObject.layer, m_layerMask)) return;
            
            DestroyProjectile();
        }

        protected virtual void Update()
        {
            if (!gameObject.activeSelf) return;

            UpdateMovement();
            
            m_traveledDist = Vector2.Distance(m_startPos, transform.position);
            if(m_traveledDist >= m_maxRange && m_destroyWhenOutOfRange)
            {
                DestroyProjectile();
            }
        }

        protected virtual void UpdateMovement()
        {
            transform.Translate(m_direction * (Time.deltaTime * m_moveSpeed));
        }

        protected virtual void DestroyProjectile()
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }

        private void OnDestroy()
        {
            if (m_damageHandler == null) return;
            m_damageHandler.OnHit -= DestroyProjectile;
        }
    }
}

