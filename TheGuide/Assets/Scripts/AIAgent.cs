using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private float[] needs;                // [hunger, thirst, ...]
    public Queue<Action> actions;         // action queue
    public List<GameObject> inventory;    // storage for items carried by NPC

    // needs thresholds 
    public float hungerThreshhold;
    public float thirstThreshhold;

    // personality parameters
    public float intelligence;

    // action state variables
    private int state = 0;                // {chilling, performing action, walking to action, follow player}      
    private Action currentAction;
    private float timeSpentOnAction = 0;

    // needs state variables
    private bool isHungry = false;
    private bool isThirsty = false;

    private void Awake()
    {
        actions = new Queue<Action>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        needs = new float[GameManager.instance.numNeeds];
        for (int i = 0; i < needs.Length; i++) needs[i] = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && state != 3) state = 3;
        DecayNeeds(Time.deltaTime);
        switch (state)
        {
            // chilling
            case 0:
                FollowPlayer(); // TODO: change to some idle animation
                ActionSelection();
                if (actions.Count > 0) ExitState();
                break;
            // performing action
            case 1:
                timeSpentOnAction += Time.deltaTime;
                if (timeSpentOnAction >= currentAction.timeToComplete) ExitState();
                break;
            // walking to action
            case 2:
                agent.destination = currentAction.transform.position;
                if ((agent.destination - transform.position).magnitude < 2f) ExitState();
                break;
            // following player
            case 3:
                FollowPlayer();
                if (Input.GetKeyDown(KeyCode.Q)) ExitState();
                break;
            // otherwise do nothing
            default:
                break;
        }
    }

    void DecayNeeds(float _deltaTime)
    {
        // hunger
        isHungry = Decay(_deltaTime, 1f, GameManager.instance.hungerIndex, hungerThreshhold);
        // thirst
        isThirsty = Decay(_deltaTime, 2f, GameManager.instance.thirstIndex, thirstThreshhold);
    }

    void ActionSelection()
    {
        float _maxScore = float.MinValue;
        float _score = 0;
        Action _nextAction = null;
        // Food
        FindMaxDelta(ref _maxScore, ref _score, ref _nextAction, GameManager.instance.allFood);
        // Water
        FindMaxDelta(ref _maxScore, ref _score, ref _nextAction, GameManager.instance.allWater);
        // Heat
        FindMaxDelta(ref _maxScore, ref _score, ref _nextAction, GameManager.instance.allHeat);
        if (_nextAction != null) actions.Enqueue(_nextAction);
    }

    float AttenuatedScoring(float[] _currentVals, float[] _deltas)
    {
        float _score = 0;
        for (int i = 0; i < _currentVals.Length; i++) _score += (10 / _currentVals[i]) - (10 / (_currentVals[i] + _deltas[i]));
        return _score;
    }

    void FollowPlayer()
    {
        agent.destination = PlayerMovement.instance.transform.position;
        agent.isStopped = (agent.destination - transform.position).magnitude < 3f;
    }

    public bool IsHungry()
    {
        return isHungry;
    }

    public bool IsThirsty()
    {
        return isThirsty;
    }

    private void FindMaxDelta(ref float _maxScore, ref float _score, ref Action _nextAction, dynamic _adverts)
    {
        for (int i = 0; i < _adverts.Count; i++)
        {
            Action _action = _adverts[i].GetCurrentAdvert(this);
            if (_action != null)
            {
                _score = AttenuatedScoring(needs, _action.GetNeedsDelta());
                if (_score > _maxScore)
                {
                    _maxScore = _score;
                    _nextAction = _action;
                }
            }
        }
    }

    private bool Decay(float _deltaTime, float _decayFactor, int _index, float _threshold)
    {
        needs[_index] = Mathf.Clamp(needs[_index] - _deltaTime * _decayFactor, 0, 100);
        if (needs[_index] >= _threshold) return false;
        return true;
    }

    private void ExitState()
    {
        int _state = state;
        switch (_state)
        {
            // get next action and start walking to it
            case 0:
                currentAction = actions.Peek();
                state = 2;
                break;
            // complete action and start chilling
            case 1:
                currentAction.CompleteAction(this);
                state = 0;
                break;
            // stop walking and start performing action
            case 2:
                currentAction.SetToStartLocation(); // TODO: replace with action animation
                state = 1;
                timeSpentOnAction = 0;
                break;
            // stop following player and return to actions
            case 3:
                state = (currentAction == null) ? 0 : 2;
                agent.isStopped = false;
                break;
        }
    }

    public float[] GetNeeds()
    {
        return needs;
    }

    public void SetNeeds(float[] _needs)
    {
        needs = _needs;
    }
}
