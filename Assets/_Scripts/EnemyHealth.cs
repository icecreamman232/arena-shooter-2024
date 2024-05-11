
using System.Collections;
using JustGame.Script.UI;
using JustGame.Scripts.Common;
using JustGame.Scripts.Managers;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace JustGame.Script.CharacterScript
{
    public class EnemyHealth : Health
    {
        [SerializeField] private MMF_Player m_deathFeedback;
        [SerializeField] private AnimationParameter m_deathAnim;
        [SerializeField] private EnemyHealthBar m_healthBar;

        protected override void UpdateHealthBar()
        {
            m_healthBar.UpdateHealthBar(MathHelpers.Remap(m_curHealth, 0, m_maxHealth, 0, 1));
            base.UpdateHealthBar();
        }

        protected override void Kill()
        {
            StartCoroutine(KillRoutine());
        }

        protected virtual IEnumerator KillRoutine()
        {
            if (m_isInvulnerable)
            {
                yield break;
            }
            m_isInvulnerable = true;
            m_deathAnim.SetTrigger();
            m_deathFeedback.PlayFeedbacks();
            yield return new WaitForSeconds(m_deathFeedback.TotalDuration + m_deathAnim.Duration);
            base.Kill();
        }
    }
}

