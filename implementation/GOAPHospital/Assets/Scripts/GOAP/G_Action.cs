using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class G_Action : MonoBehaviour
{
    public string _actionName = "Action";
    public float _cost = 1.0f;
    public GameObject _target;
    public string _targetTag;
    public float _duration = 0;
    public WorldState[] _preConditions;
    public WorldState[] _afterEffects;
    public NavMeshAgent _agent;

    public Dictionary<string, int> _preConditionsDic;
    public Dictionary<string, int> _effectsDic;


    public G_Inventory _inventory;
    public WorldStates _agentStates;

    public bool _running = false;

    public G_Action()
    { 
        _preConditionsDic = new Dictionary<string, int>();
        _effectsDic = new Dictionary<string, int>();
    }

    public void Awake()
    {
        _agent = this.gameObject.GetComponent<NavMeshAgent>();
        if(_preConditions != null)
        {
            foreach (WorldState w in _preConditions)
            {
                _preConditionsDic.Add(w._key, w._value);
            }
        }
        if (_afterEffects != null)
        {
            foreach (WorldState w in _afterEffects)
            {
                _effectsDic.Add(w._key, w._value);
            }
        }
        _inventory = this.GetComponent<G_Agent>()._inventory;

        _agentStates = this.GetComponent<G_Agent>()._agentWorldStates;
    }

    public bool IsAchievable()
    {
        return true;
    }
    
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(KeyValuePair<string, int> p in _preConditionsDic)
        {
            if (!conditions.ContainsKey(p.Key))
                return false;
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
