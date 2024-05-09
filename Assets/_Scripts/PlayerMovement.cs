using System;
using UnityEngine;

namespace JustGame.Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed;
        [SerializeField] private Vector2 m_direction;

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
            transform.Translate(m_direction *(Time.deltaTime * m_moveSpeed));
        }
    }
}
