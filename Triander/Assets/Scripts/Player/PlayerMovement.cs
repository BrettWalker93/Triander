using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    public InputMapper im;

    private float magnitude = 0;
    private Vector3 velocity = new Vector3(0, 0, 0);

    //input axes
    private float hAxis;
    private float vAxis;

    public float maxSpeed; //maximum speed
    public float a; //acceleration 

    private float boosting = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //input
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        //print(hAxis + "(" + System.Math.Sign(rb.velocity.x) + ") " + vAxis + "(" 
        //+ System.Math.Sign(rb.velocity.z) + ") v:" + velocity  +" mag: " + magnitude + " rbmag: " + rb.velocity.magnitude);
    }

    void FixedUpdate()
    {
        VelocityUpdate();
    }

    void VelocityUpdate()
    {   
        //Speed updates
        //magnitude of velocity vector on xz plane
        float t = Time.time;

        //placeholder
        velocity = rb.velocity;

        //steal y axis;
        velocity.y = 0;

        //"friction"
        if (velocity.magnitude < 0.35)
            velocity = Vector3.zero;
        else
        {
            velocity.x = 0.97f * velocity.x;
            velocity.z = 0.97f * velocity.z;
        }
    
        if (velocity.magnitude < maxSpeed)
        { 
            //x
            velocity += transform.right * hAxis * a * t;

            //z
            velocity += transform.forward * vAxis * a * t;
        }

        //diagional movement: normalize velocity and reduce magnitude to maxSpeed if exceeding
        if (velocity.magnitude > maxSpeed)
        {
            float ratio = velocity.magnitude / maxSpeed;

            if (boosting <= 0)
                velocity = new Vector3(velocity.x / ratio, velocity.y, velocity.z / ratio);
            else
                boosting -= Time.deltaTime;
        }
        
        //restore y axis
        velocity.y = rb.velocity.y;

        //apply placeholder
        rb.velocity = velocity;
    }

    public void Boost(float duration)
    {
        //boosting /
        boosting = duration;
    }

    //might be useful if want to change how input is handled
    #region InputStuff
    /*
    void AxesUpdate()
    {


        if (im.Controller)
        {
            float hFloat = Input.GetAxis("Horizontal");
            float vFloat = Input.GetAxis("Vertical");
            hAxis = (int)Mathf.Round(hFloat * 5);
            vAxis = (int)Mathf.Round(hFloat * 5);
        }

        //axis imitation, **might behave oddly if a keybind is missing
        else
        {
            if (Input.GetKey(bh.GetKeyCode("Left")))
                hAxis--;
            else if (hAxis < 0)
                hAxis++;

            if (Input.GetKey(bh.GetKeyCode("Right")))
                hAxis++;
            else if (hAxis > 0)
                hAxis--;

            if (Input.GetKey(bh.GetKeyCode("Backward")))
                vAxis--;
            else if (vAxis < 0)
                vAxis++;

            if (Input.GetKey(bh.GetKeyCode("Forward")))
                vAxis++;
            else if (vAxis > 0)
                vAxis--;

            #region float logic
            /*
            //increment 
            //float inc = 0.2f;
            if (Input.GetKey(bh.GetKeyCode("Left")))
                hAxis -= inc;
            else if (hAxis < 0)
                hAxis += inc;

            if (Input.GetKey(bh.GetKeyCode("Right")))
                hAxis += inc;
            else if (hAxis > 0)
                hAxis -= inc;

            if (Input.GetKey(bh.GetKeyCode("Backward")))
                vAxis -= inc;
            else if (vAxis < 0)
                vAxis += inc;

            if (Input.GetKey(bh.GetKeyCode("Forward")))
                vAxis += inc;
            else if (vAxis > 0)
                vAxis -= inc;

            
            #endregion

            //clamp
            hAxis = Mathf.Clamp(hAxis, -5, 5);
            vAxis = Mathf.Clamp(vAxis, -5, 5);
        }

    }
    */
    #endregion
}
