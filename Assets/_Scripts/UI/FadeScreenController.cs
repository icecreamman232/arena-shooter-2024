using DG.Tweening;
using JustGame.Scripts.ScriptableEvent;
using UnityEngine;

namespace JustGame.Script.UI
{
    public class FadeScreenController : MonoBehaviour
    {
        [SerializeField] private float m_fadeDuration;
        [SerializeField] private BoolEvent m_fadeScreenEvent;
        [SerializeField] private CanvasGroup m_canvasGroup;
        public static bool IsFading;

        private void Awake()
        {
            m_fadeScreenEvent.AddListener(OnFadeScreen);
        }

        private void OnDestroy()
        {
            m_fadeScreenEvent.RemoveListener(OnFadeScreen);
        }

        private void OnFadeScreen(bool toggle)
        {
            if (toggle)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }

        private void OnCompleteFading()
        {
            IsFading = false;
        }
        
        private void FadeIn()
        {
            if (IsFading) return;
            IsFading = true;
            m_canvasGroup.DOFade(1, m_fadeDuration)
                .SetUpdate(true)
                .OnComplete(OnCompleteFading);
        }
        
        private void FadeOut()
        {
            if (IsFading) return;
            IsFading = true;
            m_canvasGroup.DOFade(0, m_fadeDuration)
                .SetUpdate(true)
                .OnComplete(OnCompleteFading);
        }

    }
}
