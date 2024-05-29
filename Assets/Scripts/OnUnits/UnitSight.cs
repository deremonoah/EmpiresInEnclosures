using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSight : MonoBehaviour
{
    private UnitAI ai;
    private void Awake()
    {
        ai = gameObject.GetComponentInParent<UnitAI>();
        var stats =gameObject.GetComponentInParent<UnitStats>();
        var collider = GetComponent<BoxCollider2D>();
        collider.size = stats.getSightRange();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
 
        if (ai.gameObject.layer==7 && collision.gameObject.layer ==6)
        {
            ai.SeeTarget(collision.gameObject.transform.position,collision.GetComponent<HP>());
        }
        if (ai.gameObject.layer == 6 && collision.gameObject.layer == 7)
        {
            ai.SeeTarget(collision.gameObject.transform.position, collision.GetComponent<HP>());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ai.gameObject.layer == 7 && collision.gameObject.layer == 6)
        {
            ai.SeeTarget(collision.gameObject.transform.position, collision.GetComponent<HP>());
        }
        if (ai.gameObject.layer == 6 && collision.gameObject.layer == 7)
        {
            ai.SeeTarget(collision.gameObject.transform.position, collision.GetComponent<HP>());
        }
    }
}
