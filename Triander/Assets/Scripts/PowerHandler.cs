﻿using UnityEngine;
//** -> add animation calls
public class PowerHandler : MonoBehaviour
{       
    //vars = { type, charges, budget cost }
    public int[] vars = new int[3];
    private bool collided = false;

    // Start is called before the first frame update
    void Start()
    {   
        //listens for power collected
    }

    void OnCollisionEnter(Collision col)
    {
        collided = true;
        if(col.gameObject.tag == "Player")
        {
            //** player collision animation call
            Messenger.AddListener("power collected", powerCollected);
                
            Messenger.Broadcast("power collision", vars);

            Messenger.RemoveListener("power collected", powerCollected);
        }
        collided = false;
    }
    
    void powerCollected()
    {   
        if(collided)
        {
            Destroy(gameObject);
        }
    }
    
    // Update is called once per frame
    void Update()
    {        
    }
}