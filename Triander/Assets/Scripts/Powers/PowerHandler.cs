using UnityEngine;
//** -> add animation calls
public class PowerHandler : MonoBehaviour
{       
    //vars = { type, charges, budget cost }
    public int[] vars = new int[3];
    private bool collided = false;

    //trigger message, broadcast when picked up
    public string trigger;

    void OnCollisionEnter(Collision col)
    {
        collided = true;

        //broadcasts power collision message, and listens for collected message
        if (col.gameObject.tag == "Player")
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
        if (collided)
        {   
            if(trigger != "")
                Messenger.Broadcast(trigger, trigger);
            else
                print(trigger);

            Destroy(gameObject);
        }
    }
}
