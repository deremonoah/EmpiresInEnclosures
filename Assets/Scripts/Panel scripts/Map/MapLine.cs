using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLine : MonoBehaviour
{
    [SerializeField] Transform nodePos;
    [SerializeField] int count;

    public Vector2 getNodePos()
    {
        return nodePos.position;
    }

    public void SetCount(int c)
    {
        count = c;
    }

    private void Start()
    {
        if(count>1)//not sure but was spawning a 0 one idk
        {
            SpawnLine();
        }
    }

    private void SpawnLine()
    {
        //like previously we need to check positions, but we need to bias a cone of direction
        //this things rotation+or- some amount, but doesn't handle interconnected stuff
        var line = Instantiate(this.gameObject, nodePos.position, this.transform.rotation, this.transform);
        line.GetComponent<MapLine>().SetCount(count - 1);
    }
}
