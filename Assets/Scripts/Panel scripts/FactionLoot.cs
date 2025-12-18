using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFactionLoot", menuName ="EinEFactionLoot")]
public class FactionLoot : ScriptableObject
{
    [SerializeField] private Faction playersFaction;
    [SerializeField] private Faction defeatedFaction;
    [SerializeField] private List<GameObject> lootOptions;//in future this might need to be a scriptable obj or something

    public bool DoesMatchFactions(Faction p, Faction e)
    {
        if(p==playersFaction&&e==defeatedFaction)
        {
            return true;
        }
        return false;
    }

    public List<GameObject> GetLootOptions()
    {
        return lootOptions;
    }
}
