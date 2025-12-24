using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseHP : HP
{
    public HPBarManager barUI;
    public event Action<string> BattleEndedLoserCalls;


    public override void ThisAttackedYou(UnitStats us)
    {
        base.DamageTaken(us.getBaseAttack());
        barUI.UpdateHP(base.GetHPPercent());

        ulti.chargePlayerUlt(amPlayer, us.getBaseAttack()*3);//for now we will just have it be the damage
        //trying times 3 for now because ultimate is 180
        if (this.GetHP()<=0)
        {
            Die();
        }
    }

    public override void Die() 
    {
        BattleEndedLoserCalls.Invoke(this.gameObject.name);//sends out event with its name on it, so flow manager will know if player lost battle
    }
}
