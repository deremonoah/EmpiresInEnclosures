using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AuraAbility", menuName = "Abilities/AuraAbility")]
public class AuraAbility : OnDeathAbility //ondeath so it can remove buffs when it dies
{
    [SerializeField] private BuffsType _buffType;
    [SerializeField] private float _buffStrength;
    [SerializeField] private Targets _whoToTarget;
    [SerializeField] private float _AuraSize;
    private List<UnitStats> buffTargets;

    public override void UseAbility(Transform origin)//wait! this could be called for when it dies, to remove all?
    {
        RemoveAllBuffs();//called on death
    }

    public void ApplyBuff(UnitStats us)//why did I name class apply buff?
    {
        //maybe when applying buff we send our info?
        us.BuffedFrom(this);
    }

    public void RemoveBuff(UnitStats us)//this gets called when the thing dies and needs a list of what entered it on auraHolder
    {
        //check if list contains unitStats then remove it & remove any null targets
        //call remove buff
        buffTargets.Remove(us);
        us.RemovedBuffFrom(this);
        //clear any nulls from list here?
    }

    private void RemoveAllBuffs()//called when unit dies
    {
        foreach(UnitStats us in buffTargets)
        {
            RemoveBuff(us);
        }
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