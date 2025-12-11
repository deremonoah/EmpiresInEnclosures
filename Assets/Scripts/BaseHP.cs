using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseHP : HP
{
    public HPBarManager barUI;
    public event Action<string> BattleEndedLoserCalls;

    public override void DamageThis(float damge)
    {
        base.DamageThis(damge);
        barUI.UpdateHP(base.GetHPPercent());
        
        if(this.GetHP()<=0)
        {
            Die();
        }
    }

    public override void Die() 
    {
        BattleEndedLoserCalls.Invoke(this.gameObject.name);//sends out event with its name on it, so flow manager will know if player lost battle
    }
}
