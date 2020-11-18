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
        Interact(input);
        PickUpItem(input);
        Move(input);
    }

    private void Interact(PlayerInput input)
    {
        if (input.actionButtonPressed)
        {
            Interactable interactable = playerControllerStateMachine.InteractionDetector.GetInteractableObjectDetected();

            if(interactable != null)
            {
                if (interactable.IsInteractable())
                    interactable.Interact(playerControllerStateMachine);
            }
        }
    }

    private void PickUpItem(PlayerInput input)
    {
        if (input.pickUpPressed)
        {
            HoldableCoordinator itemProcced = playerControllerStateMachine.InteractionDetector.GetHoldableItemDetected();
            if(itemProcced != null && playerControllerStateMachine.PlayerHand.HandsFull() == false)
            {
                playerControllerStateMachine.PlayerHand.HoldItem(itemProcced.GetHoldableItem());
            }
        }
        else if (input.dropPressed)
        {
            
            if (playerControllerStateMachine.PlayerHand.HasAnyItemInHand())
            {
                HoldableItem droppedItem = playerControllerStateMachine.PlayerHand.DropItem();

                Interactable interactable = playerControllerStateMachine.InteractionDetector.GetInteractableObjectDetected();
                if (interactable != null)
                    interactable.PlayerDroppedItem(droppedItem, playerControllerStateMachine);
            }
        }
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
        playerMovement += Vector3.ProjectOnPlane(gameCameraTransform.right, playerControllerStateMachine.PlayerCharacterController.transform.up) * input.moveInput.x;
        // Increment the forward movement by the game camera's up direct * the y input movement
        // Note: We project the camera's up direction on the Vector3.Up since the camera is looking down at the player.
            // ProjectOnPlane will ensure the camera's up direction is projected into the grounds forward direction
        playerMovement += Vector3.ProjectOnPlane(gameCameraTransform.up, playerControllerStateMachine.PlayerCharacterController.transform.up) * input.moveInput.y;
        // Finally, move the player controller with the player movement vector projected
        playerControllerStateMachine.PlayerCharacterController.Move(playerMovement * playerControllerStateMachine.PlayerMoveSpeed * Time.deltaTime);
        Rotate(playerMovement);
    }

    /// <summary>
    /// Rotates the player based on the player movement vector
    /// </summary>
    /// <param name="playerMovement"></param>
    private void Rotate(Vector3 playerMovement)
    {
        // Normalize the movement vector
        playerMovement.Normalize();
        // If the magnitude of the movement direction vector is greater than 0.0f (epsilon),
        // Then rotate towards the direction being travelled
        if (playerMovement.magnitude > Mathf.Epsilon)
        {
            // Rotate the player towards the direction being travelled
            playerControllerStateMachine.PlayerCharacterController.transform.rotation = Quaternion.RotateTowards(playerControllerStateMachine.PlayerCharacterController.transform.rotation,
            // Create a look rotation based on the movement direction (Up direction remains the same)
            Quaternion.LookRotation(playerMovement, playerControllerStateMachine.PlayerCharacterController.transform.up),
            // Rotate the player at the player rotation speed assigned
            playerControllerStateMachine.PlayerRotationSpeed * Time.deltaTime);
        }
    }
}
