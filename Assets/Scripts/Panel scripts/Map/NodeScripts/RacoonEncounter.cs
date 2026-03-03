using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacoonEncounter : EncounterNode
{
    public override List<Reward> GenerateRewardOptions()
    {
        Debug.Log("in encounter node");
        List<Reward> trash = new List<Reward>();
        Debug.Log("count in nodesNextToMe " + nodesNextToMe.Count);
        foreach(NodeData no in nodesNextToMe)
        {
            List<Reward> temp = new List<Reward>(no.GenerateRewardOptions());
            foreach(Reward re in temp)
            {
                rewardOptions.Add(re);
            }
        }
        //this generates all the options
        Debug.Log("list count " + rewardOptions.Count);
        return RemoveDuplicateUpgrages(rewardOptions);
    }
}
