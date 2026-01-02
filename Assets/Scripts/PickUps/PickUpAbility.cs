using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUpAbility : ScriptableObject
{
    protected UnitManager um;

    private void OnEnable()
    {
        //does it get enabled?
    }

    public abstract void ActivatePickUp(Transform origin, int isPlayerLayer);
}
