﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Controller Input Name Settings")]
    /// <summary>
    /// The horizontal move input axis name
    /// </summary>
    [SerializeField] string horizontalMoveInputAxisName;
    /// <summary>
    /// The vertical move input axis name
    /// </summary>
    [SerializeField] string verticalMoveInputAxisName;
    /// <summary>
    /// The input string name to interact with items
    /// </summary>
    [SerializeField] string actionInteractInputName;
    /// <summary>
    /// The input string name to pick up / drop items
    /// </summary>
    [SerializeField] string pickUpDropInputName;

    /// <summary>
    /// The input string name to open the pause menu
    /// </summary>
    [SerializeField] string pauseMenuInputName;

    /// <summary>
    /// The player move speed
    /// </summary>
    [Header("Player Controller Settings")]

    [Tooltip("The player representing this controller")]
    [SerializeField] Players player = Players.Player1;

    [SerializeField] float playerMoveSpeed = 1.0f;

    /// <summary>
    /// The player rotation speed
    /// </summary>
    [SerializeField] float playerRotationSpeed = 1.0f;

    /// <summary>
    /// The game camera used by this player controller
    /// </summary>
    [Header("Game Object References")]
    [Tooltip("The camera game used by this player")]
    [SerializeField] Camera gameCamera;

    [Tooltip("The hand manager used by this player")]
    /// <summary>
    /// The player hand manager for this player
    /// </summary>
    [SerializeField] PlayerHandManager playerHand;

    [Tooltip("The interaction detector used by this player")]
    /// <summary>
    /// The player interaction detector for this player
    /// </summary>
    [SerializeField] PlayerInteractionDetector interactionDetector;

    /// <summary>
    /// The currently active state
    /// </summary>
    PlayerState activeState;

    float clampedYPosition;

    Vector3 _positionToClamp;

    /// <summary>
    /// The player driving this controller
    /// </summary>
    public Players Player
    {
        get { return player; }
    }

    /// <summary>
    /// The player's character controller
    /// </summary>
    public CharacterController PlayerCharacterController
    {
        private set;
        get;
    }

    
    /// <summary>
    /// The movement speed of the player
    /// </summary>
    public float PlayerMoveSpeed
    {
        get { return playerMoveSpeed; }
    }

    /// <summary>
    /// The rotation speed of the player
    /// </summary>
    public float PlayerRotationSpeed
    {
        get { return playerRotationSpeed; }
    }

    /// <summary>
    /// The game camera used by this player controller
    /// </summary>
    public Camera GameCamera
    {
        get { return gameCamera; }
    }

    /// <summary>
    /// The hand manager of this player
    /// </summary>
    public PlayerHandManager PlayerHand
    {
        get { return playerHand; }
    }

    /// <summary>
    /// The interaction detector of this player
    /// </summary>
    public PlayerInteractionDetector InteractionDetector
    {
        get { return interactionDetector; }
    }

    private void Awake()
    {
        // Assign the player character controller
        PlayerCharacterController = GetComponent<CharacterController>();
        activeState = new DefaultPlayerState(this);
        clampedYPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Create a struct with the player input data
        PlayerInput inputs = new PlayerInput();
        // Retrieve and store the movement input into a vector2
        Vector2 movementVector = new Vector2(Input.GetAxis(horizontalMoveInputAxisName), Input.GetAxis(verticalMoveInputAxisName));
        inputs.moveInput = movementVector;
        // Store whether or not the action button has been pressed this frame
        inputs.actionButtonPressed = Input.GetButtonDown(actionInteractInputName);
        // Store whether or not the action button is being held this frame
        inputs.actionButonHeld = Input.GetButton(actionInteractInputName);
        // Store whether or not the pick-up / drop button is being held this frame
        inputs.pickUpDropPressed = Input.GetButtonDown(pickUpDropInputName);
        // Store whether or not the pause menu button has been pressed this frame
        inputs.pauseMenuButtonPressed = Input.GetButtonDown(pauseMenuInputName);
        // Finally, pass the input into the active state
        activeState.HandlePlayerInput(inputs);

        ClampYPosition();
    }

    private void ClampYPosition()
    {
        _positionToClamp = transform.position;
        _positionToClamp.y = clampedYPosition;
        transform.position = _positionToClamp;
    }

    /// <summary>
    /// Method called to set the current player controller state
    /// </summary>
    /// <param name="state"></param>
    public void SetPlayerState(PlayerState state)
    {
        this.activeState = state;
    }
}
