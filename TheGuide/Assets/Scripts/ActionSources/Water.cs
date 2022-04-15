using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // public variables
    public Action putItemInInventoryTask;
    public Action drinkWaterTask;
    public float thirstBoost = 0;
    public Advertiser advertiser;

    // Start is called before the first frame update
    void Start()
    {
        advertiser = new Advertiser();
        advertiser.SetNeed(GameManager.instance.thirstIndex, thirstBoost);
    }

    public Action GetCurrentAdvert(AIAgent _aiAgent)
    {
        if (_aiAgent.IsThirsty()) return advertiser.PrepActionForReturn(drinkWaterTask, gameObject);
        if (!putItemInInventoryTask.IsComplete()) return advertiser.PrepActionForReturn(putItemInInventoryTask, gameObject);
        return null;
    }
}