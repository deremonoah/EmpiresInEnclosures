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

    public virtual List<Reward> GenerateRewardOptions()
    {
        Debug.LogError("returned list from Node Data, its empty");
        return new List<Reward>();
    }

    protected List<Reward> RemoveDuplicateUpgrages(List<Reward> re)
    {
        List<GameObject> prevUps = UnitManager.instance.GetUpgradeHistory();

        //could remove dups from the loot list first? then pull random number
        for (int lcv = 0; lcv < prevUps.Count; lcv++)
        {
            for (int lcv2 = 0; lcv2 < re.Count; lcv2++)
            {
                if (re[lcv2] == prevUps[lcv])
                {
                    re.RemoveAt(lcv2);
                    lcv2 = 0;
                    //do we put a break here? I think it should work
                }
            }
        }

        return re;
    }
}
public enum NodeType { start, enemy, shop, Boss}
