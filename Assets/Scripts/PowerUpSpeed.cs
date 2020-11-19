using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : MonoBehaviour
{
    [SerializeField] float duration = 10.0f;

    [Min(0.0f)]
    [SerializeField] float speedModAMT = 0.25f;


    public void ActivatePowerUp(PlayerController player)
    {
        StartCoroutine(PowerUpCoroutine(player));
    }

    IEnumerator PowerUpCoroutine(PlayerController player)
    {
        player.AddToMoveSpeedModifier(speedModAMT);
        yield return new WaitForSeconds(duration);
        player.AddToMoveSpeedModifier(-speedModAMT);
        yield return null;
        Destroy(this.gameObject);
    }
}
