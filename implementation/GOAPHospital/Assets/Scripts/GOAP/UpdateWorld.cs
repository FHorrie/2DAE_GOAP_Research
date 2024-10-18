using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWorld : MonoBehaviour
{
    [SerializeField] public Text states;

    void LateUpdate()
    {
        Dictionary<string, int> worldStates = G_World.Instance.GetWorld().GetStates();
        states.text = "";
        foreach (KeyValuePair<string, int> pair in worldStates)
        {
            states.text += pair.Key + ", " + pair.Value + "\n";
        }
    }
}
