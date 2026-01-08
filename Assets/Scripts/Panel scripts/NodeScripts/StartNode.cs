using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : NodeData
{
    private void Start()
    {
        FindObjectOfType<MapPanel>().AddConqueredNode(this);
    }
}
