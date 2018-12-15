using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* to do
* tweak numbers
* implement more powers
* **falling through floor
*/
public class sc_playerHandler : MonoBehaviour
{
    //initialize
    
    //variables for player movement
    private Vector3 velocity = new Vector3(0, 0, 0); //for XZ velocity updates
    private float magnitude = 0;
    Rigidbody rb;
    public float maxSpeed; //maximum speed
    public float a; //acceleration
    
    //jump
    public float jumpSpeed;

    //powers (jump, side-boost, air-up, air-side, blink, hover, pause, points)
    public int[,] powers = new int[,] { { 0, 0 }, { 0, 0 }, { 0, 0}, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 5 } };
    public float powerCD = 0;

    // Use this for initialization
    void Start()
    {
        //get the rigid body component for later use
        rb = GetComponent<Rigidbody>();
        //listens for power collisions
        Messenger.AddListener<int[]>("power collision", powerCollision);
        Messenger.MarkAsPermanent("power collision");
    }

    private void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        velocityUpdate();  

        powerUse();

        if (powerCD > 0)
            powerCD -= Time.deltaTime;
    }

    private void velocityUpdate()
    {

        //Speed updates
        //magnitude of velocity vector on xz plane
        magnitude = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z);
        float t = Time.deltaTime;

        float hAxis = Input.GetAxis("Horizontal"); //x
        float vAxis = Input.GetAxis("Vertical"); //y     

        //stop if absolutely no input
        if (hAxis == 0 && vAxis == 0)
        {
            if (Mathf.Abs(velocity.x) < 2) velocity.x = 0;
            else velocity.x += -1f * System.Math.Sign(velocity.x) * a * t / 10f;

            if (Mathf.Abs(velocity.z) < 2) velocity.z = 0;
            else velocity.z += -1f * System.Math.Sign(velocity.z) * a * t / 10f;
        }

        //x
        //slow x if no horizontal input
        else if (hAxis == 0)
            velocity.x -= System.Math.Sign(velocity.x) * a * t;

        //accelerate, but not beyond maxSpeed
        else if (hAxis != 0 && magnitude <= maxSpeed)
            velocity.x += System.Math.Sign(hAxis) * a * t;

        //stop if input opposite to instantaneous velocity
        else if (System.Math.Sign(hAxis) * System.Math.Sign(velocity.x) == -1)
            velocity.x = 0;// -= System.Math.Sign(velocity.x) * a * t;

        //z
        //slow z if no vertical input
        if (vAxis == 0 && hAxis != 0)
            velocity.z -= System.Math.Sign(velocity.z) * a * t;

        //accelerate, but not beyond maxSpeed
        else if (vAxis != 0 && magnitude < maxSpeed)
            velocity.z += System.Math.Sign(vAxis) * a * t;


        //stop if input opposite to instantaneous velocity
        else if (System.Math.Sign(vAxis) * System.Math.Sign(velocity.z) == -1)
            velocity.z = 0; //-= System.Math.Sign(velocity.z) * a * t;

        //allow turning at maxSpeed
        if (hAxis != 0 && vAxis != 0)
        {
            //float diff = 0;
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
                velocity.x -= System.Math.Sign(velocity.x) * a * t / 2f;

            if (velocity.z > (Mathf.Sqrt(maxSpeed) / 2f))
                velocity.z -= System.Math.Sign(velocity.z) * a * t / 2f;        
        }

        //update RigidBody velocity accordingly
        Vector3 vSet = transform.rotation * velocity;
        rb.velocity = new Vector3(vSet.x, rb.velocity.y, vSet.z);
    }

    //input is budget cost of power, returns true if room in budget for new power
    private void powerCollision(int[] a0)
    {
        //if room in budget, collect power and destroy pick-up
        if(powers[7, 0] + a0[2] <= powers[7, 1])
        {  
            powers[a0[0], 0] = 1;
            powers[a0[0], 1] += a0[1];
              
            Messenger.Broadcast("power collected");
        }
    }
    
    private void powerUse()
    {
        //Space : Jump
        if (Input.GetKeyDown(KeyCode.Space) &&  powers[0,0] == 1)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, 0.55f))
                rb.velocity += transform.rotation * new Vector3(0, jumpSpeed, 0);            
        }

        //1 : Side-boost
        if (Input.GetKeyDown(KeyCode.Alpha1) && (powers[1, 0]*powers[1, 1]) >= 1 && powerCD <= 0)
        {
            
            velocity.x = velocity.x * 2;
            velocity.z = velocity.z * 2;

            powers[1,1] -= 1;
            powerCD += 2;
        }

        //2 : Up-boost
        if (Input.GetKeyDown(KeyCode.Alpha2) && (powers[2, 0] * powers[2, 1]) >= 1 && powerCD <= 0)
        {
            rb.velocity += transform.rotation * new Vector3(0, jumpSpeed, 0);
            powers[2, 1] -= 1;
            powerCD += 2;
        }
    }
}