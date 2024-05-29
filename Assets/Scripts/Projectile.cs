using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    private Vector2 Target;
    public float moveSpeed;

    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, Target, step);
        
    }

    public void SetTarget(Vector2 pos,GameObject shooter)
    {
        Target = pos;
        this.gameObject.layer = shooter.layer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //I need to declaire if its enemy or friendly projectile layer
        Debug.Log("hit something");
        if (this.gameObject.layer != collision.gameObject.layer)
        {
            collision.gameObject.GetComponent<HP>().DamageThis(damage);
        }
        Destroy(this.gameObject);
    }
}
