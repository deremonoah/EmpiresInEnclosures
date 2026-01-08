using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData : MonoBehaviour
{
    [SerializeField] List<NodeData> nodesNextToMe;
    //prefrence for strategy
    //map to load or it loads based off faction on the map (I am thinking just disabled in scene)

    public void SendNodeData()
    {
        FindObjectOfType<MapPanel>().lookAtMapNode(this);//this feels weird and prob not right, should I use events?
    }

    public List<NodeData> GetNearbyNodes()
    {
        return nodesNextToMe;
    }

}
public enum NodeType { start, enemy, shop, Boss}
