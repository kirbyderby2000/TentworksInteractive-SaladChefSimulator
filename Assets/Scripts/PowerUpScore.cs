using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScore : MonoBehaviour
{
    [Min(1)]
    [SerializeField] int scoreToAddToPlayer = 10;
    public void ActivatePowerUp(PlayerController player)
    {
        GameManager.GameManagerSingleton.ModifyPlayerScore(player.Player, scoreToAddToPlayer);
        Invoke("DestroySelf", 8.0f);
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
