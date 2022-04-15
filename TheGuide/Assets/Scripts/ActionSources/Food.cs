using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // public variables
    public Action putItemInInventoryTask;
    public Action eatFoodTask;
    public float hungerBoost = 0;
    public bool isRaw = false;
    public Advertiser advertiser;

    // Start is called before the first frame update
    void Start()
    {
        advertiser = new Advertiser();
        advertiser.SetNeed(GameManager.instance.hungerIndex, hungerBoost);
    }

    public Action GetCurrentAdvert(AIAgent _aiAgent)
    {
        if (_aiAgent.IsHungry())
        {
            if (isRaw) return RawFoodDecision(_aiAgent);
            return advertiser.PrepActionForReturn(eatFoodTask, gameObject);
        } 
        else
        {
            if (!putItemInInventoryTask.IsComplete()) return advertiser.PrepActionForReturn(putItemInInventoryTask, gameObject);
            return null;
        }
    }

    private Action RawFoodDecision(AIAgent _aiAgent)
    {
        float _decision = Random.Range(0f, 100f);
        if (_decision > _aiAgent.intelligence) return advertiser.PrepActionForReturn(eatFoodTask, gameObject);
        if (!putItemInInventoryTask.IsComplete()) return advertiser.PrepActionForReturn(putItemInInventoryTask, gameObject);
        return null;
    }
}
