using System;
using JustGame.Scripts.ScriptableEvent;
using UnityEngine;
using UnityEngine.UI;

namespace JustGame.Script.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Image m_healthBar;
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private FloatEvent m_playerHealthEvent;

        private void Awake()
        {
            m_healthBar.fillAmount = 1;
            m_playerHealthEvent.AddListener(UpdateHealthBar);
        }
        
        private void OnDestroy()
        {
            m_playerHealthEvent.RemoveListener(UpdateHealthBar);
        }

        private void UpdateHealthBar(float value)
        {
            m_canvasGroup.alpha = value > 0 ? 1 : 0;
            m_healthBar.fillAmount = value;
        }
    }
}
