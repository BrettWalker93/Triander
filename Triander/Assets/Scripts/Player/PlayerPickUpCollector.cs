using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpCollector : MonoBehaviour
{    
    PlayerHandler ph;
    PlayerPowerHandler pph;

    public int BudgetTotal;

    private int BudgetSpent { get; set; }

    private void Awake()
    {
        BudgetSpent = 0;
    }

    private void Start()
    {
        ph = GetComponent<PlayerHandler>();
        pph = GetComponent<PlayerPowerHandler>();

        Messenger.AddListener<int, string>("pickup collision", PickUpCollision);
    }

    void PickUpCollision(int pickUpCost, string pickUpType)
    {
        //if room in budget, collect power and destroy pick-up
        if (BudgetSpent + pickUpCost <= BudgetTotal)
        {
            pph.AddPower(pickUpType);

            BudgetSpent += pickUpCost;

            //broadcast collection
            Messenger.Broadcast("pickup collected");
        }
    }
}
