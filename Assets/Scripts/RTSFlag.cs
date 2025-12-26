using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSFlag : MonoBehaviour
{
    private int numToCross;
    public void NumberToldToGo(int count)
    {
        numToCross = count;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.gameObject;
        if(unit.layer==7)//so if a player's unit walks over it it disapears, or maybe it waits for all of the commanded units to walk past it?
        {
            numToCross--;
            if(numToCross<=0)
            {
                //oops need to tell it to remove itself
                List<Vector2> temp = new List<Vector2>();
                temp.Add(this.transform.position);
                FindObjectOfType<RTSController>().ForgotFlag(temp);
                Destroy(this.gameObject);
            }
        }
    }
}
