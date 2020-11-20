using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledPlayerState : PlayerState
{
    public DisabledPlayerState(PlayerController playerControllerStateMachine) : base(playerControllerStateMachine)
    {

    }

    public override void HandlePlayerInput(PlayerInput input)
    {

    }
}
