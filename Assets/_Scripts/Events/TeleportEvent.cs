using System;
using JustGame.Script.Level;
using UnityEngine;

[CreateAssetMenu(menuName = "JustGame/Teleport Event")]
public class TeleportEvent : ScriptableObject
{
    protected Action<Door, Door> m_listeners;
        
    public void AddListener(Action<Door, Door> addListener)
    {
        m_listeners += addListener;
    }

    public void RemoveListener(Action<Door, Door> removeListener)
    {
        m_listeners -= removeListener;
    }

    public void Raise(Door cur, Door next)
    {
        m_listeners?.Invoke(cur, next);
    }
}
