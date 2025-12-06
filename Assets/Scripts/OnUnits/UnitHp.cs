using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHp : HP
{
    //handles taking damage and dieing
    private HPBarManager myhp;
    void Start()
    {
        maxHp = GetComponent<UnitStats>().getMaxHp();
        currentHp = maxHp;
        anim = GetComponentInChildren<UnitAnimator>();
        myhp = GetComponent<HPBarManager>();
    }

    public override void DamageThis(float damge)
    {
        base.DamageThis(damge);
        myhp.UpdateHP(base.GetHPPercent());
        /*if(this.gameObject.layer==6 && this.GetHP()<=0)
        {
            //idk what this if was for????
        }*/
    }

    public override void Die()
    {
        UnitManager um = FindObjectOfType<UnitManager>();
        um.PlayerGetsPower(GetComponent<UnitStats>().getFriendlyPayOnDeath(),false);
        um.EnemyGetsPower(GetComponent<UnitStats>().getEnemyPayOnDeath(), false);
        UltimateManager ulti = FindObjectOfType<UltimateManager>();
        if(this.gameObject.layer==7)//enemy unit layer
        {
            ulti.chargePlayerUlt(false, 2);
        }
        else if(this.gameObject.layer == 6)//enemy unit layer
        {
            ulti.chargePlayerUlt(false, 2);//5 seemed too high for many units dying, ulti happened very often
        }
        Destroy(this.gameObject);
        
    }


    //maybe healing later
}
