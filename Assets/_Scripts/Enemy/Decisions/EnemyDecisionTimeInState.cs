using JustGame.Scripts.Enemy;
using UnityEngine;

public class EnemyDecisionTimeInState : BrainDecision
{
    [SerializeField] private float m_minTime;
    [SerializeField] private float m_maxTime;
    private float m_time;

    public override void OnEnterState()
    {
        base.OnEnterState();
        m_time = Random.Range(m_minTime, m_maxTime);
    }

    public override bool CheckDecision()
    {
        return m_brain.TimeInState >= m_time;
    }
}
