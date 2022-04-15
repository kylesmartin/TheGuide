using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    // action identifiers
    public bool putSubjectInInventory = false;
    public bool cookSubject = false;
    public bool consumeSubject = false;

    // general settings
    public float timeToComplete = 0;
    public bool yieldsResult = false;
    public Transform startLocation;
    public Transform completedLocation;
    private bool isComplete;
    private float[] needsDelta;
    private GameObject subject;

    // Start is called before the first frame update
    void Start()
    {
        needsDelta = new float[GameManager.instance.numNeeds];
        isComplete = false;
        subject = null;
    }

    public void CompleteAction(AIAgent _aiAgent)
    {
        // mark action as complete
        _aiAgent.actions.Dequeue();
        isComplete = true;
        // update needs
        float[] _agentNeeds = _aiAgent.GetNeeds();
        if (yieldsResult) for (int i = 0; i < _agentNeeds.Length; i++) _agentNeeds[i] = Mathf.Clamp(_agentNeeds[i] + needsDelta[i], 0, 100);
        _aiAgent.SetNeeds(_agentNeeds);
        // set subject to completed location
        if (completedLocation != null) {
            subject.transform.parent = completedLocation;
            subject.transform.localPosition = Vector3.zero;
        }
        // complete standards for each action id
        ActionIdentifierTasks(_aiAgent);
    }

    public float[] GetNeedsDelta()
    {
        return needsDelta;
    }

    public void SetNeedsDelta(float[] _needsDelta)
    {
        needsDelta = _needsDelta;
    }

    public bool IsComplete()
    {
        return isComplete;
    }

    public void SetSubject(GameObject _subject)
    {
        subject = _subject;
    }

    public void SetToStartLocation()
    {
        if (startLocation != null)
        {
            subject.transform.parent = startLocation.transform;
            subject.transform.localPosition = Vector3.zero;
        }
    }

    private void ActionIdentifierTasks(AIAgent _aiAgent)
    {
        if (putSubjectInInventory)
        {
            _aiAgent.inventory.Add(subject);
        }
        else if (cookSubject)
        {
            Food _food = subject.GetComponent<Food>();
            _food.isRaw = false;
            _food.advertiser.SetNeedsDelta(needsDelta);
        }
        else if (consumeSubject)
        {
            if (subject.GetComponent<Water>() != null) GameManager.instance.allWater.Remove(subject.GetComponent<Water>());
            if (subject.GetComponent<Food>() != null) GameManager.instance.allFood.Remove(subject.GetComponent<Food>());
            if (_aiAgent.inventory.Contains(subject)) _aiAgent.inventory.Remove(subject);
            Destroy(subject);
        }
    }
}
