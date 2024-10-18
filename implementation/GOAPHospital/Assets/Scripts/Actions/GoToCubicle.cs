using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCubicle : G_Action
{
    public override bool PrePerform()
    {
        _target = _inventory.FindItemWithTag("Cubicle");
        if(_target == null)
            return false;

        G_World.Instance.GetWorld().ModifyState("TreatingPatient", 1);
        return true;
    }

    public override bool PostPerform()
    {
        
        G_World.Instance.GetWorld().ModifyState("TreatingPatient", -1);
        G_World.Instance.AddCubicle(_target);
        _inventory.RemoveItem(_target);
        G_World.Instance.GetWorld().ModifyState("FreeCubicle", 1);
        return true;
    }
}
