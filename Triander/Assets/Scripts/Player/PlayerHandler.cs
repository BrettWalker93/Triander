using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* to do
* tweak numbers
* implement more powers
* cycle through indicators (rehaul)
*/


public class PlayerHandler : MonoBehaviour
{
    PlayerMovement pm;

    PlayerPowerHandler pph;
     
    private void Start()
    {
        pph = GetComponent<PlayerPowerHandler>();
        pm = GetComponent<PlayerMovement>();
        Messenger.AddListener<string>("state change", StateChange);
    }

    void StateChange(string state)
    {
        if (state == "menu")
        {
            pm.enabled = false;
            pph.enabled = false;
        }
        else if (state == "game")
        {
            pm.enabled = true;
            pph.enabled = true;
        }
    }
}