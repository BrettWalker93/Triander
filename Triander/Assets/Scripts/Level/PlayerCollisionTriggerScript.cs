using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionTriggerScript : MonoBehaviour
{
    public string trigger;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player" && trigger != "")
        {
            Messenger.Broadcast(trigger, trigger);
        }
    }
}
