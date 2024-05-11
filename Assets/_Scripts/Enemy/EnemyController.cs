using System;
using JustGame.Scripts.Enemy;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyBrain m_curBrain;

    public void SetBrain(EnemyBrain brain)
    {
        m_curBrain = brain;
    }

    public void StopBrain()
    {
        m_curBrain.BrainActive = false;
        m_curBrain.gameObject.SetActive(false);
    }

    public void StartBrain(EnemyBrain brain = null)
    {
        if (brain != null)
        {
            SetBrain(brain);
        }
        m_curBrain.BrainActive = true;
        m_curBrain.gameObject.SetActive(true);
    }
}
