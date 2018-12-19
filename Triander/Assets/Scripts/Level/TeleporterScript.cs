using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    public Transform destination;

    //timer variables
    private bool go = false;
    public float timer;
    private float count = 0;

    //player
    PlayerHandler ph;

    private void Start()
    {
        //disables updates until collision
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //teleport if time
        if (count >= timer && ph != null)
        {
            ph.transform.position = destination.position + new Vector3(0, 1, 0);
        }

        //timer control
        if (go)
            count += Time.deltaTime;
        else
            count = 0;
    }

    //start count on collision enter
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            enabled = true;
            ph = col.gameObject.GetComponent<PlayerHandler>();
            go = true;
        }
    }
    
    //reset count on collision exit
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            enabled = false;
            ph = null; 
            go = false;        
        }
    }
}
