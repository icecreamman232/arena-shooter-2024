using JustGame.Scripts.Managers;
using JustGame.Scripts.ScriptableEvent;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace JustGame.Script.CharacterScript
{
    public class PlayerHealth : Health
    {
        [SerializeField] private MMF_Player m_hitFeedback;
        [SerializeField] private FloatEvent m_playerHealthBarEvent;

        protected override void Start()
        {
            base.Start();
            UpdateHealthBar();
        }

        protected override void UpdateHealthBar()
        {
            m_playerHealthBarEvent.Raise(MathHelpers.Remap(m_curHealth,0,m_maxHealth,0,1));
            base.UpdateHealthBar();
        }

        public override void TakeDamage(int damage)
        {
            if (damage <= 0) return;

            if (m_isInvulnerable) return;
            
            m_curHealth -= damage;
            UpdateHealthBar();

            m_hitFeedback.PlayFeedbacks();
            
            if (m_curHealth <= 0)
            {
                Kill();
                return;
            }

            StartCoroutine(OnInvulnerable());
        }
    }
}

