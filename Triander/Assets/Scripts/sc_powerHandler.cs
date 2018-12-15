using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//** -> add animation calls
public class sc_powerHandler : MonoBehaviour
{       
    //vars = { type, charges, budget cost }
    public int[] vars = new int[3];
    private bool collided = false;
    
    // Start is called before the first frame update
    void Start()
    {   
        //listens for power collected
        Messenger.AddListener("power collected", powerCollected);
    }

    void OnCollisionEnter(Collision col)
    {
        collided = true;
        if(col.gameObject.name == "i_player")
        { 
            //** player collision animation call

            Messenger.Broadcast("power collision", vars);
        }
        collided = false;
    }
    
    void powerCollected()
    {   
        if(collided)
            {
                //** power collected animation call

                Destroy(gameObject);
            }
    }
    
    // Update is called once per frame
    void Update()
    {        
    }
}
