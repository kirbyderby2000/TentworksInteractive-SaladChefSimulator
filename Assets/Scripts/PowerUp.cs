using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] Players playerToReward;
    [SerializeField] Collider colliderToDisableUponPickUp;
    [SerializeField] ParticleSystem particleSystemToDisableUponPickUp;

    private bool _consumed;

    public Vector3 PowerUpBoundExtents
    {
        get { return colliderToDisableUponPickUp.bounds.extents; }
    }

    public void AssignPlayerToReward(Players player)
    {
        playerToReward = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_consumed)
            return;

        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            if(playerController.Player == playerToReward)
            {
                OnPowerUpCollected.Invoke(playerController);
                _consumed = true;
                colliderToDisableUponPickUp.enabled = false;
                particleSystemToDisableUponPickUp.Stop();
            }
        }
    }

    [SerializeField] PlayerControllerHandler OnPowerUpCollected;

}

[System.Serializable]
public class PlayerControllerHandler : UnityEngine.Events.UnityEvent<PlayerController>
{

}
