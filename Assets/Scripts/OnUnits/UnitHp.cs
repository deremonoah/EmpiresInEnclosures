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
        myhp = GetComponent<HPBarManager>();
        StartUp();
    }

    public override void ThisAttackedYou(UnitStats us)
    {
        base.DamageTaken(us.getAttack());//the ultimate can't deal damage with this current code
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
        
        ulti.chargePlayerUlt(amPlayer, 2);
        
        Destroy(this.gameObject);
        
    }


    //maybe healing later
}
