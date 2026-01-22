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
}
public enum Faction { Penguins, Giraffes, PolarBears, Beavers, Monkeys, Goats, Seals }