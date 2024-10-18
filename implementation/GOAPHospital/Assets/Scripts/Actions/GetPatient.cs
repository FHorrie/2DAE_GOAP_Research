using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatient : G_Action
{
    GameObject _resource;
    public override bool PrePerform()
    {
        _target = G_World.Instance.DequeuePatient();
        if(_target == null)
            return false;

        _resource = G_World.Instance.DequeueCubicle();
        if (_resource != null)
            _inventory.AddItem(_resource);
        else
        {
            G_World.Instance.AddPatient(_target);
            _target = null;
            return false;
        }

        G_World.Instance.GetWorld().ModifyState("FreeCubicle", -1);
        return true;
    }

    public override bool PostPerform()
    {
        G_World.Instance.GetWorld().ModifyState("Waiting", -1);
        if (_target)
        {
            _target.GetComponent<G_Agent>()._inventory.AddItem(_resource);
        }
        return true;
    }
}
