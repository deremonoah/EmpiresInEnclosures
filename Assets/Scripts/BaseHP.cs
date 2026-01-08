using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseHP : HP
{
    public HPBarManager barUI;
    public event Action<string> BattleEndedLoserCalls;

    private void Start()
    {
        if (gameObject.layer == 6)
        {
            var healthBar = FindObjectOfType<EnemyHealthBarFinder>().getHPBar();
            barUI.SetHPBar(healthBar);
        }
        base.StartUp();
        //will need a similar case for player
    }

    public override void ThisAttackedYou(UnitStats us)
    {
        base.DamageTaken(us.getBaseAttack());
        barUI.UpdateHP(base.GetHPPercent());
        //for defending your own base
        if(!amPlayer)
        {
            FindObjectOfType<EnemyBaseAI>().TheyHurtMe(us.gameObject);
        }
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
