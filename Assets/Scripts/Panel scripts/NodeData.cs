using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData : MonoBehaviour
{
    [SerializeField] NodeType myType; //this should probably just be a parent child thing, parent node class & shop, enemy & boss enherit
    [SerializeField] Faction factionToFight;//this gets sent to unit manager? for who the enemy is
    [SerializeField] List<GameObject> units;//this should be all possible units then based on "turn" count uses more of them
    //prefrence for strategy
    //map to load or it loads based off faction on the map (I am thinking just disabled in scene)
    //could this data be on the faction?

    //refrences to the nodes next to it to allow you to pick from them, or maybe active ones next to it?
    //can you go back across areas you have beaten?
    public void SendNodeData()
    {
        FindObjectOfType<MapPanel>().lookAtMapNode(this);//this feels weird and prob not right, should I use events?
    }

    public Faction getFactionOnNode()
    {
        return factionToFight;
    }

    public NodeType getNodeType()
    {
        return myType;
    }

    public List<GameObject> getUnits()
    {
        return units;
    }
    //either the enemyAi will grab unit info or this sends its info, probably grab info better?
}
public enum NodeType { enemy, shop, Boss}