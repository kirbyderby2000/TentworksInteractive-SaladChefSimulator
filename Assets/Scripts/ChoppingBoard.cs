using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : Interactable
{

    ChoppingState _activeChoppingState = null;

    private Choppable choppableItemOnBoard = null;

    public override void PlayerDroppedItem(HoldableItem droppedItem, PlayerController player)
    {
        HoldableItem itemOnBoard = transform.GetComponentInChildren<HoldableItem>();
        if (itemOnBoard == null)
        {
            droppedItem.transform.parent = transform;
            droppedItem.transform.position = transform.position;
            droppedItem.ToggleRigidBodyKinematic(true);
            choppableItemOnBoard = droppedItem.GetComponent<Choppable>();
            if (choppableItemOnBoard != null)
            {
                droppedItem.OnHoldStateChange.AddListener(OnChoppableItemOnBoardPickedUp);
                OnChoppableItemBoarded.Invoke(choppableItemOnBoard);
            }
        }
    }

    private void  OnChoppableItemOnBoardPickedUp(HoldableItem itemOnBoard)
    {
        if(itemOnBoard.HoldingState == HoldableItem.HeldState.Held)
        {
            itemOnBoard.OnHoldStateChange.RemoveListener(OnChoppableItemOnBoardPickedUp);
            OnChoppableItemUnboarded.Invoke(choppableItemOnBoard);
            choppableItemOnBoard = null;
        }
    }


    protected override void HandleInteraction(PlayerController playerInteracting)
    {
        // If theere is already a player chopping items on the board, then do nothing
        if (_activeChoppingState != null)
            return;
        // If the player has any items in hand, then the player can't chop items on a board
        if (playerInteracting.PlayerHand.HasAnyItemInHand())
            return;
        // If all the items on the board aren't choppable, then the player can't chop items on the board
        Choppable choppableItem = transform.GetComponentInChildren<Choppable>();
        if (choppableItemOnBoard == null)
            return;
        // If the choppable item on the board is already chopped, then the player can't chop items on the board
        else if (choppableItem.ChopComplete)
            return;

        // Otherwise, let's get chopping
        // Create a new player state with the player that's interacting with this chopping board
        // and the choppable item to chop
        _activeChoppingState = new ChoppingState(playerInteracting, choppableItem, OnChoppingEnded);
        // Assign the new chopping state to the player
        playerInteracting.SetPlayerState(_activeChoppingState);
        // Call the on chop starting event
        OnChopStarting.Invoke(choppableItem);
    }

    private void OnChoppingEnded(Choppable itemChopped)
    {
        Debug.Log("End of chop");
        // Call the on chop ending event
        OnChopEnding.Invoke(itemChopped);

        if (itemChopped.ChopComplete)
        {
            Debug.Log("Item chop complete");
            FoodGameObject choppedResult = Instantiate(itemChopped.ChoppedIngredient.IngredientPrefab, itemChopped.transform.position, itemChopped.transform.rotation);
            choppedResult.transform.parent = itemChopped.transform.parent;
            choppedResult.GetHoldableItemComponent().ToggleRigidBodyKinematic(true);
            OnChoppableItemUnboarded.Invoke(itemChopped);
            Destroy(itemChopped.gameObject);
        }
        
        _activeChoppingState = null;
    }


    /// <summary>
    /// Event invoked when a choppable item is boarded
    /// </summary>
    public ChoppableEventHandler OnChoppableItemBoarded;

    /// <summary>
    /// Event invoked when a choppable item is unboarded
    /// </summary>
    public ChoppableEventHandler OnChoppableItemUnboarded;

    /// <summary>
    /// Event invoked when chopping starts on this board
    /// </summary>
    public ChoppableEventHandler OnChopStarting;

    /// <summary>
    /// Event invoked when chopping ends on this board
    /// </summary>
    public ChoppableEventHandler OnChopEnding;

}


public class ChoppingState : PlayerState
{
    Choppable itemBeingChopped;
    private bool chopComplete = false;
    System.Action<Choppable> choppingEndCallbackHandler;
    public ChoppingState(PlayerController playerControllerStateMachine, Choppable choppableItem, System.Action<Choppable> choppingEndCallbackHandler) : base(playerControllerStateMachine)
    {
        itemBeingChopped = choppableItem;
        this.choppingEndCallbackHandler = choppingEndCallbackHandler;
    }
    

    public override void HandlePlayerInput(PlayerInput input)
    {
        if (chopComplete == true)
            return;

        if (input.actionButonHeld || input.actionButtonPressed)
        {
            Debug.Log("Chopping");
            itemBeingChopped.AddChoppedTime(Time.deltaTime);
            if (itemBeingChopped.ChopComplete)
            {
                chopComplete = true;
                playerControllerStateMachine.SetPlayerState(new DefaultPlayerState(playerControllerStateMachine));
                choppingEndCallbackHandler(itemBeingChopped);
            }
        }
        else
        {
            chopComplete = true;
            playerControllerStateMachine.SetPlayerState(new DefaultPlayerState(playerControllerStateMachine));
            choppingEndCallbackHandler(itemBeingChopped);
        }
    }



    public override void ExittingState()
    {
        if(chopComplete == false)
        {
            choppingEndCallbackHandler(itemBeingChopped);
        }
    }


}