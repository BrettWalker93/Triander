using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuHandler : MonoBehaviour
{
    public InputMapper im;

    void Start()
    {
        Messenger.AddListener("state change", StateChange);
        transform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            im.StateUpdate("mainmenu");
    }

    void StateChange()
    {
        if (im.State == "options")
        {
            enabled = true;
            transform.gameObject.SetActive(true);
        }
        else
        {
            enabled = false;
            transform.gameObject.SetActive(false);
        }   
    }    

}
