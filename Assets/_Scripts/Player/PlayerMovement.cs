using System;
using UnityEngine;

namespace JustGame.Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed;
        [SerializeField] private Vector2 m_direction;
        [SerializeField] private Rigidbody2D m_rigidbody2D;

        private void Update()
        {
            UpdateInput();
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateInput()
        {
            if (Input.GetKey(KeyCode.A))
            {
                m_direction.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                m_direction.x = 1;
            }
            else
            {
                m_direction.x = 0;
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                m_direction.y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                m_direction.y = -1;
            }
            else
            {
                m_direction.y = 0;
            }
        }

        public void Stop()
        {
            m_direction = Vector2.zero;
            m_rigidbody2D.velocity = Vector2.zero;
            m_rigidbody2D.angularVelocity = 0;
        }
        
        private void UpdateMovement()
        {
            m_rigidbody2D.MovePosition((Vector2)transform.position + m_direction * (Time.fixedDeltaTime * m_moveSpeed));
        }
    }
}
