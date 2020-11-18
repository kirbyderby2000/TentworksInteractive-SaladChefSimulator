using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerServingArea : Interactable
{
    [SerializeField] Transform servingAreaParent;

    public override void PlayerDroppedItem(HoldableItem droppedItem, PlayerController playerThatDroppedTheItem)
    {
        if (servingAreaParent.transform.childCount >= 1)
            return;
        droppedItem.ToggleRigidBodyKinematic(true);
        droppedItem.transform.SetParent(servingAreaParent);
        droppedItem.transform.position = servingAreaParent.transform.position;
        OnAreaServed.Invoke(droppedItem, playerThatDroppedTheItem);
    }

    public CustomerAreaServedHandler OnAreaServed;

}

[System.Serializable]
public class CustomerAreaServedHandler : UnityEngine.Events.UnityEvent<HoldableItem, PlayerController>
{

}
