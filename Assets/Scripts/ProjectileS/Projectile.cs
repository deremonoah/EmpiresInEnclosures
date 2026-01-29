using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;//set by the unit ai rn
    private Vector2 Target;// will keep this around for homing shots, bees! other homing shots wouldn't make sense imo
    private Vector2 Direction;
    public float moveSpeed;
    public float lifeSpan;
    protected UnitStats myShootersStats;

    void Update()
    {
        Vector2 step = moveSpeed * Time.deltaTime * Direction;
        //transform.position = Vector2.MoveTowards(transform.position, Target, step); old way shoots at one pos
        transform.position += (Vector3)step;
        if(lifeSpan<=0)
        {
            Destroy(this.gameObject);
        }
        else { lifeSpan -= Time.deltaTime; }
    }

    public virtual void SetTarget(Vector2 Targetpos,GameObject shooter)
    {
        //calculate direction
        Target = Targetpos;
        //goes from current position, which would be shoot point not position of shooter which is could cause problems
        Vector2 shootPos = new Vector2(transform.position.x, transform.position.y);
        Direction= Targetpos- shootPos;
        Direction = Direction.normalized;
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
                if(myShootersStats!=null)
                { collision.gameObject.GetComponent<HP>().ThisAttackedYou(myShootersStats); }
                else { collision.gameObject.GetComponent<HP>().DamageTaken(damage); }
            }
        }
        //Debug.Log("collided with " + collision.gameObject.name);
        Destroy(this.gameObject);
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }
}
