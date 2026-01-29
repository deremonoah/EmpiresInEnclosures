using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterNode : NodeData
{
    //like slay the spire encounters
    //a shop if there is some currency, that could be part of the known information, so you might pick one enemy over another
    //otherwise it might be a pick of several options, like the fountains giving temporary buffs, or popsycle stand
    [SerializeField] List<Reward> rewardOptions;//might need scriptable objects for these, maybe just whole encounter?
    //can we reuse the code for upgrade panel?
    public override List<Reward> GenerateRewardOptions()
    {
        Debug.Log("in encounter node");
        return RemoveDuplicateUpgrages(rewardOptions);
    }
}
