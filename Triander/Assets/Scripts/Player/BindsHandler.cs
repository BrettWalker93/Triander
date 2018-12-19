using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindsHandler : MonoBehaviour
{
    //manipulatable string arrays to store keybinds
    //powerBinds = { jump, side boost, up boost, blink, hover, freeze, laser }
    public string[] powerBinds = new string[7];
    //uiBinds = { blink indicator on/off }
    public string[] uiBinds = new string[1];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NewBinds()
    {
        GetComponent<PlayerHandler>().LoadBinds();
    }
}
