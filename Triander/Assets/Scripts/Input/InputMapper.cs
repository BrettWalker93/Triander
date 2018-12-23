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

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
    }

    void StateUpdate()
    {

    }

    private void EnterMenu()
    {
        State = "menu";
    }    

    private void ExitMenu()
    {
        State = "game";
    }

}
