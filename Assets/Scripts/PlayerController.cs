using System.Collections;
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
    /// The game camera used by this player controller
    /// </summary>
    [Header("Game Object References")]
    [SerializeField] Camera gameCamera;

    /// <summary>
    /// The currently active state
    /// </summary>
    PlayerState activeState;

    /// <summary>
    /// The player's character controller
    /// </summary>
    public CharacterController PlayerCharacterController
    {
        private set;
        get;
    }

    /// <summary>
    /// The game camera used by this player controller
    /// </summary>
    public Camera GameCamera
    {
        get { return gameCamera; }
    }

    private void Awake()
    {
        // Assign the player character controller
        PlayerCharacterController = GetComponent<CharacterController>();
        activeState = new DefaultPlayerState(this);
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
        // Finally, pass the input into the active state
        activeState.HandlePlayerInput(inputs);
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
