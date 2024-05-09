using System;
using UnityEngine;

namespace JustGame.Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed;
        [SerializeField] private Vector2 m_direction;
        [SerializeField] private Vector2 m_limitation;

        private Vector2 m_deltaMovement;
        
        private void Update()
        {
            UpdateInput();
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

        private void UpdateMovement()
        {
            transform.Translate(m_direction * (Time.deltaTime* m_moveSpeed));

            m_deltaMovement = transform.position;
            if (Mathf.Abs(m_deltaMovement.x) > m_limitation.x)
            {
                m_deltaMovement.x = m_deltaMovement.x > 0 ? m_limitation.x : -m_limitation.x;
                transform.position = m_deltaMovement;
            }
            if (Mathf.Abs(m_deltaMovement.y) > m_limitation.y)
            {
                m_deltaMovement.y = m_deltaMovement.y > 0 ? m_limitation.y : -m_limitation.y;
                transform.position = m_deltaMovement;
            }
        }
    }
}
