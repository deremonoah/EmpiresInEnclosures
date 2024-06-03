using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] private float currentHp, maxHp;
    private UnitAnimator anim;

    //handles taking damage and dieing
    void Start()
    {
        maxHp = GetComponent<UnitStats>().getMaxHp();
        currentHp = maxHp;
        anim = GetComponent<UnitAnimator>();
    }

    public virtual void DamageThis(float damge)
    {
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

    public virtual bool Die()
    {
        
        return true;
    }
}
