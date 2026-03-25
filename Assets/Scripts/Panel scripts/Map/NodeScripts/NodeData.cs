using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeData : MonoBehaviour
{
    [SerializeField] protected List<NodeData> nodesNextToMe;
    [SerializeField] private Image backgroundSquare;//for now will use this for telling player where they can go or have been
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
        return new List<Reward>();
    }

    protected List<Reward> RemoveDuplicateUpgrages(List<Reward> re)
    {
        List<Reward> prevUps = EquipManagerPlayer.instance.GetUpgradeHistory();
        
        //could remove dups from the loot list first? then pull random number
        for (int lcv = 0; lcv < prevUps.Count; lcv++)//this throws a null exception if player history is blank
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

    public void DisplayThisColor(Color col)
    {
        backgroundSquare.color = col;//in future may have to worry about bosses taking over nodes, would prob change to red
    }
}
public enum NodeType { start, enemy, shop, Boss}
