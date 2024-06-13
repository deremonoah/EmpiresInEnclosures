using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : Projectile
{
    public GameObject SpawnPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //I need to declaire if its enemy or friendly projectile layer
        Debug.Log("hit something");
        if (this.gameObject.layer != collision.gameObject.layer)
        {
            collision.gameObject.GetComponent<HP>().DamageThis(damage);
            
        }
        var dude =Instantiate(SpawnPrefab, this.transform.position, this.transform.rotation);
        dude.gameObject.layer = this.gameObject.layer;
        //get target of which base from unit manager
        Transform tar = FindObjectOfType<UnitManager>().GetmoveTarget(dude.gameObject.layer);
        dude.GetComponent<UnitAI>().SetMoveTarget(tar.position);
        Destroy(this.gameObject);
    }
}
