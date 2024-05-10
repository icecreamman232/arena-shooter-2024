
using System.Collections;
using JustGame.Scripts.Common;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace JustGame.Script.CharacterScript
{
    public class EnemyHealth : Health
    {
        [SerializeField] private MMF_Player m_deathFeedback;
        [SerializeField] private AnimationParameter m_deathAnim;

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

