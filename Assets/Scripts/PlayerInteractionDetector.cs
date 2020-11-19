using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for detecting interactables / holdable items within a player's trigger box collider
/// </summary>
public class PlayerInteractionDetector : MonoBehaviour
{
    /// <summary>
    /// List of holdable item game objects within the player's interaction trigger box area
    /// </summary>
    List<HoldableCoordinator> holdableItems = new List<HoldableCoordinator>();

    /// <summary>
    /// List of interactable game objects within the player's interaction trigger box area
    /// </summary>
    List<Interactable> interactableObjects = new List<Interactable>();

    /// <summary>
    /// Returns the holdable item within the player's proximity; This will return null if no item is within proximity
    /// </summary>
    /// <returns></returns>
    public HoldableCoordinator GetHoldableItemDetected()
    {
        for (int i = 0; i < holdableItems.Count; i++)
        {
            if(holdableItems[i] == null)
            {
                holdableItems.RemoveAt(i);
                i--;
            }
            else if(holdableItems[i].CanBeHeld() == false)
            {
                holdableItems.RemoveAt(i);
                i--;
            }
        }

        //If the holdable items list count is 0, then return null
        if (holdableItems.Count <= 0)
            return null;
        // If the holdable items list count is 1, then return the only element in the list
        else if (holdableItems.Count == 1)
            return holdableItems[0];
        // If there are multiple elements in the holdable items list, 
        // Then return the closest item to the player
        else
        {
            // Cache the first element in the list
            // Cache the distance to the first element, this will be used to store the closest holdable item's distance
            HoldableCoordinator closestItem = holdableItems[0];
            float closestDistance = Vector3.Distance(transform.position, closestItem.transform.position);
            // Iterate through all the holdable items (start at the 2nd index since we initialize the 1st index item as the closest item)
            for (int i = 1; i < holdableItems.Count; i++)
            {
                // Calculate the distance between the player and the holdable item's position in the list
                float distance = Vector3.Distance(transform.position, holdableItems[i].transform.position);
                // If the distance is less than the closest item cached, then reassign the closest item
                // and the closest item distance
                if(distance < closestDistance)
                {
                    closestItem = holdableItems[i];
                    closestDistance = distance;
                }
            }

            // Finally, return the closest item
            return closestItem;
        }
    }

    

    /// <summary>
    /// Returns the interactable game object within the player's proximity; This will return null if no item is iwthin proximity
    /// </summary>
    /// <returns></returns>
    public Interactable GetInteractableObjectDetected()
    {
        for (int i = 0; i < interactableObjects.Count; i++)
        {
            if (interactableObjects[i] == null)
            {
                interactableObjects.RemoveAt(i);
                i--;
            }
            else if(interactableObjects[i].IsInteractable() == false)
            {
                interactableObjects.RemoveAt(i);
                i--;
            }
        }

        // If there are no interactables within proximity, return null
        if (interactableObjects.Count == 0)
            return null;
        // If there is exactly one interactable within proximity, return it
        else if (interactableObjects.Count == 1)
            return interactableObjects[0];
        // If there is more than one interactable within proximity, return the closest one
        else
        {
            // Cache the first element in the list
            // Cache the distance to the first element, this will be used to store the closest interactable item's distance
            Interactable closestInteractable = interactableObjects[0];
            float closestInteractableDistance = Vector3.Distance(transform.position, closestInteractable.transform.position);
            // Iterate through all the holdable items (start at the 2nd index since we initialize the 1st index item as the closest item)
            for (int i = 1; i < interactableObjects.Count; i++)
            {
                // Calculate the distance between the player and the holdable item's position in the list
                float distance = Vector3.Distance(transform.position, interactableObjects[i].transform.position);
                // If the distance is less than the closest item cached, then reassign the closest item
                // and the closest item distance
                if (distance < closestInteractableDistance)
                {
                    closestInteractable = interactableObjects[i];
                    closestInteractableDistance = distance;
                }
            }

            // Finally, return the closest interactable
            return closestInteractable;
        }
    }

    /// <summary>
    /// Method called when this holdable item detector collides with holdable coordinators or interactable objects
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        #region Check for a holdable item coordinator component in the enetering colliding object
        // Get any abstract holdable coordinator component on the object
        HoldableCoordinator holdableItemProc = other.GetComponent<HoldableCoordinator>();
        // If it has a holdable coordinator component, evaluate it
        if (holdableItemProc != null)
        {
            // If our list of holdable items already has this coordinator, then do nothing
            if (holdableItems.Contains(holdableItemProc))
                return;
            // If the coordinated item can be held, then add it to our list of holdable items
            if (holdableItemProc.CanBeHeld() == true)
                holdableItems.Add(holdableItemProc);
            // Add a listener to the coordinator for any hold state changes
            holdableItemProc.onHoldableStateChange.AddListener(OnHoldableCoordinatorChange);
        }
        #endregion

        #region Check for an interactble component in the entering colliding object
        Interactable interactable = other.GetComponent<Interactable>();

        if(interactable != null)
        {
            if (interactableObjects.Contains(interactable))
                return;
            if (interactable.IsInteractable() == true)
                interactableObjects.Add(interactable);
            interactable.OnInteractableStateChanged.AddListener(OnInteractableStateChange);
        }

        #endregion
    }

    /// <summary>
    /// Event called when a holdable item coordinator hold state changes
    /// </summary>
    /// <param name="holdcoord"></param>
    private void OnHoldableCoordinatorChange(HoldableCoordinator holdcoord)
    {
        // If the item is in our list of holdable items
        if (holdableItems.Contains(holdcoord))
        {
            // and the item cannot be held,
            if(holdcoord.CanBeHeld() == false)
            {
                // Then remove the item from our list
                holdableItems.Remove(holdcoord);
            }
        }
        else if (holdcoord.CanBeHeld())
        {
            holdableItems.Add(holdcoord);
        }
    }

    private void OnInteractableStateChange(Interactable interactable)
    {
        if (interactableObjects.Contains(interactable))
        {
            if(interactable.IsInteractable() == false)
            {
                interactableObjects.Remove(interactable);
            }
        }
        else if (interactable.IsInteractable())
        {
            interactableObjects.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        #region Check for a holdable item coordinator component in the exiting colliding object
        // Get any abstract holdable coordinator component on the object exiting the proximity trigger box
        HoldableCoordinator holdableItemProc = other.GetComponent<HoldableCoordinator>();
        // If it has a holdable coordinator component, evaluate it
        if (holdableItemProc != null)
        {
            // If the coordinator is in our list of holdable items, then remove it from our list
            if (holdableItems.Contains(holdableItemProc))
            {
                holdableItems.Remove(holdableItemProc);
            }
            // Unsubsribe from the holdable state change event in the coordinator
            holdableItemProc.onHoldableStateChange.RemoveListener(OnHoldableCoordinatorChange);
        }
        #endregion


        #region Check for an interactble component in the exiting colliding object
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable != null)
        {
            if (interactableObjects.Contains(interactable))
            {
                interactableObjects.Remove(interactable);
            }  
            interactable.OnInteractableStateChanged.RemoveListener(OnInteractableStateChange);
        }
        #endregion
    }
}
