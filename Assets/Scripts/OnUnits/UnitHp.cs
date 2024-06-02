using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHp : HP
{
    //handles taking damage and dieing

    public override bool Die()
    {
        UnitManager um = FindObjectOfType<UnitManager>();
        um.PlayerGetsPower(GetComponent<UnitStats>().getFriendlyPayOnDeath(),false);
        um.EnemyGetsPower(GetComponent<UnitStats>().getEnemyPayOnDeath(), false);
        Destroy(this.gameObject);
        return true;
    }


    //maybe healing later
}
