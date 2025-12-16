using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    private Vector2 Target;
    public float moveSpeed;
    public float lifeSpan;
    private UnitStats myShootersStats;

    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, Target, step);
        if(lifeSpan<=0)
        {
            Destroy(this.gameObject);
        }
        else { lifeSpan -= Time.deltaTime; }
    }

    public virtual void SetTarget(Vector2 pos,GameObject shooter)
    {
        Target = pos;
        //player shot, so projectile on 9
        if (shooter.layer == 7)
        { this.gameObject.layer = 9; }

        //enemy shot on 6 so projectile on 8
        if (shooter.layer == 6)
        { this.gameObject.layer = 8; }
        myShootersStats = shooter.GetComponent<UnitStats>();//anything that shoots should have stats
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //I need to declaire if its enemy or friendly projectile layer
        //if (this.gameObject.layer != collision.gameObject.layer) old way
        //this should now check only the layers that have hp and it should have caught that before tho with the null checck
        if (collision.gameObject.layer < 8 && collision.gameObject.layer > 5)
        {
            if (collision.gameObject.GetComponent<HP>() != null)
            {
                //this is for when projectiles collide with each other
                collision.gameObject.GetComponent<HP>().ThisAttackedYou(myShootersStats);
            }
        }
        Debug.Log("collided with " + collision.gameObject.name);
        Destroy(this.gameObject);
    }
}
