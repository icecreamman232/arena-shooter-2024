using System;
using JustGame.Script.CharacterScript;
using JustGame.Script.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] private int m_minDamage;
    [SerializeField] private int m_maxDamage;
    [SerializeField] private LayerMask m_targetMask;

    public Action OnHit;
    
    protected virtual int GetDamage()
    {
        return Random.Range(m_minDamage, m_maxDamage + 1);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!LayerManager.IsInLayerMask(other.gameObject.layer, m_targetMask)) return;
        CauseDamage(other.gameObject);
    }

    protected virtual void CauseDamage(GameObject target)
    {
        var health = target.GetComponent<Health>();
        if (health == null) return;
        
        health.TakeDamage(GetDamage());
        OnHit?.Invoke();
    }
}
