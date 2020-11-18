using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component used for managing each player's hand
/// </summary>
public class PlayerHandManager : MonoBehaviour
{
    [Tooltip("The transform reference position for the player's hand")]
    [SerializeField] Transform playerHand;

    /// <summary>
    /// List of items in hand
    /// </summary>
    List<HoldableItem> itemsInHand = new List<HoldableItem>();

    /// <summary>
    /// Method called to hold an item
    /// </summary>
    /// <param name="item"></param>
    public void HoldItem(HoldableItem item)
    {
        // If hands are full, then do nothing
        if (HandsFull())
        {
            Debug.LogWarning("Hads are full. Could not hold item " + item.name);
            return;
        }
        // Otherwise, place the item in the hands position
        item.transform.position = playerHand.transform.position;
        item.transform.parent = playerHand.transform;
        item.transform.rotation = transform.rotation;
        // Set the holdable item to kinematic
        item.ToggleRigidBodyKinematic(true);
        // Add the holdable item into the list of items held in hand
        itemsInHand.Add(item);
        // Notify the item that it's being held
        item.ItemHeld();
    }

    /// <summary>
    /// Method called to drop the item held in hand
    /// </summary>
    public HoldableItem DropItem()
    {
        // Get the next item dropped from the hand
        HoldableItem droppedItem = DropAndGetHeldItem();
        // If the dropped item isn't null, 
        // Then set its parent to null and 
        // turn off the IsKinematic on the rigidbody
        if(droppedItem != null)
        {
            droppedItem.transform.parent = null;
            droppedItem.ToggleRigidBodyKinematic(false);
        }
        return droppedItem;
    }

    /// <summary>
    /// Method called to drop the next item held by the player
    /// </summary>
    /// <returns></returns>
    public HoldableItem DropAndGetHeldItem()
    {
        // If the items in hand count is more than 0, then 
        // return the first item in the list
        // Remove the item from the list of items held in hand
        // Notify the item that it's been dropped
        if (itemsInHand.Count > 0)
        {
            HoldableItem itemDropped = itemsInHand[0];
            itemsInHand.RemoveAt(0);
            itemDropped.ItemDropped();
            return itemDropped;
        }
        // Otherwise, return null
        else
        {
            return null;
        }
            
    }

    /// <summary>
    /// Returns whether or not the player's hands are full
    /// </summary>
    /// <returns></returns>
    public bool HandsFull()
    {
        return itemsInHand.Count >= 2;
    }

    /// <summary>
    /// Returns whether or not the player has any item in their hand
    /// </summary>
    /// <returns></returns>
    public bool HasAnyItemInHand()
    {
        return itemsInHand.Count > 0;
    }
    
}
