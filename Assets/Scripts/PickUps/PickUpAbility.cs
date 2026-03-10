using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUpAbility : ScriptableObject
{
    protected UnitManager um;
    [SerializeField] protected float TimeToPickUp;
    [SerializeField] protected PickUpType itemType;

    private void OnEnable()
    {
        //does it get enabled?
    }

    public float getTimeToPickUP()
    {
        return TimeToPickUp;
    }

    public PickUpType getPickupType()
    {
        return itemType;
    }

    public abstract void ActivatePickUp(Transform origin, int isPlayerLayer);
}
public enum PickUpType
{
    food,
    bluePrint
}//blueprint is relevant for engineer ability