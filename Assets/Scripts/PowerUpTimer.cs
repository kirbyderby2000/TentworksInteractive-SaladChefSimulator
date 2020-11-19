using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTimer : MonoBehaviour
{
    [Min(1)]
    [SerializeField] int timeToAddToPlayer = 30;
    public void ActivatePowerUp(PlayerController player)
    {
        GameManager.GameManagerSingleton.ModifyPlayerTimer(player.Player, timeToAddToPlayer);
        Invoke("DestroySelf", 8.0f);
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
