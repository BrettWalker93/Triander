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
    public InputMapper im;

    public bool HasShadow { get; set; }

    PlayerMovement pm;

    PlayerPowerHandler pph;
     
    private void Start()
    {
        pph = GetComponent<PlayerPowerHandler>();
        pm = GetComponent<PlayerMovement>();
        HasShadow = false;
    }

    void Update()
    {
        //checks input mapper for state, enables/disables input accordingly
        if (im.State == "menu")
        { 
            pm.enabled = false;
            pph.enabled = false;
        }
        else if (!pph.enabled && im.State == "game")
        {
            pm.enabled = true;
            pph.enabled = true;
        }
    }
}