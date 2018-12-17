using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //manipulatable string arrays to store keybinds
    public string[] powerBinds = new string[7];

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
