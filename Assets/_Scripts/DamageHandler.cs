using System;
using JustGame.Script.CharacterScript;
using JustGame.Script.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] private int m_minDamage;
    [SerializeField] private int m_maxDamage;
    [SerializeField] private Vector2 m_knockbackForce;
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

    // protected void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (!LayerManager.IsInLayerMask(other.gameObject.layer, m_targetMask)) return;
    //     CauseDamage(other.gameObject);
    // }

    protected virtual void CauseDamage(GameObject target)
    {
        var health = target.GetComponent<Health>();
        if (health == null) return;

        //Try to add knock back force on target if its possible
        var rigidBody2D = health.gameObject.GetComponent<Rigidbody2D>();
        if (rigidBody2D != null && m_knockbackForce != Vector2.zero)
        {
            var directionToTarget = (target.transform.position - transform.position).normalized;
            rigidBody2D.AddForce(directionToTarget * m_knockbackForce);
        }
        
        health.TakeDamage(GetDamage());
        OnHit?.Invoke();
    }
}
