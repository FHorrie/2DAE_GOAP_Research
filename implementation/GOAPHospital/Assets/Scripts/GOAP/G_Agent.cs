using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class SubGoal
{
    public Dictionary<string, int> _sgoals;
    public bool _remove;

    public SubGoal(string goalName, int value, bool shouldRemove)
    {
        _sgoals = new Dictionary<string, int>
        {
            { goalName, value }
        };
        _remove = shouldRemove;
    }
}

public class G_Agent : MonoBehaviour
{
    public List<G_Action> _actions = new List<G_Action>();
    public Dictionary<SubGoal, int> _goals = new Dictionary<SubGoal, int>();
    public G_Inventory _inventory = new G_Inventory();
    public WorldStates _agentWorldStates = new WorldStates();

    public float _taskRange = 2.5f;

    public G_Planner _planner;
    Queue<G_Action> _actionQueue;
    public G_Action _currentAction;
    SubGoal _currentGoal;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        G_Action[] actionsVec = this.GetComponents<G_Action>();
        foreach (G_Action action in actionsVec) 
            _actions.Add(action);
    }

    bool invoked = false;

    void CompleteAction()
    { 
        _currentAction._running = false;
        _currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate()
    {
        if(_currentAction != null && _currentAction._running)
        {
            float distanceToTarget = Vector3.Distance(_currentAction._target.transform.position, this.transform.position);
            if(_currentAction._agent.hasPath && distanceToTarget < _taskRange)//_currentAction._agent.remainingDistance < 1.0f)
            {
                //Debug.Log("Distance to Goal: " + _currentAction._agent.remainingDistance);
                if(!invoked)
                {
                    Invoke("CompleteAction", _currentAction._duration);
                    invoked = true;
                }
            }
            return;
        }

        if(_planner == null || _actionQueue == null)
        {
            _planner = new G_Planner();

            var sortedGoals = from entry in _goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal, int> subGoal in sortedGoals)
            {
                _actionQueue = _planner.plan(_actions, subGoal.Key._sgoals, _agentWorldStates);
                if (_actionQueue != null)
                {
                   _currentGoal = subGoal.Key;
                    break;
                }
            }
        }

        if(_actionQueue != null && _actionQueue.Count == 0)
        {
            if(_currentGoal._remove)
            {
                _goals.Remove(_currentGoal);
            }
            _planner = null;
        }

        if (_actionQueue != null && _actionQueue.Count > 0)
        {
            _currentAction = _actionQueue.Dequeue();
            if(_currentAction.PrePerform())
            {
                if (_currentAction._target == null && _currentAction._targetTag != "")
                    _currentAction._target = GameObject.FindWithTag(_currentAction._targetTag);

                if (_currentAction._target != null)
                {
                    _currentAction._running = true;
                    _currentAction._agent.SetDestination(_currentAction._target.transform.position);
                }
            }
            else
            {
                _actionQueue = null;
            }
            //Discard plan when an action cannot be performed
        }
    }
}
