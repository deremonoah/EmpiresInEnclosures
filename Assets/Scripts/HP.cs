using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] protected float currentHp, maxHp;
    public UnitAnimator anim;
    protected UltimateManager ulti;
    protected bool amPlayer;

    //handles taking damage and dieing
    void Start()
    {
        StartUp();
    }

    protected void StartUp()
    {
        currentHp = maxHp;
        anim = GetComponentInChildren<UnitAnimator>();

        ulti = FindObjectOfType<UltimateManager>();

        if (this.gameObject.layer == 7)//player layer
        { amPlayer = true; }
        else //enemy layer
        { amPlayer = false; }
    }

    public virtual void DamageTaken(float damage)
    {
        //Debug.Log("in damage this");
        currentHp -= damage;
        if (anim != null) { anim.TookDamage(); }
        if (currentHp <= 0)
        {
            //maybe other stuff here at some point?
            Die();
        }
    }

    public virtual void ThisAttackedYou(UnitStats us)
    {

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
