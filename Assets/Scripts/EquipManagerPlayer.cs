using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManagerPlayer : MonoBehaviour
{
    //the job of this class is to handle any upgrades to player units or bases
    [SerializeField] private GameObject playerBasePrefab;
    [SerializeField] private GameObject playerTowerPrefab;//serialize field for now

    //handle buffs to tower or base
    //handle blue prints in here?
    //handle equp buffs like to all units or bases or unit types

    public void InitialEquip(Faction fac)
    {
        //based on faction it will grab initial base & tower prefabs
    }

    public GameObject GetPlayerBase()
    {
        return playerBasePrefab;
    }

    public GameObject GetPlayerTower()
    {
        return playerTowerPrefab;
    }
}
