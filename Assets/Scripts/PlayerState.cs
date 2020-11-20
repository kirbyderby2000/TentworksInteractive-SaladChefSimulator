using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base abstract class used to handle the player controller state
/// </summary>
public abstract class PlayerState
{
    /// <summary>
    /// The player controller
    /// </summary>
    protected PlayerController playerControllerStateMachine;

    /// <summary>
    /// The PlayerController using this PlayerState
    /// </summary>
    /// <param name="playerControllerStateMachine"></param>
    public PlayerState(PlayerController playerControllerStateMachine)
    {
        this.playerControllerStateMachine = playerControllerStateMachine;
    }

    /// <summary>
    /// Method used to handle the player input data
    /// </summary>
    /// <param name="input"></param>
    public abstract void HandlePlayerInput(PlayerInput input);

    /// <summary>
    /// Method called when this player state is being exited out
    /// </summary>
    public virtual void ExittingState()
    {

    }
}

/// <summary>
/// Basic struct used to house player input data
/// </summary>
public struct PlayerInput
{
    /// <summary>
    /// The player's movement input
    /// </summary>
    public Vector2 moveInput;

    /// <summary>
    /// Whether or not the pick-up button has been pressed
    /// </summary>
    public bool pickUpPressed;

    /// <summary>
    /// Whether or not the drop button has been pressed
    /// </summary>
    public bool dropPressed;

    /// <summary>
    /// Whether or not the action / interact button has been pressed
    /// </summary>
    public bool actionButtonPressed;

    /// <summary>
    /// Whether or not the action / interact button is being held down
    /// </summary>
    public bool actionButonHeld;

    /// <summary>
    /// Whether or not the pause menu button has been pressed
    /// </summary>
    public bool pauseMenuButtonPressed;
}
