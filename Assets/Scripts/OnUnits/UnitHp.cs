using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHp : HP
{
    //handles taking damage and dieing

    public override bool Die()
    {
        Destroy(this.gameObject);
        return true;
    }


    //maybe healing later
}
