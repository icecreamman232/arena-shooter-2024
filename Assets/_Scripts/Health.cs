using System;
using System.Collections;
using UnityEngine;

namespace JustGame.Script.CharacterScript
{
    public class Health : MonoBehaviour
    {
        [SerializeField] protected int m_maxHealth;
        [SerializeField] protected int m_curHealth;
        [SerializeField] protected float m_invulnerableDuration;

        protected bool m_isInvulnerable;

        private void Start()
        {
            m_curHealth = m_maxHealth;
        }

        public virtual void TakeDamage(int damage)
        {
            if (damage <= 0) return;

            if (m_isInvulnerable) return;
            
            m_curHealth -= damage;

            if (m_curHealth <= 0)
            {
                Kill();
                return;
            }

            StartCoroutine(OnInvulnerable());
        }

        protected virtual IEnumerator OnInvulnerable()
        {
            if (m_isInvulnerable)
            {
                yield break;
            }

            m_isInvulnerable = true;
            yield return new WaitForSeconds(m_invulnerableDuration);
            m_isInvulnerable = false;
        }

        protected virtual void Kill()
        {
            m_isInvulnerable = true;
            Destroy(this.gameObject);
        }
    }
}

