using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    public InputMapper im;

    private float magnitude = 0;
    private Vector3 velocity = Vector3.zero;

    //input axes
    private float hAxis;
    private float vAxis;

    public float maxSpeed; //maximum speed
    public float a; //acceleration 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        VelocityUpdate();
    }
   
    void VelocityUpdate()
    {
        //Speed updates
        //magnitude of velocity vector on xz plane
        magnitude = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z);
        float t = Time.fixedDeltaTime;  

        //x
        //slow x if no horizontal input
        if (hAxis == 0)
        {   
            if (System.Math.Abs(velocity.x) < 1)
                velocity.x = 0;
            else
                velocity.x -= System.Math.Sign(velocity.x) * a * t / 2f;
        }
        
        //stop if input opposite to instantaneous velocity
        else if (System.Math.Sign(hAxis) * System.Math.Sign(velocity.x) == -1)
            velocity.x = 0;
        
        //accelerate, but not beyond maxSpeed
        else if (hAxis != 0 && magnitude < maxSpeed)
            velocity.x += System.Math.Sign(hAxis) * a * t;

        //z
        //slow z if no vertical input
        if (vAxis == 0)
        {  
            if (System.Math.Abs(velocity.z) < 1)
                velocity.z = 0;
            else
                velocity.z -= System.Math.Sign(velocity.z) * a * t / 2f;
        } 
   
        //stop if input opposite to instantaneous velocity
        else if (System.Math.Sign(vAxis) * System.Math.Sign(velocity.z) == -1)
            velocity.z = 0;

        //accelerate, but not beyond maxSpeed
        else if (vAxis != 0 && magnitude < maxSpeed)
            velocity.z += System.Math.Sign(vAxis) * a * t;

        //allow turning at maxSpeed
        if (hAxis != 0 && vAxis != 0)
        {
            //if horizontal instantaneous speed is greater than vertical instantaneous speed
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z))
            {
                velocity.x = System.Math.Sign(velocity.x) * Mathf.Sqrt(maxSpeed * maxSpeed / 2f);
                velocity.z = System.Math.Sign(vAxis) * Mathf.Sqrt(maxSpeed * maxSpeed / 2f);
            }
            //if vertical instantaneous speed is greater than horizontal instantaneous speed           
            else if (Mathf.Abs(velocity.z) > Mathf.Abs(velocity.x))
            {
                velocity.z = System.Math.Sign(velocity.z) * Mathf.Sqrt(maxSpeed * maxSpeed / 2f);
                velocity.x = System.Math.Sign(hAxis) * Mathf.Sqrt(maxSpeed * maxSpeed / 2f);
            }
        }

        //return to maxSpeed after boost
        if (magnitude >= maxSpeed)
        {
            if (velocity.x > (Mathf.Sqrt(maxSpeed) / 2f))
                velocity.x -= System.Math.Sign(velocity.x) * a * t / 4f;

            if (velocity.z > (Mathf.Sqrt(maxSpeed) / 2f))
                velocity.z -= System.Math.Sign(velocity.z) * a * t / 4f;
        }

        //update RigidBody velocity accordingly
        Vector3 vSet = transform.rotation * velocity;
        rb.velocity = new Vector3(vSet.x, rb.velocity.y, vSet.z);
    }

    public void Boost()
    {
        velocity.x += velocity.x;
        velocity.z += velocity.z;
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
