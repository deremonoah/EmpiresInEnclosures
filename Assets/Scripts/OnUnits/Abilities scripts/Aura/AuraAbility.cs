using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AuraAbility", menuName = "Abilities/AuraAbilityOrBuffReward")]
public class AuraAbility : OnDeathAbility //ondeath so it can remove buffs when it dies
{
    [SerializeField] private BuffsType _buffType;
    [SerializeField] private float _buffStrength;
    [SerializeField] private Targets _whoToTarget;
    [SerializeField] private UnitType _whatToBuff;
    [SerializeField] private float _AuraSize;

    public override void UseAbility(Transform origin)//wait! this could be called for when it dies, to remove all?
    {
        origin.gameObject.GetComponent<UnitStats>().BuffedFrom(this);
    }

    //getters for unitStats
    public BuffsType getBuffType()
    {
        return _buffType;
    }

    public float getBuffStength()
    {
        return _buffStrength;
    }

    public Targets getWhoToTarget()
    {
        return _whoToTarget;
    }

    public float getAuraSize()
    {
        return _AuraSize;
    }

    public UnitType getTypeToBuff()
    { return _whatToBuff; }
}
public enum BuffsType 
{ 
    attack,
    attackToBase,
    attackSpeed,
    HP,
    moveSpeed,
    armor,
    sightRange,
    attackRange,
    BonusPayOnDeathFriendly,
    BonusPayOnDeathEnemy
}

public enum Targets
{
    Friendly,
    Opponents,
    All
}

