using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerHandler : MonoBehaviour
{
    Rigidbody rb;    

    private List<PlayerPower> playerPowers;

    // Start is called before the first frame update
    void Start()
    {
        playerPowers = new List<PlayerPower>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        for (int i = 0; i < playerPowers.Count; i++)
        {
            if (Input.GetButton(playerPowers[i].Name) && !playerPowers[i].Use)
            {
                playerPowers[i].Use = true;
            }
        }
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
            }
        }
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
    }
}
