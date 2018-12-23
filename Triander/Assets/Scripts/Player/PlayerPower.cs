using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

#region Power Template
/* TEMPLATE
public class POWER_CLASS_NAME : PlayerPower
{
    public POWER_CLASS_NAME()
    {
        //Charges != 0 results in use of power
        //Charges == 0 results in destruction of power instance
        //Negative charges is treated as "infinite charges"

        //initial number of charges
        Charges = -1;

        //For PlayerPowers: name should correspond to a retrievable "Action" in BindsHandler.ActionsK        
        Name = null;

        //Cooldown, in seconds, of the power
        cooldown = 0;

        //initial timer for cooldown calculation
        timer = Time.fixedTime;
        Use = false;
    }

    public override void UsePower(Rigidbody rb)
    {
        //called during FixedUpdate
        //rb is rigidbody component of object to which the caller is attached (player object)

        if (Time.time - Timer > Cooldown)
        { 
            //Power logic goes here

            //charge depletion if you need it
            //Charges -= 1;

            //always end with Timer update and set Use to false
            Timer = Time.time;
            Use = false;
        }
    }
}

*/
#endregion

public class PlayerPower : ScriptableObject
{
    public int Charges { get; set; }
    public string Name { get; set; }
    public bool Use { get; set; }

    protected float Cooldown { get; set; }
    protected float Timer { get; set; }
    
    public PlayerPower()
    {
        Charges = -1;
        Name = null;
        Cooldown = 0f;
        Timer = Time.fixedTime;
        Use = false;
    }

    public virtual void UsePower(Rigidbody rb)
    {
        //nothing doing
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        PlayerPower objAsPlayerPower = obj as PlayerPower;
        if (objAsPlayerPower == null) return false;
        else return Equals(objAsPlayerPower);
    }

    public bool Equals(PlayerPower other)
    {
        if (other == null)
            return false;
        return Name.Equals(other.Name);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

public class Jump : PlayerPower
{
    private float jumpSpeed = 15;

    public Jump()
    {
        Charges = -1;
        Name = "Jump";
        Cooldown = 0.5f;    
        Timer = Time.fixedTime;
        Use = false;
    }

    public override void UsePower(Rigidbody rb)
    {
        if (Time.fixedTime - Timer > Cooldown)   
        { 
            RaycastHit hit;
            Ray ray = new Ray(rb.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, 0.55f))
                rb.velocity += rb.transform.rotation * new Vector3(0, jumpSpeed, 0);

            Timer = Time.fixedTime;
            Use = false;
        }
    }
}

public class SideBoost : PlayerPower
{
    public SideBoost()
    {
        Charges = -1;
        Name = "SideBoost";
        Cooldown = 1f;
        Timer = Time.fixedTime;
        Use = false;
    }

    public override void UsePower(Rigidbody rb)
    {
        if (Time.fixedTime - Timer > Cooldown)
        {   
            rb.GetComponent<PlayerMovement>().Boost();
            //Charges -= 1;
            Timer = Time.fixedTime;
            Use = false;
        }
    }
}


