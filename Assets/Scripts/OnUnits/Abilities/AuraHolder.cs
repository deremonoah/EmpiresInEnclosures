using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraHolder : MonoBehaviour
{
    [SerializeField] private AuraAbility ability;//maybe could care about it having things in its range, maybe it buffs itself for grapple?
    private CircleCollider2D _myCol;
    private List<Collider2D> results = new();

    private void OnEnable()
    {
        _myCol=GetComponent<CircleCollider2D>();
        _myCol.radius = ability.getAuraSize();

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

    private void handleCollision(GameObject go)
    {
        UnitStats us =go.GetComponent<UnitStats>();
        if(us==null)
        {
            return;
        }

        if (ability.getWhoToTarget() == Targets.Opponents)
        {
            if ((gameObject.layer == 7 && go.layer == 6) || (gameObject.layer == 6 && go.layer == 7))//targets opposite teams, but not all auras will
            {
                ability.ApplyBuff(us);
            }
        }
        else if (ability.getWhoToTarget() == Targets.Friendly)
        {
            if (gameObject.layer == go.layer)//same layer = same team
            {
                ability.ApplyBuff(us);
            }
        }
        else if (ability.getWhoToTarget() == Targets.All)
        {
            if (go.layer == 7 || go.layer == 6) //if enemy or player unit
            {
                ability.ApplyBuff(us);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UnitStats us = collision.gameObject.GetComponent<UnitStats>();
        if(us!=null)
        {
            ability.RemoveBuff(us);
        }
    }
}
