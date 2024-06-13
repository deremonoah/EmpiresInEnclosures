using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMulti : Projectile
{
    private List<Projectile> followers;

    private void Awake()
    {
        List<GameObject> temp = new List<GameObject>();
        followers = new List<Projectile>();
        foreach(Transform child in this.transform)
        {
            temp.Add(child.gameObject);
        }
        Debug.Log("temp count: "+temp.Count);
        for (int lcv = 0; lcv < temp.Count; lcv++)
        {
            followers.Add(temp[lcv].GetComponent<Projectile>());
            if(followers[lcv]==null)
            { Debug.Log("why the fuck this null???!!!!!!!!"); }
        }
        Debug.Log("how many followers: " + followers.Count);
        Debug.Log("followers null? "+followers == null);
    }
    public override void SetTarget(Vector2 pos, GameObject shooter)
    {
        Debug.Log("foll: "+followers.Count);
        Debug.Log(followers);
        for(int lcv=0;lcv<followers.Count;lcv++)
        {
            followers[lcv].SetTarget(pos, shooter);
            Debug.Log("in list: " + lcv + " times");
        }
        base.SetTarget(pos, shooter);
    }
}
