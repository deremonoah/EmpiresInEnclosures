using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHP : HP
{
    public HPBarManager barUI;

    public override void DamageThis(float damge)
    {
        base.DamageThis(damge);
        barUI.UpdateHP(base.GetHP());
    }

    public override bool Die() 
    {
        Debug.Log("somebody lost");
        return true;
    }
}
