using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerHandler : MonoBehaviour
{
    Rigidbody rb;

    public Transform shadow;

    private List<PlayerPower> playerPowers;

    private bool hasShadow;

    // Start is called before the first frame update
    void Start()
    {
        playerPowers = new List<PlayerPower>();
        rb = GetComponent<Rigidbody>();
        hasShadow = false;
    }

    void Update()
    {
        for (int i = 0; i < playerPowers.Count; i++)
        {
            if (Input.GetButtonDown(playerPowers[i].Name) && !playerPowers[i].Use)
            {
                playerPowers[i].Use = true;
            }
        }
        
        //shadow toggle
        if (Input.GetButtonDown("Shadow"))
            hasShadow = !hasShadow;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < playerPowers.Count; i++)
        {
            if (playerPowers[i].Use)
            {
                playerPowers[i].UsePower(rb);

                if (playerPowers[i].Charges == 0)
                {
                    Destroy(playerPowers[i]);
                    playerPowers.Remove(playerPowers[i]);
                }
                print(playerPowers[i].Name);
            }
        }
        //DrawShadow();
    }

    //replace with something more elegant (enum/switch or Dictionary)
    public void AddPower(string pickUpType)
    {
        if (pickUpType == "Jump" && !playerPowers.Contains(new Jump()))
        {
            playerPowers.Add(new Jump());
        }
        
        else if (pickUpType == "SideBoost" && !playerPowers.Contains(new SideBoost()))  
        {
            playerPowers.Add(new SideBoost());
        }

        else if (pickUpType == "UpBoost" && !playerPowers.Contains(new UpBoost()))
        {
            playerPowers.Add(new UpBoost());
        }

        else if (pickUpType == "Blink" && !playerPowers.Contains(new Blink()))
        {
            playerPowers.Add(new Blink());
        }
    }

    public bool HasPower(object obj)
    {
       return playerPowers.Contains((PlayerPower)obj);
    }

    void DrawShadow()
    {
        if (hasShadow)
        { 
            shadow.transform.position = rb.transform.position;
            shadow.transform.position += rb.transform.rotation * new Vector3(0, 0, 20);
        }
        else
        {
            shadow.transform.position = rb.transform.position;
        }
    }
}
