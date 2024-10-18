using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTreated : G_Action
{
    public override bool PrePerform()
    {
        _target = _inventory.FindItemWithTag("Cubicle");
        if(_target == null)
            return false;
        return true;
    }

    public override bool PostPerform()
    {
        G_World.Instance.GetWorld().ModifyState("Treated", 1);
        _agentStates.ModifyState("IsCured", 1);
        _inventory.RemoveItem(_target);
        return true;
    }
}
