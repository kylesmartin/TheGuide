using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int numNeeds = 100;
    public int hungerIndex = 0;
    public int thirstIndex = 1;
    public List<Food> allFood;
    public List<Heat> allHeat;
    public List<Water> allWater;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            allFood = new List<Food>(FindObjectsOfType<Food>());
            allHeat = new List<Heat>(FindObjectsOfType<Heat>());
            allWater = new List<Water>(FindObjectsOfType<Water>());
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
    }
}
