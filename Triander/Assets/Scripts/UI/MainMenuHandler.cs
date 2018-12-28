using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public InputMapper im;

    private void Start()
    {
        Messenger.AddListener("state change", StateChange);
        transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            im.StateUpdate("game");
    }

    void StateChange()
    {
        if (im.State == "mainmenu")
        {
            enabled = true;
            transform.gameObject.SetActive(true);
        }

        else if (im.State == "game")
        {
            enabled = false;
            transform.gameObject.SetActive(false);
        }

        else
            enabled = false;
    }
}
