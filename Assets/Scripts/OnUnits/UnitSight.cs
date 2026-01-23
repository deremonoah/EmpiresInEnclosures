using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSight : MonoBehaviour
{
    private UnitAI ai;
    private int aiLayer;
    private float Timer, TickMax = .3f;
    private CircleCollider2D _myCol;
    private List<Collider2D> results = new();
    private int LayerToTarget;

    private void OnEnable()//ontrigger enter will proc on enalbe & it needs to know its pappy
    {
        ai = gameObject.GetComponentInParent<UnitAI>();
    }

    private void Start()//needs to be on start so parent has enough time to get its layer to be correct for held layer refrence
    {
        aiLayer = ai.gameObject.layer;//easier way to grab parent layer?
        var stats =gameObject.GetComponentInParent<UnitStats>();
        _myCol = GetComponent<CircleCollider2D>();
        _myCol.radius = stats.getSightRange();
        Timer = TickMax;

        StartCoroutine(SetUpEnemyLayerRoutine());//this is to see if it will help with pulled pair not seeing things?
        //if I ever made a convert ability or unit, then this will need to be called on the convert
    }

    private IEnumerator SetUpEnemyLayerRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (aiLayer == 7)//player targets enemy
        {
            LayerToTarget = 6;
        }
        if (aiLayer == 6)//enemy targets player
        {
            LayerToTarget = 7;
        }
    }

    private void Update()
    {
        if (Timer <= 0)
        {
            Physics2D.OverlapCollider(_myCol, new ContactFilter2D().NoFilter(), results);
            if(results.Count>0)//only on tick speed do we check then if something is in collider
            {
                //Debug.Log("we are in the results >0");
                GameObject target = null;
                Vector3 myPos = transform.position;
                foreach (Collider2D co in results)
                {
                    GameObject go = co.gameObject;
                    Vector3 doh = go.transform.position;
                    float disNew = Vector3.Distance(myPos, doh);
                    if (target == null)
                    {
                        if (go.layer == LayerToTarget)
                        { target = go; }
                    }
                    else
                    if(go.layer == LayerToTarget)
                    {
                        if (Vector3.Distance(myPos, doh) <= Vector3.Distance(myPos, target.transform.position))
                        {
                            target = go;
                        }
                    }
                }
                //Debug.Log("past the for each loop");
                if (target!=null)
                {
                    //Debug.Log("target!=nullTarget's name: " + target.gameObject.name);
                    HP targetHP = target.GetComponent<HP>();
                    if (target.layer == LayerToTarget)//you saw
                    {
                        ai.SeeTarget(target.transform.position, targetHP);
                    }
                }
                Timer = TickMax;
            }
            
        }
        else { Timer -= Time.deltaTime; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        HP targetHP = target.GetComponent<HP>();
        if (target.layer == LayerToTarget)//you saw
        {
            ai.SeeTarget(target.transform.position, targetHP);
        }
    }

}
