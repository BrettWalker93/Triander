using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* to do
* tweak numbers
* implement more powers
* cycle through indicators (rehaul)
*/
public class PlayerHandler : MonoBehaviour
{
    //initialize
    
    //variables for player movement
    private Vector3 velocity = new Vector3(0, 0, 0); //for x & z velocity updates
    private float magnitude = 0; //holds magnitude of x & z velocity vectors
    Rigidbody rb;
    public float maxSpeed; //maximum speed
    public float a; //acceleration
    public float jumpSpeed; //jump speed
    

    //powers (jump, side-boost, up-boost, blink, hover, freeze, lasers, budget) { x: presence of power, y: charges left | budget: spent, total}
    private int[,] powers = new int[,] { { 0, 0 }, { 0, 0 }, { 0, 0}, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 5 } };
    private float[] powerCD = new float[] {0, 0, 0, 0, 0, 0};

    public float blinkD; //blink distance
    public Transform shadow;

    

    //holds keybinds
    private string[] binds = new string[7];

    private void Awake()
    {   
        //refresh binds    
        LoadBinds();
    }

    // Use this for initialization
    void Start()
    {
        //get the rigid body component for later use
        rb = GetComponent<Rigidbody>();

        //listens for power collisions
        Messenger.AddListener<int[]>("power collision", PowerCollision);
        Messenger.MarkAsPermanent("power collision");
    }

    // Update is called once per frame
    void Update()
    {
        VelocityUpdate();  

        PowerUse();
        
        //cooldown timer
        for(int i = 0; i < powerCD.Length; i++)
        { 
            if (powerCD[i] > 0)
                powerCD[i] -= Time.deltaTime;
        }
            
        //print test
        print(transform.eulerAngles.y + "yes");
    }

    //updates binds; call if binds are changed in InputHandler
    public void LoadBinds()
    {
        InputHandler inputHandler = GetComponent<InputHandler>();
        inputHandler.powerBinds.CopyTo(binds, 0);
    }
    
    //updates player velocity based on input
    void VelocityUpdate()
    {

        //Speed updates
        //magnitude of velocity vector on xz plane
        magnitude = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z);
        float t = Time.deltaTime;

        float hAxis = Input.GetAxis("Horizontal"); //x
        float vAxis = Input.GetAxis("Vertical"); //z     

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
    void PowerCollision(int[] a0)
    {
        //if room in budget, collect power and destroy pick-up
        if(powers[7, 0] + a0[2] <= powers[7, 1])
        {  
            powers[a0[0], 0] = 1;
            powers[a0[0], 1] += a0[1];
            powers[7, 1]  += a0[2];
            Messenger.Broadcast("power collected");
        }
    }
    
    //checks for input and uses powers if appropriate
    void PowerUse()
    {   
        //Jump
        if (Input.GetKeyDown(binds[0]) &&  powers[0,0] == 1)
            Jump();

        //Side-boost
        if (Input.GetKeyDown(binds[1]) && (powers[1, 0]*powers[1, 1]) >= 1 && powerCD[0] <= 0)
            SideBoost();

        //Up-boost
        if (Input.GetKeyDown(binds[2]) && (powers[2, 0] * powers[2, 1]) >= 1 && powerCD[1] <= 0)
            UpBoost();

        //blink*
        if (Input.GetKeyDown(binds[3]) && (powers[3, 0] * powers[3, 1]) >= 1 && powerCD[2] <= 0)
            Blink(false);

        //draw blink indicator
        else if ((powers[3, 0] * powers[3, 1]) >= 1)
            Blink(true);

        //don't draw
        else
            shadow.position = transform.position;

        //hover*
        if (Input.GetKeyDown(binds[4]) && (powers[4, 0] * powers[4, 1]) >= 1 && powerCD[3] <= 0)
            Hover();

        //freeze*
        if (Input.GetKeyDown(binds[5]) && (powers[5, 0] * powers[5, 1]) >= 1 && powerCD[4] <= 0)
            Freeze();

        //laser*
        if (Input.GetKeyDown(binds[6]) && (powers[6, 0] * powers[6, 1]) >= 1 && powerCD[5] <= 0)
            Laser();
    }

    void Jump()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, 0.55f))
            rb.velocity += transform.rotation * new Vector3(0, jumpSpeed, 0);
    }

    void SideBoost()
    {
        velocity.x = velocity.x * 2;
        velocity.z = velocity.z * 2;

        powers[1, 1] -= 1;
        powerCD[0] += 2;
    }

    void UpBoost()
    {
        rb.velocity.Set(rb.velocity.x, 0, rb.velocity.z);
        rb.velocity += transform.rotation * new Vector3(0, jumpSpeed, 0);
        powers[2, 1] -= 1;
        powerCD[1] += 2;
    }

    void Blink(bool drawShadow)
    {
        if (!drawShadow)
        { 
            transform.position += transform.rotation * new Vector3(0, 0, blinkD);
            shadow.position = transform.position;
        }
        else
            shadow.position = transform.position + transform.rotation * new Vector3(0, 0, blinkD);
    }

    void Hover()
    {
        rb.velocity.Set(0,0,0);
        powers[4, 1] -= 1;
        powerCD[3] += 2;
    }

    void Freeze()
    {

        powers[5, 1] -= 1;
        powerCD[4] += 2;
    }

    void Laser()
    {

        powers[6, 1] -= 1;
        powerCD[5] += 2;
    }

}