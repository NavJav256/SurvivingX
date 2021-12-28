using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatableData : ScriptableObject
{
    public event System.Action onValuesUpdated;
    public bool autoUpdate;

    protected virtual void OnValidate()
    {
        if (autoUpdate) notifyUpdatedValues();
    }

    public void notifyUpdatedValues()
    {
        if (onValuesUpdated != null) onValuesUpdated();
    }
}
