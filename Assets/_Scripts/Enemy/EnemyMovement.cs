using System;
using JustGame.Script.CharacterScript;
using JustGame.Script.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private Vector2 m_direction;
    [SerializeField] private Rigidbody2D m_rigidbody2D;
    [SerializeField] private LayerMask m_obstacleMask;
    
    public void SetDirection(Vector2 newDirection)
    {
        m_direction = newDirection;
    }

    public void Stop()
    {
        m_direction = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!LayerManager.IsInLayerMask(other.gameObject.layer, m_obstacleMask))
        {
            return;
        }
        
        //Change direction on collide with obstacle
        m_direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }
    
    private void UpdateMovement()
    {
        m_rigidbody2D.MovePosition((Vector2)transform.position + m_direction * (Time.fixedDeltaTime * m_moveSpeed));
    }
}
