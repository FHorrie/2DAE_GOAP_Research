using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : G_Action
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        _agentStates.RemoveState("Exhausted");
        return true;
    }
}
