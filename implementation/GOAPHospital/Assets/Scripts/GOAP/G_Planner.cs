using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node _parent;
    public float _cost;
    public Dictionary<string, int> _states;
    public G_Action _action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, G_Action action)
    {
        _parent = parent;
        _cost = cost;
        _states = new Dictionary<string, int>(allStates);
        _action = action;
    }
    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> agentStates, G_Action action)
    {
        _parent = parent;
        _cost = cost;
        _states = new Dictionary<string, int>(allStates);

        foreach(KeyValuePair<string, int> state in agentStates)
            if(!_states.ContainsKey(state.Key))
                _states.Add(state.Key, state.Value);

        _action = action;
    }
}

public class G_Planner
{
    private string _planString;

    public Queue<G_Action> plan(List<G_Action> actions, Dictionary<string, int> goal, WorldStates agentStates)
    {
        List<G_Action> useableActions = new List<G_Action>();
        foreach (G_Action action in actions)
        {
            if(action.IsAchievable())
                useableActions.Add(action);
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, G_World.Instance.GetWorld().GetStates(), agentStates.GetStates(), null);

        bool succes = BuildGraph(start, leaves, useableActions, goal);

        if(!succes)
        {
            Debug.Log("NO PLAN WAS CONSTRUCTED");
            return null;
        }

        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else if (leaf._cost < cheapest._cost)
                cheapest = leaf;
        }

        List<G_Action> result = new List<G_Action>();
        Node n = cheapest;
        while (n != null)
        {
            if(n._action != null)
            {
                result.Insert(0, n._action);
            }
            n = n._parent;
        }

        Queue<G_Action> queue = new Queue<G_Action>();
        foreach(G_Action action in result)
        {
            queue.Enqueue(action);
        }

        _planString = "The Plan is:\n";
        foreach(G_Action action in queue)
        {
            _planString += "Q: " + action._actionName + "\n";
        }

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<G_Action> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(G_Action action in usableActions)
        {
            if(action.IsAchievableGiven(parent._states))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent._states);
                foreach(KeyValuePair<string, int> eff in action._effectsDic)
                    if(!currentState.ContainsKey(eff.Key))
                        currentState.Add(eff.Key, eff.Value);

                Node node = new Node(parent, parent._cost + action._cost, currentState, action);

                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<G_Action> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if(found)
                        foundPath = true;
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goals, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> goal in goals)
        {
            if (!state.ContainsKey(goal.Key))
                return false;
        }
        return true;
    }

    private List<G_Action> ActionSubset(List<G_Action> actions, G_Action removeAction)
    {
        List<G_Action> subset = new List<G_Action>();
        foreach(G_Action action in actions)
        {
            if(!action.Equals(removeAction))
                subset.Add(action);
        }
        return subset;
    }

    public string GetPlanString()
    {
        return _planString;
    }

}
