using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunisher : MonoBehaviour
{
    [SerializeField] PlayerController playerToPunish;
    [Min(1.0f)]
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float rotateSpeed = 360.0f;

    public void OnPlayerRanOutOfTime()
    {
        PlayerPunishedState playerPunishState = new PlayerPunishedState(playerToPunish, moveSpeed, rotateSpeed);
        playerToPunish.SetPlayerState(playerPunishState);

    }
}

public class PlayerPunishedState : PlayerState
{
    Vector3 playerStartingPosition;
    Vector3 destinationPosition;
    float moveSpeed;
    float rotateSpeed;

    Vector3 startingEulerAngle;

    public PlayerPunishedState(PlayerController playerControllerStateMachine, float moveSpeed, float rotateSpeed) : base(playerControllerStateMachine)
    {
        this.moveSpeed = moveSpeed;
        this.rotateSpeed = rotateSpeed;
        Vector3 playerViewportPosition = MainCamera.CameraInstance.WorldToViewportPoint(playerControllerStateMachine.transform.position);
        if(playerViewportPosition.x < 0.5f)
        {
            playerViewportPosition.x = 0.0f;
        }
        else
        {
            playerViewportPosition.x = 1.0f;
        }

        if (playerViewportPosition.y < 0.5f)
        {
            playerViewportPosition.y = 0.0f;
        }
        else
        {
            playerViewportPosition.y = 1.0f;
        }

        playerViewportPosition.z = Vector3.Distance(MainCamera.CameraInstance.transform.position, playerControllerStateMachine.transform.position);

        destinationPosition = MainCamera.CameraInstance.ViewportToWorldPoint(playerViewportPosition);
        destinationPosition.y = MainCamera.CameraInstance.transform.position.y;

        startingEulerAngle = playerControllerStateMachine.transform.right;

        while (playerControllerStateMachine.PlayerHand.HasAnyItemInHand())
        {
            playerControllerStateMachine.PlayerHand.DropItem();
        }
    }

    public override void HandlePlayerInput(PlayerInput input)
    {
        playerControllerStateMachine.transform.position = Vector3.MoveTowards(playerControllerStateMachine.transform.position,
            destinationPosition,
            Time.deltaTime * moveSpeed);

        playerControllerStateMachine.transform.Rotate(startingEulerAngle, Time.deltaTime * rotateSpeed);
        if (Vector3.Distance(playerControllerStateMachine.transform.position, destinationPosition) <= Mathf.Epsilon)
        {
            playerControllerStateMachine.TogglePlayerControls(false);
        }
    }
}
