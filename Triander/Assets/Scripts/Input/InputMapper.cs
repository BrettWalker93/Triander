using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class InputMapper : ScriptableObject
{
    #region Private Variables
   

    #endregion

    #region Public Variables

    public string State { get; private set; }

    public bool Controller { get; set; }

    #endregion

    void Awake()
    {
        State = "game";
        Controller = false;
    }

    void Update()
    {
        //input logic for state changes goes here
    }

    void StateUpdate()
    {
        Messenger.Broadcast("state change", State);
    }

    private void EnterMenu()
    {
        State = "menu";
        StateUpdate();
    }    

    private void ExitMenu()
    {
        State = "game";
        StateUpdate();
    }
}
