using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayerState : PlayerState
{
    public DefaultPlayerState(PlayerController playerControllerStateMachine): base(playerControllerStateMachine)
    {

    }

    public override void HandlePlayerInput(PlayerInput input)
    {
        Move(input);
    }

    /// <summary>
    /// Method called to move the player with the given input
    /// </summary>
    /// <param name="input"></param>
    private void Move(PlayerInput input)
    {
        // Get the player's game camera transform
        Transform gameCameraTransform = playerControllerStateMachine.GameCamera.transform;
        // Cache the projected player movement
        Vector3 playerMovement = new Vector3();
        // Increment the horizontal movement by the game camera's right direction * the x input movement
        playerMovement += gameCameraTransform.right * input.moveInput.x;
        // Increment the forward movement by the game camera's up direct * the y input movement
        // Note: We project the camera's up direction on the Vector3.Up since the camera is looking down at the player.
            // ProjectOnPlane will ensure the camera's up direction is projected into the grounds forward direction
        playerMovement += Vector3.ProjectOnPlane(gameCameraTransform.up, Vector3.up) * input.moveInput.y;
        // Finally, move the player controller with the player movement vector projected
        playerControllerStateMachine.PlayerCharacterController.Move(playerMovement);
    }
}
