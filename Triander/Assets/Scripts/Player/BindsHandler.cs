using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindsHandler : MonoBehaviour
{
    //manipulatable string arrays to store keybinds
    //powerBinds = { jump, side boost, up boost, blink, hover, freeze, laser }
    public string[] powerBinds = new string[7];
    //uiBinds = { blink indicator on/off }
    public string[] hudBinds = new string[1];

    void NewBinds()
    {
        GetComponent<PlayerHandler>().LoadBinds();
    }
}
