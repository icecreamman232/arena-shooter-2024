using JustGame.Script.CharacterScript;
using UnityEngine;
using UnityEngine.Events;

public class EnemyLayout : MonoBehaviour
{
    [SerializeField] private Health[] m_enemies;
    [SerializeField] private UnityEvent m_OnAllEnemyKilled;
    private int m_numberEnemy;

    private void Start()
    {
        m_numberEnemy = m_enemies.Length;
        for (int i = 0; i < m_enemies.Length; i++)
        {
            m_enemies[i].OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        m_numberEnemy--;
        if (m_numberEnemy <= 0)
        {
            m_OnAllEnemyKilled?.Invoke();
        }
    }
}
