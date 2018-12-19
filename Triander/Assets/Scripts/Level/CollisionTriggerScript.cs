using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTriggerScript : MonoBehaviour
{
    public string trigger;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Messenger.Broadcast(trigger, trigger);
        }
    }
}
