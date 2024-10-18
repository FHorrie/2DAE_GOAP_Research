using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHome : G_Action
{
    [SerializeField] GameObject _patient = null;
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        G_World.Instance.GetWorld().ModifyState("Home", 1);
        Destroy(_patient);
        return true;
    }
}
