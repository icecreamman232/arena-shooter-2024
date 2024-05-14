using JustGame.Script.Manager;
using UnityEngine;

public class Stair : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerManager.PlayerLayer)
        {
            
        }
    }
}
