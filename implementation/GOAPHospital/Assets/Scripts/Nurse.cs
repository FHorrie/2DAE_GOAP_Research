using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Nurse : G_Agent
{
    [SerializeField] Text _currentStatePrint;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _taskRange = 4.0f;

        SubGoal s1 = new SubGoal("TreatPatient", 1, false);
        _goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("Rested", 1, false);
        _goals.Add(s2, 5);

        Invoke("GetTired", Random.Range(10, 20));
    }

    private void Update()
    {
        if (_currentStatePrint != null && _planner != null)
            _currentStatePrint.text = "Nurse 1 Action: " + _currentAction._actionName + "\n" + _planner.GetPlanString();
    }

    void GetTired()
    {
        _agentWorldStates.ModifyState("Exhausted", 0);
        Invoke("GetTired", Random.Range(10, 20));
    }
}
