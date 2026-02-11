using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAbility : UnitAbility
{
    public override void UseAbility(Transform origin)
    {
        Debug.Log("use ability doesn't do anything in Attackability ");
    }

    public virtual void UseAttackAbility(HP target, GameObject attacker, int layerToAttack)
    {

    }
}
