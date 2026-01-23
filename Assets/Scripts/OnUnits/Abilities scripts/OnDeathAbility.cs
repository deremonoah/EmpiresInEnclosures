using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathAbility : UnitAbility
{
    public override void UseAbility(Transform origin)
    {
        Debug.Log("in on deathAbility not DropOnDeath");
    }
}
