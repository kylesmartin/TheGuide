using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour
{
    // public variables
    public Action cookFoodTask;
    public float heatBoost = 0;
    public Advertiser advertiser;

    // Start is called before the first frame update
    void Start()
    {
        advertiser = new Advertiser();
        advertiser.SetNeed(GameManager.instance.hungerIndex, heatBoost);
    }

    public Action GetCurrentAdvert(AIAgent _aiAgent)
    {
        float _maxHungerBoost = float.MinValue;
        Action _returnAction = null;
        for (int i = 0; i < _aiAgent.inventory.Count; i++)
        {
            Food _food = _aiAgent.inventory[i].GetComponent<Food>();
            if (_food != null && _food.hungerBoost > _maxHungerBoost && _food.isRaw)
            {
                _maxHungerBoost = _food.hungerBoost;
                float[] _sum = new float[GameManager.instance.numNeeds];
                for (int j = 0; j < GameManager.instance.numNeeds; j++) _sum[j] = _food.advertiser.GetNeedsDelta()[j] + advertiser.GetNeedsDelta()[j];
                cookFoodTask.SetNeedsDelta(_sum);
                cookFoodTask.SetSubject(_food.gameObject);
                _returnAction = cookFoodTask;
            }
        }
        return _returnAction;
    }
}












