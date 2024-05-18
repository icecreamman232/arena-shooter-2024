using MoreMountains.Tools;
using UnityEngine;

namespace JustGame.Script.Manager
{
    public class TimeManager : MMSingleton<TimeManager>
    {
        [SerializeField] private bool m_isPaused;

        public bool IsPaused => m_isPaused;
        
        public void Pause()
        {
            m_isPaused = true;
            Time.timeScale = 0;
        }

        public void Unpause()
        {
            m_isPaused = false;
            Time.timeScale = 1;
        }
        
    }
}

