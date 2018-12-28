using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]

public class InputMapper : ScriptableObject
{
    public string State { get; set; }

    void Awake()
    {
        State = "game";
    }

    public void StateUpdate(string newState)
    {
        State = newState;
        Messenger.Broadcast("state change");
    }
}
