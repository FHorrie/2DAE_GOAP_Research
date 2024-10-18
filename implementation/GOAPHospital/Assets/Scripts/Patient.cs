using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Patient : G_Agent
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("IsWaiting", 1, true);
        _goals.Add(s1, 1);

        SubGoal s2 = new SubGoal("IsTreated", 1, true);
        _goals.Add(s2, 3);

        SubGoal s3 = new SubGoal("IsHome", 1, true);
        _goals.Add(s3, 5);
    }
}
