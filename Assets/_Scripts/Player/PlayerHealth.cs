using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace JustGame.Script.CharacterScript
{
    public class PlayerHealth : Health
    {
        [SerializeField] private MMF_Player m_hitFeedback;

        public override void TakeDamage(int damage)
        {
            if (damage <= 0) return;

            if (m_isInvulnerable) return;
            
            m_curHealth -= damage;

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

