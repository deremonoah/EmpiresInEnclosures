using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public float currentHp, maxHp;
    public UnitAnimator anim;

    //handles taking damage and dieing
    void Start()
    {
        currentHp = maxHp;
        anim = GetComponentInChildren<UnitAnimator>();
    }

    public virtual void DamageThis(float damge)
    {
        Debug.Log("in damage this");
        currentHp -= damge;
        if (anim != null) { anim.TookDamage(); }
        if (currentHp <= 0)
        {
            //maybe other stuff here at some point?
            Die();
        }
    }

    public float GetHP()
    {
        return currentHp;
    }

    public float GetHPPercent()
    {
        return currentHp / maxHp;
    }

    public virtual void Die()
    {
        
    }

}
