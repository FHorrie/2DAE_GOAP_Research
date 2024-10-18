using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingRoom : G_Action
{
    public override bool PrePerform()
    {
        return true;
    }
    
    public override bool PostPerform()
    {
        G_World.Instance.GetWorld().ModifyState("Waiting", 1);
        G_World.Instance.AddPatient(this.gameObject);
        _agentStates.ModifyState("InWaitingRoom", 1);
        return true;
    }
}
