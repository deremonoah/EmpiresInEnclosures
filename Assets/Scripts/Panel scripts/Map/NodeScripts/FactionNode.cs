using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionNode : NodeData
{
    [SerializeField] List<Faction> factionsOnTeam;
    [SerializeField] List<GameObject> units;//this should be all possible units then based on "turn" count uses more of them
    //picture? or just on prefab? like a node prefab
    [SerializeField] bool isBoss;

    public Faction getMainFactionOnNode()
    {
        return factionsOnTeam[0];
    }

    public List<Faction> GetIncludedFactions()
    {
        return factionsOnTeam;
    }

    public List<GameObject> getUnits()
    {
        return units;
    }

    public override List<Reward> GenerateRewardOptions()
    {
        Faction pf= UnitManager.instance.GetPlayerFaction();
        

        List<Reward> lootList = new List<Reward>();
        //find matching loot table
        var FactionLootLists = LootPanel.instance.FactionLootLists;

        for (int fcv = 0; fcv < factionsOnTeam.Count; fcv++)
        {
            for (int lcv = 0; lcv < FactionLootLists.Count; lcv++)
            {
                if (FactionLootLists[lcv].DoesMatchFactions(pf, factionsOnTeam[fcv]))
                {
                    lootList.AddRange(FactionLootLists[lcv].GetLootOptions());
                }
            }
        }
        Debug.Log("loot list count " + lootList.Count);
        if (lootList == null)
        {
            Debug.LogError("loot is Null");
        }

        return RemoveDuplicateUpgrages(lootList);//this is so the player can't upgrades they already have
    }

}
public enum Faction { Penguins, Giraffes, PolarBears, Beavers, Monkeys, Goats, Seals }