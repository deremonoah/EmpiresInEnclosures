using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraHolder : MonoBehaviour
{
    [SerializeField] private AuraAbility ability;//maybe could care about it having things in its range, maybe it buffs itself for grapple?
    private CircleCollider2D _myCol;
    private List<Collider2D> results = new();
    [SerializeField]
    private List<UnitStats> buffTargets;
    [SerializeField] private int parentsLayer;
    private void Start()
    {
        _myCol=GetComponent<CircleCollider2D>();
        _myCol.radius = ability.getAuraSize();
        parentsLayer=this.transform.parent.gameObject.layer;//when we set units layer doesn't set below stuff I think
        results.Clear();
        Physics2D.OverlapCollider(_myCol, new ContactFilter2D().NoFilter(), results);

        foreach(var target in results)
        {
            GameObject go=target.gameObject;
            if(go!=null)
            {
                handleCollision(go);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var go = collision.gameObject;
        handleCollision(go);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UnitStats us = collision.gameObject.GetComponent<UnitStats>();
        if (us != null)
        {
            RemoveBuff(us);
        }
    }

    private void handleCollision(GameObject go)
    {
        Debug.Log("in handle collision");
        UnitStats us =go.GetComponent<UnitStats>();
        if(us==null)
        {
            Debug.Log("unit stats is null");
            return;
        }

        if (ability.getWhoToTarget() == Targets.Opponents)
        {
            if ((parentsLayer == 7 && go.layer == 6) || (parentsLayer == 6 && go.layer == 7))//targets opposite teams, but not all auras will
            {
                ability.UseAbility(us.transform); 
            }
        }
        else if (ability.getWhoToTarget() == Targets.Friendly)
        {
            Debug.Log("friendly unit being handled");
            if (parentsLayer == go.layer)//same layer = same team
            {
                ability.UseAbility(us.transform); Debug.Log("in friendly target");
            }
        }
        else if (ability.getWhoToTarget() == Targets.All)
        {
            if (go.layer == 7 || go.layer == 6) //if enemy or player unit
            {
                ability.UseAbility(us.transform);
            }
        }
    }

    private void RemoveAllBuffs()//called when unit dies
    {
        foreach (UnitStats us in buffTargets)
        {
            RemoveBuff(us);
        }
    }

    public void RemoveBuff(UnitStats us)//this gets called when the thing dies and needs a list of what entered it on auraHolder
    {
        //check if list contains unitStats then remove it & remove any null targets
        //call remove buff
        buffTargets.Remove(us);
        us.RemovedBuffFrom(ability);
        //clear any nulls from list here?
    }

    private void OnDestroy()
    {
        RemoveAllBuffs();
    }
}
