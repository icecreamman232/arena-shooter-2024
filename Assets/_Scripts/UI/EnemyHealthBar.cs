using System;
using UnityEngine;
using UnityEngine.UI;

namespace JustGame.Script.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Image m_healthBar;
        [SerializeField] private CanvasGroup m_canvasGroup;
        
        private void Start()
        {
            m_canvasGroup.alpha = 0;
        }

        public void UpdateHealthBar(float value)
        {
            m_canvasGroup.alpha = value > 0 ? 1 : 0;
            m_healthBar.fillAmount = value;
        }
    }

}
