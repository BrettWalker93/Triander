using UnityEngine;
//** -> add animation calls
public class PickUpHandler : MonoBehaviour
{       
    //parameters
    public int cost;
    public string type;

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
            Messenger.AddListener("pickup collected", powerCollected);
                
            Messenger.Broadcast("pickup collision", cost, type);

            Messenger.RemoveListener("pickup collected", powerCollected);
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
