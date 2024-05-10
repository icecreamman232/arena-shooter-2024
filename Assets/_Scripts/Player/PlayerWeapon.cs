using JustGame.Script.Player;
using JustGame.Scripts.Managers;
using UnityEngine;

namespace JustGame.Script.Weapon
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private bool m_isWeaponActive;
        [SerializeField] private ObjectPooler m_objectPooler;
        [SerializeField] private PlayerAim m_playerAim;
        [SerializeField] private Vector2 m_offsetShooting;
        private PlayerMovement m_movement;
        
        private void Start()
        {
            m_playerAim = GetComponentInParent<PlayerAim>();
            m_movement = GetComponentInParent<PlayerMovement>();
        }

        private void Update()
        {
            if (!m_isWeaponActive) return;

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            m_movement.Stop();
            var projectilGO = m_objectPooler.GetPooledGameObject();
            var projectile = projectilGO.GetComponent<Projectile>();
            
            projectile.Spawn(
                (Vector2)transform.position + m_playerAim.AimDirection * m_offsetShooting,
                m_playerAim.AimDirection,
                m_playerAim.AimRotation);
        }
    }
}
