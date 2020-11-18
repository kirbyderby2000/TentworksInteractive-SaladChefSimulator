using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract interactable class used for interactable objects
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    [Tooltip("Event invoked when the interactable state of this object changes")]
    public InteractableStateChangeHandler OnInteractableStateChanged;

    /// <summary>
    /// Interact with this object
    /// </summary>
    /// <param name="playerInteracting">The player interacting with this object</param>
    public void Interact(PlayerController playerInteracting)
    {
        // If this object can't be interacted with, then do nothing and log an error
        if (IsInteractable() == false)
        {
            Debug.LogError("Interact() called but this object is currently not interactable.", this.gameObject);
            return;
        }
        // Otherwise, call the handle interaction method
        HandleInteraction(playerInteracting);
    }

    /// <summary>
    /// Method that handles interaction 
    /// </summary>
    /// <param name="playerInteracting"></param>
    protected virtual void HandleInteraction(PlayerController playerInteracting)
    {

    }

    /// <summary>
    /// Drop an object on this interactable object
    /// </summary>
    /// <param name="droppedItem">The holdable item dropped on this interactable object</param>
    public virtual void PlayerDroppedItem(HoldableItem droppedItem, PlayerController playerThatDroppedTheItem)
    {

    }

    /// <summary>
    /// Returns whether or not this object is interactable
    /// </summary>
    /// <returns></returns>
    public virtual bool IsInteractable()
    {
        return this.gameObject.activeInHierarchy;
    }
}

[System.Serializable]
public class InteractableStateChangeHandler : UnityEngine.Events.UnityEvent<Interactable>
{

}