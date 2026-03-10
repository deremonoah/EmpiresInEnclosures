using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackWStat", menuName = "Abilities/AttackWStat")]
public class AttackWStatAbility : AttackAbility
{
    [SerializeField] BuffsType stat;

    public override void UseAttackAbility(HP target, GameObject attacker, int layerToAttack)
    {
        UnitStats us=attacker.GetComponent<UnitStats>();
        //if their attack is 0 then this can attack instead and deal damage equal to the stat
        float damage = 0;//this might get changed later if we rework unit stats
        //the original idea was to buff attack to the number, but i would have to create aura ability at run time

        switch(stat)//their attack is set to 0 so they still get buffs in get attack
        {
            case BuffsType.HP:
                damage =attacker.GetComponent<HP>().getCurrentHP()+us.getAttackBuffs();
                break;
            case BuffsType.armor:
                damage = us.getArmor() + us.getAttack();
                break;
            default:
                Debug.LogError("in attackWStat using a varable we don't have in switch case");
                break;
            //maybe more in future if this can get put on units?
        }
        Debug.Log("ability health number is " + damage);
        target.DamageTaken(damage);
        
    }
}
