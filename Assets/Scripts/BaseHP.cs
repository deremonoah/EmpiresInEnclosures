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
        /*if(this.gameObject.layer==6 && this.GetHP()<=0)
        {
            //idk what this if was for????
        }*/
    }

    public override bool Die() 
    {
        Debug.Log("somebody lost");
        return true;
    }
}
