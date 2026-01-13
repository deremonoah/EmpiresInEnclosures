using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUpAbility : ScriptableObject
{
    protected UnitManager um;
    [SerializeField] protected float TimeToPickUp;

    private void OnEnable()
    {
        //does it get enabled?
    }

    public float getTimeToPickUP()
    {
        return TimeToPickUp;
    }

    public abstract void ActivatePickUp(Transform origin, int isPlayerLayer);
}
