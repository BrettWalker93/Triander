using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    #region Variables
    //variables for platform movement
    Transform t;
    public float speed;
    
    //trigger strings
    public string triggerOn;
    public string triggerOff;

    //on/off switch
    private bool on = false;
    //going to or coming from end (end = coords[1])
    private bool going = true;
    
    //player carrying
    PlayerHandler ph;
    private Vector3 p1 = Vector3.zero;
    
    //elevator properties
    public Vector3[] coords = new Vector3[2]; //start and stop
    private Vector3 path;
    public bool continuous; //does the elevator run continuously once triggered
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        path = coords[1] - coords[0];
        path = path / path.magnitude;
        if(triggerOn != "")
            Messenger.AddListener<string>(triggerOn, EleCmd);

        //disables updates until trigger
        enabled = false;
    }

    void FixedUpdate()
    {         
        if (on)
            CheckPos();
        
        //carries the player along
        if (ph != null)
        {
            ph.transform.position += (t.position - p1);
        }

        p1 = t.position;
    }

    //receives trigger string and follows command
    public void EleCmd(string a0)
    {
        //enables script on first trigger
        if (!enabled)
            enabled = true;

        if (a0 == triggerOn)
        {
            on = true;
            Messenger.RemoveListener<string>(triggerOn, EleCmd);
            if (triggerOff != "")
                Messenger.AddListener<string>(triggerOff, EleCmd);
        }
        else if (triggerOff != "" && a0 == triggerOff)
        {                
            on = false;
            Messenger.RemoveListener<string>(triggerOff, EleCmd);
            Messenger.AddListener<string>(triggerOn, EleCmd);
        }
    }

    //controls movement, called in FixedUpdate
    void CheckPos()
    {
        if(continuous || going)
        {
            //stop at the end
            if((t.position - coords[1]).magnitude <= 0.05)
                going = false;
            //restart at the start
            else if(continuous && (t.position - coords[0]).magnitude <= 0.05)
                going = true;
        }

        //move
        if(going)
            t.position += path * speed * Time.deltaTime;

        //reverse direction at the end for continuous elevators
        else if (continuous) 
            t.position += -1 * path * speed * Time.deltaTime;
    }

    #region PlayerCarrying
    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
            ph = col.gameObject.GetComponent<PlayerHandler>();
    }

    public void OnCollisionExit(Collision col)
    {
        if(col.gameObject.tag == "Player")
            ph = null;
    }
    #endregion

}
