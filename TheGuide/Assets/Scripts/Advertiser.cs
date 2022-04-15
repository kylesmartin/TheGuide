using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advertiser
{
    // private variables
    private float[] needsDelta;
    
    public Advertiser()
    {
        needsDelta = new float[GameManager.instance.numNeeds];
    }

    public void SetNeed(int _index, float _value)
    {
        needsDelta[_index] = _value;
    }

    public float[] GetNeedsDelta()
    {
        return needsDelta;
    }

    public void SetNeedsDelta(float[] _needsDelta)
    {
        needsDelta = _needsDelta;
    }

    public Action PrepActionForReturn(Action _action, GameObject _subject)
    {
        _action.SetNeedsDelta(needsDelta);
        _action.SetSubject(_subject);
        return _action;
    }
}