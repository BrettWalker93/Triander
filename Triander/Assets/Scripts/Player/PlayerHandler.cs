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
    
    public InputMapper im;
 
    private void Start()
    {
        pph = GetComponent<PlayerPowerHandler>();
        pm = GetComponent<PlayerMovement>();
        Messenger.AddListener("state change", StateChange);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            im.StateUpdate("mainmenu");
            transform.gameObject.SetActive(true);
        }
    }

    void StateChange()
    {
        if (im.State == "game")
        {
            pm.enabled = true;
            pph.enabled = true;
        }
        else
        {
            pm.enabled = false;
            pph.enabled = false;
        }
    }
}